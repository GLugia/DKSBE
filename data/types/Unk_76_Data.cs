using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_76_Data : ITableObject
	{
		public int[]? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			offset -= sizeof(int) * 2;
			int end = ReadInt32(origin, ref offset);
			offset += sizeof(int);
			this.data = ReadArray<int>(origin, ref offset, (end - offset) / sizeof(int));
			return null;
		}
	}
}
