using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Groupingz.Internal
{
	public class Converter<TValue> : IConverter<TValue, int>
	{
		readonly Dictionary<TValue, int> valueToIdentifier = new Dictionary<TValue, int>(PEqualityComparer<TValue>.Default);
		readonly Dictionary<int, TValue> identifierToValue = new Dictionary<int, TValue>();

		int identifierCounter;

		public int ConvertTo(TValue value)
		{
			int identifier;

			if (!valueToIdentifier.TryGetValue(value, out identifier))
			{
				identifier = ++identifierCounter;
				valueToIdentifier[value] = identifier;
				identifierToValue[identifier] = value;
			}

			return identifier;
		}

		public TValue ConvertFrom(int identifier)
		{
			TValue value;
			identifierToValue.TryGetValue(identifier, out value);

			return value;
		}
	}
}
