using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_2
{
	class Program
	{
		const int Input = 325489;

		static void Main(string[] args)
		{
			int x = 0, y = 0;
			int dx = 1, dy = 0;
			int ring = 1;

			var values = new Dictionary<Tuple<int, int>, int>();

			values[Tuple.Create(0, 0)] = 1;

			int sum;

			void CheckAndAdd(int checkX, int checkY)
			{
				var key = Tuple.Create(checkX, checkY);

				int value;

				values.TryGetValue(key, out value);

				sum += value;
			}

			while (true)
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

				sum = 0;

				for (int checkDX = -1; checkDX <= 1; checkDX++)
					for (int checkDY = -1; checkDY <= 1; checkDY++)
						CheckAndAdd(x + checkDX, y + checkDY);

				values[Tuple.Create(x, y)] = sum;

				if (sum > Input)
					break;
			}

			Console.WriteLine(sum);
		}
	}
}
