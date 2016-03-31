using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.IO;

namespace Pseudo
{
	public class SchemaWriter : IDisposable
	{
		readonly FileStream stream;
		string text;
		int indentation;

		public SchemaWriter(FileStream stream)
		{
			this.stream = stream;
		}

		public void Append(string text)
		{
			this.text += text;
		}

		public void AppendLine(string text, bool indent = true)
		{
			if (indent)
				AppendIndentation();

			this.text += text;
			AppendNewLine();
		}

		public void AppendNewLine()
		{
			text += "\r\n";
		}

		public void AppendIndentation()
		{
			for (int i = 0; i < indentation; i++)
				text += "	";
		}

		public void AppendFieldDeclaration(string typeName, string fieldName, string defaultValue = null)
		{
			AppendLine(string.Format("public {0} {1}{2};", typeName, fieldName, string.IsNullOrEmpty(defaultValue) ? "" : " = " + defaultValue));
		}

		public void BeginPropertyDeclaration(string typeName, string propertyName)
		{
			AppendLine(string.Format("public {0} {1}", typeName, propertyName));
			AppendLine("{");
			indentation++;
		}

		public void EndPropertyDeclaration()
		{
			indentation--;
			AppendLine("}");
		}

		public void BeginPropertyGetter()
		{
			AppendLine("get");
			AppendLine("{");
			indentation++;
		}

		public void EndPropertyGetter()
		{
			indentation--;
			AppendLine("}");
		}

		public void BeginPropertySetter()
		{
			AppendLine("set");
			AppendLine("{");
			indentation++;
		}

		public void EndPropertySetter()
		{
			indentation--;
			AppendLine("}");
		}

		public void BeginTypeDeclaration(string typeName, string inheritType)
		{
			AppendLine(string.Format("public class {0} : {1}", typeName, inheritType));
			AppendLine("{");
			indentation++;
		}

		public void EndTypeDeclaration()
		{
			indentation--;
			AppendLine("}");
		}

		public void BeginMethodDeclaration(string methodName, string returnTypeName, params string[] parameterDeclarations)
		{
			AppendLine(string.Format("public {0} {1}({2})", returnTypeName, methodName, string.Join(", ", parameterDeclarations)));
			AppendLine("{");
			indentation++;
		}

		public void EndMethodDeclaration()
		{
			indentation--;
			AppendLine("}");
		}

		public override string ToString()
		{
			return text;
		}

		public void Close()
		{
			using (var writer = new StreamWriter(stream))
				writer.Write(text);
		}

		public void Dispose()
		{
			Close();
		}
	}
}
