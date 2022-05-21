using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.controllers
{
	public unsafe class Unk_6E : PtrController
	{
		public uint* id => (uint*)this.origin + 1;
		public float* x => (float*)this.origin + 2;
		public float* y => (float*)this.origin + 3;
	}
}
