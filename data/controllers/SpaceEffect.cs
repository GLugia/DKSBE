using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSBE.data.controllers
{
	public unsafe class SpaceEffect : PtrController
	{
		public byte* id_sup => this.origin + 4;
		public byte* id_sub => this.origin + 5;
		public byte* unk_00 => this.origin + 6;
		public byte* unk_01 => this.origin + 7;
		public int* unk_02 => (int*)(this.origin + 8);
		public int* value_raw => (int*)(this.origin + 12);
		public StringPtr value_display => new(this.origin + 16);
	}
}
