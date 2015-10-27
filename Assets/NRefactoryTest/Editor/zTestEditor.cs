using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEditor;
using Pseudo.Internal.Editor;
using ICSharpCode.NRefactory.Parser.CSharp;
using ICSharpCode.NRefactory.Parser;
using ICSharpCode.NRefactory;
using System.IO;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;
using ICSharpCode.NRefactory.PrettyPrinter;

[CustomEditor(typeof(zTest))]
public class zTestEditor : CustomEditorBase
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Generate"))
			Generate();
	}

	void Generate()
	{
		StreamReader reader = new StreamReader("Assets/zTest.cs");
		IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, reader);
		List<string> script = new List<string>(File.ReadAllLines("Assets/zTest.cs"));
		CopyMethodReplacerVisitor replacer = new CopyMethodReplacerVisitor(script);

		parser.Parse();
		parser.CompilationUnit.AcceptVisitor(replacer, null);
		PDebug.Log(script.Concat('\n'));
	}
}

public class CopyMethodReplacerVisitor : AbstractAstVisitor
{
	List<string> script;

	public CopyMethodReplacerVisitor(List<string> script)
	{
		this.script = script;
	}

	public override object VisitMethodDeclaration(MethodDeclaration methodDeclaration, object data)
	{
		base.VisitMethodDeclaration(methodDeclaration, data);

		CSharpOutputVisitor currentCopyMethod = new CSharpOutputVisitor();
		CSharpOutputVisitor targetCopyMethod = new CSharpOutputVisitor();

		currentCopyMethod.VisitMethodDeclaration(methodDeclaration, data);
		methodDeclaration.Name = "BOBA_FEETTIMEERR";
		targetCopyMethod.VisitMethodDeclaration(methodDeclaration, data);

		script.RemoveRange(methodDeclaration.StartLocation.Line - 1, methodDeclaration.EndLocation.Line - methodDeclaration.StartLocation.Line);
		script.InsertRange(methodDeclaration.StartLocation.Line - 1, targetCopyMethod.Text.Split('\n'));

		return null;
	}
}