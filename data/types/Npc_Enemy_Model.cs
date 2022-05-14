using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Npc_Enemy_Model : ITableObject
	{
		public ushort id;
		public byte unk_00;
		public byte unk_01;
		public float unk_02;
		public string? name;
		[Unused]
		public int unk_03;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt16(origin, ref offset);
			this.unk_00 = ReadByte(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.unk_02 = ReadSingle(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			this.unk_03 = ReadInt32(origin, ref offset);
			if (this.unk_03 != 0)
			{
				;
			}
			return null;
		}
	}
}
