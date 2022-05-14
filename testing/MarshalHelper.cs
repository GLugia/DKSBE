using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.testing
{
	public static unsafe class MarshalHelper
	{
		public static void Shift(this ref IntPtr ptr, int size_of_data, long distance)
		{
			byte* data = (byte*)ptr;
			if (distance > 0)
			{
				for (int i = size_of_data - 1; i > -1; i--)
				{
					*(data + i + distance) = data[i];
					*(data + i) = 0;
				}
			}
			else
			{
				for (int i = 0; i < size_of_data; i++)
				{
					*(data + i + distance) = data[i];
					*(data + i) = 0;
				}
			}
			ptr = new IntPtr(data + distance);
		}
	}
}
