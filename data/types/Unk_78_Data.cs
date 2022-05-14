using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_78_Data : ITableObject
	{
		public byte[]? unk_00;
		public byte[]? unk_01;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadValueTerminatedArray<byte>(origin, ref offset, 0);
			this.unk_01 = ReadValueTerminatedArray<byte>(origin, ref offset, 0);
			return null;
		}
	}
}
