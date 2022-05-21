using DKSBE.data.lists;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSBE.data.controllers
{
	public unsafe class Unk_85 : PtrController
	{
		public List_SE<byte> array_0 { get; private set; } = new();

		public List_SE<byte> array_1 { get; private set; } = new();

		public long count => this.array_0.count + this.array_1.count;

		public override long size => this.array_0.size + this.array_1.size - sizeof(int);

		public byte this[int index]
		{
			get
			{
				if (index >= this.array_0.count)
				{
					return this.array_1[index - this.array_0.count];
				}
				return this.array_0[index];
			}
			set
			{
				if (index >= this.array_0.count)
				{
					this.array_1[index - this.array_0.count] = value;
					return;
				}
				this.array_0[index] = value;
			}
		}

		public override void OnCreate()
		{
			this.array_0 = CreateInstance<List_SE<byte>>(this.file_origin, this.origin - this.file_origin);
			this.array_1 = CreateInstance<List_SE<byte>>(this.file_origin, this.origin - this.file_origin + 4);
		}

		public override void ModifyOrigin(long alloc_difference, byte* target, long size_difference)
		{
			base.ModifyOrigin(alloc_difference, target, size_difference);
			this.array_0.ModifyOrigin(alloc_difference, target, size_difference);
			this.array_1.ModifyOrigin(alloc_difference, target, size_difference);
		}

		public override void ModifyPointers(long target, int difference)
		{
			this.array_0.ModifyPointers(target, difference);
			this.array_1.ModifyPointers(target, difference);
		}

		public override void LockOffsets(Dictionary<long, PtrData> dict)
		{
			this.array_0.LockOffsets(dict);
			this.array_1.LockOffsets(dict);
		}

		public override void UnlockOffsets(Dictionary<long, PtrData> dict)
		{
			this.array_0.UnlockOffsets(dict);
			this.array_1.UnlockOffsets(dict);
		}
	}
}
