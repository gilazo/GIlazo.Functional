using System.Diagnostics.CodeAnalysis;

namespace Gilazo.Functional
{
	/// <summary>
	/// Right representation of the Either TL TR
	/// </summary>
	/// <typeparam name="TL"></typeparam>
	/// <typeparam name="TR"></typeparam>
	public sealed class Right<TL, TR> : Either<TL, TR>
	{
		/// <summary>
		///	Ctor for building Right TL TR
		/// Is the Right representation of Either TL TR
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Left TL TR</returns>
		public Right([DisallowNull] TR value) : base(value) { }

		/// <summary>
		/// Deconstructs the Right TL TR by extracting TR
		/// Use in pattern matching
		/// </summary>
		/// <param name="value"></param>
		public void Deconstruct(out TR value) => value = (TR)_value;

		/// <summary>
		/// Implicitly convert TR to Right TL TR
		/// </summary>
		/// <param name="value"></param>
		/// <typeparam name="TL"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <returns>Right TL TR</returns>
		public static implicit operator Right<TL, TR>([DisallowNull] TR value) =>
			new Right<TL, TR>(value);

		/// <summary>
		/// Implicitly convert Right TL TR to TR
		/// </summary>
		/// <param name="either"></param>
		/// <returns>TR</returns>
		public static implicit operator TR(Right<TL, TR> either) =>
			(TR)either._value;
	}
}
