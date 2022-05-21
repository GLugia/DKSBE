using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSBE.data.controllers
{
	public unsafe class Unk_68 : PtrController
	{
		public byte* id => this.origin + 1;
		public byte* unk_00 => this.origin + 2;
		public int* value => (int*)this.origin + 1;
	}
}
