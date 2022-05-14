namespace DKSBE.data
{
	public unsafe struct FileData
	{
		public readonly byte* origin;
		public readonly long length;
		public readonly long header;
		private readonly Dictionary<long, testing.PtrData> ptr_data;

		public FileData(byte* origin, long length, long header)
		{
			this.origin = origin;
			this.length = length;
			this.header = header;
			this.ptr_data = new();
		}

		public void Close()
		{
			this.ptr_data.Clear();
		}

		public bool LockOffset(byte* origin, long offset, long length)
		{
			if (this.origin == origin && !this.ptr_data.ContainsKey(offset))
			{
				this.ptr_data.Add(offset, new(origin, length));
				return true;
			}
			return false;
		}

		public bool IsOffsetLocked(byte* origin, ref long offset, out byte* data)
		{
			if (this.origin == origin && this.ptr_data.ContainsKey(offset))
			{
				data = this.ptr_data[offset].origin;
				offset += this.ptr_data[offset].length;
				return true;
			}
			data = null;
			return false;
		}

		public bool UnlockOffset(byte* origin, long offset)
		{
			if (this.origin == origin && this.ptr_data.ContainsKey(offset))
			{
				return this.ptr_data.Remove(offset);
			}
			return false;
		}
	}
}
