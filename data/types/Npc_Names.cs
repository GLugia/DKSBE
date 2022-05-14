using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Npc_Names : ITableObject
	{
		public uint id;
		public List<string>? descriptions;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt32(origin, ref offset);
			this.descriptions = new();
			int start = ReadInt32(origin, ref offset);
			long ret_offset = offset;
			offset = start;
			do
			{
				string str = ReadString(origin, ref offset, sizeof(short));
				this.descriptions?.Add(str);
			}
			while (*(ushort*)(origin + offset) != 0);
			long length = offset - start;
			offset = ret_offset;
			return new() // new dictionary
			{
				{
					start,
					new() // new ptr_data
					{
						value = this.descriptions,
						size = length
					}
				}
			};
		}
	}
}
