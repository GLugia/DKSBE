using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DKSBE
{
	public static class Utils
	{
		public static string? ToHex(this object obj, bool add_0x = true)
		{
			if (obj is null)
			{
				return "NULL";
			}
			Type type = obj.GetType();
			if (!type.IsValueType || type.IsArray)
			{
				return ArrayToString(obj);
			}
			int size = obj switch
			{
				char or byte or sbyte => 1,
				ushort or short => 2,
				uint or int or float or bool => 4,
				ulong or long or double => 8,
				_ => 0
			};
			if (size == 0)
			{
				return obj?.ToString();
			}
			switch (obj)
			{
				case float f: return $"{(add_0x ? "0x" : "")}{BitConverter.ToString(BitConverter.GetBytes(f).Reverse().ToArray()).Replace("-", "")}";
				case double d: return $"{(add_0x ? "0x" : "")}{BitConverter.ToString(BitConverter.GetBytes(d).Reverse().ToArray()).Replace("-", "")}";
				default:
					{
						byte[] bytes = new byte[size];
						for (int i = 0; i < size; i++)
						{
							bytes[i] = (byte)(((dynamic)obj >> (8 * i)) & 0xFF);
						}
						return $"{(add_0x ? "0x" : "")}{BitConverter.ToString(bytes.Reverse().ToArray()).Replace("-", "")}";
					}
			}
		}

		public static string? ArrayToString(this object? o)
		{
			int indent = 0;
			return HandleArrayString(o, ref indent);
		}

		public static string? ArrayToString(this object? o, int indentation)
		{
			string indent = BuildIndent(indentation);
			return $"{indent}{HandleArrayString(o, ref indentation)}";
		}

		private static string? HandleArrayString(object? o, ref int indent)
		{
			if (o is null)
			{
				return "NULL";
			}
			Type type = o.GetType();
			if (type.IsPrimitive || type.Name.Equals("String"))
			{
				return PrimitiveString(o);
			}
			else if (type.IsArray || type.Name.Contains("List`") || type.Name.Contains("Dictionary`"))
			{
				return ArrayString(o, ref indent);
			}
			else if ((type.IsClass && !type.IsAbstract) || (type.IsValueType && !type.IsEnum))
			{
				return StructString(o, ref indent);
			}
			else
			{
				return o?.ToString();
			}
		}

		private static string? StructString(object o, ref int indentation)
		{
			string indent = BuildIndent(indentation);
			string ret = $"{o.GetType().Name}\n{indent}{{\n";
			indentation++;
			indent = BuildIndent(indentation);
			Type type = o.GetType();
			TypedReference tref = __makeref(o);
			FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
			for (int i = 0; i < fields.Length; i++)
			{
				ret += $"{indent}{fields[i].Name} = {HandleArrayString(fields[i].GetValueDirect(tref), ref indentation)}";
				if (i != fields.Length - 1)
				{
					ret += $",\n";
				}
			}
			indentation--;
			indent = BuildIndent(indentation);
			ret += $"\n{indent}}}";
			return ret;
		}

		private static string? ArrayString(object o, ref int indentation)
		{
			string indent = BuildIndent(indentation);
			string ret = $"{o.GetType().GetElementType()?.Name ?? o.GetType().Name}";
			int grave_index = ret.LastIndexOf('`');
			if (grave_index != -1)
			{
				ret = ret[..grave_index];
			}
			switch (o.GetType().Name)
			{
				case "List`1":
					{
						dynamic list = o;
						Type[] generics = o.GetType().GetGenericArguments();
						ret += $"<{generics[0].Name}>[{list.Count}]\n{indent}{{\n";
						indentation++;
						indent = BuildIndent(indentation);
						for (int i = 0; i < list.Count; i++)
						{
							ret += $"{indent}{HandleArrayString(Enumerable.ElementAt(list, i), ref indentation)}";
							if (i != list.Count - 1)
							{
								ret += $",\n";
							}
						}
						break;
					}
				case "SortedDictionary`2":
				case "Dictionary`2":
					{
						dynamic dict = o;
						Type[] generics = o.GetType().GetGenericArguments();
						ret += $"<{generics[0].Name}, {generics[1].Name}>[{dict.Count}]\n{indent}{{\n";
						indentation++;
						indent = BuildIndent(indentation);
						for (int i = 0; i < dict.Count; i++)
						{
							ret += $"{indent}{HandleArrayString(Enumerable.ElementAt(dict.Keys, i), ref indentation)}->{HandleArrayString(Enumerable.ElementAt(dict.Values, i), ref indentation)}";
							if (i != dict.Count - 1)
							{
								ret += $",\n";
							}
						}
						break;
					}
				default:
					{
						Array? array = o as Array;
						ret += $"[{array?.Length ?? 0}]\n{indent}{{\n";
						indentation++;
						indent = BuildIndent(indentation);
						for (int i = 0; i < (array?.Length ?? 0); i++)
						{
							ret += $"{indent}{HandleArrayString(array?.GetValue(i), ref indentation)}";
							if (i != (array?.Length ?? 0) - 1)
							{
								ret += $",\n";
							}
						}
						break;
					}
			}
			indentation--;
			indent = BuildIndent(indentation);
			ret += $"\n{indent}}}";
			return ret;
		}

		private static string? PrimitiveString(object o)
		{
			return o switch
			{
				bool val => $"{(val ? "true" : "false")}",
				float val => $"{val}f",
				double val => $"{val}d",
				char val => $"\'{(val == 0 ? "\\0" : val)}\'",
				string val => $"\"{val.Replace("\0", "*")}\"",
				_ => o.ToString(),
			};
		}

		private static string BuildIndent(int indent)
		{
			string ret = "";
			for (int i = 0; i < indent; i++)
			{
				ret += "    ";
			}
			return ret;
		}

		public static bool EnumToType<T>(in T value, out Type? o) where T : struct
		{
			// store the name of the value
			string name = $"{value}";
			string real_name = "";
			string[] sections = name.Split('_');
			for (int i = 0; i < sections.Length; i++)
			{
				if (sections[i].Length == 0)
				{
					real_name += "_";
					continue;
				}
				else if (byte.TryParse(sections[i], NumberStyles.HexNumber, null, out _))
				{
					real_name += sections[i];
				}
				else
				{
					real_name += $"{char.ToUpperInvariant(sections[i][0])}{sections[i][1..].ToLowerInvariant()}";
				}

				if (i + 1 < sections.Length)
				{
					real_name += "_";
				}
			}
			o = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(a => a.IsClass && !a.IsAbstract && a.Name == real_name);
			return o != null;
		}
	}
}
