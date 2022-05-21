using DKSBE.data.controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.lists
{
	public unsafe class List_Ptr<T> : PtrController where T : PtrController
	{
		public int count { get; private set; }
		private int[] pointers;
		private T[] controllers;

		private long pre_pad_size
		{
			get
			{
				long ret = sizeof(int) + 1;
				for (int i = 0; i < this.count; i++)
				{
					ret += this.controllers[i].size;
				}
				ret += this.pointers.Length * sizeof(int);
				return ret;
			}
		}
		public override long size => this.pre_pad_size + (sizeof(int) - (this.pre_pad_size % sizeof(int)));
		private long old_size;

		public KeyValuePair<int, T> this[int index]
		{
			get
			{
				if (index < 0 || index >= this.count)
				{
					throw new IndexOutOfRangeException(index.ToHex());
				}
				return new KeyValuePair<int, T>(this.pointers[index], this.controllers[index]);
			}
		}

		public override void OnCreate()
		{
			this.count = 0;
			this.pointers = Array.Empty<int>();
			this.controllers = Array.Empty<T>();
			for (int* ptr = (int*)this.origin; *ptr != 0; ptr++)
			{
				Array.Resize(ref this.pointers, this.pointers.Length + 1);
				this.pointers[^1] = *ptr;
				this.count++;
			}
			for (int i = 0; i < this.count; i++)
			{
				Array.Resize(ref this.controllers, this.controllers.Length + 1);
				this.controllers[^1] = CreateInstance<T>(this.file_origin, this.pointers[i]);
			}
		}

		public void Add(T controller)
		{
			// store the old size for later use in this.Write()
			this.old_size = this.size;
			Stagebase.instance.ModifyAllPointers(this.file_origin, (this.origin - this.file_origin) + (this.count * sizeof(int)), sizeof(int));
			int offset = this.count > 0 ? (int)(this.pointers[^1] + this.controllers[^1].size) : (int)(this.origin - this.file_origin + sizeof(int));
			Array.Resize(ref this.pointers, this.count + 1);
			this.pointers[^1] = offset;
			Array.Resize(ref this.controllers, this.count + 1);
			this.controllers[^1] = controller;
			this.count++;
			this.Write();
		}

		public bool Remove(int index)
		{
			// if the index is not within range, do not continue
			if (index < 0 || index >= this.count)
			{
				return false;
			}
			this.old_size = this.size;
			int[] temp_pointers = this.pointers;
			this.pointers = new int[this.count - 1];
			T[] temp_controllers = this.controllers;
			this.controllers = new T[this.count - 1];
			int j = 0;
			for (int i = 0; i < this.count; i++)
			{
				if (i == index)
				{
					continue;
				}
				this.pointers[j] = temp_pointers[i];
				this.controllers[j++] = temp_controllers[i];
			}
			this.count--;
			this.Write();
			return true;
		}

		private void Write()
		{
			byte[] data = new byte[this.size];
			int offset = 0;
			for (int i = 0; i < this.count; i++)
			{
				BitConverter.GetBytes(this.pointers[i]).CopyTo(data, offset);
				offset += sizeof(int);
			}
			offset += sizeof(int);
			for (int i = 0; i < this.count; i++)
			{
				for (int j = 0; j < this.controllers[i].size; j++)
				{
					data[offset++] = *(this.controllers[i].origin + j);
				}
			}
			offset++;
			offset += sizeof(int) - (offset % sizeof(int));
			Stagebase.instance.Write(this.origin, this.old_size, data);
			this.old_size = this.size;
		}

		public override void ModifyOrigin(long alloc_difference, byte* target, long size_difference)
		{
			base.ModifyOrigin(alloc_difference, target, size_difference);
			for (int i = 0; i < this.count; i++)
			{
				long old = this.controllers[i] - this.controllers[i].file_origin;
				this.controllers[i].ModifyOrigin(alloc_difference, target, size_difference);
				Logger.Debug($"{((int)old).ToHex()} -> {((int)(this.controllers[i] - this.controllers[i].file_origin)).ToHex()}");
			}
			Logger.Debug("\n");
		}

		public override void ModifyPointers(long target, int difference)
		{
			for (int i = 0; i < this.count; i++)
			{
				if (this.pointers[i] >= target)
				{
					int old = *((int*)this.origin + i);
					this.pointers[i] += difference;
					MarshalHelper.Write((int*)this.origin + i, this.pointers[i]);
					Logger.Debug($"{old.ToHex()} -> {(*((int*)this.origin + i)).ToHex()}");
				}
				this.controllers[i].ModifyPointers(target, difference);
			}
			Logger.Debug("\n");
			;
		}
	}
}
