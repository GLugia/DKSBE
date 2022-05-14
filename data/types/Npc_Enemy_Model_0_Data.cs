using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Npc_Enemy_Model_0_Data : ITableObject
	{
		public ushort[]? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.data = ReadValueTerminatedArray<ushort>(origin, ref offset, 0xFFFF, alignment: sizeof(ushort));
			return null;
		}
	}
}
