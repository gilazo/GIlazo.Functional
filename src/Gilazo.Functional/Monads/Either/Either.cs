using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Gilazo.Functional
{
	[DebuggerStepThrough]
	public sealed class Either<TL, TR>
	{
		[AllowNull]
		private readonly TR _right;

		[AllowNull]
		private readonly TL _left;

		public bool IsRight { get; }

		public bool IsLeft => !IsRight;

		public Either(TR right) => (_right, _left, IsRight) = (right, default, true);

		public Either(TL left) => (_left, _right, IsRight) = (left, default, false);

		public static implicit operator Either<TL, TR>(TR right) => new Either<TL, TR>(right);

		public static implicit operator Either<TL, TR>(TL left) => new Either<TL, TR>(left);

		public static implicit operator Task<Either<TL, TR>>(Either<TL, TR> either) => Task.FromResult(either);

		#region Match

		public void Match(Action<TR> right, Action<TL> left)
		{
			if (IsRight)
			{
				right(_right);
			}
			else
			{
				left(_left);
			}
		}

		public TTo Match<TTo>(Func<TR, TTo> right, Func<TL, TTo> left) =>
			IsRight
				? right(_right)
				: left(_left);

		#endregion

		#region Match async

		public async Task Match(Func<TR, Task> right, Action<TL> left)
		{
			if (IsRight)
			{
				await right(_right);
			}
			else
			{
				left(_left);
			}
		}

		public async Task Match(Action<TR> right, Func<TL, Task> left)
		{
			if (IsRight)
			{
				right(_right);
			}
			else
			{
				await left(_left);
			}
		}

		public async Task Match(Func<TR, Task> right, Func<TL, Task> left)
		{
			if (IsRight)
			{
				await right(_right);
			}
			else
			{
				await left(_left);
			}
		}

		public async Task<TTo> Match<TTo>(Func<TR, Task<TTo>> right, Func<TL, TTo> left) =>
			IsRight
				? await right(_right)
				: left(_left);

		public async Task<TTo> Match<TTo>(Func<TR, TTo> right, Func<TL, Task<TTo>> left) =>
			IsRight
				? right(_right)
				: await left(_left);

		public async Task<TTo> Match<TTo>(Func<TR, Task<TTo>> right, Func<TL, Task<TTo>> left) =>
			IsRight
				? await right(_right)
				: await left(_left);

		#endregion

		#region Map

		public Either<TL, TTo> Map<TTo>(Func<TR, TTo> right) =>
			IsRight
				? new Either<TL, TTo>(right(_right))
				: new Either<TL, TTo>(_left);

		public Either<TLTo, TRTo> Map<TLTo, TRTo>(Func<TR, TRTo> right, Func<TL, TLTo> left) =>
			IsRight
				? new Either<TLTo, TRTo>(right(_right))
				: new Either<TLTo, TRTo>(left(_left));

		#endregion

		#region Map async

		public async Task<Either<TL, TTo>> Map<TTo>(Func<TR, Task<TTo>> right) =>
			IsRight
				? new Either<TL, TTo>(await right(_right))
				: new Either<TL, TTo>(_left);

		public async Task<Either<TLTo, TRTo>> Map<TLTo, TRTo>(Func<TR, Task<TRTo>> right, Func<TL, TLTo> left) =>
			IsRight
				? new Either<TLTo, TRTo>(await right(_right))
				: new Either<TLTo, TRTo>(left(_left));

		public async Task<Either<TLTo, TRTo>> Map<TLTo, TRTo>(Func<TR, TRTo> right, Func<TL, Task<TLTo>> left) =>
			IsRight
				? new Either<TLTo, TRTo>(right(_right))
				: new Either<TLTo, TRTo>(await left(_left));

		public async Task<Either<TLTo, TRTo>> Map<TLTo, TRTo>(Func<TR, Task<TRTo>> right, Func<TL, Task<TLTo>> left) =>
			IsRight
				? new Either<TLTo, TRTo>(await right(_right))
				: new Either<TLTo, TRTo>(await left(_left));

		#endregion

		#region Bind

		public Either<TL, TR> Bind(Func<TR, Either<TL, TR>> right) =>
			IsRight
				? right(_right)
				: this;

		public Either<TL, TR> Bind(Func<TR, Either<TL, TR>> right, Func<TL, Either<TL, TR>> left) =>
			IsRight
				? right(_right)
				: left(_left);

		#endregion

		#region Bind async

		public async Task<Either<TL, TR>> Bind(Func<TR, Task<Either<TL, TR>>> right) =>
			IsRight
				? await right(_right)
				: this;

		public async Task<Either<TL, TR>> Bind(Func<TR, Task<Either<TL, TR>>> right, Func<TL, Either<TL, TR>> left) =>
			IsRight
				? await right(_right)
				: left(_left);

		public async Task<Either<TL, TR>> Bind(Func<TR, Either<TL, TR>> right, Func<TL, Task<Either<TL, TR>>> left) =>
			IsRight
				? right(_right)
				: await left(_left);

		public async Task<Either<TL, TR>> Bind(Func<TR, Task<Either<TL, TR>>> right, Func<TL, Task<Either<TL, TR>>> left) =>
			IsRight
				? await right(_right)
				: await left(_left);

		#endregion
	}
}
