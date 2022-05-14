﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.system
{
	public unsafe class Sys_0C : ITableObject
	{
		public byte id;
		public byte unk_00;
		public byte unk_01;
		public byte unk_02;
		public uint unk_03;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadByte(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.unk_02 = ReadByte(origin, ref offset);
			this.unk_03 = ReadUInt32(origin, ref offset);
			return null;
		}
	}
}
