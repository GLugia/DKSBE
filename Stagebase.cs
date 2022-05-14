using DKSBE.data;
using DKSBE.data.types;
using DKSBE.testing;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace DKSBE
{
	
	public unsafe class Stagebase
	{
		[NotNull]
		private static Stagebase instance;
		/// <summary>
		/// The <see cref="FileData"/> for STAGEBASE.DAT.
		/// </summary>
		public FileData stagebase { get; private set; }
		/// <summary>
		/// The <see cref="FileData"/> for each file contained in STAGEBASE.DAT.
		/// </summary>
		private readonly FileData[] stagebase_files;
		/// <summary>
		/// The id of the file currently being read.
		/// </summary>
		public static int current_file_index { get; private set; }
		/// <summary>
		/// The current file being read.
		/// </summary>
		public static FileData current_stagebase_file => instance.stagebase_files[current_file_index];

		/// <summary>
		/// Pointer references for each file in STAGEBASE.DAT.
		/// </summary>
		private readonly SortedDictionary<long, testing.PtrData>[] ptr_data;
		/// <summary>
		/// The pointer references pertaining to the current file being read from STAGEBASE.DAT.
		/// </summary>
		public static SortedDictionary<long, testing.PtrData> current_ptr_data => instance.ptr_data[current_file_index];

		/// <summary>
		/// A list of object instances sorted by their <see cref="ObjectHandler"/> ID.
		/// </summary>
		private readonly Dictionary<byte, List<PtrController>> _objects;
		/// <summary>
		/// A readonly version of <see cref="_objects"/>.
		/// </summary>
		public static IReadOnlyDictionary<byte, List<PtrController>> objects => instance._objects;

		/// <summary>
		/// Handles loading, reading, and writing all data contained in STAGEBASE.DAT
		/// </summary>
		public Stagebase()
		{
			#region Load Stagebase to memory

			Logger.Write("Loading StageBase into memory...");
			byte[] stagebase_bytes = File.ReadAllBytes("STAGEBASE.DAT");
			IntPtr temp = Marshal.AllocHGlobal(stagebase_bytes.Length);
			Marshal.Copy(stagebase_bytes, 0, temp, stagebase_bytes.Length);
			this.stagebase = new((byte*)temp, stagebase_bytes.Length, 0);
			Logger.Write($"Stagebase loaded to {((long)this.stagebase.origin).ToHex()} with a length of {((int)((long)this.stagebase.length - (long)this.stagebase.origin)).ToHex()}\n");

			#endregion



			#region Load file data within Stagebase

			Logger.Write("Loading files...\n");
			// initialize the array of files
			this.stagebase_files = Array.Empty<FileData>();
			// iterate over the data to store each file separately
			for (long offset = this.stagebase.header; offset < this.stagebase.length;)
			{
				// resize the array of files to fit an extra one
				Array.Resize(ref stagebase_files, stagebase_files.Length + 1);
				// read the length of the file
				int length = *((int*)(this.stagebase.origin + offset) + 1);
				this.stagebase_files[^1] = new(this.stagebase.origin + offset, length, *((int*)(this.stagebase.origin + offset) + 2));
				length += 16 - (length % 16);
				offset += length;
				string bin_name = Program.shift_jis?.GetString(this.stagebase_files[^1].origin, 4) ?? Encoding.ASCII.GetString(this.stagebase_files[^1].origin, 4);
				Logger.Debug($"\t{bin_name} loaded to {((long)this.stagebase_files[^1].origin).ToHex()} with a length of {length.ToHex()}\n");
			}
			Logger.Write("Done.\n");

			#endregion

			this._objects = new();
			instance = this;
		}

		public bool Read()
		{
			return false;
		}

		/*
		public bool Read()
		{
			byte last_handler = 0;
			int sub_index = 0;
			for (int i = 0; i < (stagebase_files?.Length ?? 0); i++)
			{
				Logger.Write($"\nLoading {Program.shift_jis?.GetString(BitConverter.GetBytes(Marshal.ReadInt32(stagebase_files?[i]?.origin ?? IntPtr.Zero)))}...\n");

				current_file_index = i;
				// append a new list of pointer references
				Array.Resize(ref ptr_data, (ptr_data?.Length ?? 0) + 1);
				ptr_data[i] = new();

				for (long offset = Marshal.ReadInt32(stagebase_files?[i]?.header ?? IntPtr.Zero); offset < Marshal.ReadInt32(stagebase_files?[i]?.length ?? IntPtr.Zero);)
				{
					// if the offset is not aligned
					if (offset % sizeof(int) != 0)
					{
						// add the amount needed
						offset += sizeof(int) - (offset % sizeof(int));
						_objects_order?.Add(new(last_handler, sub_index++));

						// jump back to the start of the loop
						continue;
					}
					if ((ptr_data[i]?.ContainsKey(offset)).GetValueOrDefault())
					{
						// if the data at this offset is somehow null
						if (ptr_data[i]?[offset] == null)
						{
							Logger.Error($"PtrData was null at {((int)offset).ToHex()}");
							// stop reading
							return false;
						}
						Logger.Write($"Skipping locked offset {((int)offset).ToHex()} -> ");
						// add the size of the data
						offset += ptr_data[i]?[offset]?.size ?? 0;
						Logger.Write($"{((int)offset).ToHex()}\n");
						_objects_order?.Add(new(last_handler, sub_index++));
						// jump back to the beginning of the loop
						continue;
					}
					// read the handler id
					int _handler = MarshalHelper.ReadInt32(stagebase_files?[i]?.origin ?? IntPtr.Zero, ref offset);
					if (_handler <= 0 || _handler > byte.MaxValue)
					{
						Logger.Error($"Invalid Handler ID: {((byte)_handler).ToHex()} at {((int)offset - sizeof(int)).ToHex()}");
						// stop reading
						return false;
					}
					Logger.Write($"Reading {((byte)_handler).ToHex()} at {((int)offset - sizeof(int)).ToHex()}...\n");
					// if the handler is 0 or can't be parsed to a type
					if (table_objects[_handler] == null)
					{
						Logger.Error($"Invalid Handler ID: {((byte)_handler).ToHex()} at {((int)offset - sizeof(int)).ToHex()}");
						// stop reading
						return false;
					}
					// create an instance of the type
					ITableObject? obj = (ITableObject)(Activator.CreateInstance(table_objects[_handler] ?? typeof(Dummy_File)) ?? new Dummy_File());
					// dynamically call the Read function to let the class itself handle its own data
					Dictionary<long, PtrData?>? sections_to_skip = obj?.Read(stagebase_files?[i]?.origin ?? IntPtr.Zero, ref offset);
					Logger.Debug(obj?.ArrayToString(1));
					// iterate all pointer references and add each unique instance to the dictionary
					if (sections_to_skip != null)
					{
						foreach ((long ptr, PtrData? data) in sections_to_skip)
						{
							if (!(ptr_data[i]?.ContainsKey(ptr)).GetValueOrDefault())
							{
								ptr_data[i]?.Add(ptr, data);
							}
						}
					}
					// create a ref of the resulting object and store it in the dictionary
					if (!(_objects?.ContainsKey((byte)_handler)).GetValueOrDefault())
					{
						_objects?.Add((byte)_handler, new());
					}
					_objects?[(byte)_handler]?.Add(obj);
					if (last_handler != _handler)
					{
						sub_index = 0;
						last_handler = (byte)_handler;
					}
					_objects_order?.Add(new((byte)_handler, sub_index++));
					Logger.Write("Done.\n\n");
				}
			}
			return true;
		}
		
		/// <summary>
		/// Writes all data to memory before pushing it to STAGEBASE_NEW.DAT.
		/// </summary>
		/// <returns></returns>
		public bool Write()
		{
			using (BinaryWriter writer = new(File.Create("test.dat", 0x800, FileOptions.RandomAccess), Program.shift_jis ?? Encoding.ASCII, false))
			{
				if (_objects_order != null)
				{
					foreach (KeyValuePair<byte, int>? pair in _objects_order)
					{
						ITableObject? obj = _objects?[pair?.Key ?? 0]?[pair?.Value ?? 0];
						writer.Write(pair?.Key ?? 0);
						//writer.BaseStream.Position += 
					}
				}
			}
			return false;
		}*/

		/// <summary>
		/// Frees and nulls all data.
		/// </summary>
		public void Close()
		{
			for (int i = 0; i < this.stagebase_files.Length; i++)
			{
				Marshal.FreeHGlobal((IntPtr)this.stagebase_files[i].origin);
			}
			Marshal.FreeHGlobal((IntPtr)this.stagebase.origin);
			this._objects.Clear();
			for (int i = 0; i < (ptr_data?.Length ?? 0); i++)
			{
				this.ptr_data[i]?.Clear();
			}
			current_file_index = 0;
		}

		private static readonly Type?[] table_objects = new Type?[0xFF]
		{
			/* 0x00 */ typeof(Dummy_File),
			/* 0x01 */ typeof(testing.controllers.FileLabel),
			/* 0x02 */ null,
			/* 0x03 */ typeof(Data_Section),
			/* 0x04 */ null,
			/* 0x05 */ typeof(Stage),
			/* 0x06 */ null,
			/* 0x07 */ null,
			/* 0x08 */ null,
			/* 0x09 */ null,
			/* 0x0A */ null,
			/* 0x0B */ null,
			/* 0x0C */ null,
			/* 0x0D */ null,
			/* 0x0E */ null,
			/* 0x0F */ null,
			/* 0x10 */ null,
			/* 0x11 */ null,
			/* 0x12 */ null,
			/* 0x13 */ null,
			/* 0x14 */ null,
			/* 0x15 */ null,
			/* 0x16 */ null,
			/* 0x17 */ typeof(Unk_17),
			/* 0x18 */ null,
			/* 0x19 */ null,
			/* 0x1A */ null,
			/* 0x1B */ null,
			/* 0x1C */ typeof(Unk_1C),
			/* 0x1D */ typeof(Unk_1D),
			/* 0x1E */ typeof(Unk_1E),
			/* 0x1F */ null,
			/* 0x20 */ null,
			/* 0x21 */ null,
			/* 0x22 */ null,
			/* 0x23 */ null,
			/* 0x24 */ null,
			/* 0x25 */ null,
			/* 0x26 */ null,
			/* 0x27 */ null,
			/* 0x28 */ null,
			/* 0x29 */ typeof(Job_Model_4_7),
			/* 0x2A */ typeof(Unk_2A),
			/* 0x2B */ typeof(Unk_2B),
			/* 0x2C */ typeof(Unk_2C),
			/* 0x2D */ typeof(Unk_2D),
			/* 0x2E */ typeof(Job_Money),
			/* 0x2F */ typeof(Unk_2F),
			/* 0x30 */ null,
			/* 0x31 */ null,
			/* 0x32 */ null,
			/* 0x33 */ null,
			/* 0x34 */ null,
			/* 0x35 */ null,
			/* 0x36 */ null,
			/* 0x37 */ typeof(Location),
			/* 0x38 */ typeof(Job_38),
			/* 0x39 */ typeof(Job_39),
			/* 0x3A */ typeof(Npc_Names),
			/* 0x3B */ typeof(Job_Mastery),
			/* 0x3C */ typeof(Job_Skills),
			/* 0x3D */ typeof(Job_Descriptions),
			/* 0x3E */ typeof(Job_3E),
			/* 0x3F */ typeof(Unk_3F),
			/* 0x40 */ typeof(Job_Stats),
			/* 0x41 */ typeof(Job_Model),
			/* 0x42 */ typeof(Job),
			/* 0x43 */ typeof(Job_43),
			/* 0x44 */ typeof(Job_Bag),
			/* 0x45 */ typeof(Job_Model_0_3),
			/* 0x46 */ typeof(Unk_46),
			/* 0x47 */ typeof(Unk_47),
			/* 0x48 */ typeof(Unk_48),
			/* 0x49 */ typeof(Unk_49),
			/* 0x4A */ typeof(Npc_Text_List),
			/* 0x4B */ typeof(Npc_Text),
			/* 0x4C */ typeof(Unk_4C),
			/* 0x4D */ typeof(Unk_4D),
			/* 0x4E */ typeof(Unk_4E),
			/* 0x4F */ typeof(Unk_4F),
			/* 0x50 */ typeof(Npc_Enemy),
			/* 0x51 */ typeof(Npc_Enemy_Model),
			/* 0x52 */ typeof(Unk_52),
			/* 0x53 */ typeof(Npc_Enemy_Drop_Table),
			/* 0x54 */ typeof(Unk_54),
			/* 0x55 */ typeof(Npc),
			/* 0x56 */ typeof(Npc_Model),
			/* 0x57 */ typeof(Npc_Model_0),
			/* 0x58 */ typeof(Weapon),
			/* 0x59 */ typeof(Weapon_Model),
			/* 0x5A */ typeof(Weapon_Descriptions),
			/* 0x5B */ typeof(Unk_5B),
			/* 0x5C */ typeof(Unk_5C),
			/* 0x5D */ typeof(Unk_5D),
			/* 0x5E */ typeof(Shield),
			/* 0x5F */ typeof(Shield_Model),
			/* 0x60 */ typeof(Shield_Descriptions),
			/* 0x61 */ typeof(Npc_Enemy_Model_0),
			/* 0x62 */ typeof(Unk_62),
			/* 0x63 */ typeof(Weapon_Bonus_Descriptions),
			/* 0x64 */ typeof(Accessory),
			/* 0x65 */ typeof(Accessory_Descriptions),
			/* 0x66 */ typeof(Unk_66),
			/* 0x67 */ typeof(Temple),
			/* 0x68 */ typeof(Unk_68),
			/* 0x69 */ typeof(Item),
			/* 0x6A */ typeof(Item_Descriptions),
			/* 0x6B */ typeof(Item_Func),
			/* 0x6C */ typeof(Item_Gift),
			/* 0x6D */ typeof(Town),
			/* 0x6E */ typeof(Unk_6E),
			/* 0x6F */ typeof(Unk_6F),
			/* 0x70 */ typeof(Magic_Off),
			/* 0x71 */ typeof(Magic_Off_Descriptions),
			/* 0x72 */ typeof(Magic_Def),
			/* 0x73 */ typeof(Magic_Def_Descriptions),
			/* 0x74 */ typeof(Magic_Item),
			/* 0x75 */ typeof(Magic_Item_Descriptions),
			/* 0x76 */ typeof(Unk_76),
			/* 0x77 */ typeof(Unk_77),
			/* 0x78 */ typeof(Unk_78),
			/* 0x79 */ typeof(Unk_79),
			/* 0x7A */ typeof(Ability_Battle),
			/* 0x7B */ typeof(Ability_Field),
			/* 0x7C */ typeof(Npc_Enemy_Descriptions),
			/* 0x7D */ typeof(Ability_Battle_Descriptions),
			/* 0x7E */ typeof(Ability_Field_Descriptions),
			/* 0x7F */ typeof(Unk_7F),
			/* 0x80 */ null,
			/* 0x81 */ typeof(Status_Permanent),
			/* 0x82 */ typeof(Status_Battle),
			/* 0x83 */ typeof(Status_Field),
			/* 0x84 */ typeof(Unk_84),
			/* 0x85 */ typeof(Unk_85),
			/* 0x86 */ typeof(Space_Effect),
			/* 0x87 */ typeof(Space),
			/* 0x88 */ typeof(Space_Descriptions),
			/* 0x89 */ typeof(Item_Type),
			/* 0x8A */ typeof(Magic_Type_Battle),
			/* 0x8B */ typeof(Magic_Type_Field),
			/* 0x8C */ typeof(Unk_8C),
			/* 0x8D */ null,
			/* 0x8E */ typeof(Status_8E),
			/* 0x8F */ typeof(Job_8F),
			/* 0x90 */ null,
			/* 0x91 */ null,
			/* 0x92 */ null,
			/* 0x93 */ typeof(Unk_93),
			/* 0x94 */ typeof(Unk_94),
			/* 0x95 */ typeof(Hair_Descriptions),
			/* 0x96 */ typeof(Hair),
			/* 0x97 */ typeof(Hair_Model),
			/* 0x98 */ null,
			/* 0x99 */ null,
			/* 0x9A */ typeof(Status_9A),
			/* 0x9B */ typeof(Status_9B),
			/* 0x9C */ typeof(Unk_9C),
			/* 0x9D */ typeof(Unk_9D),
			/* 0x9E */ typeof(Unk_9E),
			/* 0x9F */ typeof(Magic_9F),
			/* 0xA0 */ typeof(Unk_A0),
			/* 0xA1 */ typeof(Unk_A1),
			/* 0xA2 */ typeof(Unk_A2),
			/* 0xA3 */ typeof(Unk_A3),
			/* 0xA4 */ typeof(Unk_A4),
			/* 0xA5 */ typeof(Unk_A5),
			/* 0xA6 */ null,
			/* 0xA7 */ null,
			/* 0xA8 */ null,
			/* 0xA9 */ null,
			/* 0xAA */ null,
			/* 0xAB */ null,
			/* 0xAC */ typeof(Unk_AC),
			/* 0xAD */ typeof(Unk_AD),
			/* 0xAE */ typeof(Unk_AE),
			/* 0xAF */ typeof(Unk_AF),
			/* 0xB0 */ typeof(Unk_B0), // equation data
			/* 0xB1 */ typeof(Unk_B1), // equation data
			/* 0xB2 */ typeof(Unk_B2), // equation data
			/* 0xB3 */ typeof(Unk_B3), // equation data
			/* 0xB4 */ typeof(Unk_B4), // equation data
			/* 0xB5 */ typeof(Unk_B5), // equation data
			/* 0xB6 */ typeof(Unk_B6), // equation data
			/* 0xB7 */ typeof(Unk_B7), // equation data
			/* 0xB8 */ typeof(Unk_B8), // equation data
			/* 0xB9 */ typeof(Unk_B9), // equation data
			/* 0xBA */ typeof(Unk_BA), // equation data
			/* 0xBB */ typeof(Unk_BB), // equation data
			/* 0xBC */ typeof(Unk_BC), // equation data
			/* 0xBD */ typeof(Unk_BD),
			/* 0xBE */ typeof(Unk_BE),
			/* 0xBF */ typeof(Unk_BF),
			/* 0xC0 */ typeof(Unk_C0),
			/* 0xC1 */ null,
			/* 0xC2 */ null,
			/* 0xC3 */ null,
			/* 0xC4 */ null,
			/* 0xC5 */ null,
			/* 0xC6 */ null,
			/* 0xC7 */ null,
			/* 0xC8 */ null,
			/* 0xC9 */ null,
			/* 0xCA */ null,
			/* 0xCB */ null,
			/* 0xCC */ null,
			/* 0xCD */ null,
			/* 0xCE */ null,
			/* 0xCF */ null,
			/* 0xD0 */ typeof(Item_D0),
			/* 0xD1 */ typeof(Magic_D1),
			/* 0xD2 */ typeof(Unk_D2),
			/* 0xD3 */ typeof(Unk_D3),
			/* 0xD4 */ typeof(Magic_D4),
			/* 0xD5 */ typeof(Magic_D5),
			/* 0xD6 */ typeof(Job_D6),
			/* 0xD7 */ typeof(Item_D7),
			/* 0xD8 */ typeof(Ability_Darkling_Descriptions),
			/* 0xD9 */ typeof(Ability_Darkling),
			/* 0xDA */ typeof(Unk_DA),
			/* 0xDB */ typeof(Unk_DB),
			/* 0xDC */ null,
			/* 0xDD */ null,
			/* 0xDE */ null,
			/* 0xDF */ typeof(Unk_DF),
			/* 0xE0 */ typeof(Unk_E0),
			/* 0xE1 */ typeof(Unk_E1),
			/* 0xE2 */ typeof(Unk_E2),
			/* 0xE3 */ null,
			/* 0xE4 */ null,
			/* 0xE5 */ null,
			/* 0xE6 */ null,
			/* 0xE7 */ null,
			/* 0xE8 */ null,
			/* 0xE9 */ null,
			/* 0xEA */ null,
			/* 0xEB */ null,
			/* 0xEC */ null,
			/* 0xED */ null,
			/* 0xEE */ null,
			/* 0xEF */ null,
			/* 0xF0 */ null,
			/* 0xF1 */ null,
			/* 0xF2 */ null,
			/* 0xF3 */ null,
			/* 0xF4 */ null,
			/* 0xF5 */ null,
			/* 0xF6 */ null,
			/* 0xF7 */ null,
			/* 0xF8 */ null,
			/* 0xF9 */ null,
			/* 0xFA */ null,
			/* 0xFB */ null,
			/* 0xFC */ null,
			/* 0xFD */ null,
			/* 0xFE */ null
		};
	}
}
