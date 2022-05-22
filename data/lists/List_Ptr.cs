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
			int offset = this.count > 0 ? (int)(this.pointers[^1] + this.controllers[^1].size) : (int)(this.origin - this.file_origin + sizeof(int));
			Stagebase.instance.Write((byte*)((int*)this.origin + this.count), 0, BitConverter.GetBytes(offset));
			Array.Resize(ref this.pointers, this.count + 1);
			this.pointers[^1] = offset;
			byte[] data = new byte[controller.size];
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = *(controller.origin + i);
			}
			Stagebase.instance.Write(this.file_origin + offset, 0, data);
			controller.file_origin = this.file_origin;
			controller.origin = this.file_origin + offset;
			Array.Resize(ref this.controllers, this.count + 1);
			this.controllers[^1] = controller;
			this.count++;
		}

		public bool Remove(int index)
		{
			// if the index is not within range, do not continue
			if (index < 0 || index >= this.count)
			{
				return false;
			}
			int[] pointers = new int[this.count - 1];
			T[] controllers = new T[this.count - 1];
			int j = 0;
			for (int i = 0; i < this.count; i++)
			{
				if (i == index)
				{
					continue;
				}
				pointers[j] = this.pointers[i];
				controllers[j] = this.controllers[i];
				j++;
			}
			this.pointers = pointers;
			this.controllers = controllers;
			this.count--;
			Stagebase.instance.Write((byte*)((int*)this.origin + index), 4, Array.Empty<byte>());
			Stagebase.instance.Write(controllers[index].origin, controllers[index].size, Array.Empty<byte>());
			return true;
		}

		public override void ModifyOrigin(long alloc_difference, byte* target, long size_difference)
		{
			base.ModifyOrigin(alloc_difference, target, size_difference);
			for (int i = 0; i < this.count; i++)
			{
				this.controllers[i].ModifyOrigin(alloc_difference, target, size_difference);
			}
		}

		public override void ModifyPointers(long target, int difference)
		{
			for (int i = 0; i < this.count; i++)
			{
				if (this.pointers[i] >= target)
				{
					this.pointers[i] += difference;
					Write((int*)this.origin + i, this.pointers[i]);
				}
				this.controllers[i].ModifyPointers(target, difference);
			}
		}
	}
}
