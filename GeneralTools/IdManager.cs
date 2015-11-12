using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System;

namespace Pseudo
{
	[Serializable]
	public class IdManager<T> where T : IIdentifiable
	{
		[SerializeField]
		protected List<T> identifiables = new List<T>();

		Dictionary<int, T> idIdentifiableDict = new Dictionary<int, T>();
		protected Dictionary<int, T> IdIdentifiableDict
		{
			get
			{
				if (idIdentifiableDict == null)
					BuildIdentifiableDict();

				return idIdentifiableDict;
			}
		}

		int idCounter;

		public virtual int[] GetIds()
		{
			return IdIdentifiableDict.GetKeyArray();
		}

		public virtual T GetIdentifiable(int id)
		{
			T identifiable;

			if (!IdIdentifiableDict.TryGetValue(id, out identifiable))
				Debug.LogError(string.Format("{0} with id {1} was not found.", typeof(T).Name, id));

			return identifiable;
		}

		public virtual List<T> GetIdentifiables()
		{
			return identifiables;
		}

		public virtual void SetUniqueId(T identifiable)
		{
			idCounter += 1;
			identifiable.Id = idCounter;

			AddIdentifiable(identifiable);
		}

		public virtual void SetUniqueIds(IList<T> identifiables)
		{
			for (int i = 0; i < identifiables.Count; i++)
			{
				T identifiable = identifiables[i];
				SetUniqueId(identifiable);
			}
		}

		public virtual void AddIdentifiable(T identifiable)
		{
			if (!identifiables.Contains(identifiable))
				identifiables.Add(identifiable);

			IdIdentifiableDict[identifiable.Id] = identifiable;
		}

		public virtual void RemoveId(int id)
		{
			if (IdIdentifiableDict.ContainsKey(id))
				RemoveIdentifiable(IdIdentifiableDict[id]);
		}

		public virtual void RemoveIdentifiable(T identifiable)
		{
			identifiables.Remove(identifiable);
			IdIdentifiableDict.Remove(identifiable.Id);
		}

		public virtual void ResetUniqueIds(IList<T> identifiables)
		{
			Reset();
			SetUniqueIds(identifiables);
		}

		public void Reset()
		{
			identifiables.Clear();
			IdIdentifiableDict.Clear();
			idCounter = 0;
		}

		public virtual bool ContainsId(int id)
		{
			return IdIdentifiableDict.ContainsKey(id);
		}

		public virtual bool ContainsIdentifiable(T identifiable)
		{
			return identifiables.Contains(identifiable);
		}

		public void BuildIdentifiableDict()
		{
			idIdentifiableDict = new Dictionary<int, T>();

			for (int i = 0; i < identifiables.Count; i++)
			{
				T identifiable = identifiables[i];
				idIdentifiableDict[identifiable.Id] = identifiable;
			}
		}
	}
}

