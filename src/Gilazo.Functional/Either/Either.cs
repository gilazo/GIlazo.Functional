using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Gilazo.Functional
{
    /// <summary>
    /// Base Either TL TR
    /// </summary>
    /// <typeparam name="TL"></typeparam>
    /// <typeparam name="TR"></typeparam>
    [DebuggerStepThrough]
    public abstract class Either<TL, TR>
    {
        [DisallowNull]
        protected readonly object Value;

        /// <summary>
        /// True if this is type of Right TL TR
        /// </summary>
        /// <value></value>
        public bool IsRight =>
            this switch
            {
                Right<TL, TR> _ => true,
                _ => false
            };

        /// <summary>
        /// True if this is not type of Right TL TR
        /// </summary>
        public bool IsLeft => !IsRight;

        /// <summary>
        /// Ctor for building Either TR
        /// </summary>
        /// <param name="value"></param>
        protected Either([DisallowNull] TR value) => Value = value;

        /// <summary>
        /// Ctor for building Either TL
        /// </summary>
        /// <param name="value"></param>
        protected Either([DisallowNull] TL value) => Value = value;

        /// <summary>
        /// Implicitly convert TR to Either TL TR
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="TL"></typeparam>
        /// <typeparam name="TR"></typeparam>
        public static implicit operator Either<TL, TR>([DisallowNull] TR value) =>
            new Right<TL, TR>(value);

        /// <summary>
        /// Implicitly convert Either TL TR to TR
        /// </summary>
        /// <param name="either"></param>
        public static implicit operator TR(Either<TL, TR> either) =>
            (TR)(Right<TL, TR>)either;

        /// <summary>
        /// Implicitly convert TL to Either TL TR
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="TL"></typeparam>
        /// <typeparam name="TR"></typeparam>
        public static implicit operator Either<TL, TR>([DisallowNull] TL value) =>
            new Left<TL, TR>(value);

        /// <summary>
        /// Implicitly convert Either TL TR to TL
        /// </summary>
        /// <param name="either"></param>
        public static implicit operator TL(Either<TL, TR> either) =>
            (TL)(Left<TL, TR>)either;

        #region Match

        /// <summary>
        /// Match executes right if Either TR else executes left
        /// Use for demoting Either._value
        /// </summary>
        /// <param name="right"></param>
        /// <param name="left"></param>
        /// <typeparam name="TTo"></typeparam>
        /// <returns>TTo</returns>
        public TTo Match<TTo>(Func<TR, TTo> right, Func<TL, TTo> left) =>
            IsRight
                ? right((TR)Value)
                : left((TL)Value);

        /// <summary>
        /// Match executes right if Either TR else executes left
        /// Use for inline execution of actions on Either._value
        /// </summary>
        /// <param name="right"></param>
        /// <param name="left"></param>
        /// <returns>Either TL TR</returns>
        public Either<TL,TR> Match(Action<TR> right, Action<TL> left)
        {
            if (IsRight)
            {
                right((TR)Value);
            }
            else
            {
                left((TL)Value);
            }

            return this;
        }

        #endregion

        #region Map

        /// <summary>
        /// Map executes func if Either TR else returns Either TL
        /// Use for converting Either TL TR to Either TL TTo
        /// </summary>
        /// <param name="func"></param>
        /// <typeparam name="TTo"></typeparam>
        /// <returns>Either TL TTo</returns>
        public Either<TL, TTo> Map<TTo>(Func<TR, TTo> func) where TTo : notnull =>
            IsRight
                ? (Either<TL, TTo>)new Right<TL, TTo>(func((TR)Value))
                : (Either<TL, TTo>)new Left<TL, TTo>((TL)Value);

        /// <summary>
        /// MapLeft executes func if Either TL else returns Either TR
        /// Use for converting Either TL TR to Either TTo TR
        /// </summary>
        /// <param name="func"></param>
        /// <typeparam name="TTo"></typeparam>
        /// <returns>Either TTo TR</returns>
        public Either<TTo, TR> MapLeft<TTo>(Func<TL, TTo> func) where TTo : notnull =>
            IsRight
                ? (Either<TTo, TR>)new Right<TTo, TR>((TR)Value)
                : (Either<TTo, TR>)new Left<TTo, TR>(func((TL)Value));

        #endregion

        #region Bind

        /// <summary>
        /// Bind exectues func if Either TR else returns Either TL
        /// Use for binding two or more functions together
        /// </summary>
        /// <param name="func"></param>
        /// <returns>Either TL TR</returns>
        public Either<TL, TR> Bind(Func<TR, Either<TL, TR>> func) =>
            IsRight
                ? func((TR)Value)
                : this;

        /// <summary>
        /// BindLeft executes func if Either TL else returns Either TR
        /// Use for binding two or more functions together
        /// </summary>
        /// <param name="func"></param>
        /// <returns>Either TL TR</returns>
        public Either<TL, TR> BindLeft(Func<TL, Either<TL, TR>> func) =>
            IsRight
                ? this
                : func((TL)Value);

        #endregion
    }
}
