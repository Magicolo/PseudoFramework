using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Internal.References;

namespace Pseudo
{
	[Serializable]
	public class IntProperty : PropertyReference<int> { }
	[Serializable]
	public class FloatProperty : PropertyReference<float> { }
	[Serializable]
	public class Vector2Property : PropertyReference<Vector2> { }
	[Serializable]
	public class Vector3Property : PropertyReference<Vector3> { }
	[Serializable]
	public class Vector4Property : PropertyReference<Vector4> { }
	[Serializable]
	public class ColorProperty : PropertyReference<Color> { }
	[Serializable]
	public class RectProperty : PropertyReference<Rect> { }
	[Serializable]
	public class BoundsProperty : PropertyReference<Bounds> { }
	[Serializable]
	public class StringProperty : PropertyReference<string> { }

	namespace Internal.References
	{
		public class PropertyReference<TValue> : PropertyReference, IPropertyReference, ISerializationCallbackReceiver
		{
			public override Type ValueType
			{
				get { return typeof(TValue); }
			}
			public TValue Value
			{
				get { return getter(); }
				set { setter(value); }
			}

			Func<TValue> getter;
			Action<TValue> setter;

			public PropertyReference()
			{
				Initialize();
			}

			public PropertyReference(UnityEngine.Object reference, string propertyName) : base(reference, propertyName) { }

			object IPropertyReference.Value
			{
				get { return Value; }
				set { Value = (TValue)value; }
			}

			protected override void Initialize()
			{
				base.Initialize();

				if (property == null || target == null)
				{
					getter = delegate { return default(TValue); };
					setter = delegate { };
				}
				else
				{
					getter = (Func<TValue>)Delegate.CreateDelegate(typeof(Func<TValue>), target, property.GetGetMethod(true));
					setter = (Action<TValue>)Delegate.CreateDelegate(typeof(Action<TValue>), target, property.GetSetMethod(true));
				}
			}

			public static implicit operator TValue(PropertyReference<TValue> reference)
			{
				return reference.Value;
			}
		}

		public abstract class PropertyReference : ISerializationCallbackReceiver
		{
			public UnityEngine.Object Target
			{
				get { return target; }
			}
			public PropertyInfo Property
			{
				get { return property; }
			}
			public abstract Type ValueType { get; }

			[SerializeField]
			protected UnityEngine.Object target;
			[SerializeField]
			protected string propertyName;
			protected PropertyInfo property;

			protected PropertyReference() { }

			protected PropertyReference(UnityEngine.Object reference, string propertyName)
			{
				this.target = reference;
				this.propertyName = propertyName;

				Initialize();
			}

			protected virtual void Initialize()
			{
				if (target != null && !string.IsNullOrEmpty(propertyName))
					property = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			}

			void ISerializationCallbackReceiver.OnBeforeSerialize() { }

			void ISerializationCallbackReceiver.OnAfterDeserialize()
			{
				Initialize();
			}
		}
	}
}
