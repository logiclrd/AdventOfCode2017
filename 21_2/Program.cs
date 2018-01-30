using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21_2
{
	class Program
	{
		static readonly bool[,] StartingPattern =
			new bool[3, 3]
			{
				{ false, false, true },
				{ true, false, true },
				{ false, true, true },
			};

		const string TestRules =
@"../.# => ##./#../...
.#./..#/### => #..#/..../..../#..#";

		const string Rules =
@"../.. => .../.##/##.
#./.. => .##/.##/#..
##/.. => ..#/.../###
.#/#. => #.#/..#/##.
##/#. => .#./.#./..#
##/## => #.#/#../###
.../.../... => ..../#.../.##./..#.
#../.../... => ####/#.##/##.#/..#.
.#./.../... => ..##/..##/..##/..##
##./.../... => ..../..#./##../##.#
#.#/.../... => ##.#/..../####/...#
###/.../... => .#.#/.###/.#../.#.#
.#./#../... => .###/#.#./...#/##..
##./#../... => #.##/#.../####/###.
..#/#../... => ####/...#/...#/#.##
#.#/#../... => .#../##../..##/..#.
.##/#../... => .#../..##/..../.##.
###/#../... => #.../..#./.#.#/#..#
.../.#./... => #.#./.#.#/.###/...#
#../.#./... => ###./.#../...#/.#..
.#./.#./... => ##.#/.#../#..#/##..
##./.#./... => #..#/...#/.#.#/###.
#.#/.#./... => .##./#.../#..#/.###
###/.#./... => .#.#/##.#/..../##.#
.#./##./... => ##.#/#.##/.#.#/#.##
##./##./... => #.##/..#./..#./.##.
..#/##./... => ..../#.../..#./..##
#.#/##./... => .##./####/####/####
.##/##./... => #.##/####/#.##/#..#
###/##./... => .#../.###/##../...#
.../#.#/... => ...#/...#/#.##/####
#../#.#/... => ..#./..#./###./.##.
.#./#.#/... => .##./##../.###/.#.#
##./#.#/... => #.#./.#../.##./...#
#.#/#.#/... => ##.#/..##/#.../##.#
###/#.#/... => ..##/##../.#.#/..##
.../###/... => .#../#.../.##./....
#../###/... => ..##/..##/...#/.##.
.#./###/... => #..#/..#./#.#./..##
##./###/... => #.##/.#../##.#/##.#
#.#/###/... => ####/###./.##./...#
###/###/... => #..#/#.##/..../.##.
..#/.../#.. => #.#./.#../##../..#.
#.#/.../#.. => ##.#/####/##../.#.#
.##/.../#.. => ####/##../#..#/..#.
###/.../#.. => ##../..#./####/##.#
.##/#../#.. => ##../#.#./###./..##
###/#../#.. => ..../.#../#..#/...#
..#/.#./#.. => ..#./...#/.###/.#.#
#.#/.#./#.. => ###./..../#.#./###.
.##/.#./#.. => ####/#.##/.#.#/.#..
###/.#./#.. => ###./#.##/##../####
.##/##./#.. => ##.#/..##/..#./.#..
###/##./#.. => ##.#/.##./.###/.##.
#../..#/#.. => #.../###./##.#/#..#
.#./..#/#.. => ..##/.###/...#/..#.
##./..#/#.. => ##../#.#./...#/.#..
#.#/..#/#.. => ..#./###./##../.###
.##/..#/#.. => #.../.##./..../#.#.
###/..#/#.. => .#.#/#.##/#.##/..#.
#../#.#/#.. => ..##/..##/#.../####
.#./#.#/#.. => #.../...#/..../..##
##./#.#/#.. => ###./..##/.#../.##.
..#/#.#/#.. => ...#/..##/..#./.#..
#.#/#.#/#.. => #.#./.#../..../##..
.##/#.#/#.. => ..#./.###/##.#/....
###/#.#/#.. => #.##/..##/...#/##..
#../.##/#.. => #.#./##../###./.#.#
.#./.##/#.. => .###/#..#/.##./....
##./.##/#.. => .#.#/.#../.###/.##.
#.#/.##/#.. => .#../..##/###./#.##
.##/.##/#.. => ##../.##./..#./.#..
###/.##/#.. => .#.#/..#./#..#/.###
#../###/#.. => #.##/#..#/.#.#/#.#.
.#./###/#.. => #.../#..#/#.../.#.#
##./###/#.. => ##../####/##../.###
..#/###/#.. => #.../..../####/##.#
#.#/###/#.. => ...#/..../...#/..##
.##/###/#.. => .#../####/#.##/.#..
###/###/#.. => ###./.#.#/#.../##..
.#./#.#/.#. => ...#/##../####/...#
##./#.#/.#. => ####/#..#/###./#.##
#.#/#.#/.#. => .###/#..#/..#./...#
###/#.#/.#. => ###./.###/##.#/###.
.#./###/.#. => #..#/#.../..#./####
##./###/.#. => #.../..../#..#/..##
#.#/###/.#. => #..#/.#.#/#.../##..
###/###/.#. => .#.#/..../.#.#/#.##
#.#/..#/##. => .#../..##/...#/###.
###/..#/##. => .###/..#./##.#/##.#
.##/#.#/##. => ####/#.##/.##./##..
###/#.#/##. => #..#/#..#/####/#.##
#.#/.##/##. => .###/#.#./#..#/.#.#
###/.##/##. => #.#./#.#./#.##/..##
.##/###/##. => ####/###./##.#/##.#
###/###/##. => ##../..##/#.#./#...
#.#/.../#.# => .#../###./.###/##.#
###/.../#.# => ..../.#.#/#..#/##..
###/#../#.# => ..#./#.../.##./...#
#.#/.#./#.# => ...#/#.../##.#/.##.
###/.#./#.# => ..../..../#.#./##.#
###/##./#.# => .#../...#/...#/###.
#.#/#.#/#.# => ...#/#.../##../.###
###/#.#/#.# => #.../...#/.#../#.##
#.#/###/#.# => ..../.##./..../##..
###/###/#.# => .##./.#.#/#.##/.##.
###/#.#/### => #.#./####/.##./.##.
###/###/### => .#.#/..##/#.##/.##.";

		static List<bool> ParsePattern(string pattern)
		{
			return pattern.Where(ch => (ch == '.') || (ch == '#')).Select(ch => ch == '#').ToList();
		}

		static void FlipHorizontal(List<bool> pattern)
		{
			int size = (int)Math.Sqrt(pattern.Count);

			if (size * size != pattern.Count)
				throw new Exception("Pattern size failure");

			void Swap(int i, int j)
			{
				bool tmp = pattern[i];
				pattern[i] = pattern[j];
				pattern[j] = tmp;
			}

			for (int row = 0; row < size; row++)
			{
				int rowStartOffset = row * size;
				int rowEndOffset = rowStartOffset + size - 1;

				for (int column = 0; column < size; column += 2)
				{
					Swap(rowStartOffset, rowEndOffset);

					rowStartOffset++;
					rowEndOffset--;
				}
			}
		}

		static void Rotate90(List<bool> pattern)
		{
			void Rotate(int a, int b, int c, int d)
			{
				bool tmp = pattern[a];
				pattern[a] = pattern[b];
				pattern[b] = pattern[c];
				pattern[c] = pattern[d];
				pattern[d] = tmp;
			}

			if (pattern.Count == 4)
				Rotate(0, 1, 3, 2);
			else if (pattern.Count == 9)
			{
				Rotate(0, 2, 8, 6);
				Rotate(1, 5, 7, 3);
			}
			else
				throw new Exception("Unsupported size");
		}

		static ushort ToUInt16(IList<bool> pattern)
		{
			ushort accumulator = 0;

			for (int i = 0; i < pattern.Count; i++)
			{
				accumulator += accumulator;

				if (pattern[i])
					accumulator++;
			}

			return accumulator;
		}

		static void Main(string[] args)
		{
			var patterns2to3 = new Dictionary<ushort, ushort>();
			var patterns3to4 = new Dictionary<ushort, ushort>();

			using (var reader = new StringReader(Rules))
			{
				while (true)
				{
					var line = reader.ReadLine();

					if (line == null)
						break;

					int separator = line.IndexOf(" => ");

					if (separator < 0)
						throw new Exception("Parse error");

					var from = ParsePattern(line.Substring(0, separator));
					var to = ParsePattern(line.Substring(separator + 4));

					Dictionary<ushort, ushort> stash;

					if ((from.Count == 4) && (to.Count == 9))
						stash = patterns2to3;
					else if ((from.Count == 9) && (to.Count == 16))
						stash = patterns3to4;
					else
						throw new Exception("Parse error");

					for (int i = 0; i < 4; i++)
					{
						stash[ToUInt16(from)] = ToUInt16(to);

						FlipHorizontal(from);
						stash[ToUInt16(from)] = ToUInt16(to);
						FlipHorizontal(from);

						if (i < 3)
							Rotate90(from);
					}
				}
			}

			bool[,] picture = StartingPattern;

			Console.WriteLine("Start:");

			for (int row = 0; row < 3; row++)
			{
				Console.Write("  ");

				for (int column = 0; column < 3; column++)
					Console.Write(picture[column, row] ? '#' : '.');

				Console.WriteLine();
			}

			Console.WriteLine();

			for (int iteration = 0; iteration < 18; iteration++)
			{
				int size = picture.GetLength(0);

				int lookupSize;
				Dictionary<ushort, ushort> replacements;

				if ((size & 1) == 0)
				{
					lookupSize = 2;
					replacements = patterns2to3;
				}
				else
				{
					lookupSize = 3;
					replacements = patterns3to4;
				}

				int outputSize = lookupSize + 1;

				int lookups = size / lookupSize;

				int resultSize = lookups * outputSize;

				bool[,] newPicture = new bool[resultSize, resultSize];
				bool[] lookup = new bool[lookupSize * lookupSize];

				for (int lookupX = 0; lookupX < lookups; lookupX++)
					for (int lookupY = 0; lookupY < lookups; lookupY++)
					{
						int offsetX = lookupX * lookupSize;
						int offsetY = lookupY * lookupSize;

						for (int y = 0, o = 0; y < lookupSize; y++)
							for (int x = 0; x < lookupSize; x++, o++)
								lookup[o] = picture[offsetX + x, offsetY + y];

						var lookupValue = ToUInt16(lookup);

						var outputPattern = replacements[lookupValue];

						offsetX = lookupX * outputSize;
						offsetY = lookupY * outputSize;

						for (int y = outputSize - 1; y >= 0; y--)
							for (int x = outputSize - 1; x >= 0; x--, outputPattern >>= 1)
								newPicture[offsetX + x, offsetY + y] = (outputPattern & 1) > 0;
					}

				picture = newPicture;

				Console.WriteLine("After iteration {0}:", iteration);

				if (resultSize > 75)
					Console.WriteLine("  ({0} dots, {1} of which are on)", picture.Length, picture.Cast<bool>().Sum(x => x ? 1 : 0));
				else
				{
					for (int row = 0; row < resultSize; row++)
					{
						Console.Write("  ");

						for (int column = 0; column < resultSize; column++)
							Console.Write(picture[column, row] ? '#' : '.');

						Console.WriteLine();
					}
				}

				Console.WriteLine();
			}

			Console.WriteLine("Set pixels: {0}", picture.Cast<bool>().Sum(x => x ? 1 : 0));
		}
	}
}
