using System;
using Xunit;

namespace Gilazo.Functional
{
    public class RightTests
    {
        [Theory]
        [InlineData("Hello, World!")]
        public void Ctor_String_Right_String(string right)
        {
            // Arrange
            // Act
            var actual = new Right<int, string>(right);

            // Assert
            Assert.True(actual.IsRight);
        }

        [Theory]
        [InlineData("Hello, World!")]
        public void Implicit_String_Right_String(string right)
        {
            // Arrange
            // Act
            Right<int, string> actual = right;

            // Assert
            Assert.IsType<Right<int, string>>(actual);
        }

        [Theory]
        [InlineData("Hello, World!")]
        public void Implicit_String_Either_String(string right)
        {
            // Arrange
            // Act
            Either<int, string> actual = right;

            // Assert
            Assert.IsType<Right<int, string>>(actual);
        }

        [Theory]
        [InlineData("Hello, World!")]
        public void Implicit_String_Right_String_String(string right)
        {
            // Arrange
            // Act
            Right<string, string> actual = right;

            // Assert
            Assert.IsType<Right<string, string>>(actual);
        }

        [Theory]
        [InlineData("Hello, World!")]
        public void Explicit_String_Either_String_String(string right)
        {
            // Arrange
            // Act
            Either<string, string> actual = (Right<string, string>)right;

            // Assert
            Assert.IsType<Right<string, string>>(actual);
        }

        [Theory]
        [InlineData("Hello, World!")]
        public void Implicit_Right_String_String(string right)
        {
            // Arrange
            var either = new Right<int, string>(right);

            // Act
            string actual = either;

            // Assert
            Assert.Equal(right, actual);
        }

        [Theory]
        [InlineData("Hello, World!")]
        public void Implicit_Either_Int_String_String(string right)
        {
            // Arrange
            Either<int, string> either = new Right<int, string>(right);

            // Act
            string actual = either;

            // Assert
            Assert.Equal(right, actual);
        }

        [Theory]
        [InlineData("Hello, World!")]
        public void Match_IsRight_RightInvoked(string initial)
        {
            // Arrange
            var either = new Right<int, string>(initial);

            // Act
            //Assert
            either.Match(
                right => Assert.Equal(initial, right),
                left => throw new InvalidOperationException(left.ToString())
            );
        }

        [Theory]
        [InlineData("-1")]
        public void Map_IsRight_RightInvoked(string initial)
        {
            // Arrange
            var either = new Right<int, string>(initial);

            // Act
            var actual = either.Map<int>(i => Convert.ToInt32(i));

            // Assert
            Assert.IsType<Right<int, int>>(actual);
            Assert.Equal(-1, (int)(Right<int, int>)actual);
        }

        [Theory]
        [InlineData(-1)]
        public void Map_IsLeft_RightNotInvoked(int initial)
        {
            // Arrange
            var either = new Left<int, string>(initial);

            // Act
            var actual = either.Map<int>(i => Convert.ToInt32(i));

            // Assert
            Assert.IsType<Left<int, int>>(actual);
            Assert.Equal(initial, (int)(Left<int, int>)actual);
        }

        [Theory]
        [InlineData("Hello")]
        public void Bind_IsRight_RightFuncsInvoked(string initial)
        {
            // Arrange
            var either = new Right<int, string>(initial);

            static Either<int, string> AppendComma(string value) => $"{value},";
            static Either<int, string> AppendSpace(string value) => $"{value} ";
            static Either<int, string> AppendWorld(string value) => $"{value}World";

            // Act
            var actual = either
                .Bind(AppendComma)
                .Bind(AppendSpace)
                .Bind(AppendWorld);

            // Assert
            Assert.IsType<Right<int, string>>(actual);
            Assert.Equal("Hello, World", (string)(Right<int, string>)actual);
        }

        [Theory]
        [InlineData(-1)]
        public void Bind_IsLeft_RightFuncsNotInvoked(int initial)
        {
            // Arrange
            var either = new Left<int, string>(initial);

            static Either<int, string> AppendComma(string value) => $"{value},";
            static Either<int, string> AppendSpace(string value) => $"{value} ";
            static Either<int, string> AppendWorld(string value) => $"{value}World";

            // Act
            var actual = either
                .Bind(AppendComma)
                .Bind(AppendSpace)
                .Bind(AppendWorld);

            // Assert
            Assert.IsType<Left<int, string>>(actual);
            Assert.Equal(initial, (int)(Left<int, string>)actual);
        }

        // [Theory]
        // [InlineData("82")]
        // public void Map_IsRight_RightInvoked(string initial)
        // {
        // 	// Arrange
        // 	var either = new Either<int, string>(initial);

        // 	// Act
        // 	var actual = either.Map(right => Convert.ToInt32(right), left => left.ToString());

        // 	// Assert
        // 	Assert.True(actual.IsRight);
        // 	Assert.IsType<Either<string, int>>(actual);
        // 	Assert.Equal(Convert.ToInt32(initial), actual.Match(right => right, left => Convert.ToInt32(left)));
        // }

        // [Theory]
        // [InlineData(-1)]
        // public void Map_IsLeft_LeftInvoked(int initial)
        // {
        // 	// Arrange
        // 	var either = new Either<int, string>(initial);

        // 	// Act
        // 	var actual = either.Map(right => Convert.ToInt32(right), left => left.ToString());

        // 	// Assert
        // 	Assert.True(actual.IsLeft);
        // 	Assert.IsType<Either<string, int>>(actual);
        // 	Assert.Equal(initial, actual.Match(right => right, left => Convert.ToInt32(left)));
        // }

        // [Theory]
        // [InlineData("Hello")]
        // public void Bind_IsRight_RightInvoked(string initial)
        // {
        // 	// Arrange
        // 	var either = new Either<int, string>(initial);

        // 	// Act
        // 	var actual = either.Bind(right => new Either<int, string>($"{right}, World!"), left => new Either<int, string>(left));

        // 	// Assert
        // 	Assert.True(actual.IsRight);
        // 	Assert.Equal("Hello, World!", actual.Match(right => right, left => left.ToString()));
        // }

        // [Theory]
        // [InlineData(-1)]
        // public void Bind_IsLeft_LeftInvoked(int initial)
        // {
        // 	// Arrange
        // 	var either = new Either<int, string>(initial);

        // 	// Act
        // 	var actual = either.Bind(right => new Either<int, string>($"{right}, World!"), left => new Either<int, string>(left + 1));

        // 	// Assert
        // 	Assert.True(actual.IsLeft);
        // 	Assert.Equal(0, actual.Match(right => Convert.ToInt32(right), left => left));
        // }
    }
}
