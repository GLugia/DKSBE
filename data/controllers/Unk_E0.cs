using DKSBE.data.lists;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSBE.data.controllers
{
	public unsafe class Unk_E0 : PtrController
	{
		public List_EP<ushort> data { get; private set; } = new();

		public override long size => this.data.size;

		public ushort this[int index]
		{
			get => this.data[index];
			set => this.data[index] = value;
		}

		public override void OnCreate()
		{
			this.data = CreateInstance<List_EP<ushort>>(this.file_origin, this.origin - this.file_origin);
		}

		public override void ModifyOrigin(long alloc_difference, byte* target, long size_difference)
		{
			base.ModifyOrigin(alloc_difference, target, size_difference);
			this.data.ModifyOrigin(alloc_difference, target, size_difference);
		}

		public override void ModifyPointers(long target, int difference)
		{
			this.data.ModifyPointers(target, difference);
		}

		public override void LockOffsets(Dictionary<long, PtrData> dict)
		{
			this.data.LockOffsets(dict);
		}

		public override void UnlockOffsets(Dictionary<long, PtrData> dict)
		{
			this.data.UnlockOffsets(dict);
		}
	}
}
