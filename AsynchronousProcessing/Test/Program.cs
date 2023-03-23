using System.Diagnostics;

namespace Test
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var lockObj = new object();
			decimal money = 0;
			var thread1 = new Thread(() =>
			{
				for (int i = 0; i < 100000; i++)
				{
					lock (lockObj)
					{
						money++;
					}
				}
			});
			thread1.Start();

			var thread2 = new Thread(() =>
			{
				for (int i = 0; i < 100000; i++)
				{
					lock (lockObj)
					{
						money++;
					}
				}
			});
			thread2.Start();

			thread1.Join();
			thread2.Join();

			Console.WriteLine(money);

			//var thread = new Thread(MyThreadMainMethod);
			//thread.Start();

			//while (true)
			//{
			//	var line = Console.ReadLine();
			//	Console.WriteLine(line.ToUpper());
			//}
		}

		private static void MyThreadMainMethod()
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