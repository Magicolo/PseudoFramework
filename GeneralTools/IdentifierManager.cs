using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System;

namespace Pseudo
{
	[Serializable]
	public class IdentifierManager<T> where T : IIdentifiable
	{
		[SerializeField]
		protected List<T> identifiables = new List<T>();

		Dictionary<int, T> identifierToIdentifiable = new Dictionary<int, T>();
		protected Dictionary<int, T> IdentifierToIdentifiable
		{
			get
			{
				if (identifierToIdentifiable == null)
					BuildIdentifiableDict();

				return identifierToIdentifiable;
			}
		}

		int idCounter;

		public virtual int[] GetIdentifiers()
		{
			return IdentifierToIdentifiable.GetKeyArray();
		}

		public virtual T GetIdentifiable(int identifier)
		{
			T identifiable;

			if (!IdentifierToIdentifiable.TryGetValue(identifier, out identifiable))
				Debug.LogError(string.Format("{0} with id {1} was not found.", typeof(T).Name, identifier));

			return identifiable;
		}

		public virtual List<T> GetIdentifiables()
		{
			return identifiables;
		}

		public virtual void SetUniqueIdentifier(T identifiable)
		{
			idCounter += 1;
			identifiable.Id = idCounter;

			AddIdentifiable(identifiable);
		}

		public virtual void SetUniqueIdentifiers(IList<T> identifiables)
		{
			for (int i = 0; i < identifiables.Count; i++)
			{
				T identifiable = identifiables[i];
				SetUniqueIdentifier(identifiable);
			}
		}

		public virtual void AddIdentifiable(T identifiable)
		{
			if (!identifiables.Contains(identifiable))
				identifiables.Add(identifiable);

			IdentifierToIdentifiable[identifiable.Id] = identifiable;
		}

		public virtual void RemoveIdentifier(int identifier)
		{
			if (IdentifierToIdentifiable.ContainsKey(identifier))
				RemoveIdentifiable(IdentifierToIdentifiable[identifier]);
		}

		public virtual void RemoveIdentifiable(T identifiable)
		{
			identifiables.Remove(identifiable);
			IdentifierToIdentifiable.Remove(identifiable.Id);
		}

		public virtual void ResetUniqueIdentifiers(IList<T> identifiables)
		{
			Reset();
			SetUniqueIdentifiers(identifiables);
		}

		public void Reset()
		{
			identifiables.Clear();
			IdentifierToIdentifiable.Clear();
			idCounter = 0;
		}

		public virtual bool ContainsIdentifier(int id)
		{
			return IdentifierToIdentifiable.ContainsKey(id);
		}

		public virtual bool ContainsIdentifiable(T identifiable)
		{
			return identifiables.Contains(identifiable);
		}

		public void BuildIdentifiableDict()
		{
			identifierToIdentifiable = new Dictionary<int, T>();

			for (int i = 0; i < identifiables.Count; i++)
			{
				T identifiable = identifiables[i];
				identifierToIdentifiable[identifiable.Id] = identifiable;
			}
		}
	}
}

