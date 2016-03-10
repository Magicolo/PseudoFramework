using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Schema
{
	public delegate void Function();
	public delegate void FunctionIn<in TIn>(TIn argument);
	public delegate void FunctionIn<in TIn1, in TIn2>(TIn1 argument1, TIn2 argument2);
	public delegate void FunctionIn<in TIn1, in TIn2, in TIn3>(TIn1 argument1, TIn2 argument2, TIn3 argument3);
	public delegate void FunctionIn<in TIn1, in TIn2, in TIn3, in TIn4>(TIn1 argument1, TIn2 argument2, TIn3 argument3, TIn4 argument4);
	public delegate void FunctionIn<in TIn1, in TIn2, in TIn3, in TIn4, in TIn5>(TIn1 argument1, TIn2 argument2, TIn3 argument3, TIn4 argument4, TIn5 argument5);

	public delegate TOut FunctionOut<out TOut>();
	public delegate void FunctionOut<TOut1, TOut2>(out TOut1 argument1, out TOut2 argument2);
	public delegate void FunctionOut<TOut1, TOut2, TOut3>(out TOut1 argument1, out TOut2 argument2, out TOut3 argument3);

	public delegate TOut FunctionInOut<in TIn, out TOut>(TIn argument);
	public delegate TOut FunctionInOut<in TIn1, in TIn2, out TOut>(TIn1 argument1, TIn2 argument2);
	public delegate TOut FunctionInOut<in TIn1, in TIn2, in TIn3, out TOut>(TIn1 argument1, TIn2 argument2, TIn3 argument3);
	public delegate TOut FunctionInOut<in TIn1, in TIn2, in TIn3, in TIn4, out TOut>(TIn1 argument1, TIn2 argument2, TIn3 argument3, TIn4 argument4);
	public delegate TOut FunctionInOut<in TIn1, in TIn2, in TIn3, in TIn4, in TIn5, out TOut>(TIn1 argument1, TIn2 argument2, TIn3 argument3, TIn4 argument4, TIn5 argument5);

	public class FunctionDefinition : FunctionDefinitionBase<Function>
	{
		public FunctionDefinition(object instance, MethodInfo method) : base(instance, method) { }

		public override IFunctionNode CreateNode()
		{
			return new FunctionNode(this);
		}
	}

	public class FunctionInDefinition<TIn> : FunctionDefinitionBase<FunctionIn<TIn>>
	{
		public FunctionInDefinition(object instance, MethodInfo method) : base(instance, method) { }

		public override IFunctionNode CreateNode()
		{
			return new FunctionInNode<TIn>(this);
		}
	}

	public class FunctionInDefinition<TIn1, TIn2> : FunctionDefinitionBase<FunctionIn<TIn1, TIn2>>
	{
		public FunctionInDefinition(object instance, MethodInfo method) : base(instance, method) { }

		public override IFunctionNode CreateNode()
		{
			return new FunctionInNode<TIn1, TIn2>(this);
		}
	}

	public class FunctionInDefinition<TIn1, TIn2, TIn3> : FunctionDefinitionBase<FunctionIn<TIn1, TIn2, TIn3>>
	{
		public FunctionInDefinition(object instance, MethodInfo method) : base(instance, method) { }

		public override IFunctionNode CreateNode()
		{
			return new FunctionInNode<TIn1, TIn2, TIn3>(this);
		}
	}

	public class FunctionInDefinition<TIn1, TIn2, TIn3, TIn4> : FunctionDefinitionBase<FunctionIn<TIn1, TIn2, TIn3, TIn4>>
	{
		public FunctionInDefinition(object instance, MethodInfo method) : base(instance, method) { }

		public override IFunctionNode CreateNode()
		{
			return new FunctionInNode<TIn1, TIn2, TIn3, TIn4>(this);
		}
	}

	public class FunctionInDefinition<TIn1, TIn2, TIn3, TIn4, TIn5> : FunctionDefinitionBase<FunctionIn<TIn1, TIn2, TIn3, TIn4, TIn5>>
	{
		public FunctionInDefinition(object instance, MethodInfo method) : base(instance, method) { }

		public override IFunctionNode CreateNode()
		{
			return new FunctionInNode<TIn1, TIn2, TIn3, TIn4, TIn5>(this);
		}
	}

	public class FunctionOutDefinition<TOut> : FunctionDefinitionBase<FunctionOut<TOut>>
	{
		public FunctionOutDefinition(object instance, MethodInfo method) : base(instance, method) { }

		public override IFunctionNode CreateNode()
		{
			return new FunctionOutNode<TOut>(this);
		}
	}

	public class FunctionOutDefinition<TOut1, TOut2> : FunctionDefinitionBase<FunctionOut<TOut1, TOut2>>
	{
		public FunctionOutDefinition(object instance, MethodInfo method) : base(instance, method) { }

		public override IFunctionNode CreateNode()
		{
			return new FunctionOutNode<TOut1, TOut2>(this);
		}
	}

	public class FunctionOutDefinition<TOut1, TOut2, TOut3> : FunctionDefinitionBase<FunctionOut<TOut1, TOut2, TOut3>>
	{
		public FunctionOutDefinition(object instance, MethodInfo method) : base(instance, method) { }

		public override IFunctionNode CreateNode()
		{
			return new FunctionOutNode<TOut1, TOut2, TOut3>(this);
		}
	}

	public class FunctionInOutDefinition<TIn, TOut> : FunctionDefinitionBase<FunctionInOut<TIn, TOut>>
	{
		public FunctionInOutDefinition(object instance, MethodInfo method) : base(instance, method) { }

		public override IFunctionNode CreateNode()
		{
			return new FunctionInOutNode<TIn, TOut>(this);
		}
	}

	public class FunctionInOutDefinition<TIn1, TIn2, TOut> : FunctionDefinitionBase<FunctionInOut<TIn1, TIn2, TOut>>
	{
		public FunctionInOutDefinition(object instance, MethodInfo method) : base(instance, method) { }

		public override IFunctionNode CreateNode()
		{
			return new FunctionInOutNode<TIn1, TIn2, TOut>(this);
		}
	}

	public class FunctionInOutDefinition<TIn1, TIn2, TIn3, TOut> : FunctionDefinitionBase<FunctionInOut<TIn1, TIn2, TIn3, TOut>>
	{
		public FunctionInOutDefinition(object instance, MethodInfo method) : base(instance, method) { }

		public override IFunctionNode CreateNode()
		{
			return new FunctionInOutNode<TIn1, TIn2, TIn3, TOut>(this);
		}
	}

	public class FunctionInOutDefinition<TIn1, TIn2, TIn3, TIn4, TOut> : FunctionDefinitionBase<FunctionInOut<TIn1, TIn2, TIn3, TIn4, TOut>>
	{
		public FunctionInOutDefinition(object instance, MethodInfo method) : base(instance, method) { }

		public override IFunctionNode CreateNode()
		{
			return new FunctionInOutNode<TIn1, TIn2, TIn3, TIn4, TOut>(this);
		}
	}

	public class FunctionInOutDefinition<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> : FunctionDefinitionBase<FunctionInOut<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>>
	{
		public FunctionInOutDefinition(object instance, MethodInfo method) : base(instance, method) { }

		public override IFunctionNode CreateNode()
		{
			return new FunctionInOutNode<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(this);
		}
	}
}
