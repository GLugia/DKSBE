using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_47 : ITableObject
	{
		public int unk_00;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			unk_00 = ReadInt32(origin, ref offset);
			return null;
		}
	}
}
