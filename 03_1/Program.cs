using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_1
{
	class Program
	{
		const int Input = 325489;

		static void Main(string[] args)
		{
			int x = 0, y = 0;
			int dx = 1, dy = 0;
			int ring = 1;

			for (int i = 1; i < Input; i++)
			{
				x += dx;
				y += dy;

				if ((dx == 1) && (x == ring))
				{
					dx = 0;
					dy = 1;
				}
				else if ((dy == 1) && (y == ring))
				{
					dx = -1;
					dy = 0;
				}
				else if ((dx == -1) && (x == -ring))
				{
					dx = 0;
					dy = -1;
				}
				else if ((dy == -1) && (y == -ring))
				{
					dx = 1;
					dy = 0;
					ring++;
				}
			}

			Console.WriteLine("({0}, {1}) => {2}", x, y, Math.Abs(x) + Math.Abs(y));
		}
	}
}
