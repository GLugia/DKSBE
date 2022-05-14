using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.testing.controllers
{
	public unsafe class FileLabel : PtrController
	{
		/// <summary>
		/// specifies the offset in which <see cref="label"/> should stop reading
		/// </summary>
		public int* end_of_string => (int*)this.origin + 1;
		/// <summary>
		/// This is not a 0-terminated char*.
		/// </summary>
		public char* label => (char*)this.origin + 8;

		public override long size => *(this.end_of_string - (this.origin - Stagebase.current_stagebase_file.origin));
	}
}
