using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.PoolingNOOOO.Internal
{
	public abstract class PoolBase : IPool
	{
		public IPoolingFactory Factory
		{
			get { return factory; }
		}
		public IPoolingUpdater Updater
		{
			get { return updater; }
		}
		public IInitializer Initializer
		{
			get { return initializer; }
		}
		public Type Type
		{
			get { return reference.GetType(); }
		}
		public int Size
		{
			get { return hashedInstances.Count; }
		}

		readonly object reference;
		readonly IPoolingFactory factory;
		readonly IPoolingUpdater updater;
		readonly IInitializer initializer;
		//readonly int startSize;
		readonly HashSet<object> hashedInstances = new HashSet<object>();

		//readonly Constructor constructor;
		//readonly Destructor destructor;
		//readonly bool isPoolable;
		//readonly IPoolUpdater updater;
		//bool updating;

		protected PoolBase(object reference, IPoolingFactory factory, IPoolingUpdater updater, IInitializer initializer, int startSize)
		{
			this.reference = reference;
			this.factory = factory;
			this.updater = updater;
			this.initializer = initializer;
			//this.startSize = startSize;
		}

		//protected PoolBase(object reference, Type type, Constructor constructor, Destructor destructor, int startSize, bool initialize)
		//{
		//	this.reference = reference;
		//	this.constructor = constructor ?? Construct;
		//	this.destructor = destructor ?? Destroy;
		//	this.startSize = startSize;

		//	Type = type;
		//	hashedInstances = new HashSet<object>();
		//	updater = ApplicationUtility.IsMultiThreaded ? new AsyncPoolUpdater() : new SyncPoolUpdater();
		//}

		//public virtual object Create()
		//{
		//	return CreateObject();
		//}

		//public virtual void Recycle(object instance)
		//{
		//	if (instance == null)
		//		return;

		//	if (instance.GetType() != Type)
		//		throw new ArgumentException(string.Format("The type of the instance ({0}) doesn't match the pool type ({1}).", instance.GetType().Name, Type.Name));

		//	if (hashedInstances.Contains(instance))
		//		return;

		//	if (isPoolable)
		//		((IPoolable)instance).OnRecycle();

		//	Enqueue(instance, true);
		//	updater.Update();
		//}

		//public virtual void RecycleElements(IList elements)
		//{
		//	if (elements == null)
		//		return;

		//	for (int i = 0; i < elements.Count; i++)
		//		Recycle(elements[i]);

		//	elements.Clear();
		//}

		public bool Contains(object instance)
		{
			return hashedInstances.Contains(instance);
		}

		public object Create()
		{
			throw new NotImplementedException();
		}

		public void Recycle(object instance)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public void Reset()
		{
			throw new NotImplementedException();
		}

		//public virtual void Clear()
		//{
		//	updater.Clear();

		//	var enumerator = hashedInstances.GetEnumerator();

		//	while (enumerator.MoveNext())
		//		destructor(enumerator.Current);

		//	enumerator.Dispose();

		//	hashedInstances.Clear();
		//}

		//public virtual void Reset()
		//{
		//	Initialize();
		//	updater.Reset();
		//}

		//protected void Initialize()
		//{
		//	updater.Initializer = PoolUtility.GetPoolInitializer(reference);

		//	while (Size < startSize)
		//		Enqueue(CreateInstance(), false);
		//}

		//protected virtual object CreateObject()
		//{
		//	var instance = GetInstance();

		//	if (isPoolable)
		//		((IPoolable)instance).OnCreate();

		//	updater.Update();

		//	return instance;
		//}

		//protected virtual object GetInstance()
		//{
		//	var instance = Dequeue();

		//	if (instance == null)
		//		instance = CreateInstance();

		//	return instance;
		//}

		//protected virtual object CreateInstance()
		//{
		//	var instance = constructor();
		//	updater.Initializer.InitializeFields(instance);

		//	return instance;
		//}

		//protected virtual void Enqueue(object instance, bool initialize)
		//{
		//	if (hashedInstances.Add(instance))
		//		updater.Enqueue(instance, initialize);
		//}

		//protected virtual object Dequeue()
		//{
		//	var instance = updater.Dequeue();
		//	hashedInstances.Remove(instance);

		//	return instance;
		//}

		//protected virtual object Construct()
		//{
		//	return Activator.CreateInstance(Type);
		//}

		//protected virtual void Destroy(object instance) { }
	}
}
