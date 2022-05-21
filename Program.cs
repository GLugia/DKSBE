using DKSBE.data;
using DKSBE.data.controllers;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace DKSBE
{
	public class Program
	{
		[NotNull]
		public static Encoding shift_jis { get; private set; }

		static Program()
		{
			shift_jis = CodePagesEncodingProvider.Instance.GetEncoding("shift-jis") ?? Encoding.ASCII;
		}

		[STAThread]
		static unsafe void Main()
		{
			Console.OutputEncoding = shift_jis ?? Encoding.ASCII;
			using Logger logger = new("DKSBE.log");
			ConsoleHelper.SetCurrentFont("Kozuka Mincho Pro", 16);
			Stagebase sb = new();
			sb.Read();
			
			int handler = Stagebase.GetHandlerID<Unk_78>();
			Unk_78 first_instance = (Unk_78)(Stagebase.instance.objects[handler][0] ?? new Dummy());
			byte[] data = new byte[]
			{
				0xAA,
				0xAA,
				0xAA,
				0xAA,
				0x00,
				0xAA,
				0xAA,
				0xAA,
				0xAA,
				0x00
			};
			IntPtr ptr = Marshal.AllocHGlobal(data.Length);
			Marshal.Copy(data, 0, ptr, data.Length);
			Unk_78_Data new_instance = PtrController.CreateInstance<Unk_78_Data>((byte*)ptr, 0);
			first_instance.array.Add(new_instance);
			sb.Write();
			sb.Close();
		}
	}
}
