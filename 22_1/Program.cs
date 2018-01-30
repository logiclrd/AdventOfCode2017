using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _22_1
{
	class Program
	{
		const string Input =
@"#..#...##.#.###.#.#.#...#
.####....#..##.#.##....##
...#..#.#.#......##..#..#
##.####.#.##........#...#
##.#....##..#.####.###...
#..#..###...#.#..##..###.
.##.##..#.####.#.#.....##
#....#......######...#...
..#.#.##.#..#...##.#.####
#.#..#.....#..####.#.#.##
...##.#..##.###.###......
###..#.####.#..#####..#..
...##.##.#.###.#..##.#.##
.####.#.##.#####.##.##..#
#.##.#...##.#.###.###..#.
..#.#..#..#..##..###...##
##.##.#..#.###....###..##
.#...###..#####.#..#.#.##
....##..####.##...#..#.##
#..#..###..#..###...#..##
.##.#.###..####.#.#..##.#
..###.#....#...###...##.#
.#...##.##.####...##.####
###.#.#.####.##.###..#...
#.#######.#######..##.#.#";

		class Map
		{
			const int PartSize = 25;

			Dictionary<(int x, int y), bool[,]> _mapParts = new Dictionary<(int x, int y), bool[,]>();

			int GetPartIndex(ref int x)
			{
				try
				{
					if (x >= 0)
						return x / PartSize;
					else
						return (x - PartSize + 1) / PartSize;
				}
				finally
				{
					x = ((x % PartSize) + PartSize) % PartSize;
				}
			}

			bool[,] GetPart(ref int x, ref int y)
			{
				int px = GetPartIndex(ref x);
				int py = GetPartIndex(ref y);

				if (!_mapParts.TryGetValue((px, py), out bool[,] ret))
					ret = _mapParts[(px, py)] = new bool[PartSize, PartSize];

				return ret;
			}

			public bool this[int x, int y]
			{
				get
				{
					bool[,] part = GetPart(ref x, ref y);

					return part[x, y];
				}
				set
				{
					bool[,] part = GetPart(ref x, ref y);

					part[x, y] = value;
				}
			}
		}

		static void Main(string[] args)
		{
			var map = new Map();

			(int X, int Y) position = (0, 0);
			int direction = 0;

			int numRows = 0;

			using (var reader = new StringReader(Input))
			{
				while (true)
				{
					var row = reader.ReadLine();

					if (row == null)
						break;

					position.X = row.Length / 2;
					position.Y += (numRows & 1);

					for (int column = 0; column < row.Length; column++)
						map[column, numRows] = row[column] == '#';

					numRows++;
				}
			}

			(int X, int Y)[] DirectionVectors =
				new(int X, int Y)[]
				{
					(0, -1),
					(-1, 0),
					(0, +1),
					(+1, 0),
				};

			int infections = 0;

			for (int iteration = 0; iteration < 10000; iteration++)
			{
				bool isInfected = map[position.X, position.Y];

				direction = (direction + (isInfected ? 3 : 1)) % 4;

				if (map[position.X, position.Y] = !isInfected)
					infections++;

				position.X += DirectionVectors[direction].X;
				position.Y += DirectionVectors[direction].Y;
			}

			Console.WriteLine("Infections: {0}", infections);
		}
	}
}
