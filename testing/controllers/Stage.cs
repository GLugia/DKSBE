using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.testing.controllers
{
	public unsafe class Stage : PtrController
	{
		public int* id => (int*)this.origin + 1;
		public int* file_ptr => (int*)this.origin + 2;
		public char* file => (char*)this.origin + *this.file_ptr;
		public override long size => 0xC;
	}
}
