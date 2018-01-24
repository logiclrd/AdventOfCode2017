using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15_1
{
	class Program
	{
		/*
		Input:

		Generator A starts with 289
		Generator B starts with 629
		*/
		class Generator
		{
			public int LastValue;
			public readonly long Factor;

			public Generator(int seed, int factor)
			{
				this.LastValue = seed;
				this.Factor = factor;
			}

			public int GetNextValue()
			{
				return LastValue = (int)(LastValue * Factor % 0x7FFFFFFFL);
			}
		}

		static void Main(string[] args)
		{
			var generatorA = new Generator(289, 16807);
			var generatorB = new Generator(629, 48271);

			int matches = 0;

			for (int iteration = 0; iteration < 40_000_000; iteration++)
			{
				int valueA = generatorA.GetNextValue();
				int valueB = generatorB.GetNextValue();

				if ((valueA & 0xFFFF) == (valueB & 0xFFFF))
					matches++;
			}

			Console.WriteLine(matches);
		}
	}
}
