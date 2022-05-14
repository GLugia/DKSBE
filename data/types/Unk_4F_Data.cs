using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_4F_Data : ITableObject
	{
		public byte[]? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.data = ReadValueTerminatedArray<byte>(origin, ref offset, 0xFF);
			return null;
		}
	}
}
