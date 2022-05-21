namespace DKSBE.data
{
	public unsafe struct FilePtr
	{
		public byte* origin;
		public int length => *((int*)this.origin + 1) + sizeof(int);
		public int header => *((int*)this.origin + 2);

		public FilePtr(byte* origin)
		{
			this.origin = origin;
		}
	}
}
