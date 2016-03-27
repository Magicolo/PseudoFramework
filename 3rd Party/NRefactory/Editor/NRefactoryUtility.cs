using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.PrettyPrinter;
using ICSharpCode.NRefactory;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using Pseudo;
using System.IO;

namespace Pseudo.Internal
{
	public static class NRefactoryUtility
	{
		public static IOutputAstVisitor CreateOutputVisitor(SupportedLanguage language = SupportedLanguage.CSharp)
		{
			switch (language)
			{
				default:
				case SupportedLanguage.CSharp:
					return new CSharpOutputVisitor();
				case SupportedLanguage.VBNet:
					return new VBNetOutputVisitor();
			}
		}

		public static CodeDomProvider CreateCodeProvider(SupportedLanguage language = SupportedLanguage.CSharp)
		{
			switch (language)
			{
				default:
				case SupportedLanguage.CSharp:
					return new CSharpCodeProvider();
				case SupportedLanguage.VBNet:
					return new VBCodeProvider();
			}
		}

		public static CompilationUnit Parse(string code, SupportedLanguage language = SupportedLanguage.CSharp)
		{
			using (var reader = new StringReader(code))
			{
				var parser = ParserFactory.CreateParser(language, reader);
				parser.Parse();

				return parser.CompilationUnit;
			}
		}

		public static BlockStatement ParseBlock(string code, SupportedLanguage language = SupportedLanguage.CSharp)
		{
			using (var reader = new StringReader(code))
			{
				var parser = ParserFactory.CreateParser(language, reader);
				return parser.ParseBlock();
			}
		}

		public static Expression ParseExpression(string code, SupportedLanguage language = SupportedLanguage.CSharp)
		{
			using (var reader = new StringReader(code))
			{
				var parser = ParserFactory.CreateParser(language, reader);
				return parser.ParseExpression();
			}
		}

		public static List<INode> ParseTypeMembers(string code, SupportedLanguage language = SupportedLanguage.CSharp)
		{
			using (var reader = new StringReader(code))
			{
				var parser = ParserFactory.CreateParser(language, reader);
				return parser.ParseTypeMembers();
			}
		}

		public static TypeReference ParseTypeReference(string code, SupportedLanguage language = SupportedLanguage.CSharp)
		{
			using (var reader = new StringReader(code))
			{
				var parser = ParserFactory.CreateParser(language, reader);
				return parser.ParseTypeReference();
			}
		}

		public static int GetSortScore(INode node)
		{
			int score = 0;

			if (node is UsingDeclaration)
				score = 100000;
			else if (node is NamespaceDeclaration)
				score = 10000;
			else if (node is TypeDeclaration)
				score = 1000;
			else if (node is FieldDeclaration)
				score = 100;
			else if (node is PropertyDeclaration)
				score = 10;
			else if (node is MethodDeclaration)
				score = 1;

			if (node is AttributedNode)
			{
				var attributedNode = (AttributedNode)node;

				if (attributedNode.HasModifier(Modifiers.Public))
					score *= 3;
				else if (attributedNode.HasModifier(Modifiers.Protected))
					score *= 2;
				else
					score *= 1;
			}

			return score;
		}

		public static TypeReference CreateTypeReference(Type type)
		{
			if (type.IsGenericType)
				return new TypeReference(type.GetGenericTypeDefinition().FullName, type.GetGenericArguments().Select(t => CreateTypeReference(t)).ToList());
			else
				return new TypeReference(type.FullName);
		}

		public static TypeReference CreateTypeReference(string typeName)
		{
			var type = TypeUtility.GetType(typeName);

			if (type == null)
				return new TypeReference(typeName, IsKeyword(typeName));
			else
				return CreateTypeReference(type);
		}

		public static TypeDeclaration CreateTypeDeclaration(string name, ClassType type, Modifiers modifiers)
		{
			return new TypeDeclaration(modifiers, new List<AttributeSection>())
			{
				Name = name,
				Type = type,
			};
		}

		public static FieldDeclaration CreateFieldDeclaration(string name, Type type, Modifiers modifiers)
		{
			return CreateFieldDeclaration(name, type.FullName, modifiers);
		}

		public static FieldDeclaration CreateFieldDeclaration(string name, string typeName, Modifiers modifiers)
		{
			return new FieldDeclaration(new List<AttributeSection>(), CreateTypeReference(typeName), modifiers)
			{
				Fields = new List<VariableDeclaration> { new VariableDeclaration(name) }
			};
		}

		public static PropertyDeclaration CreateAutoPropertyDeclaration(string name, Type type, Modifiers modifiers)
		{
			return CreateAutoPropertyDeclaration(name, type.FullName, modifiers);
		}

		public static PropertyDeclaration CreateAutoPropertyDeclaration(string name, string type, Modifiers modifiers)
		{
			return new PropertyDeclaration(modifiers, new List<AttributeSection>(), name, new List<ParameterDeclarationExpression>())
			{
				TypeReference = CreateTypeReference(type),
				GetRegion = CreateAutoPropertyGetRegion(),
				SetRegion = CreateAutoPropertySetRegion()
			};
		}

		public static PropertyGetRegion CreateAutoPropertyGetRegion()
		{
			return new PropertyGetRegion(BlockStatement.Null, new List<AttributeSection>());
		}

		public static PropertySetRegion CreateAutoPropertySetRegion()
		{
			return new PropertySetRegion(BlockStatement.Null, new List<AttributeSection>());
		}

		public static MethodDeclaration CreateEmptyMethodDeclaration(string name, Type returnType, Modifiers modifiers, List<ParameterDeclarationExpression> parameters)
		{
			return CreateEmptyMethodDeclaration(name, returnType.FullName, modifiers, parameters);
		}

		public static MethodDeclaration CreateEmptyMethodDeclaration(string name, string returnTypeName, Modifiers modifiers, List<ParameterDeclarationExpression> parameters)
		{
			return new MethodDeclaration
			{
				Name = name,
				TypeReference = CreateTypeReference(returnTypeName),
				Modifier = modifiers,
				Parameters = parameters,
				Body = CreateEmptyBlockStatement(returnTypeName)
			};
		}

		public static BlockStatement CreateEmptyBlockStatement(Type returnType)
		{
			return CreateEmptyBlockStatement(returnType.FullName);
		}

		public static BlockStatement CreateEmptyBlockStatement(string returnTypeName)
		{
			var block = new BlockStatement();

			if (!IsVoid(returnTypeName))
				block.AddChildren(new ReturnStatement(new DefaultValueExpression(CreateTypeReference(returnTypeName))));

			return block;
		}

		public static bool IsKeyword(string value, SupportedLanguage language = SupportedLanguage.CSharp)
		{
			return CreateCodeProvider(language).CreateValidIdentifier(value) != value;
		}

		public static bool IsVoid(string typeName)
		{
			return typeName == "void" || TypeUtility.GetType(typeName) == typeof(void);
		}
	}
}