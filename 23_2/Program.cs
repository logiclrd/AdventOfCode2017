using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _23_2
{
	class Program
	{
		const string Input =
@"set b 81
set c b
jnz a 2
jnz 1 5
mul b 100
sub b -100000
set c b
sub c -17000
set f 1
set d 2
set e 2
set g d
mul g e
sub g b
jnz g 2
set f 0
sub e -1
set g e
sub g b
jnz g -8
sub d -1
set g d
sub g b
jnz g -13
jnz f 2
sub h -1
set g b
sub g c
jnz g 2
jnz 1 3
sub b -17
jnz 1 -23";

		/*
			First iteration:

			b = 81
			c = b

			if (a > 0)
			{
				b = b * 100
				b = b + 100000
				c = b
				c = c + 17000
			}

			while (true)
			{
				f = 1
				d = 2

				do
				{
					e = 2

					do
					{
						g = d
						g = g * e
						g = g - b

						if (g == 0)
							f = 0

						e = e + 1
						g = e
						g = g - b
					}
					while (g != 0);

					d = d + 1
					g = d
					g = g - b
				} while (g != 0);

				if (f == 0)
					h = h + 1

				g = b
				g = g - c

				if (g == 0)
					break

				b = b + 17
			}
		*/

		/*
			Second iteration:

			b = 81;
			c = b;

			if (a > 0)
			{
				b = b * 100 + 100000;
				c = b + 17000;
			}

			while (true)
			{
				f = 1
				d = 2

				do
				{
					e = 2;

					do
					{
						if (d * e == b)
							f = 0;

						e++;
					}
					while (e != b);

					d++;
				} while (d != b);

				if (f == 0)
					h++;

				if (b == c)
					break;

				b = b + 17;
			}
		*/

		/*
			Third iteration:

		static int Algorithm()
		{
			int a = 1;
			int b = 81;
			int c = b;
			int d = 0;
			int e = 0;
			int f = 0;
			int g = 0;
			int h = 0;

			if (a > 0)
			{
				b = b * 100 + 100000;
				c = b + 17000;
			}

			while (true)
			{
				f = 1;
				d = 2;

				do
				{
					e = 2;

					do
					{
						if (d * e == b)
							f = 0;

						e++;
					}
					while (e != b);

					d++;
				} while (d != b);

				if (f == 0)
					h++;

				if (b == c)
					break;

				b = b + 17;
			}

			return h;
		}
		*/

		static int Algorithm()
		{
			int[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997 };

			bool IsPrime(int x)
			{
				int stopAt = (int)Math.Sqrt(x);

				for (int i = 0; i < primes.Length; i++)
				{
					if (primes[i] > stopAt)
						return true;
					if ((x % primes[i]) == 0)
						return false;
				}

				return true;
			}

			int a = 1;
			int b = 81;
			int c = b;
			int d = 0;
			int e = 0;
			int f = 0;
			int g = 0;
			int h = 0;

			if (a > 0)
			{
				b = b * 100 + 100000;
				c = b + 17000;
			}

			while (true)
			{
				if (!IsPrime(b))
					h++;

				if (b == c)
					break;

				b = b + 17;
			}

			return h;
		}

		static void Main()
		{
			Console.WriteLine(Algorithm());
		}
	}
}
