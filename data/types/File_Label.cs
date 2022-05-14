using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class File_Label : ITableObject
	{
		public string? label;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			int end_offset = ReadInt32(origin, ref offset);
			this.label = ReadString(origin, ref offset);
			offset = end_offset;
			return null;
		}
	}
}
