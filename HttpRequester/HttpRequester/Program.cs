namespace HttpRequester
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var client = new HttpClient();
			var responce = await client.GetAsync("https://softuni.bg/");
			var result = await responce.Content.ReadAsStringAsync();

			File.WriteAllText("index.html", result);
		}
	}
}