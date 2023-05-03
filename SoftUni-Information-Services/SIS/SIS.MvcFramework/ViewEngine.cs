namespace SIS.MvcFramework
{
	using System.Text;
	using System.Reflection;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp;
	using System.Text.RegularExpressions;

	public class ViewEngine : IViewEngine
	{
		public string GetHtml(string templateHtml, object model, string user)
		{
			var methodCode = PrepareCSharpCode(templateHtml);
			var typeName = model?.GetType().FullName ?? "object";
			if (model?.GetType().IsGenericType == true)
			{
				typeName = model.GetType().Name.Replace("`1", string.Empty) +
					"<" + model.GetType().GenericTypeArguments.First().Name + ">";
			}
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
		public string GetHtml(object model, string user)
		{{
			var Model = model as {typeName};
			var User = user;
			var html = new StringBuilder();
			{methodCode}
			return html.ToString();
		}}
	}}
}}";
			IView view = GetInstanceFromCode(code, model);
			var html = view.GetHtml(model, user);
			return html;
		}

		private IView GetInstanceFromCode(string code, object model)
		{
			var compilation = CSharpCompilation
				.Create("AppViewAssembly")
				.WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
				.AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
				.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
			if (model != null)
			{
				compilation = compilation.AddReferences(MetadataReference.CreateFromFile(model.GetType().Assembly.Location));
			}

			var libraries = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();
			foreach (var library in libraries)
			{
				compilation = compilation
					.AddReferences(MetadataReference.CreateFromFile(Assembly.Load(library).Location));
			}

			compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));

			using var memoryStream = new MemoryStream();
			var compilationResut = compilation.Emit(memoryStream);
			if (!compilationResut.Success)
			{
				return new ErrorView(compilationResut.Diagnostics
					.Where(x => x.Severity == DiagnosticSeverity.Error)
					.Select(x => x.GetMessage()));
			}

			memoryStream.Seek(0, SeekOrigin.Begin);
			var assemblyByteArray = memoryStream.ToArray();
			var assembly = Assembly.Load(assemblyByteArray);
			var type = assembly.GetType("AppViewNamespace.AppViewCode");
			var instance = Activator.CreateInstance(type) as IView;

			return instance;
		}

		private string PrepareCSharpCode(string templateHtml)
		{
			var cSharpExpressionRegex = new Regex(@"[^\<\""\s&]+", RegexOptions.Compiled);
			var supportedOpperators = new[] { "if", "for", "foreach", "else" };
			var cSharpCode = new StringBuilder();
			var reader = new StringReader(templateHtml);
			string line;

			while ((line = reader.ReadLine()) != null)
			{
				if (line.TrimStart().StartsWith("{") || line.TrimStart().StartsWith("}"))
				{
					cSharpCode.AppendLine(line);
				}
				else if (supportedOpperators.Any(x => line.TrimStart().StartsWith("@" + x)))
				{
					var indexAt = line.IndexOf("@");
					line = line.Remove(indexAt, 1);
					cSharpCode.AppendLine(line);
				}
				else
				{
					var currCSharpLine = new StringBuilder($"html.AppendLine(@\"");
					while (line.Contains("@"))
					{
						var atSignLocation = line.IndexOf("@");
						var before = line.Substring(0, atSignLocation);
						currCSharpLine.Append(before.Replace("\"", "\"\"") + "\" + ");
						var cSharpAndEndOfLine = line.Substring(atSignLocation + 1);
						var cSharpExpression = cSharpExpressionRegex.Match(cSharpAndEndOfLine);
						currCSharpLine.Append(cSharpExpression.Value + " + @\"");
						var after = cSharpAndEndOfLine.Substring(cSharpExpression.Length);
						line = after;
					}

					currCSharpLine.AppendLine(line.Replace("\"", "\"\"") + "\");");
					cSharpCode.AppendLine(currCSharpLine.ToString());
				}

			}

			return cSharpCode.ToString();
		}
	}
}
