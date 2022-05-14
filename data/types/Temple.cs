﻿
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Temple : ITableObject
	{
		public byte id;
		public byte unk_01;
		public byte unk_02;
		public byte unk_03;
		public string? name;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.unk_02 = ReadByte(origin, ref offset);
			this.unk_03 = ReadByte(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			return null;
		}
	}
}