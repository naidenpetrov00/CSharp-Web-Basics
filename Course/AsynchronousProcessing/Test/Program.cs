 namespace Test
{
	using System.Diagnostics;
	using System.Collections.Concurrent;

	public class Program
	{
		public static async Task Main(string[] args)
		{
			var httpClient = new HttpClient();
			var httpResponse = await httpClient.GetAsync("https://softuni.bg");
			var result = await httpResponse.Content.ReadAsStringAsync();
			Console.WriteLine(result);


			//// - deadlock example
			//var lockObj1 = new object();
			//var lockObj2 = new object();

			//var thread1 = new Thread(() =>
			//{
			//	lock (lockObj1)
			//	{
			//		Thread.Sleep(2000);
			//		lock (lockObj2)
			//		{

			//		}
			//	}
			//});

			//var thread2 = new Thread(() =>
			//{
			//	lock (lockObj2)
			//	{
			//		Thread.Sleep(2000);
			//		lock (lockObj1)
			//		{

			//		}
			//	}
			//});

			//thread1.Start();
			//thread2.Start();
			//thread1.Join();
			//thread2.Join();
			// -

			//var numbers = new ConcurrentQueue<int>(Enumerable
			//	.Range(0, 10000)
			//	.ToList());

			//for (int i = 0; i < 5; i++)
			//{
			//	new Thread(() =>
			//	{
			//		while (numbers.Count > 0)
			//		{
			//			numbers.TryDequeue(out _);
			//		}
			//	}).Start();
			//}

			//var lockObj = new object();
			//decimal money = 0;
			//var thread1 = new Thread(() =>
			//{
			//	for (int i = 0; i < 100000; i++)
			//	{
			//		lock (lockObj)
			//		{
			//			money++;
			//		}
			//	}
			//});
			//thread1.Start();

			//var thread2 = new Thread(() =>
			//{
			//	for (int i = 0; i < 100000; i++)
			//	{
			//		lock (lockObj)
			//		{
			//			money++;
			//		}
			//	}
			//});
			//thread2.Start();

			//thread1.Join();
			//thread2.Join();

			//Console.WriteLine(money);

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