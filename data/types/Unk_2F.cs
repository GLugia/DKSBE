
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_2F : ITableObject
	{
		public int value;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.value = ReadInt32(origin, ref offset);
			return null;
		}
	}
}
