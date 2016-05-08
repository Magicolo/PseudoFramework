using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.PoolingNOOOO.Internal
{
	public class Initializer : IInitializer
	{
		static readonly ITypeAnalyzer defaultAnalyzer = new TypeAnalyzer();

		public ITypeAnalyzer Analyzer
		{
			get { return analyzer; }
			set { analyzer = value ?? defaultAnalyzer; }
		}

		ITypeAnalyzer analyzer = defaultAnalyzer;

		public void Initialize(object instance)
		{
			var info = analyzer.Analyze(instance.GetType());

			// Initialize fields
			for (int i = 0; i < info.Fields.Length; i++)
				info.Fields[i].Initialize(instance, null);

			// Initialize properties
			for (int i = 0; i < info.Properties.Length; i++)
				info.Properties[i].Initialize(instance, null);
		}
	}
}
