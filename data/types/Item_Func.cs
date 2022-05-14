using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Item_Func : ITableObject
	{
		public ushort id;
		public short param;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt16(origin, ref offset);
			this.param = ReadInt16(origin, ref offset);
			return null;
		}
	}
}
