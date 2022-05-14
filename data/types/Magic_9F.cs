﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Magic_9F : ITableObject
	{
		public int value;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.value = ReadInt32(origin, ref offset);
			return null;
		}
	}
}
