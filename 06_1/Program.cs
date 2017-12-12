using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BankKey = System.Tuple<int, int, int, int, int, int, int, System.Tuple<int, int, int, int, int, int, int, System.Tuple<int, int>>>;

namespace _06_1
{
	class Program
	{
		const string Input = "4	1	15	12	0	9	9	5	5	8	7	3	14	5	12	3";

		static void Main(string[] args)
		{
			int[] banks = Input.Split('\t').Select(v => int.Parse(v)).ToArray();

			HashSet<BankKey> seen = new HashSet<BankKey>();

			int iterations = 0;

			while (seen.Add(ToKey(banks)))
			{
				Iterate(banks);
				iterations++;
			}

			Console.WriteLine(iterations);
		}

		static void Iterate(int[] banks)
		{
			int largestBankIndex = 0;

			for (int i = 1; i < banks.Length; i++)
				if (banks[i] > banks[largestBankIndex])
					largestBankIndex = i;

			int redistribute = banks[largestBankIndex];
			int index = largestBankIndex;

			banks[largestBankIndex] = 0;

			while (redistribute > 0)
			{
				index = (index + 1) % banks.Length;
				banks[index]++;
				redistribute--;
			}
		}

		static BankKey ToKey(int[] items)
		{
			return new BankKey(
				items[0],
				items[1],
				items[2],
				items[3],
				items[4],
				items[5],
				items[6],
				new Tuple<int, int, int, int, int, int, int, Tuple<int, int>>(
					items[7],
					items[8],
					items[9],
					items[10],
					items[11],
					items[12],
					items[13],
					Tuple.Create(
						items[14],
						items[15])));
		}
	}
}
