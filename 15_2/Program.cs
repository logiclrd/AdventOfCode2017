using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15_2
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
			public readonly int UnacceptableBitMask;

			public Generator(int seed, int factor, int unacceptableBitMask)
			{
				this.LastValue = seed;
				this.Factor = factor;
				this.UnacceptableBitMask = unacceptableBitMask;
			}

			public int GetNextValue()
			{
				return LastValue = (int)(LastValue * Factor % 0x7FFFFFFFL);
			}

			public int GetNextAcceptableValue()
			{
				while (true)
				{
					int value = GetNextValue();

					if ((value & UnacceptableBitMask) == 0)
						return value;
				}
			}
		}

		static void Main(string[] args)
		{
			var generatorA = new Generator(289, 16807, unacceptableBitMask: 3);
			var generatorB = new Generator(629, 48271, unacceptableBitMask: 7);

			int matches = 0;

			for (int iteration = 0; iteration < 5_000_000; iteration++)
			{
				int valueA = generatorA.GetNextAcceptableValue();
				int valueB = generatorB.GetNextAcceptableValue();

				if ((valueA & 0xFFFF) == (valueB & 0xFFFF))
					matches++;
			}

			Console.WriteLine(matches);
		}
	}
}
