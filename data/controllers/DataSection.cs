using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.controllers
{
	public unsafe class DataSection : PtrController
	{
		public int* end_of_data => (int*)(this.origin + 4);
		public int* spacing => (int*)(this.origin + 8);

		public override void ModifyPointers(long target, int difference)
		{
			if (*this.end_of_data > target)
			{
				Write(this.end_of_data, *this.end_of_data + difference);
			}
		}
	}
}
