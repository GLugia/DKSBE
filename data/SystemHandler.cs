namespace DKSBE.data
{
	/// <summary>
	/// The ID of each system object type. See <see cref="Utils.EnumToType{T}(in T, out Type?)"/> for explanation on casing and format regarding class or struct names.
	/// </summary>
	public enum SystemHandler : byte
	{
		DUMMY_FILE = 0x00,
		SYS_01 = 0x01,
		SYS_03 = 0x03,
		SYS_06 = 0x06,
		SYS_07 = 0x07,
		SYS_09 = 0x09,
		SYS_0C = 0x0C,
		SYS_11 = 0x11,
		SYS_1A = 0x1A,
		COUNT,
	}
}
