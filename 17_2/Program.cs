using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17_2
{
	class Program
	{
		static void Main(string[] args)
		{
			// const int SpinSize = 3;
			const int SpinSize = 359;

			int listSize = 1;
			int position = 0;
			int valueInPosition1 = 0;

			for (int i = 1; i <= 50_000_000; i++)
			{
				position = (position + SpinSize) % listSize;

				if (position == 0)
					valueInPosition1 = i;

				position++;
				listSize++;

				if ((i & 8191) == 0)
					Console.Write(i + "\b\b\b\b\b\b\b\b\b\b\b\b");
			}

			Console.WriteLine(valueInPosition1);
		}
	}
}
