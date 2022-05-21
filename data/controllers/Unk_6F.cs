using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSBE.data.controllers
{
	public unsafe class Unk_6F : PtrController
	{
		public byte* id => this.origin + 4;
		public byte* unk_00 => this.origin + 5;
	}
}
