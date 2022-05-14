using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_8C : ITableObject
	{
		public ushort[]? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			int end = ReadInt32(origin, ref offset);
			this.data = Array.Empty<ushort>();
			do
			{
				Array.Resize(ref this.data, this.data.Length + 1);
				this.data[^1] = ReadUInt16(origin, ref offset);
			}
			while (this.data[^1] != 0 && offset < end);
			offset = end;
			return null;
		}
	}
}
