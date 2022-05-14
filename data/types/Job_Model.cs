using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Job_Model : ITableObject
	{
		public byte id;
		public byte gender;
		public short unk_id;
		public string? name;
		public int unk_00;


		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.gender = ReadByte(origin, ref offset);
			this.unk_id = ReadInt16(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			this.unk_00 = ReadInt32(origin, ref offset);
			return null;
		}
	}
}
