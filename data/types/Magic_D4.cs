using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Magic_D4 : ITableObject
	{
		public byte[]? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.data = new byte[ReadInt32(origin, ref offset)];
			int end = ReadInt32(origin, ref offset);
			for (int i = 0; i < this.data.Length; i++)
			{
				this.data[i] = ReadByte(origin, ref offset);
			}
			offset = end;
			return null;
		}
	}
}
