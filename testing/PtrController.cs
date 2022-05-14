using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.testing
{
	public unsafe abstract class PtrController
	{
		public byte* origin { get; internal set; }
		public abstract long size { get; }
	}
}
