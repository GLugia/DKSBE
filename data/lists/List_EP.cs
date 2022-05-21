using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.lists
{
	public unsafe class List_EP<T> : PtrController where T : struct
	{
		public int* end => (int*)this.origin + 1;
		public int* pad => (int*)this.origin + 2;

		public override long size => this.end->SizeOf() + this.pad->SizeOf();

		public long count => this.array.Length;

		private byte* array_ptr => (byte*)((int*)this.origin + 3);
		private bool needs_update;
		private T[]? _array;
		private T[] array
		{
			get
			{
				if (this._array == null || this.needs_update)
				{
					long offset = 0;
					this._array = ReadArray<T>(this.array_ptr, ref offset, (*this.end - (this.array_ptr - this.file_origin)) / Marshal.SizeOf<T>());
					this.needs_update = false;
				}
				return this._array;
			}
		}
		public T this[int index]
		{
			get
			{
				long offset = 0;
				if (this._array == null || this.needs_update)
				{
					this._array = ReadArray<T>(this.array_ptr, ref offset, (*this.end - (this.array_ptr - this.file_origin)) / Marshal.SizeOf<T>());
					this.needs_update = false;
				}
				return this._array[index];
			}
			set
			{
				int size = Marshal.SizeOf<T>();
				IntPtr _target = Marshal.AllocHGlobal(size);
				Marshal.StructureToPtr(value, _target, true);
				byte* target = (byte*)_target;
				byte[] bytes = new byte[size];
				for (int i = 0; i < size; i++)
				{
					bytes[i] = *target++;
				}
				Stagebase.instance.Write(this.array_ptr + (index * Marshal.SizeOf<T>()), size, bytes);
				Marshal.FreeHGlobal(_target);
				this.needs_update = true;
				_ = this[index];
				if (this._array != null)
				{
					this._array[index] = value;
				}
			}
		}

		public override void ModifyPointers(long target, int difference)
		{
			if (*this.end > target)
			{
				Write(this.end, *this.end + difference);
			}
			if (*this.pad > target)
			{
				Write(this.pad, *this.pad + difference);
			}
		}

		public override void LockOffsets(Dictionary<long, PtrData> dict)
		{
			dict.TryAdd(this.array_ptr - this.file_origin, new(this.origin, this.array.SizeOf() + (*this.pad - *this.end)));
		}

		public override void UnlockOffsets(Dictionary<long, PtrData> dict)
		{
			dict.Remove(this.array_ptr - this.file_origin);
		}
	}
}
