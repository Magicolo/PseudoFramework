using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[System.Serializable]
	public class IdManager<T> where T : IIdentifiable
	{
		[SerializeField]
		List<T> _identifiables = new List<T>();

		Dictionary<int, T> _idIdentifiableDict = new Dictionary<int, T>();
		protected Dictionary<int, T> IdIdentifiableDict
		{
			get
			{
				if (_idIdentifiableDict == null)
					BuildIdentifiableDict();

				return _idIdentifiableDict;
			}
		}

		int _idCounter;

		public virtual int[] GetIds()
		{
			return IdIdentifiableDict.GetKeyArray();
		}

		public virtual T GetIdentifiable(int id)
		{
			T identifiable = default(T);

			try
			{
				identifiable = IdIdentifiableDict[id];
			}
			catch
			{
				Logger.LogError(string.Format("{0} with id {1} was not found.", typeof(T).Name, id));
			}

			return identifiable;
		}

		public virtual T[] GetIdentifiables()
		{
			return _identifiables.ToArray();
		}

		public virtual T[] GetIdentifiables(IList<int> ids)
		{
			T[] identifiableArray = new T[ids.Count];

			for (int i = 0; i < ids.Count; i++)
			{
				identifiableArray[i] = GetIdentifiable(ids[i]);
			}

			return identifiableArray;
		}

		public virtual int GetUniqueId()
		{
			_idCounter += 1;
			return _idCounter;
		}

		public virtual void SetUniqueId(T identifiable)
		{
			_idCounter += 1;
			identifiable.Id = _idCounter;

			AddIdentifiable(identifiable);
		}

		public virtual void SetUniqueIds(IList<T> identifiables)
		{
			foreach (T identifiable in identifiables)
			{
				SetUniqueId(identifiable);
			}
		}

		public virtual void AddIdentifiable(T identifiable)
		{
			if (!_identifiables.Contains(identifiable))
				_identifiables.Add(identifiable);

			IdIdentifiableDict[identifiable.Id] = identifiable;
		}

		public virtual void RemoveId(int id)
		{
			if (IdIdentifiableDict.ContainsKey(id))
			{
				RemoveIdentifiable(IdIdentifiableDict[id]);
			}
		}

		public virtual void RemoveIdentifiable(T identifiable)
		{
			_identifiables.Remove(identifiable);
			IdIdentifiableDict.Remove(identifiable.Id);
		}

		public virtual void ResetUniqueIds(IList<T> identifiables)
		{
			Reset();
			SetUniqueIds(identifiables);
		}

		public void Reset()
		{
			_identifiables.Clear();
			IdIdentifiableDict.Clear();
			_idCounter = 0;
		}

		public virtual bool ContainsId(int id)
		{
			return IdIdentifiableDict.ContainsKey(id);
		}

		public virtual bool ContainsIdentifiable(T identifiable)
		{
			return _identifiables.Contains(identifiable);
		}

		public void BuildIdentifiableDict()
		{
			_idIdentifiableDict = new Dictionary<int, T>();

			for (int i = 0; i < _identifiables.Count; i++)
			{
				T identifiable = _identifiables[i];

				_idIdentifiableDict[identifiable.Id] = identifiable;
			}
		}
	}
}

