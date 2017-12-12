using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_2
{
	class Program
	{
		const string Input = "88,88,211,106,141,1,78,254,2,111,77,255,90,0,54,205";

		static void Main(string[] args)
		{
			int[] lengths = Input.Select(ch => (int)ch).Concat(new[] { 17, 31, 73, 47, 23 }).ToArray();

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

			int[] denseHash = sparseHash
				.Select(grouping => grouping.Aggregate((x, y) => x ^ y))
				.ToArray();

			Console.WriteLine(string.Concat(denseHash.Select(ch => ch.ToString("x2"))));
		}
	}
}
