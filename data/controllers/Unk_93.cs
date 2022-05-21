using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSBE.data.controllers
{
	public unsafe class Unk_93 : PtrController
	{
		public byte* id => this.origin + 4;
		public byte* unk_00 => this.origin + 5;
		public byte* unk_01 => this.origin + 6;
		public byte* unk_02 => this.origin + 7;
	}
}
