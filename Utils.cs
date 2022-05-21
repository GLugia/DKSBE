using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DKSBE
{
	public static class Utils
	{
		public static string? ToHex(this object? obj, bool add_0x = true)
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
			else if (type.IsArray || type.Name.Contains("List") || type.Name.Contains("Dictionary"))
			{
				return ArrayString(o, ref indent);
			}
			else if (o is data.StringPtr ptr)
			{
				return ptr.ToString();
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

		private static unsafe string? StructString(object o, ref int indentation)
		{
			string indent = BuildIndent(indentation);
			string ret = $"{o.GetType().Name}\n{indent}{{\n";
			indentation++;
			indent = BuildIndent(indentation);
			Type type = o.GetType();
			TypedReference tref = __makeref(o);
			List<string> list = new();
			FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			for (int i = 0; i < fields.Length; i++)
			{
				list.Add($"{indent}{fields[i].Name} = {HandleArrayString(fields[i].GetValueDirect(tref), ref indentation)}");
			}
			PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			object? value;
			for (int i = 0; i < props.Length; i++)
			{
				if (props[i].Name == "Item")
				{
					continue;
				}
				MethodInfo? get = props[i].GetGetMethod();
				ParameterInfo?[]? parameters = get?.GetParameters();
				value = props[i];
				if ((parameters?.Length).GetValueOrDefault() == 0)
				{
					Type underlying_type = props[i].PropertyType.GetElementType() ?? props[i].PropertyType;
					value = get?.Invoke(o, null);
					if (value is Pointer pointer)
					{
						void* ptr = Pointer.Unbox(value);
						value = underlying_type.Name switch
						{
							"Char" => MarshalHelper.ReadString((char*)ptr),
							"Byte" => *(byte*)ptr,
							"SByte" => *(sbyte*)ptr,
							"UInt16" => *(ushort*)ptr,
							"Int16" => *(short*)ptr,
							"UInt32" => *(uint*)ptr,
							"Int32" => *(int*)ptr,
							"UInt64" => *(ulong*)ptr,
							"Int64" => *(long*)ptr,
							"Single" => *(float*)ptr,
							"Double" => *(double*)ptr,
							_ => Marshal.PtrToStructure((IntPtr)ptr, underlying_type)
						};
					}
					else if (value is data.StringPtr ptr)
					{
						value = ptr.ToString();
					}
				}
				list.Add($"{indent}{props[i].Name} => {HandleArrayString(value, ref indentation)}");
			}
			ret += string.Join(",\n", list);
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
				case "List_Ptr`1":
					{
						dynamic list = o;
						Type[] generics = o.GetType().GetGenericArguments();
						ret += $"<{generics[0].Name}>[{list.count}]\n{indent}{{\n";
						indentation++;
						indent = BuildIndent(indentation);
						for (int i = 0; i < list.count; i++)
						{
							ret += $"{indent}{HandleArrayString(list[i], ref indentation)}";
							if (i != list.count - 1)
							{
								ret += $",\n";
							}
						}
						break;
					}
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
				_ => o.ToHex(),
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

		public static unsafe long SizeOf(params object[] objs)
		{
			long ret = 0;
			foreach (object obj in objs)
			{
				ret += obj.SizeOf();
			}
			return ret;
		}

		public static unsafe long SizeOf(this object? obj)
		{
			if (obj == null)
			{
				return 0;
			}
			Type type = obj.GetType();
			if (type.IsPrimitive)
			{
				return PrimitiveSize(type);
			}
			else if (type.IsArray)
			{
				return ArraySize(obj);
			}
			else if (obj is Pointer pointer)
			{
				Type underlying_type = type.GetElementType() ?? type;
				long size = 0;
				switch (underlying_type.Name)
				{
					case "Char":
						MarshalHelper.ReadValueTerminatedArray<byte>((byte*)Pointer.Unbox(pointer), ref size, 0);
						break;
					default: size = PrimitiveSize(underlying_type); break;
				};
				return size;
			}
			else if (obj is data.StringPtr ptr)
			{
				return ptr.length;
			}
			else if (type.IsClass && !type.IsAbstract)
			{
				return StructSize(obj);
			}
			else
			{
				return Marshal.SizeOf(obj);
			}
		}

		private static long ArraySize(object obj)
		{
			Array array = (Array)obj;
			long size = 0;
			for (int i = 0; i < array.Length; i++)
			{
				size += SizeOf(array.GetValue(i));
			}
			return size;
		}

		private static unsafe long StructSize(object obj)
		{
			Type type = obj.GetType();
			TypedReference tref = __makeref(obj);
			FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			long size = 0;
			foreach (FieldInfo field in fields)
			{
				size += SizeOf(field.GetValueDirect(tref));
			}
			foreach (PropertyInfo prop in props)
			{
				if (prop.PropertyType == typeof(data.StringPtr))
				{
					size += ((data.StringPtr)(prop.GetValue(obj) ?? 0)).length;
					continue;
				}
				MethodInfo? method = prop.GetGetMethod();
				if (method == null)
				{
					continue;
				}
				object? value = method.Invoke(obj, null);
				if (value is Pointer pointer)
				{
					Type underlying_type = prop.PropertyType.GetElementType() ?? prop.PropertyType;
					long _size = 0;
					switch (underlying_type.Name)
					{
						case "Char":
							MarshalHelper.ReadValueTerminatedArray<byte>((byte*)Pointer.Unbox(pointer), ref _size, 0);
							break;
						default: _size = PrimitiveSize(underlying_type); break;
					};
					size += _size;
				}
				else
				{
					size += SizeOf(value);
				}
			}
			return size;
		}

		private static long PrimitiveSize(Type type) => type.Name switch
			{
				"Char" => sizeof(char),
				"Byte" => sizeof(byte),
				"SByte" => sizeof(sbyte),
				"UInt16" => sizeof(ushort),
				"Int16" => sizeof(short),
				"UInt32" => sizeof(uint),
				"Int32" => sizeof(int),
				"UInt64" => sizeof(ulong),
				"Int64" => sizeof(long),
				"Single" => sizeof(float),
				"Double" => sizeof(double),
				_ => 0
			};

		public static int Align(this int value, int alignment = sizeof(int))
		{
			return value + (alignment - (value % alignment));
		}
	}
}
