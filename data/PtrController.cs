using DKSBE.data.controllers;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DKSBE.data
{
	/// <summary>
	/// The base class for each handler when reading data from STAGEBASE.DAT.
	/// </summary>
	public unsafe abstract class PtrController
	{
		public byte* file_origin { get; internal set; }
		/// <summary>
		/// The position in memory of which this instance reads from. Do not modify this value unless you know what you're doing.
		/// </summary>
		public byte* origin { get; internal set; }

		/// <summary>
		/// The size of this instance in bytes. This value should only include the length of bytes from this instance's <see cref="origin"/> to the next.
		/// </summary>
		public virtual long size => this.SizeOf();

		/// <summary>
		/// Do not allow creating an instance with the constructor.
		/// </summary>
		protected PtrController() { }

		/// <summary>
		/// Create a handled instance of <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of <see cref="PtrController"/> to create.</typeparam>
		/// <param name="origin">The position in memory to put this instance.</param>
		/// <returns>A new instance of <typeparamref name="T"/> that has had its <see cref="OnCreate"/> method called.</returns>
		/// <exception cref="NullReferenceException">No default constructor was found.</exception>
		public static T CreateInstance<T>(byte* file_origin, long offset) where T : PtrController
		{
			T? ret = Activator.CreateInstance<T>();
			if (ret != null)
			{
				ret.file_origin = file_origin;
				ret.origin = file_origin + offset;
				ret.OnCreate();
				return ret;
			}
			throw new NullReferenceException($"Failed to create an instance of {nameof(T)}");
		}

		/// <summary>
		/// Create a handled instance of <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The type of <see cref="PtrController"/> to create.</param>
		/// <param name="origin">The position in memory to put this instance.</param>
		/// <returns>A new instance of <paramref name="type"/> that has had its <see cref="OnCreate"/> method called.</returns>
		/// <remarks>If <paramref name="type"/> is null or is not a <see cref="PtrController"/> type, a new instance of <see cref="Dummy"/> will be returned.</remarks>
		public static PtrController CreateInstance(Type? type, byte* file_origin, long offset)
		{
			if (type == null)
			{
				return new Dummy();
			}
			if (!type.IsSubclassOf(typeof(PtrController)))
			{
				return new Dummy();
			}
			PtrController ret = (PtrController)(Activator.CreateInstance(type) ?? new Dummy());
			ret.file_origin = file_origin;
			ret.origin = file_origin + offset;
			ret.OnCreate();
			return ret;
		}

		public static implicit operator byte*(PtrController ptr) => ptr.origin;
		public static implicit operator long(PtrController ptr) => (long)ptr.origin;
		public static PtrController operator +(PtrController ptr, int amount)
		{
			ptr.origin += amount;
			return ptr;
		}

		public static PtrController operator +(PtrController ptr, long amount)
		{
			ptr.origin += amount;
			return ptr;
		}

		public static bool operator ==(PtrController? a, PtrController? b)
		{
			if (a is not null)
			{
				return a.Equals(b);
			}
			return b is null;
		}

		public static bool operator !=(PtrController? a, PtrController? b)
		{
			if (a is not null)
			{
				return !a.Equals(b);
			}
			return b is not null;
		}

		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			if (obj is PtrController ptr)
			{
				return this.origin == ptr.origin;
			}

			if (obj is Pointer pointer)
			{
				return this.origin == (byte*)Pointer.Unbox(pointer);
			}

			return false;
		}

		public static bool Equals(PtrController a, PtrController b)
		{
			return a.Equals(b);
		}

		public static bool Equals(PtrController a, Pointer b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// The <see cref="HashCode"/> of this instance.
		/// </summary>
		/// <returns>The combined <see cref="HashCode"/> of <see cref="origin"/> and the <see cref="HashCode"/> of the base object.</returns>
		public override int GetHashCode()
		{
			return HashCode.Combine(this.origin->GetHashCode(), base.GetHashCode());
		}

		public byte this[long offset]
		{
			get => this.origin[offset];
			set => this.origin[offset] = value;
		}

		/// <summary>
		/// Allocate data or create new instances of <see cref="PtrController"/> in this method.
		/// </summary>
		public virtual void OnCreate() { }
		/// <summary>
		/// Free or clear any data contained within the current instance.
		/// </summary>
		public virtual void OnDestroy() { }
		/// <summary>
		/// Add any important offsets to the dictionary.
		/// </summary>
		/// <param name="dict">The dictionary of <see cref="PtrData"/> contained in <see cref="Stagebase"/>.</param>
		public virtual void LockOffsets(Dictionary<long, PtrData> dict) { }
		/// <summary>
		/// Remove any important offsets from the dictionary given.
		/// </summary>
		/// <param name="dict">The dictionary of <see cref="PtrData"/> contained in <see cref="Stagebase"/>.</param>
		public virtual void UnlockOffsets(Dictionary<long, PtrData> dict) { }
		/// <summary>
		/// Used to modify any pointer values contained within the current <see cref="PtrController"/> instance.
		/// <para/>Be sure to call this function for any and all instances of <see cref="PtrController"/> contained within the current instance.
		/// </summary>
		/// <param name="target">The target offset for which changes have been made.</param>
		/// <param name="difference">The difference in size between the old and new data which was just written to memory.</param>
		public virtual void ModifyPointers(long target, int difference) { }
		/// <summary>
		/// Used to modify the <see cref="origin"/> position of the current <see cref="PtrController"/> instance.
		/// <para/>Be sure to call base.ModifyOrigin for any and all instances of <see cref="PtrController"/> contained within the current instance.
		/// </summary>
		/// <param name="difference"></param>
		public virtual void ModifyOrigin(long alloc_difference, byte* target, long size_difference)
		{
			this.file_origin += alloc_difference;
			this.origin += alloc_difference;
			if (this.origin >= target)
			{
				this.origin += size_difference;
			}
		}
	}
}
