using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Gilazo.Functional
{
	public static class EitherFunctions
	{
		/// <summary>
		/// Function Right TL TR
		/// </summary>
		/// <param name="value"></param>
		/// <typeparam name="TL"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <returns>Right TL TR</returns>
		public static Right<TL, TR> Right<TL, TR>([DisallowNull] TR value) => new Right<TL, TR>(value);

		/// <summary>
		/// Function Left TL TR
		/// </summary>
		/// <param name="value"></param>
		/// <typeparam name="TL"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <returns>Left TL TR</returns>
		public static Left<TL, TR> Left<TL, TR>([DisallowNull] TL value) => new Left<TL, TR>(value);

		/// <summary>
		/// Function Match
		/// </summary>
		/// <param name="either"></param>
		/// <param name="right"></param>
		/// <param name="left"></param>
		/// <typeparam name="TL"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <returns>Either TL TR</returns>
		public static Either<TL, TR> Match<TL, TR>(
			Either<TL, TR> either,
			Action<TR> right,
			Action<TL> left
		) => either.Match(right, left);

		/// <summary>
		/// Function Map
		/// </summary>
		/// <param name="either"></param>
		/// <param name="right"></param>
		/// <typeparam name="TL"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <typeparam name="TTo"></typeparam>
		/// <returns>Either TL TTo</returns>
		public static Either<TL, TTo> Map<TL, TR, TTo>(
			Either<TL, TR> either,
			Func<TR, TTo> right
		) where TTo : notnull => either.Map(right);

		/// <summary>
		/// Function MapLeft
		/// </summary>
		/// <param name="either"></param>
		/// <param name="left"></param>
		/// <typeparam name="TL"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <typeparam name="TTo"></typeparam>
		/// <returns>Either TL TR</returns>
		public static Either<TTo, TR> MapLeft<TL, TR, TTo>(
			Either<TL, TR> either,
			Func<TL, TTo> left
		) where TTo : notnull => either.MapLeft(left);

		/// <summary>
		/// Function Bind
		/// </summary>
		/// <param name="either"></param>
		/// <param name="funcs"></param>
		/// <typeparam name="TL"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <returns>Either TL TR</returns>
		public static Either<TL, TR> Bind<TL, TR>(
			Either<TL, TR> either,
			params Func<TR, Either<TL, TR>>[] funcs
		) => funcs.Aggregate(either, (e, func) => e.Bind(func));

		/// <summary>
		/// Function BindLeft
		/// </summary>
		/// <param name="either"></param>
		/// <param name="funcs"></param>
		/// <typeparam name="TL"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <returns>Either TL TR</returns>
		public static Either<TL, TR> BindLeft<TL, TR>(
			Either<TL, TR> either,
			params Func<TL, Either<TL, TR>>[] funcs
		) => funcs.Aggregate(either, (e, func) => e.BindLeft(func));

		/// <summary>
		/// Function FromRight
		/// Use to extract TR from Right TL TR else return @default
		/// </summary>
		/// <param name="either"></param>
		/// <param name="default"></param>
		/// <typeparam name="TL"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <returns>TR</returns>
		public static TR FromRight<TL, TR>(Either<TL, TR> either, TR @default) =>
			either.IsRight ? (TR)either : @default;

		/// <summary>
		/// Function FromLeft
		/// Use to extract TL from Left TL TR else return @default
		/// </summary>
		/// <param name="either"></param>
		/// <param name="default"></param>
		/// <typeparam name="TL"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <returns>TL</returns>
		public static TL FromLeft<TL, TR>(Either<TL, TR> either, TL @default) =>
			either.IsLeft ? (TL)(Left<TL, TR>)either : @default;

		/// <summary>
		/// Try is an extension of Either TL TR where TL is an Exception
		/// Use to safely execute IO
		/// </summary>
		/// <param name="function"></param>
		/// <typeparam name="TR"></typeparam>
		/// <returns>Either Exception TR</returns>
		public static Either<Exception, TR> Try<TR>(Func<TR> function) where TR : notnull
		{
			try
			{
				return Right<Exception, TR>(function());
			}
			catch (Exception ex)
			{
				return Left<Exception, TR>(ex);
			}
		}
	}
}
