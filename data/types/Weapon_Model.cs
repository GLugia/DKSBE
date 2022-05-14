using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Weapon_Model : ITableObject
	{
		public uint id;
		public float x1;
		public float y1;
		public float z1;
		public float x2;
		public float y2;
		public float z2;
		public string? name;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt32(origin, ref offset);
			this.x1 = ReadSingle(origin, ref offset);
			this.y1 = ReadSingle(origin, ref offset);
			this.z1 = ReadSingle(origin, ref offset);
			this.x2 = ReadSingle(origin, ref offset);
			this.y2 = ReadSingle(origin, ref offset);
			this.z2 = ReadSingle(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			return null;
		}
	}
}
