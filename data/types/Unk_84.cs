using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_84 : ITableObject
	{
		public uint id;
		public ushort unk_00;
		public ushort unk_01;
		public ushort unk_02;
		public ushort unk_03;
		public ushort unk_04;
		public ushort unk_05;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt32(origin, ref offset);
			this.unk_00 = ReadUInt16(origin, ref offset);
			this.unk_01 = ReadUInt16(origin, ref offset);
			this.unk_02 = ReadUInt16(origin, ref offset);
			this.unk_03 = ReadUInt16(origin, ref offset);
			this.unk_04 = ReadUInt16(origin, ref offset);
			this.unk_05 = ReadUInt16(origin, ref offset);
			return null;
		}
	}
}
