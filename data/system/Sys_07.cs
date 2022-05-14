using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.system
{
	public unsafe class Sys_07 : ITableObject
	{
		public byte id;
		public byte unk_00;
		public byte unk_01;
		public byte unk_02;
		public byte unk_03;
		public byte unk_04;
		public byte unk_05;
		public byte unk_06;
		public float[]? unk_07;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			Dictionary<long, PtrData?>? result = new();
			this.unk_00 = ReadByte(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.unk_02 = ReadByte(origin, ref offset);
			this.unk_03 = ReadByte(origin, ref offset);
			this.unk_04 = ReadByte(origin, ref offset);
			this.unk_05 = ReadByte(origin, ref offset);
			this.unk_06 = ReadByte(origin, ref offset);
			int ptr = ReadInt32(origin, ref offset);
			long ret_offset = offset;
			offset = ptr;
			this.unk_07 = new float[20];
			for (int i = 0; i < this.unk_07.Length; i++)
			{
				this.unk_07[i] = ReadSingle(origin, ref offset);
			}
			result.Add(ptr, new()
			{
				value = this.unk_07,
				size = offset - ptr
			});
			offset = ret_offset;
			return result;
		}
	}
}
