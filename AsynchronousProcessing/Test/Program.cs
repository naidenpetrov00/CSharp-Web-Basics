using System.Diagnostics;

namespace Test
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var sw = Stopwatch.StartNew();
			Console.WriteLine(CountPrimeNumbers(1, 10000000));
			Console.WriteLine(sw.Elapsed);
		}

		private static int CountPrimeNumbers(int from, int to)
		{
			var count = 0;

			for (int i = from; i <= to; i++)
			{
				var isPrime = true;
				for (int div = 2; div <= Math.Sqrt(i); div++)
				{
					if (i % div == 0)
					{
						isPrime = false;
					}
				}

				if (isPrime)
				{
					count++;
				}
			}

			return count;
		}
	}
}