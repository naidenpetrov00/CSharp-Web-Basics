namespace SIS.MvcFramework.Tests
{
	public class ViewEngineTests
	{
		[Theory]
		[InlineData("OnlyHtmlView")]
		[InlineData("ForForeachIfView")]
		[InlineData("ViewModelView")]
		public void GetHtmlTest(string testName)  
		{
			var viewModel = new TestViewModel()
			{
				Name = "Niki",
				Year = 2023,
				Numbers = new List<int> { 1, 10, 100, 1000, 100000 },
			};

			var viewContent = File.ReadAllText($"ViewTests/{testName}.html");
			var expectedResultContent = File.ReadAllText($"ViewTests/{testName}.Expected.html");

			var viewEngine = new ViewEngine();
			var actualResult = viewEngine.GetHtml(viewContent, viewModel, "123").TrimEnd();

			Assert.Equal(expectedResultContent, actualResult);
		}
	}
}