using System.Diagnostics.CodeAnalysis;
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
		static void Main()
		{
			Console.OutputEncoding = shift_jis ?? Encoding.ASCII;
			using Logger logger = new("DKSBE.log");
			ConsoleHelper.SetCurrentFont("Kozuka Mincho Pro", 16);
			Stagebase sb = new();
			//sb.Read();
			//sb.Write();
			sb.Close();
		}
	}
}
