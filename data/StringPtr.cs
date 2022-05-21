using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace DKSBE.data
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public unsafe struct StringPtr
	{
		internal byte* address;

		public StringPtr(byte* address)
		{
			this.address = address;
		}

		public StringPtr(char* address)
		{
			this.address = (byte*)address;
		}

		public StringPtr(int* address)
		{
			this.address = (byte*)address;
		}

		public StringPtr(long offset)
		{
			this.address = Stagebase.current_stagebase_file.origin + offset;
		}

		public StringPtr(string value, int alignment = sizeof(int))
		{
			byte[] bytes = Program.shift_jis.GetBytes(value);
			if (bytes[^1] != 0)
			{
				Array.Resize(ref bytes, bytes.Length + (alignment - (bytes.Length % alignment)));
			}
			IntPtr temp = Marshal.AllocHGlobal(bytes.Length);
			Marshal.Copy(bytes, 0, temp, bytes.Length);
			this.address = (byte*)temp;
		}

		public static implicit operator sbyte*(StringPtr ptr) => (sbyte*)ptr.address;
		public static implicit operator StringPtr(sbyte* ptr) => *(StringPtr*)&ptr;

		public static implicit operator byte*(StringPtr ptr) => ptr.address;
		public static implicit operator StringPtr(byte* ptr) => *(StringPtr*)&ptr;

		public static implicit operator char*(StringPtr ptr) => (char*)ptr.address;
		public static implicit operator StringPtr(char* ptr) => *(StringPtr*)&ptr;

		public static implicit operator string(StringPtr ptr) => new((sbyte*)ptr.address, 0, ptr.length, Program.shift_jis);
		public static implicit operator long(StringPtr ptr) => *ptr.address;

		public static StringPtr operator +(StringPtr ptr, int amount)
		{
			ptr.address += amount;
			return ptr;
		}

		public static bool operator ==(StringPtr a, StringPtr b)
		{
			return a.Equals(b, false);
		}

		public static bool operator !=(StringPtr a, StringPtr b)
		{
			return !a.Equals(b, false);
		}

		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			if (obj is StringPtr ptr)
			{
				return this.Equals(ptr, false);
			}

			if (obj is string str)
			{
				return this.Equals(str, false);
			}

			return false;
		}

		public static bool Equals(StringPtr a, StringPtr b, bool ignore_case)
		{
			return a.Equals(b, ignore_case);
		}

		public static bool Equals(StringPtr a, string b, bool ignore_case)
		{
			return a.Equals(b, ignore_case);
		}

		public bool Equals(StringPtr ptr, bool ignore_case)
		{
			byte* str_ptr_a = this.address;
			byte* str_ptr_b = ptr.address;
			byte a, b;

			do
			{
				a = *str_ptr_a++;
				b = *str_ptr_b++;
				if (a != b)
				{
					if (ignore_case
					&& (a >= 0x41 && a <= 0x5A
							&& a + 0x20 == b)
						|| (a >= 0x61 && a <= 0x7A
							&& a - 0x20 == b))
					{
						continue;
					}

					return false;
				}
			}
			while (a != 0);
			return true;
		}

		public bool Equals(string str, bool ignore_case)
		{
			byte* str_ptr_a = this.address;
			char* str_ptr_b;
			byte a, b;

			fixed (char* c = str)
			{
				str_ptr_b = c;
				do
				{
					a = *str_ptr_a++;
					b = (byte)*str_ptr_b++;
					if (a != b)
					{
						if (ignore_case
						&& (a >= 0x41 && a <= 0x5A
								&& a + 0x20 == b)
							|| (a >= 0x61 && a <= 0x7A
								&& a - 0x20 == b))
						{
							continue;
						}

						return false;
					}
				}
				while (a != 0);
			}
			return true;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(this.address->GetHashCode(), base.GetHashCode());
		}

		public override string ToString()
		{
			return Program.shift_jis.GetString(this.address, this.length);
		}

		public byte this[int index]
		{
			get => this.address[index];
			set => this.address[index] = value;
		}

		public int length
		{
			get
			{
				int len = 0;
				byte* ptr = this.address;
				while (*ptr++ != 0)
				{
					len++;
				}
				return len.Align();
			}
		}
	}
}
