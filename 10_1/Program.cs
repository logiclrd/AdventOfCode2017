using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_1
{
	class Program
	{
		const string Input = "88,88,211,106,141,1,78,254,2,111,77,255,90,0,54,205";

		static void Main(string[] args)
		{
			int[] lengths = Input.Split(',').Select(s => int.Parse(s)).ToArray();

			int[] list = Enumerable.Range(0, 256).ToArray();

			int position = 0;
			int skip = 0;

			foreach (var length in lengths)
			{
				int swapFrom = position;
				int swapTo = (position + length - 1) % list.Length;

				for (int i=1; i < length; i += 2)
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

			Console.WriteLine(list[0] * list[1]);
		}
	}
}
