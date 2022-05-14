using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.testing
{
	public unsafe struct PtrData
	{
		public readonly byte* origin;
		public readonly long length;

		public PtrData(byte* origin, long length)
		{
			this.origin = origin;
			this.length = length;
		}
	}
}
