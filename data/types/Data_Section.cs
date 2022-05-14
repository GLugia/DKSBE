
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Data_Section : ITableObject
	{
		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			offset = ReadInt32(origin, ref offset);
			return null;
		}
	}
}
