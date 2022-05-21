using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSBE.data.controllers
{
	public unsafe class Town : PtrController
	{
		public byte* id => this.origin + 4;
		public byte* continent_id => this.origin + 5;
		public ushort* unk_00 => (ushort*)(this.origin + 6);
		public StringPtr name => new(this.origin + 8);
		public int* unk_01 => (int*)((byte*)this.name + this.name.length);
		public int* unk_02 => this.unk_01 + 1;
		public int* unk_03 => this.unk_01 + 2;
	}
}
