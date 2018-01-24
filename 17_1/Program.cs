using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17_1
{
	class Program
	{
		class Node
		{
			public Node NextNode;
			public int Value;
		}

		static void Main(string[] args)
		{
			// const int SpinSize = 3;
			const int SpinSize = 359;

			Node node = new Node();

			node.NextNode = node;
			node.Value = 0;

			for (int i = 1; i <= 2017; i++)
			{
				for (int j = 0; j < SpinSize; j++)
					node = node.NextNode;

				var newNext = new Node();

				newNext.NextNode = node.NextNode;
				node.NextNode = newNext;
				node = newNext;

				node.Value = i;
			}

			Console.WriteLine(node.NextNode.Value);
		}
	}
}
