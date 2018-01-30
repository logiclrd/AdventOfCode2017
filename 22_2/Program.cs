using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _22_2
{
	class Program
	{
		const string TestInput =
@"..#
#..
...";

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

		enum NodeState : byte
		{
			Clean,
			Weakened,
			Infected,
			Flagged,
		}

		class Map
		{
			const int PartSize = 25;

			Dictionary<(int x, int y), NodeState[,]> _mapParts = new Dictionary<(int x, int y), NodeState[,]>();

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

			NodeState[,] GetPart(ref int x, ref int y)
			{
				int px = GetPartIndex(ref x);
				int py = GetPartIndex(ref y);

				if (!_mapParts.TryGetValue((px, py), out NodeState[,] ret))
					ret = _mapParts[(px, py)] = new NodeState[PartSize, PartSize];

				return ret;
			}

			public NodeState this[int x, int y]
			{
				get
				{
					NodeState[,] part = GetPart(ref x, ref y);

					return part[x, y];
				}
				set
				{
					NodeState[,] part = GetPart(ref x, ref y);

					part[x, y] = value;
				}
			}
		}

		static NodeState NextState(NodeState state)
		{
			return ((NodeState)(((int)state + 1) % 4));
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
						if (row[column] == '#')
							map[column, numRows] = NodeState.Infected;

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

			for (int iteration = 0; iteration < 10_000_000; iteration++)
			{
				NodeState state = map[position.X, position.Y];

				switch (state)
				{
					case NodeState.Clean: direction += 1; break;
					case NodeState.Infected: direction += 3; break;
					case NodeState.Flagged: direction += 2; break;
				}

				direction %= 4;

				state = NextState(state);

				map[position.X, position.Y] = state;

				if (state == NodeState.Infected)
					infections++;

				position.X += DirectionVectors[direction].X;
				position.Y += DirectionVectors[direction].Y;
			}

			Console.WriteLine("Infections: {0}", infections);
		}
	}
}
