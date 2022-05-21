using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSBE.data.controllers
{
	public unsafe class Unk_66 : PtrController
	{
		public int* value => (int*)this.origin + 1;
	}
}
