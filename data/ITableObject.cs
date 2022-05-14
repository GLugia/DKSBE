using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data
{
	public unsafe interface ITableObject
	{
		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset);
	}
}
