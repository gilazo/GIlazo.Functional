using System;
using Xunit;
using static Gilazo.Functional.EitherFunctions;

namespace Gilazo.Functional
{
	public class EitherFunctionsTests
	{
		[Theory]
		[InlineData("Hello, World!")]
		public void Right_RightEither(string right)
		{
			// Arrange
			// Act
			var actual = Right<int, string>(right);

			// Assert
			Assert.IsType<Right<int, string>>(actual);
		}

		[Theory]
		[InlineData(-1)]
		public void Left_LeftEither(int left)
		{
			// Arrange
			// Act
			var actual = Left<int, string>(left);

			// Assert
			Assert.IsType<Left<int, string>>(actual);
		}

		[Theory]
		[InlineData("Hello")]
		public void Match_IsRight_RightInvoked(string initial)
		{
			// Arrange
			Either<int, string> either = initial;

			// Act
			// Assert
			Match(either,
				right: s => Assert.Equal(initial, s),
				left: i => throw new InvalidOperationException(i.ToString())
			);
		}

		[Theory]
		[InlineData(-1)]
		public void Match_IsLeft_LeftInvoked(int initial)
		{
			// Arrange
			Either<int, string> either = initial;

			// Act
			// Assert
			Match(either,
				right: s => throw new InvalidOperationException(s),
				left: i => Assert.Equal(initial, i)
			);
		}

		[Theory]
		[InlineData("1")]
		public void Map_IsRight_FuncInvoked(string initial)
		{
			// Arrange
			Either<int, string> either = initial;

			// Act
			var actual = Map(either,
				s => Convert.ToInt32(s)
			);

			// Assert
			Assert.IsType<Right<int, int>>(actual);
			actual.Match(
				i => Assert.Equal(1, i),
				i => throw new InvalidOperationException(i.ToString())
			);
		}

		[Theory]
		[InlineData(-1)]
		public void Map_IsLeft_FuncNotInvoked(int initial)
		{
			// Arrange
			Either<int, string> either = initial;

			// Act
			var actual = Map(either,
				s => Convert.ToInt32(s)
			);

			// Assert
			Assert.IsType<Left<int, int>>(actual);
			actual.Match(
				i => throw new InvalidOperationException(i.ToString()),
				i => Assert.Equal(initial, i)
			);
		}

		[Theory]
		[InlineData("1")]
		public void MapLeft_IsRight_FuncNotInvoked(string initial)
		{
			// Arrange
			Either<int, string> either = initial;

			// Act
			var actual = MapLeft(either,
				i => i.ToString()
			);

			// Assert
			Assert.IsType<Right<string, string>>(actual);
			actual.Match(
				s => Assert.Equal(initial, s),
				s => throw new InvalidOperationException(s)
			);
		}

		[Theory]
		[InlineData(-1)]
		public void MapLeft_IsLeft_FuncInvoked(int initial)
		{
			// Arrange
			Either<int, string> either = initial;

			// Act
			var actual = MapLeft(either,
				i => i.ToString()
			);

			// Assert
			Assert.IsType<Left<string, string>>(actual);
			actual.Match(
				s => throw new InvalidOperationException(s),
				s => Assert.Equal("-1", s)
			);
		}

		[Theory]
		[InlineData("Hello")]
		public void Bind_IsRight_FuncsBound(string initial)
		{
			// Arrange
			Either<int, string> either = initial;

			// Act
			var actual = Bind(either,
				s => $"{s},",
				s => $"{s} ",
				s => $"{s}World",
				s => $"{s}!"
			);

			// Assert
			Assert.IsType<Right<int, string>>(actual);
			Assert.Equal("Hello, World!", (string)actual);
		}

		[Theory]
		[InlineData(-1)]
		public void Bind_IsLeft_FuncsBound(int initial)
		{
			// Arrange
			Either<int, string> either = initial;

			// Act
			var actual = Bind(either,
				s => $"{s},",
				s => $"{s} ",
				s => $"{s}World",
				s => $"{s}!"
			);

			// Assert
			Assert.IsType<Left<int, string>>(actual);
			Assert.Equal(-1, (int)actual);
		}

		[Theory]
		[InlineData("Hello")]
		public void BindLeft_IsRight_FuncsNotBound(string initial)
		{
			// Arrange
			Either<int, string> either = initial;

			// Act
			var actual = BindLeft(either,
				i => i + 1,
				i => i + 5,
				i => i * 2
			);

			// Assert
			Assert.IsType<Right<int, string>>(actual);
			Assert.Equal(initial, (string)actual);
		}

		[Theory]
		[InlineData(-1)]
		public void BindLeft_IsLeft_FuncsBound(int initial)
		{
			// Arrange
			Either<int, string> either = initial;

			// Act
			var actual = BindLeft(either,
				i => i + 1,
				i => i + 5,
				i => i * 2
			);

			// Assert
			Assert.IsType<Left<int, string>>(actual);
			Assert.Equal(10, (int)actual);
		}

		[Theory]
		[InlineData("Hello")]
		public void FromRight_IsRight_Right(string initial)
		{
			// Arrange
			Either<int, string> either = initial;

			// Act
			var actual = FromRight(either, "Hola");

			// Assert
			Assert.Equal(initial, actual);
		}

		[Theory]
		[InlineData(-1)]
		public void FromRight_IsLeft_Default(int initial)
		{
			// Arrange
			Either<int, string> either = initial;

			// Act
			var actual = FromRight(either, "Hola");

			// Assert
			Assert.Equal("Hola", actual);
		}

		[Theory]
		[InlineData("Hello")]
		public void FromLeft_IsRight_Default(string initial)
		{
			// Arrange
			Either<int, string> either = initial;

			// Act
			var actual = FromLeft(either, 1);

			// Assert
			Assert.Equal(1, actual);
		}

		[Theory]
		[InlineData(-1)]
		public void FromLeft_IsLeft_Left(int initial)
		{
			// Arrange
			Either<int, string> either = initial;

			// Act
			var actual = FromLeft(either, 1);

			// Assert
			Assert.Equal(initial, actual);
		}

		[Theory]
		[InlineData("Hello")]
		public void Try_NoException_Right(string initial)
		{
			// Arrange
			// Act
			var actual = Try(() => $"{initial}, World!");

			// Assert
			Assert.IsType<Right<Exception, string>>(actual);
			Assert.Equal("Hello, World!", (string)actual);
		}

		[Fact]
		public void Try_Exception_Exception()
		{
			// Arrange
			// Act
			var actual = Try<string>(() => throw new InvalidOperationException("ERROR_TEST"));

			Assert.IsType<Left<Exception, string>>(actual);
			Assert.IsType<InvalidOperationException>((Exception)actual);
			Assert.Equal("ERROR_TEST", ((Exception)actual).Message);
		}
	}
}
