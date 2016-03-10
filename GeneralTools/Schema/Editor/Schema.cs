using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using UnityEditor;
using System.IO;
using System.Text;

namespace Pseudo
{
	public class Schema : ScriptableObject
	{
		public string Name
		{
			get { return name; }
		}
		public Type BaseType
		{
			get { return baseType; }
		}

		[SerializeField]
		PType baseType;
		[SerializeField]
		List<NodeBase> nodes = new List<NodeBase>();

		public FunctionDeclarationNode CreateFunctionDeclaration(string name)
		{
			if (nodes.Find(n => n.Name == name))
				throw new ArgumentException(string.Format("A block named {0} already exist.", name));

			var function = CreateNode<FunctionDeclarationNode>();
			function.Initialize(name, this);

			return function;
		}

		public ThisNode CreateThis()
		{
			if (BaseType.IsAbstract)
				throw new ArgumentException(string.Format("Type {0} can not have a this node because it is static or not concrete.", BaseType.Name));

			var node = CreateNode<ThisNode>();
			node.Initialize(BaseType, this);

			return node;
		}

		public InstanceVariableNode CreateInstanceVariable(string name)
		{
			var field = BaseType.GetField(name, BindingFlags.Instance | BindingFlags.Public);

			if (field != null)
			{
				var variable = CreateNode<InstanceVariableNode>();
				variable.Initialize(field, this);
				variable.SetCaller(CreateThis());

				return variable;
			}

			var property = BaseType.GetProperty(name, BindingFlags.Instance | BindingFlags.Public);

			if (property != null)
			{
				var variable = CreateNode<InstanceVariableNode>();
				variable.Initialize(property, this);
				variable.SetCaller(CreateThis());

				return variable;
			}

			throw new ArgumentException(string.Format("No public instance member named {0} was found.", name));
		}

		public StaticVariableNode CreateStaticVariable(string name)
		{
			var field = BaseType.GetField(name, BindingFlags.Static | BindingFlags.Public);

			if (field != null)
			{
				var variable = CreateNode<StaticVariableNode>();
				variable.Initialize(field, this);

				return variable;
			}

			var property = BaseType.GetProperty(name, BindingFlags.Static | BindingFlags.Public);

			if (property != null)
			{
				var variable = CreateNode<StaticVariableNode>();
				variable.Initialize(property, this);

				return variable;
			}

			throw new ArgumentException(string.Format("No public static member named {0} was found.", name));
		}

		public InstanceFunctionNode CreateInstanceFunction(string name)
		{
			var method = BaseType.GetMethod(name, BindingFlags.Instance | BindingFlags.Public);

			if (method != null)
			{
				var function = CreateNode<InstanceFunctionNode>();
				function.Initialize(method, this);
				function.SetCaller(CreateThis());

				return function;
			}

			throw new ArgumentException(string.Format("No public intance method named {0} was found.", name));
		}

		public StaticFunctionNode CreateStaticFunction(string name)
		{
			var method = BaseType.GetMethod(name, BindingFlags.Static | BindingFlags.Public);

			if (method != null)
			{
				var function = CreateNode<StaticFunctionNode>();
				function.Initialize(method, this);

				return function;
			}

			throw new ArgumentException(string.Format("No public static method named {0} was found.", name));
		}

		public ParameterNode CreateParameter(ParameterInfo parameter)
		{
			var node = CreateNode<ParameterNode>();
			node.Initialize(parameter, this);

			return node;
		}

		public T CloneNode<T>(T node) where T : NodeBase
		{
			var clone = Instantiate(node);
			clone.name = clone.name.TrimEnd("(Clone)".ToCharArray());
			nodes.Add(clone);
			AssetDatabase.AddObjectToAsset(clone, this);

			return clone;
		}

		public T CreateNode<T>() where T : NodeBase
		{
			var node = CreateInstance<T>();
			nodes.Add(node);
			AssetDatabase.AddObjectToAsset(node, this);

			return node;
		}

		public void DestroyNode(NodeBase node)
		{
			DestroyNode(node, true);
		}

		public void DestroyNode(FunctionDeclarationNode node)
		{
			DestroyNode(node, true);
		}

		public void DestroyNodes()
		{
			for (int i = nodes.Count - 1; i >= 0; i--)
				DestroyNode(nodes[i], false);

			AssetDatabase.SaveAssets();
		}

		public FunctionDeclarationNode[] GetFunctionDeclarations()
		{
			return nodes
				.Where(n => n is FunctionDeclarationNode)
				.Select(n => (FunctionDeclarationNode)n)
				.ToArray();
		}

		public NodeBase[] GetNodes()
		{
			return nodes.ToArray();
		}

		public void Compile()
		{
			AssetDatabase.SaveAssets();

			if (!IsValid())
				throw new InvalidDataException("One or more nodes are invalid.");

			var path = Path.ChangeExtension(AssetDatabase.GetAssetPath(this), ".cs");

			using (var stream = File.Exists(path) ? File.Open(path, FileMode.Truncate) : File.Create(path))
			{
				using (var writer = new SchemaWriter(stream))
				{
					writer.BeginTypeDeclaration(Name, BaseType.FullName);

					var functionDeclarations = GetFunctionDeclarations();

					for (int i = 0; i < functionDeclarations.Length; i++)
						functionDeclarations[i].Write(writer);

					writer.EndTypeDeclaration();
				}
			}

			AssetDatabase.Refresh();
		}

		public bool IsValid()
		{
			return !string.IsNullOrEmpty(Name) && nodes.All(b => b.IsValid());
		}

		void DestroyNode(NodeBase node, bool save)
		{
			nodes.Remove(node);
			node.Destroy(true);

			if (save)
				AssetDatabase.SaveAssets();
		}

		void Initialize(string name, Type baseType)
		{
			this.name = name;
			this.baseType = baseType;
		}

		public static Schema Create(string path, string name, Type baseType)
		{
			var schema = CreateInstance<Schema>();
			schema.Initialize(name, baseType);
			AssetDatabase.CreateAsset(schema, path + name + ".asset");
			AssetDatabase.SaveAssets();

			return schema;
		}
	}
}
