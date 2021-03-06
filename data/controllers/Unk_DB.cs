using DKSBE.data.lists;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.controllers
{
	public unsafe class Unk_DB : PtrController
	{
		public int* unk_00 => (int*)(this.origin + 4);

		public List_EP<ushort> array { get; private set; } = new();

		public override long size => this.unk_00->SizeOf() + this.array.size;

		public ushort this[int index]
		{
			get => this.array[index];
			set => this.array[index] = value;
		}

		public override void OnCreate()
		{
			this.array = CreateInstance<List_EP<ushort>>(this.file_origin, this.origin - this.file_origin + 4);
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
