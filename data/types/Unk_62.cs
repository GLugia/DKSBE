﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_62 : ITableObject
	{
		public byte id;
		public byte unk_00;
		public byte unk_01;
		public byte unk_02;
		public byte unk_03;
		public byte unk_04;
		public byte unk_05;
		public byte unk_06;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.unk_00 = ReadByte(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.unk_02 = ReadByte(origin, ref offset);
			this.unk_03 = ReadByte(origin, ref offset);
			this.unk_04 = ReadByte(origin, ref offset);
			this.unk_05 = ReadByte(origin, ref offset);
			this.unk_06 = ReadByte(origin, ref offset);
			return null;
		}
	}
}
