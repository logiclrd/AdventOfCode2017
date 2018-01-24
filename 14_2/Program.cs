using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _14_2
{
	class Program
	{
		static byte[] KnotHash(IEnumerable<byte> input)
		{
			byte[] lengths = input.Concat(new byte[] { 17, 31, 73, 47, 23 }).ToArray();

			int[] list = Enumerable.Range(0, 256).ToArray();

			int position = 0;
			int skip = 0;

			for (int round = 0; round < 64; round++)
			{
				foreach (var length in lengths)
				{
					int swapFrom = position;
					int swapTo = (position + length - 1) % list.Length;

					for (int i = 1; i < length; i += 2)
					{
						int tmp = list[swapFrom];
						list[swapFrom] = list[swapTo];
						list[swapTo] = tmp;

						swapFrom = (swapFrom + 1) % list.Length;
						swapTo = (swapTo + list.Length - 1) % list.Length;
					}

					position = (position + length + skip) % list.Length;
					skip++;
				}
			}

			int[][] sparseHash = list
				.Select((value, idx) => Tuple.Create(idx / 16, value))
				.GroupBy(tuple => tuple.Item1)
				.Select(grouping => grouping.Select(item => item.Item2).ToArray())
				.ToArray();

			byte[] denseHash = sparseHash
				.Select(grouping => grouping.Aggregate((x, y) => x ^ y))
				.Select(i => (byte)i)
				.ToArray();

			return denseHash;
		}

		static byte[] KnotHash(string input)
		{
			return KnotHash(Encoding.ASCII.GetBytes(input));
		}

		private static void PaintRegion(int[][] bitmap, int row, int column, int regionNumber)
		{
			var queue = new Queue<(int Row, int Column)>();

			queue.Enqueue((row, column));

			while (queue.Count > 0)
			{
				var coordinate = queue.Dequeue();

				if (bitmap[coordinate.Row][coordinate.Column] == -1)
				{
					bitmap[coordinate.Row][coordinate.Column] = regionNumber;

					if (coordinate.Row > 0)
						queue.Enqueue((coordinate.Row - 1, coordinate.Column));
					if (coordinate.Row < 127)
						queue.Enqueue((coordinate.Row + 1, coordinate.Column));
					if (coordinate.Column > 0)
						queue.Enqueue((coordinate.Row, coordinate.Column - 1));
					if (coordinate.Column < 127)
						queue.Enqueue((coordinate.Row, coordinate.Column + 1));
				}
			}
		}

		static void Main(string[] args)
		{
			Console.SetWindowSize(132, 50);

			//const string Input = "flqrgnkx";
			const string Input = "hfdlxzhv";

			int sum = 0;

			int[][] bitmap = new int[128][];

			for (int row = 0; row < 128; row++)
			{
				string rowInput = Input + "-" + row;

				var rowHash = KnotHash(rowInput);

				bitmap[row] = rowHash
					.SelectMany(sector => Convert.ToString(sector | 256, 2).Substring(1).Select(ch => '0' - ch))
					.ToArray();
			}

			int regionNumber = 0;

			for (int row = 0; row < 128; row++)
			{
				for (int column = 0; column < 128; column++)
				{
					if (bitmap[row][column] == -1)
					{
						regionNumber++;
						PaintRegion(bitmap, row, column, regionNumber);
					}
				}
			}

			Console.WriteLine(regionNumber);
		}
	}
}
