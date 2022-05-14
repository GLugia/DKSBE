using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_A0 : ITableObject
	{
		public float unk_00;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadSingle(origin, ref offset);
			return null;
		}
	}
}
