using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Hair_Model : ITableObject
	{
		public uint id;
		public string? model;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt32(origin, ref offset);
			this.model = ReadString(origin, ref offset);
			return null;
		}
	}
}
