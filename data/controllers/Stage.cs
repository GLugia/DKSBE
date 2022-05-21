using static DKSBE.MarshalHelper;

namespace DKSBE.data.controllers
{
	public unsafe class Stage : PtrController
	{
		public int* id => (int*)this.origin + 1;
		public int* file_ptr => (int*)this.origin + 2;

		public StringPtr file => new(*this.file_ptr);

		public override long size => this.id->SizeOf() + this.file_ptr->SizeOf();

		public override void LockOffsets(Dictionary<long, PtrData> dict)
		{
			dict.TryAdd(*this.file_ptr, new(this.file_origin + *this.file_ptr, this.file.length));
		}

		public override void UnlockOffsets(Dictionary<long, PtrData> dict)
		{
			dict.Remove(*this.file_ptr);
		}

		public override void ModifyPointers(long target, int difference)
		{
			if (*this.file_ptr > target)
			{
				Write(this.file_ptr, *this.file_ptr + difference);
			}
		}
	}
}
