using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSBE.data.controllers
{
	public unsafe class Temple : PtrController
	{
		public uint* id => (uint*)this.origin + 1;
		public StringPtr name => new((int*)this.origin + 2);
	}
}
