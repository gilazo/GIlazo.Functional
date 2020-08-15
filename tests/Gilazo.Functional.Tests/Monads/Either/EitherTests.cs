using System;
using Xunit;

namespace Gilazo.Functional
{
	public class EitherTests
	{
		[Theory]
		[InlineData("Hello, World!")]
		public void Ctor_Right_RightEither(string right)
		{
			// Arrange
			// Act
			var actual = new Either<int, string>(right);

			// Assert
			Assert.True(actual.IsRight);
		}

		[Theory]
		[InlineData(-1)]
		public void Ctor_Left_LeftEither(int left)
		{
			// Arrange
			// Act
			var actual = new Either<int, string>(left);

			// Assert
			Assert.True(actual.IsLeft);
		}

		[Theory]
		[InlineData("Hello, World")]
		public void Match_IsRight_RightInvoked(string initial)
		{
			// Arrange
			var either = new Either<int, string>(initial);

			// Act
			var actual = either.Match(right => right, left => left.ToString());

			// Assert
			Assert.Equal(initial, actual);
		}

		[Theory]
		[InlineData(-1)]
		public void Match_IsLeft_LeftInvoked(int initial)
		{
			// Arrange
			var either = new Either<int, string>(initial);

			// Act
			var actual = either.Match(right => right, left => left.ToString());

			// Assert
			Assert.Equal(initial.ToString(), actual);
		}

		[Theory]
		[InlineData("82")]
		public void Map_IsRight_RightInvoked(string initial)
		{
			// Arrange
			var either = new Either<int, string>(initial);

			// Act
			var actual = either.Map(right => Convert.ToInt32(right), left => left.ToString());

			// Assert
			Assert.True(actual.IsRight);
			Assert.IsType<Either<string, int>>(actual);
			Assert.Equal(Convert.ToInt32(initial), actual.Match(right => right, left => Convert.ToInt32(left)));
		}

		[Theory]
		[InlineData(-1)]
		public void Map_IsLeft_LeftInvoked(int initial)
		{
			// Arrange
			var either = new Either<int, string>(initial);

			// Act
			var actual = either.Map(right => Convert.ToInt32(right), left => left.ToString());

			// Assert
			Assert.True(actual.IsLeft);
			Assert.IsType<Either<string, int>>(actual);
			Assert.Equal(initial, actual.Match(right => right, left => Convert.ToInt32(left)));
		}

		[Theory]
		[InlineData("Hello")]
		public void Bind_IsRight_RightInvoked(string initial)
		{
			// Arrange
			var either = new Either<int, string>(initial);

			// Act
			var actual = either.Bind(right => new Either<int, string>($"{right}, World!"), left => new Either<int, string>(left));

			// Assert
			Assert.True(actual.IsRight);
			Assert.Equal("Hello, World!", actual.Match(right => right, left => left.ToString()));
		}

		[Theory]
		[InlineData(-1)]
		public void Bind_IsLeft_LeftInvoked(int initial)
		{
			// Arrange
			var either = new Either<int, string>(initial);

			// Act
			var actual = either.Bind(right => new Either<int, string>($"{right}, World!"), left => new Either<int, string>(left + 1));

			// Assert
			Assert.True(actual.IsLeft);
			Assert.Equal(0, actual.Match(right => Convert.ToInt32(right), left => left));
		}
	}
}
