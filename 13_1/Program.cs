using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _13_1
{
	class Program
	{
		const string Input = @"0: 3
1: 2
2: 4
4: 6
6: 4
8: 6
10: 5
12: 8
14: 8
16: 6
18: 8
20: 6
22: 10
24: 8
26: 12
28: 12
30: 8
32: 12
34: 8
36: 14
38: 12
40: 18
42: 12
44: 12
46: 9
48: 14
50: 18
52: 10
54: 14
56: 12
58: 12
60: 14
64: 14
68: 12
70: 17
72: 14
74: 12
76: 14
78: 14
82: 14
84: 14
94: 14
96: 14";

		static void Main(string[] args)
		{
			var layerRange = new List<int>();

			using (var reader = new StringReader(Input))
			{
				while (true)
				{
					var line = reader.ReadLine();

					if (line == null)
						break;

					string[] parts = line.Split(':');

					int layerNumber = int.Parse(parts[0]);
					int range = int.Parse(parts[1].TrimStart());

					while (layerRange.Count <= layerNumber)
						layerRange.Add(-1);

					layerRange[layerNumber] = range;
				}
			}

			var scannerPosition = Enumerable.Repeat(0, layerRange.Count).ToList();
			var scannerDirection = Enumerable.Repeat(1, layerRange.Count).ToList();

			int severity = 0;

			for (int i = 0; i < layerRange.Count; i++)
			{
				if ((scannerPosition[i] == 0) && (layerRange[i] > 0))
					severity += layerRange[i] * i;

				for (int j = 0; j < layerRange.Count; j++)
				{
					if (layerRange[j] < 0)
						continue;

					scannerPosition[j] += scannerDirection[j];

					if (scannerPosition[j] == layerRange[j] - 1)
						scannerDirection[j] = -1;
					else if (scannerPosition[j] == 0)
						scannerDirection[j] = +1;
				}
			}

			Console.WriteLine(severity);
		}
	}
}
