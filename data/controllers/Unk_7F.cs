using DKSBE.data.lists;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.controllers
{
	public unsafe class Unk_7F : PtrController
	{
		public uint* id => (uint*)(this.origin + 4);
		public uint* array_ptr => (uint*)(this.origin + 8);

		public List_C<ushort> array { get; private set; } = new();

		public override long size => this.id->SizeOf() + this.array_ptr->SizeOf();

		public ushort this[int index]
		{
			get => this.array[index];
			set => this.array[index] = value;
		}

		public override void OnCreate()
		{
			this.array = CreateInstance<List_C<ushort>>(this.file_origin, *this.array_ptr);
		}

		public override void ModifyOrigin(long alloc_difference, byte* target, long size_difference)
		{
			base.ModifyOrigin(alloc_difference, target, size_difference);
			this.array.ModifyOrigin(alloc_difference, target, size_difference);
		}

		public override void ModifyPointers(long target, int difference)
		{
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
