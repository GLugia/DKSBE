using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.controllers
{
	public unsafe class Unk_78_Data : PtrController
	{
		public byte[] array_0
		{
			get
			{
				Logger.Debug($"Offset: {((int)(this.origin - this.file_origin)).ToHex()}");
				byte[] ret = Array.Empty<byte>();
				for (int i = 0; Marshal.ReadByte((IntPtr)this.origin, i) != 0; i++)
				{
					Array.Resize(ref ret, ret.Length + 1);
					ret[i] = Marshal.ReadByte((IntPtr)this.origin, i);
				}
				return ret;
			}
		}

		public byte[] array_1
		{
			get
			{
				byte[] ret = Array.Empty<byte>();
				int array_0_length = this.array_0.Length + 1;
				for (int i = 0; Marshal.ReadByte((IntPtr)this.origin, array_0_length + i) != 0; i++)
				{
					Array.Resize(ref ret, ret.Length + 1);
					ret[i] = Marshal.ReadByte((IntPtr)this.origin, array_0_length + i);
				}
				return ret;
			}
		}

		public long count => this.array_0.Length + this.array_1.Length;

		public override long size => this.array_0.Length + this.array_1.Length + 2;

		public byte this[int index]
		{
			get
			{
				if (index >= this.array_0.Length)
				{
					return this.array_1[index - this.array_0.Length];
				}
				return this.array_0[index];
			}
			set
			{
				if (index >= this.array_0.Length)
				{
					this.array_1[index - this.array_0.Length] = value;
					return;
				}
				this.array_0[index] = value;
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
