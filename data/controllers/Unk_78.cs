using DKSBE.data.lists;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSBE.data.controllers
{
	public unsafe class Unk_78 : PtrController
	{
		public uint* id => (uint*)(this.origin + 4);

		public int* array_ptr => (int*)(this.origin + 8);

		public override long size => this.id->SizeOf() + this.array_ptr->SizeOf();


		public List_Ptr<Unk_78_Data> array { get; private set; } = new();

		public long count => this.array.count;

		public Unk_78_Data this[int index] => this.array[index].Value;

		public override void OnCreate()
		{
			this.array = CreateInstance<List_Ptr<Unk_78_Data>>(this.file_origin, *this.array_ptr);
		}

		public override void ModifyOrigin(long alloc_difference, byte* target, long size_difference)
		{
			base.ModifyOrigin(alloc_difference, target, size_difference);
			this.array.ModifyOrigin(alloc_difference, target, size_difference);
		}

		public override void ModifyPointers(long target, int difference)
		{
			if (*this.array_ptr > target)
			{
				MarshalHelper.Write(this.array_ptr, *this.array_ptr + difference);
			}
			this.array.ModifyPointers(target, difference);
		}

		public override void LockOffsets(Dictionary<long, PtrData> dict)
		{
			this.array.LockOffsets(dict);
		}

		public override void UnlockOffsets(Dictionary<long, PtrData> dict)
		{
			this.array.UnlockOffsets(dict);
		}
	}
}
