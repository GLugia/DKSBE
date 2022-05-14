using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Job_Bag : ITableObject
	{
		public byte id;
		public byte gender;
		public byte item;
		public byte magic;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.gender = ReadByte(origin, ref offset);
			this.item = ReadByte(origin, ref offset);
			this.magic = ReadByte(origin, ref offset);
			return null;
		}
	}
}
