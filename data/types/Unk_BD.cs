using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_BD : ITableObject
	{
		public uint id;
		public uint unk_00;
		public uint unk_01;
		public uint unk_02;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt32(origin, ref offset);
			this.unk_00 = ReadUInt32(origin, ref offset);
			this.unk_01 = ReadUInt32(origin, ref offset);
			this.unk_02 = ReadUInt32(origin, ref offset);
			return null;
		}
	}
}
