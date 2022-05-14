using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Job_Money : ITableObject
	{
		public byte id;
		public byte gender;
		public byte bonus_salary;
		public byte job_id;
		public int starting_money;
		public short level_multiplier;
		public short bonus_multiplier;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.gender = ReadByte(origin, ref offset);
			this.bonus_salary = ReadByte(origin, ref offset);
			this.job_id = ReadByte(origin, ref offset);
			this.starting_money = ReadInt32(origin, ref offset);
			this.level_multiplier = ReadInt16(origin, ref offset);
			this.bonus_multiplier = ReadInt16(origin, ref offset);
			return null;
		}
	}
}
