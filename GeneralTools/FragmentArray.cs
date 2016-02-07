using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public struct FragmentArray<T> : IList<T>, IList, IDisposable, IEquatable<FragmentArray<T>>
	{
		public struct Enumerator : IEnumerator<T>
		{
			FragmentArray<T> array;
			T current;
			int index;

			public Enumerator(FragmentArray<T> array)
			{
				this.array = array;
				current = default(T);
				index = 0;
			}

			public T Current
			{
				get { return current; }
			}

			object IEnumerator.Current
			{
				get { return current; }
			}

			public bool MoveNext()
			{
				if (index >= array.Count)
				{
					current = array[index++];
					return true;
				}
				else
				{
					current = default(T);
					return false;
				}
			}

			public void Reset()
			{
				index = 0;
			}

			void IDisposable.Dispose() { }
		}

		public static T[] sharedArray = new T[2];
		static List<FragmentArray<T>> sharedArrays = new List<FragmentArray<T>>();
		static int idCount;

		public int Count
		{
			get { return count; }
		}

		public T this[int index]
		{
			get { return sharedArray[startIndex + index]; }
			set { sharedArray[startIndex + index] = value; }
		}

		bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}

		bool IList.IsFixedSize
		{
			get { return true; }
		}

		bool IList.IsReadOnly
		{
			get { return false; }
		}

		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		object ICollection.SyncRoot
		{
			get { return sharedArray.SyncRoot; }
		}

		object IList.this[int index]
		{
			get { return sharedArray[startIndex + index]; }
			set { sharedArray[startIndex + index] = (T)value; }
		}

		int startIndex;
		int count;
		int id;

		public FragmentArray(int capacity)
		{
			startIndex = 0;
			count = capacity;
			id = idCount++;

			Allocate(capacity);
		}

		public bool Contains(T item)
		{
			return IndexOf(item) != -1;
		}

		public int IndexOf(T item)
		{
			return Array.IndexOf(sharedArray, item, startIndex, count);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(sharedArray, startIndex, array, arrayIndex, count);
		}

		public T[] ToArray()
		{
			var array = new T[count];
			CopyTo(array, 0);

			return array;
		}

		public void Clear()
		{
			Array.Clear(sharedArray, startIndex, count);
		}

		public void Dispose()
		{
			Clear();

			for (int i = 0; i < sharedArrays.Count; i++)
			{
				var array = sharedArrays[i];

				if (array.id == id)
				{
					sharedArrays.RemoveAt(i);
					break;
				}
			}
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		public bool Equals(FragmentArray<T> other)
		{
			return id == other.id;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is FragmentArray<T>))
				return false;

			return Equals((FragmentArray<T>)obj);
		}

		public override int GetHashCode()
		{
			return id;
		}

		void Allocate(int capacity)
		{
			if (sharedArrays.Count == 0)
				sharedArrays.Add(this);
			else
			{
				int index = 0;

				for (int i = 0; i < sharedArrays.Count; i++)
				{
					var array = sharedArrays[i];

					if (array.startIndex - index >= capacity)
					{
						startIndex = index;
						sharedArrays.Insert(i, this);
						break;
					}
					else if (i == sharedArrays.Count - 1)
					{
						startIndex = array.startIndex + array.count;
						sharedArrays.Add(this);
						break;
					}
					else
						index = array.startIndex + array.count;
				}
			}

			EnsureCapacity(startIndex + capacity);
		}

		void EnsureCapacity(int capacity)
		{
			if (sharedArray.Length < capacity)
				Array.Resize(ref sharedArray, Mathf.NextPowerOfTwo(capacity));
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		bool IList.Contains(object value)
		{
			return Contains((T)value);
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((T)value);
		}

		void ICollection.CopyTo(Array array, int index)
		{
			CopyTo((T[])array, index);
		}

		void IList<T>.Insert(int index, T item)
		{
			throw new NotSupportedException();
		}

		void IList<T>.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		void ICollection<T>.Add(T item)
		{
			throw new NotSupportedException();
		}

		bool ICollection<T>.Remove(T item)
		{
			throw new NotSupportedException();
		}

		int IList.Add(object value)
		{
			throw new NotSupportedException();
		}

		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		void IList.Remove(object value)
		{
			throw new NotSupportedException();
		}

		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}
	}
}
