using System;
using System.Threading.Tasks;

namespace Gilazo.Functional
{
	public static partial class TaskExtensions
	{
		#region Match

		public static async Task Match<TL, TR>(this Task<Either<TL, TR>> self, Action<TR> right, Action<TL> left) =>
			(await self).Match(right, left);

		public static async Task<TTo> Match<TL, TR, TTo>(this Task<Either<TL, TR>> self, Func<TR, TTo> right, Func<TL, TTo> left) =>
			(await self).Match(right, left);

		public static async Task Match<TL, TR>(this Task<Either<TL, TR>> self, Func<TR, Task> right, Action<TL> left) =>
			await (await self).Match(async r => await right(r), left);

		public static async Task Match<TL, TR>(this Task<Either<TL, TR>> self, Action<TR> right, Func<TL, Task> left) =>
			await (await self).Match(right, async l => await left(l));

		public static async Task Match<TL, TR>(this Task<Either<TL, TR>> self, Func<TR, Task> right, Func<TL, Task> left) =>
			await (await self).Match(async r => await right(r), async l => await left(l));

		public static async Task<TTo> Match<TL, TR, TTo>(this Task<Either<TL, TR>> self, Func<TR, Task<TTo>> right, Func<TL, TTo> left) =>
			await (await self).Match(async r => await right(r), left);

		public static async Task<TTo> Match<TL, TR, TTo>(this Task<Either<TL, TR>> self, Func<TR, TTo> right, Func<TL, Task<TTo>> left) =>
			await (await self).Match(right, async l => await left(l));

		public static async Task<TTo> Match<TL, TR, TTo>(this Task<Either<TL, TR>> self, Func<TR, Task<TTo>> right, Func<TL, Task<TTo>> left) =>
			await (await self).Match(async r => await right(r), async l => await left(l));

		#endregion

		#region Map

		public static async Task<Either<TL, TTo>> Map<TL, TR, TTo>(this Task<Either<TL, TR>> self, Func<TR, TTo> right) =>
			(await self).Map(right);

		public static async Task<Either<TLTo, TRTo>> Map<TL, TR, TLTo, TRTo>(
			this Task<Either<TL, TR>> self,
			Func<TR, TRTo> right,
			Func<TL, TLTo> left
		) => (await self).Map(right, left);

		public static async Task<Either<TL, TTo>> Map<TL, TR, TTo>(this Task<Either<TL, TR>> self, Func<TR, Task<TTo>> right) =>
			await (await self).Map(async r => await right(r));

		public static async Task<Either<TLTo, TRTo>> Map<TL, TR, TLTo, TRTo>(
			this Task<Either<TL, TR>> self,
			Func<TR, Task<TRTo>> right,
			Func<TL, TLTo> left
		) => await (await self).Map(async r => await right(r), left);

		public static async Task<Either<TLTo, TRTo>> Map<TL, TR, TLTo, TRTo>(
			this Task<Either<TL, TR>> self,
			Func<TR, TRTo> right,
			Func<TL, Task<TLTo>> left
		) => await (await self).Map(right, async l => await left(l));

		public static async Task<Either<TLTo, TRTo>> Map<TL, TR, TLTo, TRTo>(
			this Task<Either<TL, TR>> self,
			Func<TR, Task<TRTo>> right,
			Func<TL, Task<TLTo>> left
		) => await (await self).Map(async r => await right(r), async l => await left(l));

		#endregion

		#region Bind

		public static async Task<Either<TL, TR>> Bind<TL, TR>(this Task<Either<TL, TR>> self, Func<TR, Either<TL, TR>> right) =>
			(await self).Bind(right);

		public static async Task<Either<TL, TR>> Bind<TL, TR>(
			this Task<Either<TL, TR>> self,
			Func<TR, Either<TL, TR>> right,
			Func<TL, Either<TL, TR>> left
		) => (await self).Bind(right, left);

		public static async Task<Either<TL, TR>> Bind<TL, TR>(this Task<Either<TL, TR>> self, Func<TR, Task<Either<TL, TR>>> right) =>
			await (await self).Bind(async r => await right(r));

		public static async Task<Either<TL, TR>> Bind<TL, TR>(
			this Task<Either<TL, TR>> self,
			Func<TR, Task<Either<TL, TR>>> right,
			Func<TL, Either<TL, TR>> left
		) => await (await self).Bind(async r => await right(r), left);

		public static async Task<Either<TL, TR>> Bind<TL, TR>(
			this Task<Either<TL, TR>> self,
			Func<TR, Either<TL, TR>> right,
			Func<TL, Task<Either<TL, TR>>> left
		) => await (await self).Bind(right, async l => await left(l));

		public static async Task<Either<TL, TR>> Bind<TL, TR>(
			this Task<Either<TL, TR>> self,
			Func<TR, Task<Either<TL, TR>>> right,
			Func<TL, Task<Either<TL, TR>>> left
		) => await (await self).Bind(async r => await right(r), async l => await left(l));

		#endregion

		#region Bind aliases

		public static async Task<Either<TL, TR>> Apply<TL, TR>(this Task<Either<TL, TR>> self, Func<TR, Either<TL, TR>> right) =>
			(await self).Bind(right);

		public static async Task<Either<TL, TR>> Apply<TL, TR>(
			this Task<Either<TL, TR>> self,
			Func<TR, Either<TL, TR>> right,
			Func<TL, Either<TL, TR>> left
		) => (await self).Bind(right, left);

		public static async Task<Either<TL, TR>> Apply<TL, TR>(this Task<Either<TL, TR>> self, Func<TR, Task<Either<TL, TR>>> right) =>
			await (await self).Bind(async r => await right(r));

		public static async Task<Either<TL, TR>> Apply<TL, TR>(
			this Task<Either<TL, TR>> self,
			Func<TR, Task<Either<TL, TR>>> right,
			Func<TL, Either<TL, TR>> left
		) => await (await self).Bind(async r => await right(r), left);

		public static async Task<Either<TL, TR>> Apply<TL, TR>(
			this Task<Either<TL, TR>> self,
			Func<TR, Either<TL, TR>> right,
			Func<TL, Task<Either<TL, TR>>> left
		) => await (await self).Bind(right, async l => await left(l));

		public static async Task<Either<TL, TR>> Apply<TL, TR>(
			this Task<Either<TL, TR>> self,
			Func<TR, Task<Either<TL, TR>>> right,
			Func<TL, Task<Either<TL, TR>>> left
		) => await (await self).Bind(async r => await right(r), async l => await left(l));

		#endregion
	}
}
