using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSBE.data.controllers
{
	public unsafe class Unk_2B : PtrController
	{
		public ushort* id => (ushort*)(this.origin + 4);
		public ushort* unk_00 => (ushort*)(this.origin + 6);
		public ushort* unk_01 => (ushort*)(this.origin + 8);
		public ushort* unk_02 => (ushort*)(this.origin + 10);
	}
}
