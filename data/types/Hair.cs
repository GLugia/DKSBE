using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Hair : ITableObject
	{
		public byte id;
		public byte non_job_id;
		public ushort icon;
		public string? name;
		public byte unk_00;
		public byte non_job_id_2;
		public byte unk_01;
		public byte unk_02;
		public byte unk_03;
		public byte unk_04;
		public byte unk_05;
		public byte rating;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.non_job_id = ReadByte(origin, ref offset);
			this.icon = ReadUInt16(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			this.unk_00 = ReadByte(origin, ref offset);
			this.non_job_id_2 = ReadByte(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.unk_02 = ReadByte(origin, ref offset);
			this.unk_03 = ReadByte(origin, ref offset);
			this.unk_04 = ReadByte(origin, ref offset);
			this.unk_05 = ReadByte(origin, ref offset);
			this.rating = ReadByte(origin, ref offset);
			return null;
		}
	}
}
