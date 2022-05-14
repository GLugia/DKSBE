using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Npc_Enemy_Drop_Table : ITableObject
	{
		public byte id;
		public byte chance_for_a;
		public byte chance_for_b;
		public byte padding;
		public byte item_id_a;
		public byte table_id_a;
		public byte item_id_b;
		public byte table_id_b;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.chance_for_a = ReadByte(origin, ref offset);
			this.chance_for_b = ReadByte(origin, ref offset);
			this.padding = ReadByte(origin, ref offset);
			this.item_id_a = ReadByte(origin, ref offset);
			this.table_id_a = ReadByte(origin, ref offset);
			this.item_id_b = ReadByte(origin, ref offset);
			this.table_id_b = ReadByte(origin, ref offset);
			return null;
		}
	}
}
