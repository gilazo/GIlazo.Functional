using System.Diagnostics.CodeAnalysis;

namespace Gilazo.Functional
{
    /// <summary>
    /// Left representation of Either TL TR
    /// </summary>
    /// <typeparam name="TL"></typeparam>
    /// <typeparam name="TR"></typeparam>
    public sealed class Left<TL, TR> : Either<TL, TR>
    {
        /// <summary>
        ///	Ctor for building Left TL TR
        /// Is the Left representation of Either TL TR
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Left TL TR</returns>
        public Left([DisallowNull] TL value) : base(value) { }

        /// <summary>
        /// Deconstructs the Left TL TR by extracting TL
        /// Use in pattern matching
        /// </summary>
        /// <param name="value"></param>
        public void Deconstruct(out TL value) => value = (TL)Value;

        /// <summary>
        /// Implicitly convert TL to Left TL TR
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="TL"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <returns>Left TL TR</returns>
        public static implicit operator Left<TL, TR>([DisallowNull] TL value) =>
            new Left<TL, TR>(value);

        /// <summary>
        /// Implicitly convert Left TL TR to TL
        /// </summary>
        /// <param name="either"></param>
        /// <returns>TL</returns>
        public static implicit operator TL(Left<TL, TR> either) =>
            (TL)either.Value;
    }
}
