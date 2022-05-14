using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_9E : ITableObject
	{
		public byte[]? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			int end = ReadInt32(origin, ref offset);
			int pad = ReadInt32(origin, ref offset);
			this.data = ReadArray<byte>(origin, ref offset, end - offset);
			offset = pad;
			return null;
		}
	}
}
