using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Status_Permanent : ITableObject
	{
		public ushort id;
		public ushort icon;
		public string? name;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt16(origin, ref offset);
			this.icon = ReadUInt16(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			return null;
		}
	}
}
