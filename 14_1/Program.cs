using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _14_1
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

		static void OutputBits(byte[] data)
		{
			foreach (byte b in data)
				OutputBits(b);

			Console.WriteLine();
		}

		private static void OutputBits(byte b)
		{
			for (int bit = 128; bit > 0; bit >>= 1)
				Console.Write(((b & bit) != 0) ? '#' : '.');
		}

		static int CountBits(byte b)
		{
			int count = 0;

			while (b != 0)
			{
				count++;
				b = unchecked((byte)(b & (b - 1)));
			}

			return count;
		}

		static int CountBits(byte[] bytes)
		{
			return bytes.Sum(b => CountBits(b));
		}

		static void Main(string[] args)
		{
			Console.SetWindowSize(132, 50);

			//const string Input = "flqrgnkx";
			const string Input = "hfdlxzhv";

			int sum = 0;

			for (int row = 0; row < 128; row++)
			{
				string rowInput = Input + "-" + row;

				var rowHash = KnotHash(rowInput);

				sum += CountBits(rowHash);
			}

			Console.WriteLine(sum);
		}
	}
}
