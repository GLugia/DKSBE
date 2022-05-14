
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Location : ITableObject
	{
		public uint id;
		public string? name;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt32(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			return null;
		}
	}
}
