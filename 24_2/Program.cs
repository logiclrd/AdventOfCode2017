using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _24_2
{
	class Program
	{
		const string Input =
@"31/13
34/4
49/49
23/37
47/45
32/4
12/35
37/30
41/48
0/47
32/30
12/5
37/31
7/41
10/28
35/4
28/35
20/29
32/20
31/43
48/14
10/11
27/6
9/24
8/28
45/48
8/1
16/19
45/45
0/4
29/33
2/5
33/9
11/7
32/10
44/1
40/32
2/45
16/16
1/18
38/36
34/24
39/44
32/37
26/46
25/33
9/10
0/29
38/8
33/33
49/19
18/20
49/39
18/39
26/13
19/32";

		static void Main(string[] args)
		{
			var componentsBin = new Dictionary<int, List<int>>();

			void AddComponent(int portA, int portB)
			{
				if (!componentsBin.TryGetValue(portA, out List<int> components))
					components = componentsBin[portA] = new List<int>();

				components.Add(portB);
			}

			using (var reader = new StringReader(Input))
			{
				while (true)
				{
					var component = reader.ReadLine();

					if (component == null)
						break;

					string[] parts = component.Split('/');

					int portA = int.Parse(parts[0]);
					int portB = int.Parse(parts[1]);

					AddComponent(portA, portB);
					AddComponent(portB, portA);
				}
			}

			void FindNextComponent(int strengthSoFar, int lengthSoFar, int portSize, Action<int, int> finishedBridge)
			{
				if (!componentsBin.TryGetValue(portSize, out List<int> candidates)
				 || (candidates.Count == 0))
					return;

				finishedBridge(strengthSoFar, lengthSoFar);

				for (int i = 0; i < candidates.Count; i++)
				{
					int newPortSize = candidates[i];

					if (newPortSize < 0)
						continue;

					candidates[i] = -1;

					List<int> counterpart = componentsBin[newPortSize];

					for (int j = 0; j < counterpart.Count; j++)
					{
						if (counterpart[j] == portSize)
						{
							counterpart[j] = -1;
							FindNextComponent(strengthSoFar + portSize + newPortSize, lengthSoFar + 1, newPortSize, finishedBridge);
							counterpart[j] = portSize;

							break;
						}
					}

					candidates[i] = newPortSize;

					if (strengthSoFar == 0)
						Console.WriteLine("Finished testing root candidate {0} of {1}", i + 1, candidates.Count);
				}
			}

			int maxLength = 0;
			int maxStrength = 0;

			FindNextComponent(
				strengthSoFar: 0,
				lengthSoFar: 0,
				portSize: 0,
				finishedBridge:
					(strength, length) =>
					{
						if ((length > maxLength)
						 || ((length == maxLength) && (strength > maxStrength)))
						{
							Console.WriteLine("Made a bridge {0} units long of strength {1}", length, strength);

							maxLength = length;
							maxStrength = strength;
						}
					});
		}
	}
}
