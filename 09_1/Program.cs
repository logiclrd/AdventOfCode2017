using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_1
{
	class Program
	{
		static void Main(string[] args)
		{
			string input;

			using (var stream = typeof(Program).Assembly.GetManifestResourceStream(typeof(Program).Namespace + ".Input.txt"))
				input = new StreamReader(stream).ReadToEnd();

			bool inGarbage = false;
			bool cancelled = false;
			int nestingLevel = 0;
			int score = 0;

			foreach (char ch in input)
			{
				if (cancelled)
					cancelled = false;
				else if (inGarbage)
				{
					if (ch == '!')
						cancelled = true;
					else if (ch == '>')
						inGarbage = false;
				}
				else
				{
					if (ch == '{')
						nestingLevel++;
					else if (ch == '}')
					{
						score += nestingLevel;
						nestingLevel--;
					}
					else if (ch == '<')
						inGarbage = true;
				}
			}

			Console.WriteLine(score);
		}
	}
}
