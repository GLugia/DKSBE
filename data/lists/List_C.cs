using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.lists
{
	public unsafe class List_C<T> : PtrController where T : struct
	{
		public ushort* count => (ushort*)this.origin;

		public override long size => this.count->SizeOf() + (*this.count * Marshal.SizeOf<T>()) + Marshal.SizeOf<T>();

		private bool needs_update;
		private T[]? _array;
		private T[] array
		{
			get
			{
				if (this._array == null || this.needs_update)
				{
					long offset = 0;
					this._array = ReadArray<T>(this.origin, ref offset, *this.count);
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
					this._array = ReadArray<T>(this.origin, ref offset, *this.count);
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
				Stagebase.instance.Write(this.origin + (index * Marshal.SizeOf<T>()), size, bytes);
				Marshal.FreeHGlobal(_target);
				this.needs_update = true;
				_ = this[index];
				if (this._array != null)
				{
					this._array[index] = value;
				}
			}
		}

		public override void LockOffsets(Dictionary<long, PtrData> dict)
		{
			dict.TryAdd(this.origin - this.file_origin, new(this.origin, this.size));
		}

		public override void UnlockOffsets(Dictionary<long, PtrData> dict)
		{
			dict.Remove(this.origin - this.file_origin);
		}
	}
}
