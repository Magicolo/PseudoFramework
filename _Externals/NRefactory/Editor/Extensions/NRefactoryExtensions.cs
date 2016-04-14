using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;
using System.IO;
using ICSharpCode.NRefactory.Parser;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Text;

namespace Pseudo.Internal
{
	public static class NRefactoryExtensions
	{
		public static bool TryGenerateCode(this INode node, out string code, out Errors errors, SupportedLanguage language = SupportedLanguage.CSharp)
		{
			code = node.GenerateCode(language);
			errors = node.GetErrors(language);

			return true;
		}

		public static Errors GetErrors(this INode node, SupportedLanguage language = SupportedLanguage.CSharp)
		{
			using (var reader = new StringReader(node.GenerateCode(language)))
			{
				var parser = ParserFactory.CreateParser(language, reader);
				parser.Parse();

				return parser.Errors;
			}
		}

		public static string GenerateCode(this INode node, SupportedLanguage language = SupportedLanguage.CSharp)
		{
			var output = NRefactoryUtility.CreateOutputVisitor(language);
			node.AcceptVisitor(output, null);

			return output.Text;
		}

		public static string GenerateCode(this INode node, string path, SupportedLanguage language = SupportedLanguage.CSharp)
		{
			var code = node.GenerateCode(language);
			File.WriteAllText(path, code);

			return code;
		}

		public static Assembly CompileCode(this INode node, string path, SupportedLanguage language = SupportedLanguage.CSharp)
		{
			var code = node.GenerateCode();
			var provider = NRefactoryUtility.CreateCodeProvider(language);
			var parameters = new CompilerParameters(new[]
			{
			"mscorlib.dll",
			"System.Core.dll",
			UnityEditor.EditorApplication.applicationContentsPath + "/Managed/UnityEngine.dll",
			UnityEditor.EditorApplication.applicationContentsPath + "/Managed/UnityEditor.dll",
		}, path);

			var results = provider.CompileAssemblyFromSource(parameters, code);

			if (results.Errors.Count > 0)
			{
				var builder = new StringBuilder(results.Errors.Count + 1);
				builder.AppendLine("Compilation failed:");

				foreach (var error in results.Errors)
					builder.AppendLine(error.ToString());

				throw new InvalidOperationException(builder.ToString());
			}

			return results.CompiledAssembly;
		}

		public static string GetFullTypeName(this TypeDeclaration type)
		{
			var typeName = type.Name;
			var parent = type.Parent;

			while (parent != null)
			{
				if (parent is TypeDeclaration)
					typeName = ((TypeDeclaration)parent).Name + "." + typeName;
				else if (parent is NamespaceDeclaration)
					typeName = ((NamespaceDeclaration)parent).Name + "." + typeName;

				parent = parent.Parent;
			}

			return typeName;
		}

		public static IEnumerable<Type> GetParameterTypes(this MethodDeclaration method)
		{
			return method.Parameters
				.Select(p => Type.GetType(p.TypeReference.Type));
		}

		public static void SortChildren(this INode node, bool recursive = false)
		{
			node.Children.Sort((a, b) =>
			{
				var scoreA = NRefactoryUtility.GetSortScore(a);
				var scoreB = NRefactoryUtility.GetSortScore(b);

				if (scoreA > scoreB)
					return -1;
				else if (scoreA < scoreB)
					return 1;
				else
					return 0;
			});

			if (recursive)
			{
				for (int i = 0; i < node.Children.Count; i++)
					node.Children[i].SortChildren(recursive);
			}
		}

		public static T AddChildren<T>(this T node, params INode[] children) where T : INode
		{
			node.Children.AddRange(children);

			return node;
		}

		public static IEnumerable<INode> GetChildren(this INode node, bool recursive = false)
		{
			return node.GetChildren<INode>(recursive);
		}

		public static IEnumerable<T> GetChildren<T>(this INode node, bool recursive = false) where T : INode
		{
			var children = node.Children
				.Where(c => c is T)
				.Select(c => (T)c);

			if (recursive)
			{
				for (int i = 0; i < node.Children.Count; i++)
					children = children.Concat(node.Children[i].GetChildren<T>(recursive));
			}

			return children;
		}

		public static IEnumerable<T> GetChildren<T>(this INode node, Predicate<T> match, bool recursive = false) where T : INode
		{
			return node.GetChildren<T>(recursive)
				.Where(c => match(c));
		}

		public static T GetFirstChild<T>(this INode node, bool recursive = false) where T : INode
		{
			return node.GetChildren<T>(recursive).FirstOrDefault();
		}

		public static T GetLastChild<T>(this INode node, bool recursive = false) where T : INode
		{
			return node.GetChildren<T>(recursive).LastOrDefault();
		}

		public static bool HasModifier(this AttributedNode node, Modifiers modifier)
		{
			return (node.Modifier & modifier) != 0;
		}
	}
}