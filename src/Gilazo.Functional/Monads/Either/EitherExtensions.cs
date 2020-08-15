using System;
using System.Threading.Tasks;

namespace Gilazo.Functional
{
	public static class EitherExtensions
	{
		#region Bind aliases

		public static Either<TL, TR> Apply<TL, TR>(this Either<TL, TR> self, Func<TR, Either<TL, TR>> right) =>
			self.Bind(right);

		public static Either<TL, TR> Apply<TL, TR>(this Either<TL, TR> self, Func<TR, Either<TL, TR>> right, Func<TL, Either<TL, TR>> left) =>
			self.Bind(right, left);

		#endregion

		#region Bind aliases async

		public static async Task<Either<TL, TR>> Apply<TL, TR>(this Either<TL, TR> self, Func<TR, Task<Either<TL, TR>>> right) =>
			await self.Bind(async r => await right(r));

		public static async Task<Either<TL, TR>> Apply<TL, TR>(
			this Either<TL, TR> self,
			Func<TR, Task<Either<TL, TR>>> right,
			Func<TL, Either<TL, TR>> left
		) => await self.Bind(async r => await right(r), left);

		public static async Task<Either<TL, TR>> Apply<TL, TR>(
			this Either<TL, TR> self,
			Func<TR, Either<TL, TR>> right,
			Func<TL, Task<Either<TL, TR>>> left
		) => await self.Bind(right, async l => await left(l));

		public static async Task<Either<TL, TR>> Apply<TL, TR>(
			this Either<TL, TR> self,
			Func<TR, Task<Either<TL, TR>>> right,
			Func<TL, Task<Either<TL, TR>>> left
		) => await self.Bind(async r => await right(r), async l => await left(l));

		#endregion
	}
}
