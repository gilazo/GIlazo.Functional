using System;
using Xunit;

namespace Gilazo.Functional
{
    public class LeftTests
    {
        [Theory]
        [InlineData(-1)]
        public void Ctor_Int_Left_Int(int left)
        {
            // Arrange
            // Act
            var actual = new Left<int, string>(left);

            // Assert
            Assert.True(actual.IsLeft);
        }

        [Theory]
        [InlineData(-1)]
        public void Implicit_Int_Left_Int(int left)
        {
            // Arrange
            // Act
            Left<int, string> actual = left;

            // Assert
            Assert.IsType<Left<int, string>>(actual);
        }

        [Theory]
        [InlineData(-1)]
        public void Implicit_Int_Either_Int(int left)
        {
            // Arrange
            // Act
            Either<int, string> actual = left;

            // Assert
            Assert.IsType<Left<int, string>>(actual);
        }

        [Theory]
        [InlineData(-1)]
        public void Implicit_Int_Left_Int_Int(int left)
        {
            // Arrange
            // Act
            Left<int, int> actual = left;

            // Assert
            Assert.IsType<Left<int, int>>(actual);
        }

        [Theory]
        [InlineData(-1)]
        public void Explicit_Int_Either_Int_Int(int left)
        {
            // Arrange
            // Act
            Either<int, int> actual = (Left<int, int>)left;

            // Assert
            Assert.IsType<Left<int, int>>(actual);
        }

        [Theory]
        [InlineData(-1)]
        public void Implicit_Left_Int_Int(int left)
        {
            // Arrange
            var either = new Left<int, string>(left);

            // Act
            int actual = either;

            // Assert
            Assert.Equal(left, actual);
        }

        [Theory]
        [InlineData(-1)]
        public void Implicit_Either_Int_String_Int(int left)
        {
            // Arrange
            Either<int, string> either = new Left<int, string>(left);

            // Act
            int actual = either;

            // Assert
            Assert.Equal(left, actual);
        }

        [Theory]
        [InlineData(-1)]
        public void Match_IsLeft_LeftInvoked(int initial)
        {
            // Arrange
            var either = new Left<int, string>(initial);

            // Act
            // Assert
            either.Match(
                left => throw new InvalidOperationException(left),
                left => Assert.Equal(initial, left)
            );
        }

        [Theory]
        [InlineData(-1)]
        public void MapLeft_IsLeft_LeftInvoked(int initial)
        {
            // Arrange
            var either = new Left<int, string>(initial);

            // Act
            var actual = either.MapLeft<string>(i => i.ToString());

            // Assert
            Assert.IsType<Left<string, string>>(actual);
            Assert.Equal("-1", (string)(Left<string, string>)actual);
        }

        [Theory]
        [InlineData("-1")]
        public void MapLeft_IsRight_LeftNotInvoked(string initial)
        {
            // Arrange
            var either = new Right<int, string>(initial);

            // Act
            var actual = either.MapLeft<string>(i => i.ToString());

            // Assert
            Assert.IsType<Right<string, string>>(actual);
            Assert.Equal(initial, (string)(Right<string, string>)actual);
        }

        [Theory]
        [InlineData(-1)]
        public void BindLeft_IsLeft_LeftFuncsInvoked(int initial)
        {
            // Arrange
            var either = new Left<int, string>(initial);

            static Either<int, string> AddThree(int value) => value + 3;
            static Either<int, string> SubtractThree(int value) => value - 3;
            static Either<int, string> MultiplyNegativeOne(int value) => value*-1;

            // Act
            var actual = either
                .BindLeft(AddThree)
                .BindLeft(SubtractThree)
                .BindLeft(MultiplyNegativeOne);

            // Assert
            Assert.IsType<Left<int, string>>(actual);
            Assert.Equal(1, (int)(Left<int, string>)actual);
        }

        [Theory]
        [InlineData("-1")]
        public void BindLeft_IsRight_LeftFuncsNotInvoked(string initial)
        {
            // Arrange
            var either = new Right<int, string>(initial);

            static Either<int, string> AddThree(int value) => value + 3;
            static Either<int, string> SubtractThree(int value) => value - 3;
            static Either<int, string> MultiplyNegativeOne(int value) => value*-1;

            // Act
            var actual = either
                .BindLeft(AddThree)
                .BindLeft(SubtractThree)
                .BindLeft(MultiplyNegativeOne);

            // Assert
            Assert.IsType<Right<int, string>>(actual);
            Assert.Equal(initial, (string)(Right<int, string>)actual);
        }
    }
}
