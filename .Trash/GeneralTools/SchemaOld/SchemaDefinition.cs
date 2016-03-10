using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Internal.Schema;

namespace Pseudo.Internal.Schema
{
	public class SchemaDefinition
	{
		public readonly object Instance;
		public readonly Type Type;
		public readonly List<INode> Nodes = new List<INode>();

		readonly IVariableDefinition[] variableDefinitions;
		readonly IFunctionDefinition[] functionDefinitions;
		readonly IEventDefinition[] eventDefinitions;
		readonly Dictionary<string, IVariableDefinition> nameToVariableDefinition = new Dictionary<string, IVariableDefinition>();
		readonly Dictionary<string, IFunctionDefinition> nameToFunctionDefinition = new Dictionary<string, IFunctionDefinition>();
		readonly Dictionary<string, IEventDefinition> nameToEventDefinition = new Dictionary<string, IEventDefinition>();

		public SchemaDefinition(object instance) : this(instance, instance.GetType())
		{
			InitializeMembers();
		}

		public SchemaDefinition(Type type) : this(null, type)
		{
			InitializeMembers();
		}

		SchemaDefinition(object instance, Type type)
		{
			Instance = instance;
			Type = type;

			variableDefinitions = SchemaUtility.CreateVariables(instance, type);
			functionDefinitions = SchemaUtility.CreateFunctions(instance, type);
			eventDefinitions = SchemaUtility.CreateEvents(instance, type);
		}

		public IVariableDefinition<TValue> CreateVariable<TValue>(string name)
		{
			if (nameToVariableDefinition.ContainsKey(name))
				throw new ArgumentException(string.Format("A variable named {0} already exists.", name));

			var variableDefinition = new SchemaVariableDefinition<TValue>(name);
			nameToVariableDefinition[name] = variableDefinition;

			return variableDefinition;
		}

		public IVariableDefinition GetVariable(string name)
		{
			return nameToVariableDefinition[name];
		}

		public IVariableDefinition[] GetVariables()
		{
			return variableDefinitions;
		}

		public IFunctionDefinition GetFunction(string name)
		{
			return nameToFunctionDefinition[name];
		}

		public IFunctionDefinition[] GetFunctions()
		{
			return functionDefinitions;
		}

		public IEventDefinition CreateEvent(string name)
		{
			if (nameToEventDefinition.ContainsKey(name))
				throw new ArgumentException(string.Format("An event named {0} already exists.", name));

			var eventDefinition = new EventDefinition(name);
			nameToEventDefinition[name] = eventDefinition;

			return eventDefinition;
		}

		public IEventDefinition GetEvent(string name)
		{
			return nameToEventDefinition[name];
		}

		public IEventDefinition[] GetEvents()
		{
			return eventDefinitions;
		}

		void InitializeMembers()
		{
			PopulateVariables(nameToVariableDefinition);
			PopulateFunctions(nameToFunctionDefinition);
			PopulateEvents(nameToEventDefinition);
		}

		void PopulateVariables(Dictionary<string, IVariableDefinition> nameToVariable)
		{
			foreach (var variableDefinition in variableDefinitions)
				nameToVariable[variableDefinition.Name] = variableDefinition;
		}

		void PopulateFunctions(Dictionary<string, IFunctionDefinition> nameToFunction)
		{
			foreach (var functionDefinition in functionDefinitions)
				nameToFunction[functionDefinition.Name] = functionDefinition;
		}

		void PopulateEvents(Dictionary<string, IEventDefinition> nameToEvents)
		{
			foreach (var eventDefinition in eventDefinitions)
				nameToEvents[eventDefinition.Name] = eventDefinition;
		}
	}
}
