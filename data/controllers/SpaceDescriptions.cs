using DKSBE.data.lists;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.controllers
{
	public unsafe class SpaceDescriptions : PtrController
	{
		public int* start => (int*)this.origin + 1;
		public int* end => (int*)this.origin + 2;

		public List<StringPtr> descriptions
		{
			get
			{
				List<StringPtr> ret = new();
				for (long offset = *this.start; offset < *this.end;)
				{
					StringPtr ptr = new(Stagebase.current_stagebase_file.origin + offset);
					offset += ptr.length;
					ret.Add(ptr);
				}
				return ret;
			}
		}

		public override long size => this.start->SizeOf() + this.end->SizeOf();

		public override void LockOffsets(Dictionary<long, PtrData> dict)
		{
			foreach (StringPtr str in this.descriptions)
			{
				dict.TryAdd(str - this.file_origin, new(str, str.length));
			}
		}

		public override void ModifyPointers(long target, int difference)
		{
			if (*this.start > target)
			{
				Write(this.start, *this.start + difference);
			}
			if (*this.end > target)
			{
				Write(this.end, *this.end + difference);
			}
		}

		public override void UnlockOffsets(Dictionary<long, PtrData> dict)
		{
			foreach (StringPtr str in this.descriptions)
			{
				dict.Remove(str - this.file_origin);
			}
		}
	}
}
