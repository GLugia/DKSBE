using DKSBE.data;
using DKSBE.data.controllers;

using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace DKSBE
{
	
	public unsafe class Stagebase
	{
		public static Stagebase instance { get; private set; } = new Stagebase(0);
		/// <summary>
		/// The <see cref="FilePtr"/> for each file contained in STAGEBASE.DAT.
		/// </summary>
		private FilePtr[] stagebase_files;
		/// <summary>
		/// The id of the file currently being read.
		/// </summary>
		private static int current_file_index;
		/// <summary>
		/// The current file being read.
		/// </summary>
		public static FilePtr current_stagebase_file => instance?.stagebase_files[current_file_index] ?? new FilePtr();

		/// <summary>
		/// A list of object instances sorted by their <see cref="ObjectHandler"/> ID.
		/// </summary>
		private readonly Dictionary<int, List<PtrController>> _objects;
		/// <summary>
		/// A readonly version of <see cref="_objects"/>.
		/// </summary>
		public IReadOnlyDictionary<int, List<PtrController>> objects => this._objects;

		private readonly Dictionary<long, PtrData> ptr_data;

		/// <summary>
		/// This is exists so the <see cref="instance"/> property is never null. Meaning that little green squiggly goes away.
		/// </summary>
		/// <param name="_">lol</param>
		private Stagebase(byte _ = 0)
		{
			this._objects = new();
			this.stagebase_files = Array.Empty<FilePtr>();
			this.ptr_data = new();
		}

		/// <summary>
		/// Handles loading, reading, and writing all data contained in STAGEBASE.DAT
		/// </summary>
		public Stagebase()
		{
			#region Load Stagebase to memory

			Logger.Write("Loading StageBase into memory...");
			// read all bytes in Stagebase
			byte[] stagebase_bytes = File.ReadAllBytes("STAGEBASE.DAT");
			// initialize the array of files
			this.stagebase_files = Array.Empty<FilePtr>();
			int length;
			// iterate over the bytes given
			for (int offset = 0; offset < stagebase_bytes.Length; offset += 16 - (offset % 16))
			{
				// assume there is another file to append
				Array.Resize(ref this.stagebase_files, this.stagebase_files.Length + 1);
				// read the length of the next file
				length = BitConverter.ToInt32(stagebase_bytes, offset + sizeof(int)) + sizeof(int);
				// allocate memory for it
				IntPtr file_ptr = Marshal.AllocHGlobal(length);
				// copy it to memory
				Marshal.Copy(stagebase_bytes, offset, file_ptr, length);
				// set the file pointer data to the newly allocated file
				this.stagebase_files[^1] = new((byte*)file_ptr.ToPointer());
				// increment by the real file length (4 more than it says)
				offset += this.stagebase_files[^1].length;
				// debug printing
				string bin_name = Program.shift_jis?.GetString(this.stagebase_files[^1].origin, 4) ?? Encoding.ASCII.GetString(this.stagebase_files[^1].origin, 4);
				Logger.Debug($"\t{bin_name} loaded to {((long)this.stagebase_files[^1].origin).ToHex()} with a length of {this.stagebase_files[^1].length.ToHex()}\n");
			}
			Logger.Write("Done.\n");

			#endregion

			this._objects = new();
			this.ptr_data = new();
			instance = this;
		}

		public bool Read()
		{
			int handler;
			long offset;
			Type? handler_type;
			for (int i = 0; i < this.stagebase_files.Length; i++)
			{
				current_file_index = i;
				for (offset = this.stagebase_files[i].header; offset < this.stagebase_files[i].length;)
				{
					// if the current offset has been read before, skip over it and return to the start of the loop
					if (this.ptr_data.ContainsKey(offset))
					{
						Logger.Debug($"Skipping locked offset: {((int)offset).ToHex()} -> {((int)(offset + this.ptr_data[offset].length)).ToHex()}\n");
						offset += this.ptr_data[offset].length;
						continue;
					}

					// if the offset is not aligned to int, realign it and return to the start of the loop
					if (offset % sizeof(int) != 0)
					{
						offset += sizeof(int) - (offset % sizeof(int));
						continue;
					}

					// read the handler id
					handler = MarshalHelper.ReadInt32(this.stagebase_files[i].origin, ref offset);
					handler_type = GetHandlerType(handler);
					// if the handler is invalid or there is not a controller for it, stop reading
					if (handler <= 0 || handler > byte.MaxValue || handler_type == null)
					{
						Logger.Error($"Invalid handler at {(offset - sizeof(int)).ToHex()}: {((byte)handler).ToHex()}");
						return false;
					}
					// create the controller instance
					PtrController instance = PtrController.CreateInstance(handler_type, this.stagebase_files[i].origin, offset - sizeof(int));
					// allow the instance to lock any offsets that are necessary to be skipped
					instance.LockOffsets(this.ptr_data);

					Logger.Debug($"{((int)(instance - this.stagebase_files[i].origin)).ToHex()}->{instance.ArrayToString()}\n");

					// increment the offset by the size of the object
					offset += instance.size;

					// if the handler was not read before, initialize the respective list
					if (!this._objects.ContainsKey(handler))
					{
						this._objects.Add(handler, new());
					}

					// add the controller to the list
					this._objects[handler].Add(instance);
				}
			}
			return true;
		}

		#region Writers

		public void Write(bool* target, bool value)
		{
			int real_value = Convert.ToInt32(value);
			this.Write((byte*)target, sizeof(int), (byte*)&real_value, sizeof(int));
		}

		public void Write(byte* target, byte value)
		{
			this.Write(target, sizeof(byte), &value, sizeof(byte));
		}

		public void Write(byte* target, long length, byte[] values)
		{
			IntPtr temp = Marshal.AllocHGlobal(values.Length);
			Marshal.Copy(values, 0, temp, values.Length);
			this.Write(target, length, (byte*)temp, values.Length);
		}

		public void Write(sbyte* target, sbyte value)
		{
			this.Write((byte*)target, sizeof(sbyte), (byte*)&value, sizeof(sbyte));
		}

		public void Write(ushort* target, ushort value)
		{
			this.Write((byte*)target, sizeof(ushort), (byte*)&value, sizeof(ushort));
		}

		public void Write(short* target, short value)
		{
			this.Write((byte*)target, sizeof(short), (byte*)&value, sizeof(short));
		}

		public void Write(uint* target, uint value)
		{
			this.Write((byte*)target, sizeof(uint), (byte*)&value, sizeof(uint));
		}

		public void Write(int* target, int value)
		{
			this.Write((byte*)target, sizeof(int), (byte*)&value, sizeof(int));
		}

		public void Write(ulong* target, ulong value)
		{
			this.Write((byte*)target, sizeof(ulong), (byte*)&value, sizeof(ulong));
		}

		public void Write(long* target, long value)
		{
			this.Write((byte*)target, sizeof(long), (byte*)&value, sizeof(long));
		}

		public void Write(float* target, float value)
		{
			this.Write((byte*)target, sizeof(float), (byte*)&value, sizeof(float));
		}

		public void Write(double* target, double value)
		{
			this.Write((byte*)target, sizeof(double), (byte*)&value, sizeof(double));
		}

		public void Write(StringPtr target, string value)
		{
			StringPtr ptr = new(value);
			this.Write(target, target.length, ptr, ptr.length);
		}

		private void Write(byte* target, long target_length, byte* value, long value_length)
		{
			long difference = value_length - target_length;
			PtrController controller;
			byte* old_ptr;
			byte* old_target = target;
			for (int i = 0; i < this.stagebase_files.Length; i++)
			{
				if (target > this.stagebase_files[i].origin && target < this.stagebase_files[i].origin + this.stagebase_files[i].length)
				{
					if (difference == 0)
					{
						for (byte* offset = target; offset < target + target_length; offset++)
						{
							*offset = *value++;
						}
						return;
					}
					else
					{
						old_ptr = this.stagebase_files[i].origin;
						if (difference > 0)
						{
							IntPtr file_pointer = Marshal.ReAllocHGlobal((IntPtr)this.stagebase_files[i].origin, (IntPtr)(this.stagebase_files[i].length + difference));
							this.stagebase_files[i] = new((byte*)file_pointer.ToPointer());
							target += this.stagebase_files[i].origin - old_ptr;
							for (byte* offset = this.stagebase_files[i].origin + this.stagebase_files[i].length - 1; offset >= target + target_length; offset--)
							{
								Marshal.WriteByte((IntPtr)(offset + difference), *offset);
							}
							for (long offset = 0; offset < value_length; offset++)
							{
								Marshal.WriteByte((IntPtr)(target + offset), *(value + offset));
							}
						}
						else
						{
							for (byte* offset = target + target_length + 1; offset < this.stagebase_files[i].origin + this.stagebase_files[i].length; offset++)
							{
								Marshal.WriteByte((IntPtr)(offset + difference), *offset);
							}
							for (long offset = 0; offset < value_length; offset++)
							{
								Marshal.WriteByte((IntPtr)(target + offset), *(value + offset));
							}
							IntPtr file_pointer = Marshal.ReAllocHGlobal((IntPtr)this.stagebase_files[i].origin, (IntPtr)(this.stagebase_files[i].length + difference));
							this.stagebase_files[i] = new((byte*)file_pointer.ToPointer());
							target += this.stagebase_files[i].origin - old_ptr;
						}
						Marshal.WriteInt32((IntPtr)(this.stagebase_files[i].origin + sizeof(int)), (int)((this.stagebase_files[i].length + difference) - sizeof(int)));

						// now we need to adjust the controllers' origin positions and their respective pointer values
						for (int handle = 0; handle < 0xFF; handle++)
						{
							if (!this._objects.ContainsKey(handle) || this._objects[handle].Count <= 0)
							{
								continue;
							}
							for (int instance = 0; instance < this._objects[handle].Count; instance++)
							{
								controller = this._objects[handle][instance];
								if (controller != null && controller >= old_ptr)
								{
									// remove old locked positions
									controller.UnlockOffsets(this.ptr_data);
									// adjust the origin positions by the difference in memory position
									controller.ModifyOrigin(this.stagebase_files[i].origin - old_ptr, target + target_length, difference);
									// then, adjust the pointers the same way
									controller.ModifyPointers(target - this.stagebase_files[i].origin, (int)difference);
									// and finally, lock the new positions
									controller.LockOffsets(this.ptr_data);
								}
							}
						}
					}
					break;
				}
			}
		}

		#endregion

		internal void Write()
		{
			BinaryWriter writer = new(File.Create("STAGEBASE_NEW.DAT"));
			byte[] buffer = new byte[0x10];
			for (int i = 0; i < this.stagebase_files.Length; i++)
			{
				for (long offset = 0; offset < this.stagebase_files[i].length; offset++)
				{
					writer.Write(*(this.stagebase_files[i].origin + offset));
				}
				writer.Write(buffer, 0, 16 - ((int)writer.BaseStream.Position % 16));
			}
			writer.Flush();
			writer.Close();
		}

		/// <summary>
		/// Frees and nulls all data.
		/// </summary>
		public void Close()
		{
			for (int i = 0; i < this.stagebase_files.Length; i++)
			{
				Marshal.FreeHGlobal((IntPtr)this.stagebase_files[i].origin);
			}
			this.ptr_data.Clear();
			this._objects.Clear();
			current_file_index = 0;
		}

		public static int GetHandlerID<T>() where T : PtrController => typeof(T).Name switch
		{
			nameof(Dummy) => 0x00,
			nameof(FileLabel) => 0x01,
			nameof(DataSection) => 0x02,
			nameof(Stage) => 0x05,
			nameof(Unk_2F) => 0x2F,
			nameof(Location) => 0x37,
			nameof(Unk_66) => 0x66,
			nameof(Temple) => 0x67,
			nameof(Unk_68) => 0x68,
			nameof(Town) => 0x6D,
			nameof(Unk_6E) => 0x6E,
			nameof(Unk_6F) => 0x6F,
			nameof(Unk_78) => 0x78,
			nameof(SpaceEffect) => 0x86,
			nameof(Space) => 0x87,
			nameof(SpaceDescriptions) => 0x88,
			nameof(Unk_93) => 0x93,
			nameof(Unk_DA) => 0xDA,
			nameof(Unk_DB) => 0xDB,
			nameof(Unk_E0) => 0xE0,
			_ => -1
		};

		public static Type? GetHandlerType(int handler) => handler switch
		{
			0x00 => typeof(Dummy),
			0x01 => typeof(FileLabel),
			0x02 => null, /* null, */
			0x03 => typeof(DataSection),
			0x04 => null, /* null, */
			0x05 => typeof(Stage), /* typeof(Stage), */
			0x06 => null, /* null, */
			0x07 => null, /* null, */
			0x08 => null, /* null, */
			0x09 => null, /* null, */
			0x0A => null, /* null, */
			0x0B => null, /* null, */
			0x0C => null, /* null, */
			0x0D => null, /* null, */
			0x0E => null, /* null, */
			0x0F => null, /* null, */
			0x10 => null, /* null, */
			0x11 => null, /* null, */
			0x12 => null, /* null, */
			0x13 => null, /* null, */
			0x14 => null, /* null, */
			0x15 => null, /* null, */
			0x16 => null, /* null, */
			0x17 => null, /* typeof(Unk_17), */
			0x18 => null, /* null, */
			0x19 => null, /* null, */
			0x1A => null, /* null, */
			0x1B => null, /* null, */
			0x1C => null, /* typeof(Unk_1C), */
			0x1D => null, /* typeof(Unk_1D), */
			0x1E => null, /* typeof(Unk_1E), */
			0x1F => null, /* null, */
			0x20 => null, /* null, */
			0x21 => null, /* null, */
			0x22 => null, /* null, */
			0x23 => null, /* null, */
			0x24 => null, /* null, */
			0x25 => null, /* null, */
			0x26 => null, /* null, */
			0x27 => null, /* null, */
			0x28 => null, /* null, */
			0x29 => null, /* typeof(Job_Model_4_7), */
			0x2A => null, /* typeof(Unk_2A), */
			0x2B => typeof(Unk_2B),
			0x2C => null, /* typeof(Unk_2C), */
			0x2D => null, /* typeof(Unk_2D), */
			0x2E => null, /* typeof(Job_Money), */
			0x2F => typeof(Unk_2F),
			0x30 => null, /* null, */
			0x31 => null, /* null, */
			0x32 => null, /* null, */
			0x33 => null, /* null, */
			0x34 => null, /* null, */
			0x35 => null, /* null, */
			0x36 => null, /* null, */
			0x37 => typeof(Location),
			0x38 => null, /* typeof(Job_38), */
			0x39 => null, /* typeof(Job_39), */
			0x3A => null, /* typeof(Npc_Names), */
			0x3B => null, /* typeof(Job_Mastery), */
			0x3C => null, /* typeof(Job_Skills), */
			0x3D => null, /* typeof(Job_Descriptions), */
			0x3E => null, /* typeof(Job_3E), */
			0x3F => null, /* typeof(Unk_3F), */
			0x40 => null, /* typeof(Job_Stats), */
			0x41 => null, /* typeof(Job_Model), */
			0x42 => null, /* typeof(Job), */
			0x43 => null, /* typeof(Job_43), */
			0x44 => null, /* typeof(Job_Bag), */
			0x45 => null, /* typeof(Job_Model_0_3), */
			0x46 => null, /* typeof(Unk_46), */
			0x47 => null, /* typeof(Unk_47), */
			0x48 => null, /* typeof(Unk_48), */
			0x49 => null, /* typeof(Unk_49), */
			0x4A => null, /* typeof(Npc_Text_List), */
			0x4B => null, /* typeof(Npc_Text), */
			0x4C => null, /* typeof(Unk_4C), */
			0x4D => null, /* typeof(Unk_4D), */
			0x4E => null, /* typeof(Unk_4E), */
			0x4F => null, /* typeof(Unk_4F), */
			0x50 => null, /* typeof(Npc_Enemy), */
			0x51 => null, /* typeof(Npc_Enemy_Model), */
			0x52 => null, /* typeof(Unk_52), */
			0x53 => null, /* typeof(Npc_Enemy_Drop_Table), */
			0x54 => null, /* typeof(Unk_54), */
			0x55 => null, /* typeof(Npc), */
			0x56 => null, /* typeof(Npc_Model), */
			0x57 => null, /* typeof(Npc_Model_0), */
			0x58 => null, /* typeof(Weapon), */
			0x59 => null, /* typeof(Weapon_Model), */
			0x5A => null, /* typeof(Weapon_Descriptions), */
			0x5B => null, /* typeof(Unk_5B), */
			0x5C => null, /* typeof(Unk_5C), */
			0x5D => null, /* typeof(Unk_5D), */
			0x5E => null, /* typeof(Shield), */
			0x5F => null, /* typeof(Shield_Model), */
			0x60 => null, /* typeof(Shield_Descriptions), */
			0x61 => null, /* typeof(Npc_Enemy_Model_0), */
			0x62 => null, /* typeof(Unk_62), */
			0x63 => null, /* typeof(Weapon_Bonus_Descriptions), */
			0x64 => null, /* typeof(Accessory), */
			0x65 => null, /* typeof(Accessory_Descriptions), */
			0x66 => typeof(Unk_66),
			0x67 => typeof(Temple),
			0x68 => typeof(Unk_68),
			0x69 => null, /* typeof(Item), */
			0x6A => null, /* typeof(Item_Descriptions), */
			0x6B => null, /* typeof(Item_Func), */
			0x6C => null, /* typeof(Item_Gift), */
			0x6D => typeof(Town),
			0x6E => typeof(Unk_6E),
			0x6F => typeof(Unk_6F),
			0x70 => null, /* typeof(Magic_Off), */
			0x71 => null, /* typeof(Magic_Off_Descriptions), */
			0x72 => null, /* typeof(Magic_Def), */
			0x73 => null, /* typeof(Magic_Def_Descriptions), */
			0x74 => null, /* typeof(Magic_Item), */
			0x75 => null, /* typeof(Magic_Item_Descriptions), */
			0x76 => null, /* typeof(Unk_76), */
			0x77 => null, /* typeof(Unk_77), */
			0x78 => typeof(Unk_78),
			0x79 => null, /* typeof(Unk_79), */
			0x7A => null, /* typeof(Ability_Battle), */
			0x7B => null, /* typeof(Ability_Field), */
			0x7C => null, /* typeof(Npc_Enemy_Descriptions), */
			0x7D => null, /* typeof(Ability_Battle_Descriptions), */
			0x7E => null, /* typeof(Ability_Field_Descriptions), */
			0x7F => typeof(Unk_7F),
			0x80 => null, /* null, */
			0x81 => null, /* typeof(Status_Permanent), */
			0x82 => null, /* typeof(Status_Battle), */
			0x83 => null, /* typeof(Status_Field), */
			0x84 => null, /* typeof(Unk_84), */
			0x85 => typeof(Unk_85),
			0x86 => typeof(SpaceEffect),
			0x87 => typeof(Space),
			0x88 => typeof(SpaceDescriptions),
			0x89 => null, /* typeof(Item_Type), */
			0x8A => null, /* typeof(Magic_Type_Battle), */
			0x8B => null, /* typeof(Magic_Type_Field), */
			0x8C => null, /* typeof(Unk_8C), */
			0x8D => null, /* null, */
			0x8E => null, /* typeof(Status_8E), */
			0x8F => null, /* typeof(Job_8F), */
			0x90 => null, /* null, */
			0x91 => null, /* null, */
			0x92 => null, /* null, */
			0x93 => typeof(Unk_93),
			0x94 => typeof(Unk_94),
			0x95 => null, /* typeof(Hair_Descriptions), */
			0x96 => null, /* typeof(Hair), */
			0x97 => null, /* typeof(Hair_Model), */
			0x98 => null, /* null, */
			0x99 => null, /* null, */
			0x9A => null, /* typeof(Status_9A), */
			0x9B => null, /* typeof(Status_9B), */
			0x9C => null, /* typeof(Unk_9C), */
			0x9D => null, /* typeof(Unk_9D), */
			0x9E => null, /* typeof(Unk_9E), */
			0x9F => null, /* typeof(Magic_9F), */
			0xA0 => null, /* typeof(Unk_A0), */
			0xA1 => null, /* typeof(Unk_A1), */
			0xA2 => null, /* typeof(Unk_A2), */
			0xA3 => null, /* typeof(Unk_A3), */
			0xA4 => null, /* typeof(Unk_A4), */
			0xA5 => null, /* typeof(Unk_A5), */
			0xA6 => null, /* null, */
			0xA7 => null, /* null, */
			0xA8 => null, /* null, */
			0xA9 => null, /* null, */
			0xAA => null, /* null, */
			0xAB => null, /* null, */
			0xAC => null, /* typeof(Unk_AC), */
			0xAD => null, /* typeof(Unk_AD), */
			0xAE => null, /* typeof(Unk_AE), */
			0xAF => null, /* typeof(Unk_AF), */
			0xB0 => null, /* typeof(Unk_B0), */ // equation data
			0xB1 => null, /* typeof(Unk_B1), */ // equation data
			0xB2 => null, /* typeof(Unk_B2), */ // equation data
			0xB3 => null, /* typeof(Unk_B3), */ // equation data
			0xB4 => null, /* typeof(Unk_B4), */ // equation data
			0xB5 => null, /* typeof(Unk_B5), */ // equation data
			0xB6 => null, /* typeof(Unk_B6), */ // equation data
			0xB7 => null, /* typeof(Unk_B7), */ // equation data
			0xB8 => null, /* typeof(Unk_B8), */ // equation data
			0xB9 => null, /* typeof(Unk_B9), */ // equation data
			0xBA => null, /* typeof(Unk_BA), */ // equation data
			0xBB => null, /* typeof(Unk_BB), */ // equation data
			0xBC => null, /* typeof(Unk_BC), */ // equation data
			0xBD => null, /* typeof(Unk_BD), */
			0xBE => null, /* typeof(Unk_BE), */
			0xBF => null, /* typeof(Unk_BF), */
			0xC0 => null, /* typeof(Unk_C0), */
			0xC1 => null, /* null, */
			0xC2 => null, /* null, */
			0xC3 => null, /* null, */
			0xC4 => null, /* null, */
			0xC5 => null, /* null, */
			0xC6 => null, /* null, */
			0xC7 => null, /* null, */
			0xC8 => null, /* null, */
			0xC9 => null, /* null, */
			0xCA => null, /* null, */
			0xCB => null, /* null, */
			0xCC => null, /* null, */
			0xCD => null, /* null, */
			0xCE => null, /* null, */
			0xCF => null, /* null, */
			0xD0 => null, /* typeof(Item_D0), */
			0xD1 => null, /* typeof(Magic_D1), */
			0xD2 => null, /* typeof(Unk_D2), */
			0xD3 => null, /* typeof(Unk_D3), */
			0xD4 => null, /* typeof(Magic_D4), */
			0xD5 => null, /* typeof(Magic_D5), */
			0xD6 => null, /* typeof(Job_D6), */
			0xD7 => null, /* typeof(Item_D7), */
			0xD8 => null, /* typeof(Ability_Darkling_Descriptions), */
			0xD9 => null, /* typeof(Ability_Darkling), */
			0xDA => typeof(Unk_DA),
			0xDB => typeof(Unk_DB),
			0xDC => null, /* null, */
			0xDD => null, /* null, */
			0xDE => null, /* null, */
			0xDF => null, /* typeof(Unk_DF), */
			0xE0 => typeof(Unk_E0),
			0xE1 => null, /* typeof(Unk_E1), */
			0xE2 => null, /* typeof(Unk_E2), */
			0xE3 => null, /* null, */
			0xE4 => null, /* null, */
			0xE5 => null, /* null, */
			0xE6 => null, /* null, */
			0xE7 => null, /* null, */
			0xE8 => null, /* null, */
			0xE9 => null, /* null, */
			0xEA => null, /* null, */
			0xEB => null, /* null, */
			0xEC => null, /* null, */
			0xED => null, /* null, */
			0xEE => null, /* null, */
			0xEF => null, /* null, */
			0xF0 => null, /* null, */
			0xF1 => null, /* null, */
			0xF2 => null, /* null, */
			0xF3 => null, /* null, */
			0xF4 => null, /* null, */
			0xF5 => null, /* null, */
			0xF6 => null, /* null, */
			0xF7 => null, /* null, */
			0xF8 => null, /* null, */
			0xF9 => null, /* null, */
			0xFA => null, /* null, */
			0xFB => null, /* null, */
			0xFC => null, /* null, */
			0xFD => null, /* null, */
			0xFE => null,
			_ => null
		};
	}
}
