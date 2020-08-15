using System;
using System.Threading.Tasks;

namespace Gilazo.Functional
{
	public static class Methods
	{
		public static Either<TL, TR> Right<TL, TR>(TR right) => new Either<TL, TR>(right);

		public static Either<TL, TR> Left<TL, TR>(TL left) => new Either<TL, TR>(left);

		public static Either<Exception, TR> Try<TR>(Func<TR> function)
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

		public static async Task<Either<Exception, TR>> Try<TR>(Func<Task<TR>> function)
		{
			try
			{
				return Right<Exception, TR>(await function());
			}
			catch (Exception ex)
			{
				return Left<Exception, TR>(ex);
			}
		}
	}
}
