using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_A1 : ITableObject
	{
		public float unk_00;
		public float unk_01;
		public float unk_02;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadSingle(origin, ref offset);
			this.unk_01 = ReadSingle(origin, ref offset);
			this.unk_02 = ReadSingle(origin, ref offset);
			return null;
		}
	}
}
