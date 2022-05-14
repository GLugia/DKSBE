using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Job_D6 : ITableObject
	{
		public uint id;
		public short unk_00;
		public short unk_01;
		public short unk_02;
		public short unk_03;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt32(origin, ref offset);
			this.unk_00 = ReadInt16(origin, ref offset);
			this.unk_01 = ReadInt16(origin, ref offset);
			this.unk_02 = ReadInt16(origin, ref offset);
			this.unk_03 = ReadInt16(origin, ref offset);
			return null;
		}
	}
}
