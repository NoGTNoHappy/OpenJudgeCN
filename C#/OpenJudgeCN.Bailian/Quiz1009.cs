using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenJudgeCN.Bailian
{
    public class Quiz1009
    {
        public const string Introduce = @"
        描述
        IONU Satellite Imaging, Inc. records and stores very large images using run length encoding. You are to write a program that
        reads a compressed image, finds the edges in the image, as described below, and outputs another compressed image of the detected edges.
        A simple edge detection algorithm sets an output pixel's value to be the maximum absolute value of the differences between it
        and all its surrounding pixels in the input image. Consider the input image below:

             15  15  15  15 100 100 100          85  85  85  85  85   0   0
            100 100 100 100 100 100 100          85  85  85  85  85  75  75
            100 100 100 100 100  25  25          75  75  75  75  75  75  75
            175 175  25  25  25  25  25          75 150 150  75  75  75   0
            175 175  25  25  25  25  25           0 150 150   0   0   0   0

                        input                               output

        The upper left pixel in the output image is the maximum of the values |15-15|,|15-100|, and |15-100|, which is 85.
        The pixel in the 4th row, 2nd column is computed as the maximum of |175-100|, |175-100|, |175-100|, |175-175|, |175-25|, |175-175|,
        |175-175|, and |175-25|, which is 150. Images contain 2 to 1,000,000,000 (109) pixels. All images are encoded using run length encoding (RLE).
        This is a sequence of pairs, containing pixel value (0-255) and run length (1-109). Input images have at most 1,000 of these pairs.
        Successive pairs have different pixel values. All lines in an image contain the same number of pixels.

        输入
        Input consists of information for one or more images. Each image starts with the width, in pixels, of each image line.
        This is followed by the RLE pairs, one pair per line. A line with 0 0 indicates the end of the data for that image.
        An image width of 0 indicates there are no more images to process. The first image in the example input encodes the 5x7 input image above.

        输出
        Output is a series of edge-detected images, in the same format as the input images, except that there may be more than 1,000 RLE pairs.

        样例输入
        7
        15 4
        100 15
        25 2
        175 2
        25 5
        175 2
        25 5
        0 0
        10
        35 500000000
        200 500000000
        0 0
        3
        255 1
        10 1
        255 2
        10 1
        255 2
        10 1
        255 1
        0 0
        0

        样例输出
        7
        85 5
        0 2
        85 5
        75 10
        150 2
        75 3
        0 2
        150 2
        0 4
        0 0
        10
        0 499999990
        165 20
        0 499999990
        0 0
        3
        245 9
        0 0
        0

        提示
        A brute force solution that attempts to compute an output value for every individual pixel will likely fail due to space or time constraints.";

        public static void Test()
        {
            var matrixList = new List<Matrix>();
            while (true)
            {
                var colCount = int.Parse(Console.ReadLine());
                if (colCount == 0) break;
                var originList = new List<string>();
                while (true)
                {
                    var input = Console.ReadLine();
                    if (input == "0 0") break;
                    originList.Add(input);
                }

                var matrix = Matrix.Create(colCount, originList);
                matrixList.Add(matrix);
            }

            Console.WriteLine();

            foreach (var matrix in matrixList)
                Console.WriteLine(matrix.ToOutput().ToString());
        }

        private struct Matrix
        {
            private readonly int _colCount;
            private readonly int _rowCount;
            private readonly byte[,] _matrix;

            public static Matrix Create(int colCount, List<string> originList)
            {
                var list = originList.Select(o =>
                {
                    var splits = o.Split(" ");
                    return Tuple.Create(byte.Parse(splits[0]), int.Parse(splits[1]));
                }).ToList();

                var count = list.Sum(o => o.Item2);
                var rowCount = count / colCount;
                var matrix = new Matrix(rowCount, colCount);
                var rowOffset = 0;
                var colOffset = 0;
                foreach (var t in list)
                    for (var i = 0; i < t.Item2; ++i)
                    {
                        matrix._matrix[rowOffset, colOffset] = t.Item1;
                        ++colOffset;
                        if (colOffset >= colCount)
                        {
                            ++rowOffset;
                            colOffset = 0;
                        }
                    }

                return matrix;
            }

            public Matrix(int rowCount, int colCount)
            {
                _colCount = colCount;
                _rowCount = rowCount;
                _matrix = new byte[rowCount, colCount];
            }

            public Matrix ToOutput()
            {
                var unitArr = new MatrixUnit[_rowCount, _colCount];
                var dic = new Dictionary<MatrixUnit, byte>(MatrixUnit.MatrixUnitEqualityComparer.Instance);
                for (var r = 0; r < _rowCount; r++)
                for (var c = 0; c < _colCount; c++)
                {
                    var rounds = new byte[8];
                    var center = _matrix[r, c];
                    rounds[0] = GetRound(r - 1, c - 1, center);
                    rounds[1] = GetRound(r - 1, c, center);
                    rounds[2] = GetRound(r - 1, c + 1, center);
                    rounds[3] = GetRound(r, c - 1, center);
                    rounds[4] = GetRound(r, c + 1, center);
                    rounds[5] = GetRound(r + 1, c - 1, center);
                    rounds[6] = GetRound(r + 1, c, center);
                    rounds[7] = GetRound(r + 1, c + 1, center);
                    var matrixUnit = MatrixUnit.Create(center, rounds);
                    unitArr[r, c] = matrixUnit;
                    if (dic.TryAdd(matrixUnit, 0))
                        dic[matrixUnit] = matrixUnit.Calculate();
                }

                var res = new Matrix(_rowCount, _colCount);
                for (var r = 0; r < _rowCount; r++)
                for (var c = 0; c < _colCount; c++)
                    res._matrix[r, c] = dic[unitArr[r, c]];

                return res;
            }

            private byte GetRound(int rowPos, int colPos, byte defByte)
            {
                if (rowPos >= 0 && rowPos <= _rowCount - 1 && colPos >= 0 && colPos <= _colCount - 1)
                    return _matrix[rowPos, colPos];

                return defByte;
            }

            public override string ToString()
            {
                var buffer = new StringBuilder();
                buffer.AppendLine(_colCount.ToString());
                byte? current = null;
                var currentCount = 0;
                for (var r = 0; r < _rowCount; r++)
                for (var c = 0; c < _colCount; c++)
                {
                    var innerCurrent = _matrix[r, c];
                    if (current != innerCurrent)
                    {
                        if (current != null)
                            buffer.AppendLine($"{current} {currentCount}");
                        current = innerCurrent;
                        currentCount = 1;
                    }
                    else
                    {
                        ++currentCount;
                    }
                }

                if (currentCount != 1)
                    buffer.AppendLine($"{current} {currentCount}");

                buffer.AppendLine("0 0");
                return buffer.ToString();
            }

            public struct MatrixUnit
            {
                private readonly byte _center;
                private readonly byte[] _rounds;

                public static MatrixUnit Create(byte center, byte[] rounds)
                {
                    var orderedRounds = rounds.Distinct().OrderBy(o => o).ToArray();
                    return new MatrixUnit(center, orderedRounds);
                }

                private MatrixUnit(byte center, byte[] rounds)
                {
                    _center = center;
                    _rounds = rounds;
                }

                public byte Calculate()
                {
                    var center = _center;
                    var res = _rounds.Select(o => Math.Abs(Convert.ToInt16(o) - center)).Max();
                    return Convert.ToByte(res);
                }

                public class MatrixUnitEqualityComparer : IEqualityComparer<MatrixUnit>
                {
                    private MatrixUnitEqualityComparer()
                    {
                    }

                    public static MatrixUnitEqualityComparer Instance => new MatrixUnitEqualityComparer();

                    public bool Equals(MatrixUnit x, MatrixUnit y)
                    {
                        if (x._center != y._center) return false;
                        var xRounds = x._rounds;
                        var yRounds = y._rounds;
                        if (xRounds.Length != yRounds.Length) return false;
                        for (var i = 0; i < xRounds.Length; ++i)
                            if (xRounds[i] != yRounds[i])
                                return false;

                        return true;
                    }

                    public int GetHashCode(MatrixUnit obj)
                    {
                        return obj._center * 10 + obj._rounds.Length;
                    }
                }
            }
        }
    }
}