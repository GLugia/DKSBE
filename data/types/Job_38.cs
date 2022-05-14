﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Job_38 : ITableObject
	{
		public byte id;
		public byte gender;
		public short padding;
		public string? unk_00;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.gender = ReadByte(origin, ref offset);
			this.padding = ReadInt16(origin, ref offset);
			this.unk_00 = ReadString(origin, ref offset);
			return null;
		}
	}
}