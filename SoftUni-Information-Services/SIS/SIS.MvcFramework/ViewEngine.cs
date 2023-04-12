namespace SIS.MvcFramework
{
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp;
	using System.Reflection;

	public interface IView
	{
		string GetHtml(object model);

	}

	public class ViewEngine : IViewEngine
	{
		public string GetHtml(string templateHtml, object model)
		{
			var methodCode = PrepareCSharpCode(templateHtml);
			var code =
$@"namespace AppViewNamespace
{{
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SIS.MvcFramework;
 
	public class AppViewCode : IView
	{{
		public string GetHtml(object model)
		{{
			var html = new StringBuilder();
			{methodCode}
			return html.ToString();
		}}
	}}
}}";

			IView view = GetInstanceFromCode(code, model);
			var html = view.GetHtml(model);
			return html;
		}

		private IView GetInstanceFromCode(string code, object model)
		{
			var compilation = CSharpCompilation
				.Create("AppViewAssembly")
				.WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
				.AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
				.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
				.AddReferences(MetadataReference.CreateFromFile(model.GetType().Assembly.Location));
			var libraries = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();
			foreach (var library in libraries)
			{
				compilation = compilation
					.AddReferences(MetadataReference.CreateFromFile(Assembly.Load(library).Location));
			}

			compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));
			compilation.Emit("appView.dll");

			return null;
		}

		private string PrepareCSharpCode(string templateHtml)
		{
			return string.Empty;
		}
	}
}
