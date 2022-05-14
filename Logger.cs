using System.Linq;
using System.Text;

namespace DKSBE
{
	public sealed class Logger : IDisposable
	{
		private StreamWriter? stream;
		private static Logger? instance;
		public long length => stream?.BaseStream?.Length ?? 0;


		internal Logger(string file)
		{
			this.stream = new(file, Program.shift_jis ?? Encoding.ASCII, new FileStreamOptions()
			{
				Access = FileAccess.ReadWrite,
				BufferSize = 0x800,
				Mode = FileMode.Create,
				Options = FileOptions.SequentialScan,
				Share = FileShare.ReadWrite
			})
			{
				AutoFlush = true,
				NewLine = "\n"
			};
			instance = this;
		}

		public void Dispose()
		{
			this.stream?.Flush();
			this.stream?.Dispose();
		}

		public void Close()
		{
			this.stream?.Flush();
			this.stream?.Close();
			this.stream = null;
		}

		public static void Write(object? o, ConsoleColor color = ConsoleColor.White)
		{
			instance?.stream?.Write(o);
			Console.ForegroundColor = color;
			Console.Out.Write(o);
			Console.ResetColor();
		}

		public static void Debug(object? o)
		{
#if DEBUG
			WriteLine(o, ConsoleColor.Cyan);
#endif
		}

		public static void Warn(object? o)
		{
			WriteLine(o, ConsoleColor.Yellow);
		}

		public static void Error(object? o)
		{
			WriteLine($"\n\n\t{o}\n\n", ConsoleColor.Red);
		}

		public static void WriteLine(object? o, ConsoleColor color = ConsoleColor.White)
		{
			instance?.stream?.WriteLine(o);
			Console.ForegroundColor = color;
			Console.Out.WriteLine(o);
			Console.ResetColor();
		}
	}
}
