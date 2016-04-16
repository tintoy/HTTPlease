using System;

namespace HTTPlease
{
	/// <summary>
	///		Represents <c>void</c>; lack of a value.
	/// </summary>
	public struct Unit
		: IEquatable<Unit>
	{
		/// <summary>
		///		Singleton value for <see cref="Unit"/>.
		/// </summary>
		public static readonly Unit Value = new Unit();

		/// <summary>
		///		Determine if <see cref="Unit"/> is equal to the <see cref="Unit"/>.
		/// </summary>
		/// <param name="other">
		///		The other unit.
		/// </param>
		/// <returns>
		///		<c>true</c> (unit is a singleton value).
		/// </returns>
		public bool Equals(Unit other)
		{
			return true;
		}

		/// <summary>
		///		Determine if another object is equal to the <see cref="Unit"/>.
		/// </summary>
		/// <param name="other">
		///		The other object.
		/// </param>
		/// <returns>
		///		<c>true</c>, if <paramref name="other"/> is a <see cref="Unit"/>; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object other)
		{
			Unit? otherUnit = other as Unit?;

			return otherUnit != null;
		}

		/// <summary>
		///		Get a hash code for the <see cref="Unit"/>.
		/// </summary>
		/// <returns>
		///		The hash code.
		/// </returns>
		public override int GetHashCode()
		{
			return 1;
		}

		/// <summary>
		///		Equality operator for 2 <see cref="Unit"/>s.
		/// </summary>
		/// <param name="left">
		///		The left-hand <see cref="Unit"/>.
		/// </param>
		/// <param name="right">
		///		The right-hand <see cref="Unit"/>.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the <see cref="Unit"/>s are equal; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator ==(Unit left, Unit right)
		{
			return left.Equals(right);
		}

		/// <summary>
		///		Inequality operator for 2 <see cref="Unit"/>s.
		/// </summary>
		/// <param name="left">
		///		The left-hand <see cref="Unit"/>.
		/// </param>
		/// <param name="right">
		///		The right-hand <see cref="Unit"/>.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the <see cref="Unit"/>s are not equal; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=(Unit left, Unit right)
		{
			return !left.Equals(right);
		}
	}
}
