#region Header
//+ <source name="MatrixTest.cs" language="C#" begin="14-Nov-2020">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2020">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Collections.Generic;
using M64.Engine.Utilities;
using NUnit.Framework;
#endregion

namespace M64.Engine.Test
{
    [TestFixture]
    class MatrixTest
    {
        #region Fields
        private const int Width = 8;
        private const int Height = 8;
        #endregion

        #region Tests
        [Test]
        public void CreateMatrixWithInvalidMinDimension()
        {
            Assert.That(() =>
                new Matrix(Matrix.MinDimensionLength - 1, Matrix.MinDimensionLength - 1),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CreateMatrixWithInvalidMaxDimension()
        {
            Assert.That(() =>
                new Matrix(Matrix.MaxDimensionLength + 1, Matrix.MaxDimensionLength + 1),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ScrambleWithInvalidMagnitude()
        {
            Matrix matrix = new Matrix(Width, Height);
            Assert.That(() => matrix.Scramble(0), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CopyToMatrixWithDifferentDimensions()
        {
            Assert.That(() =>
                new Matrix(3, 3).CopyTo(new Matrix(4, 4)),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void CreateMatrix()
        {
            Matrix matrix = new Matrix(Width, Height);
            Assert.AreEqual(matrix.Width, Width);
            Assert.AreEqual(matrix.Height, Height);
            Assert.IsTrue(matrix.IsSolved);
        }

        [Test]
        public void Clone()
        {
            Matrix matrix1 = new Matrix(Width, Height);
            Matrix matrix2 = (Matrix) matrix1.Clone();

            Assert.AreEqual(matrix1, matrix2);
            Assert.AreNotSame(matrix1, matrix2);
        }

        [Test]
        public void Copy()
        {
            Matrix matrix1 = new Matrix(Width, Height);
            Matrix matrix2 = new Matrix(Width, Height);

            for (int y = 0; y < matrix1.Height; y++) {
                for (int x = 0; x < matrix1.Width; x++) {
                    matrix1[x, y] = RandomUtility.GetRandomInt();
                    matrix2[x, y] = RandomUtility.GetRandomInt();
                }
            }

            matrix1.CopyTo(matrix2);
            Assert.AreEqual(matrix1, matrix2);
        }

        [Test]
        public void Equal()
        {
            Matrix matrix1 = new Matrix(Width, Height);
            Matrix matrix2 = new Matrix(Width, Height);
            int value;

            for (int y = 0; y < matrix1.Height; y++) {
                for (int x = 0; x < matrix1.Width; x++) {
                    value = RandomUtility.GetRandomInt();
                    matrix1[x, y] = value;
                    matrix2[x, y] = value;
                }
            }
            
            Assert.AreEqual(matrix1, matrix2);
        }

        [Test]
        public void NotEqual()
        {
            Matrix matrix1 = new Matrix(Width, Height);
            Matrix matrix2 = new Matrix(Width, Height);

            for (int y = 0; y < matrix1.Height; y++) {
                for (int x = 0; x < matrix1.Width; x++) {
                    matrix1[x, y] = RandomUtility.GetRandomInt();
                    matrix2[x, y] = RandomUtility.GetRandomInt();
                }
            }

            Assert.AreNotEqual(matrix1, matrix2);
        }

        [Test]
        public void GetOrder()
        {
            Matrix matrix = new Matrix(Width, Height);

            for (int y = 0; y < matrix.Height; y++) {
                for (int x = 0; x < matrix.Width; x++) {
                    Assert.AreEqual(x + (y * matrix.Width), matrix.GetOrder(new Position(x, y))); 
                }
            }
        }

        [Test]
        public void GetInvalidPosition()
        {
            Matrix matrix = new Matrix(Width, Height);
            Assert.That(() =>
                matrix.GetPosition((Width * Height) + 1),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetPosition()
        {
            Matrix matrix = new Matrix(Width, Height);

            Position startPosition = new Position(Width - 1, 0);
            int order = matrix.GetOrder(startPosition);
            matrix.MoveFrom(startPosition);

            Assert.AreEqual(new Position(startPosition.X, startPosition.Y + 1), matrix.GetPosition(order));
        }

        [Test]
        public void GetIterator()
        {
            Matrix matrix = new Matrix(Width, Height);

            for (int y = 0; y < matrix.Height; y++) {
                for (int x = 0; x < matrix.Width; x++) {
                    matrix[x, y] = x + (y * matrix.Width);
                }
            }

            int value = 0;
            foreach (int i in matrix) {
                Assert.AreEqual(value++, i);
            }
        }

        [Test]
        public void GetPossibleStartPositions()
        {
            Matrix matrix = new Matrix(Width, Height);
            matrix.MoveFrom(new Position(Width - 2, Height -1));
            matrix.MoveFrom(new Position(Width - 2, Height - 2));

            List<Position> possibleStartPositions = matrix.GetPossibleStartPositions(true);
            Assert.AreEqual(4, possibleStartPositions.Count);

            possibleStartPositions = matrix.GetPossibleStartPositions();
            Assert.AreEqual((Width - 1) + (Height - 1), possibleStartPositions.Count);
        }

        [Test]
        public void Move()
        {
            Matrix matrix = new Matrix(Width, Height);

            // Move left.
            Position startPosition = new Position(Width / 2, Height - 1);
            Assert.IsTrue(matrix.MoveFrom(startPosition));
            Assert.AreEqual(startPosition, matrix.BlankPosition);

            startPosition = new Position(0, Height - 1);
            Assert.IsTrue(matrix.MoveFrom(startPosition));
            Assert.AreEqual(matrix.BlankPosition, startPosition);

            // Move up.
            startPosition = new Position(0, Height / 2);
            Assert.IsTrue(matrix.MoveFrom(startPosition));
            Assert.AreEqual(matrix.BlankPosition, startPosition);

            startPosition = new Position(0, 0);
            Assert.IsTrue(matrix.MoveFrom(startPosition));
            Assert.AreEqual(matrix.BlankPosition, startPosition);

            // Move right.
            startPosition = new Position(Width / 2, 0);
            Assert.IsTrue(matrix.MoveFrom(startPosition));
            Assert.AreEqual(matrix.BlankPosition, startPosition);

            startPosition = new Position(Width - 1, 0);
            Assert.IsTrue(matrix.MoveFrom(startPosition));
            Assert.AreEqual(matrix.BlankPosition, startPosition);
            
            // Move down.
            startPosition = new Position(Width - 1, Height / 2);
            Assert.IsTrue(matrix.MoveFrom(startPosition));
            Assert.AreEqual(matrix.BlankPosition, startPosition);

            startPosition = new Position(Width - 1, Height - 1);
            Assert.IsTrue(matrix.MoveFrom(startPosition));
            Assert.AreEqual(matrix.BlankPosition, startPosition);
        }

        [Test]
        public void OnMoveHorizontaly()
        {
            Matrix matrix = new Matrix(Width, Height);
            Position startPosition = new Position(0, Height - 1);
            Position blankPosition = matrix.BlankPosition;
            int currentPosition = blankPosition.X;

            EventHandler<ElementMovedEventArgs> onElementMoved = delegate(Object sender, ElementMovedEventArgs args)
            {
                Assert.AreEqual(currentPosition, args.NewPosition.X);
                Assert.AreEqual(currentPosition - 1, args.OldPosition.X);
                Assert.AreEqual(args.NewPosition.Y, args.OldPosition.Y);
                Assert.IsFalse(args.IsScrambling);

                if (args.NewPosition.X == blankPosition.X && args.NewPosition.X == startPosition.X) {
                    Assert.IsTrue(args.IsFirst);
                }

                if (args.NewPosition.X == startPosition.X + 1) {
                    Assert.IsTrue(args.IsLast);
                }

                currentPosition--;
            };

            matrix.ElementMoved += onElementMoved;
            matrix.MoveFrom(startPosition);
            matrix.ElementMoved -= onElementMoved;
        }

        [Test]
        public void OnMoveVertically()
        {
            Matrix matrix = new Matrix(Width, Height);
            Position startPosition = new Position(Width - 1, 0);
            Position blankPosition = matrix.BlankPosition;
            int currentPosition = blankPosition.Y;

            EventHandler<ElementMovedEventArgs> onElementMoved = delegate(Object sender, ElementMovedEventArgs args)
            {
                Assert.AreEqual(currentPosition, args.NewPosition.Y);
                Assert.AreEqual(currentPosition - 1, args.OldPosition.Y);
                Assert.AreEqual(args.NewPosition.X, args.OldPosition.X);
                Assert.IsFalse(args.IsScrambling);

                if (args.NewPosition.Y == startPosition.Y && args.NewPosition.Y == blankPosition.Y) {
                    Assert.IsTrue(args.IsFirst);
                }

                if (args.NewPosition.Y == startPosition.Y + 1) {
                    Assert.IsTrue(args.IsLast);
                }

                currentPosition--;
            };

            matrix.ElementMoved += onElementMoved;
            matrix.MoveFrom(startPosition);
        }

        [Test]
        public void Scramble()
        {
            Matrix matrix = new Matrix(Width, Height);
            matrix.ElementMoved += delegate(Object sender, ElementMovedEventArgs args)
            {
                Assert.IsTrue(args.IsScrambling);
            };

            for (int y = 0; y < matrix.Height; y++) {
                for (int x = 0; x < matrix.Width; x++) {
                    matrix[x, y] = x + (y * matrix.Width);
                }
            }

            matrix.Scramble(5);
            Assert.IsFalse(matrix.IsSolved);

            bool[] flags = new bool[Height * Width];
            for (int i = 0; i < flags.Length; i++) flags[i] = false;

            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) flags[(int) matrix[x, y]] = true;
            }

            for (int i = 0; i < flags.Length; i++) {
                if (!flags[i]) {
                    Assert.Fail("Element " + i + " not found.");
                    break;
                }
            }

            Assert.Pass();
        }
        #endregion
    }
}
