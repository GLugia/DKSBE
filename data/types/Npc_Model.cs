using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Npc_Model : ITableObject
	{
		public ushort id;
		public ushort unk_00;
		public ushort unk_01;
		public short padding;
		public string? name;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt16(origin, ref offset);
			this.unk_00 = ReadUInt16(origin, ref offset);
			this.unk_01 = ReadUInt16(origin, ref offset);
			this.padding = ReadInt16(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			return null;
		}
	}
}
