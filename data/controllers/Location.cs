using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.controllers
{
	public unsafe class Location : PtrController
	{
		public uint* id_ptr => (uint*)this.origin + 1;
		public StringPtr name => new((int*)this.origin + 2);
	}
}
