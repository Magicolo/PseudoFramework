using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Internal.Schema
{
	public class FunctionNode : FunctionNodeBase<FunctionDefinition>
	{
		public FunctionNode(FunctionDefinition functionDefinition) : base(functionDefinition) { }

		public override ExecutionResults Execute()
		{
			functionDefinition.Method();

			return base.Execute();
		}

		public override ValueInlet GetValueInlet(int index)
		{
			return null;
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return null;
		}
	}

	public class FunctionInNode<TIn> : FunctionNodeBase<FunctionInDefinition<TIn>>
	{
		readonly ValueInlet<TIn> inlet = new ValueInlet<TIn>();

		public FunctionInNode(FunctionInDefinition<TIn> functionDefinition) : base(functionDefinition) { }

		public override ExecutionResults Execute()
		{
			functionDefinition.Method(inlet.PullValue());

			return base.Execute();
		}

		public override ValueInlet GetValueInlet(int index)
		{
			return inlet;
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return null;
		}
	}

	public class FunctionInNode<TIn1, TIn2> : FunctionNodeBase<FunctionInDefinition<TIn1, TIn2>>
	{
		readonly ValueInlet<TIn1> inlet1 = new ValueInlet<TIn1>();
		readonly ValueInlet<TIn2> inlet2 = new ValueInlet<TIn2>();

		public FunctionInNode(FunctionInDefinition<TIn1, TIn2> functionDefinition) : base(functionDefinition) { }

		public override ExecutionResults Execute()
		{
			functionDefinition.Method(inlet1.PullValue(), inlet2.PullValue());

			return base.Execute();
		}

		public override ValueInlet GetValueInlet(int index)
		{
			switch (index)
			{
				default:
				case 0:
					return inlet1;
				case 1:
					return inlet2;
			}
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return null;
		}
	}

	public class FunctionInNode<TIn1, TIn2, TIn3> : FunctionNodeBase<FunctionInDefinition<TIn1, TIn2, TIn3>>
	{
		readonly ValueInlet<TIn1> inlet1 = new ValueInlet<TIn1>();
		readonly ValueInlet<TIn2> inlet2 = new ValueInlet<TIn2>();
		readonly ValueInlet<TIn3> inlet3 = new ValueInlet<TIn3>();

		public FunctionInNode(FunctionInDefinition<TIn1, TIn2, TIn3> functionDefinition) : base(functionDefinition) { }

		public override ExecutionResults Execute()
		{
			functionDefinition.Method(inlet1.PullValue(), inlet2.PullValue(), inlet3.PullValue());

			return base.Execute();
		}

		public override ValueInlet GetValueInlet(int index)
		{
			switch (index)
			{
				default:
				case 0:
					return inlet1;
				case 1:
					return inlet2;
				case 2:
					return inlet3;
			}
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return null;
		}
	}

	public class FunctionInNode<TIn1, TIn2, TIn3, TIn4> : FunctionNodeBase<FunctionInDefinition<TIn1, TIn2, TIn3, TIn4>>
	{
		readonly ValueInlet<TIn1> inlet1 = new ValueInlet<TIn1>();
		readonly ValueInlet<TIn2> inlet2 = new ValueInlet<TIn2>();
		readonly ValueInlet<TIn3> inlet3 = new ValueInlet<TIn3>();
		readonly ValueInlet<TIn4> inlet4 = new ValueInlet<TIn4>();

		public FunctionInNode(FunctionInDefinition<TIn1, TIn2, TIn3, TIn4> functionDefinition) : base(functionDefinition) { }

		public override ExecutionResults Execute()
		{
			functionDefinition.Method(inlet1.PullValue(), inlet2.PullValue(), inlet3.PullValue(), inlet4.PullValue());

			return base.Execute();
		}

		public override ValueInlet GetValueInlet(int index)
		{
			switch (index)
			{
				default:
				case 0:
					return inlet1;
				case 1:
					return inlet2;
				case 2:
					return inlet3;
				case 3:
					return inlet4;
			}
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return null;
		}
	}

	public class FunctionInNode<TIn1, TIn2, TIn3, TIn4, TIn5> : FunctionNodeBase<FunctionInDefinition<TIn1, TIn2, TIn3, TIn4, TIn5>>
	{
		readonly ValueInlet<TIn1> inlet1 = new ValueInlet<TIn1>();
		readonly ValueInlet<TIn2> inlet2 = new ValueInlet<TIn2>();
		readonly ValueInlet<TIn3> inlet3 = new ValueInlet<TIn3>();
		readonly ValueInlet<TIn4> inlet4 = new ValueInlet<TIn4>();
		readonly ValueInlet<TIn5> inlet5 = new ValueInlet<TIn5>();

		public FunctionInNode(FunctionInDefinition<TIn1, TIn2, TIn3, TIn4, TIn5> functionDefinition) : base(functionDefinition) { }

		public override ExecutionResults Execute()
		{
			functionDefinition.Method(inlet1.PullValue(), inlet2.PullValue(), inlet3.PullValue(), inlet4.PullValue(), inlet5.PullValue());

			return base.Execute();
		}

		public override ValueInlet GetValueInlet(int index)
		{
			switch (index)
			{
				default:
				case 0:
					return inlet1;
				case 1:
					return inlet2;
				case 2:
					return inlet3;
				case 3:
					return inlet4;
				case 4:
					return inlet5;
			}
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return null;
		}
	}

	public class FunctionOutNode<TOut> : FunctionNodeBase<FunctionOutDefinition<TOut>>
	{
		readonly PushValueOutlet<TOut> outlet = new PushValueOutlet<TOut>();

		public FunctionOutNode(FunctionOutDefinition<TOut> functionDefinition) : base(functionDefinition) { }

		public override ExecutionResults Execute()
		{
			outlet.PushValue(functionDefinition.Method());

			return base.Execute();
		}

		public override ValueInlet GetValueInlet(int index)
		{
			return null;
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return outlet;
		}
	}

	public class FunctionOutNode<TOut1, TOut2> : FunctionNodeBase<FunctionOutDefinition<TOut1, TOut2>>
	{
		readonly PushValueOutlet<TOut1> outlet1 = new PushValueOutlet<TOut1>();
		readonly PushValueOutlet<TOut2> outlet2 = new PushValueOutlet<TOut2>();

		public FunctionOutNode(FunctionOutDefinition<TOut1, TOut2> functionDefinition) : base(functionDefinition) { }

		public override ExecutionResults Execute()
		{
			TOut1 result1;
			TOut2 result2;

			functionDefinition.Method(out result1, out result2);

			outlet1.PushValue(result1);
			outlet2.PushValue(result2);

			return base.Execute();
		}

		public override ValueInlet GetValueInlet(int index)
		{
			return null;
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			switch (index)
			{
				default:
				case 0:
					return outlet1;
				case 1:
					return outlet2;
			}
		}
	}

	public class FunctionOutNode<TOut1, TOut2, TOut3> : FunctionNodeBase<FunctionOutDefinition<TOut1, TOut2, TOut3>>
	{
		readonly PushValueOutlet<TOut1> outlet1 = new PushValueOutlet<TOut1>();
		readonly PushValueOutlet<TOut2> outlet2 = new PushValueOutlet<TOut2>();
		readonly PushValueOutlet<TOut3> outlet3 = new PushValueOutlet<TOut3>();

		public FunctionOutNode(FunctionOutDefinition<TOut1, TOut2, TOut3> functionDefinition) : base(functionDefinition) { }

		public override ExecutionResults Execute()
		{
			TOut1 result1;
			TOut2 result2;
			TOut3 result3;

			functionDefinition.Method(out result1, out result2, out result3);

			outlet1.PushValue(result1);
			outlet2.PushValue(result2);
			outlet3.PushValue(result3);

			return base.Execute();
		}

		public override ValueInlet GetValueInlet(int index)
		{
			return null;
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			switch (index)
			{
				default:
				case 0:
					return outlet1;
				case 1:
					return outlet2;
				case 2:
					return outlet3;
			}
		}
	}

	public class FunctionInOutNode<TIn, TOut> : FunctionNodeBase<FunctionInOutDefinition<TIn, TOut>>
	{
		readonly ValueInlet<TIn> inlet = new ValueInlet<TIn>();
		readonly PushValueOutlet<TOut> outlet = new PushValueOutlet<TOut>();

		public FunctionInOutNode(FunctionInOutDefinition<TIn, TOut> functionDefinition) : base(functionDefinition) { }

		public override ExecutionResults Execute()
		{
			outlet.PushValue(functionDefinition.Method(inlet.PullValue()));

			return base.Execute();
		}

		public override ValueInlet GetValueInlet(int index)
		{
			return inlet;
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return outlet;
		}
	}

	public class FunctionInOutNode<TIn1, TIn2, TOut> : FunctionNodeBase<FunctionInOutDefinition<TIn1, TIn2, TOut>>
	{
		readonly ValueInlet<TIn1> inlet1 = new ValueInlet<TIn1>();
		readonly ValueInlet<TIn2> inlet2 = new ValueInlet<TIn2>();
		readonly PushValueOutlet<TOut> outlet = new PushValueOutlet<TOut>();

		public FunctionInOutNode(FunctionInOutDefinition<TIn1, TIn2, TOut> functionDefinition) : base(functionDefinition) { }

		public override ExecutionResults Execute()
		{
			outlet.PushValue(functionDefinition.Method(inlet1.PullValue(), inlet2.PullValue()));

			return base.Execute();
		}

		public override ValueInlet GetValueInlet(int index)
		{
			switch (index)
			{
				default:
				case 0:
					return inlet1;
				case 1:
					return inlet2;
			}
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return outlet;
		}
	}

	public class FunctionInOutNode<TIn1, TIn2, TIn3, TOut> : FunctionNodeBase<FunctionInOutDefinition<TIn1, TIn2, TIn3, TOut>>
	{
		readonly ValueInlet<TIn1> inlet1 = new ValueInlet<TIn1>();
		readonly ValueInlet<TIn2> inlet2 = new ValueInlet<TIn2>();
		readonly ValueInlet<TIn3> inlet3 = new ValueInlet<TIn3>();
		readonly PushValueOutlet<TOut> outlet = new PushValueOutlet<TOut>();

		public FunctionInOutNode(FunctionInOutDefinition<TIn1, TIn2, TIn3, TOut> functionDefinition) : base(functionDefinition) { }

		public override ExecutionResults Execute()
		{
			outlet.PushValue(functionDefinition.Method(inlet1.PullValue(), inlet2.PullValue(), inlet3.PullValue()));

			return base.Execute();
		}

		public override ValueInlet GetValueInlet(int index)
		{
			switch (index)
			{
				default:
				case 0:
					return inlet1;
				case 1:
					return inlet2;
				case 2:
					return inlet3;
			}
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return outlet;
		}
	}

	public class FunctionInOutNode<TIn1, TIn2, TIn3, TIn4, TOut> : FunctionNodeBase<FunctionInOutDefinition<TIn1, TIn2, TIn3, TIn4, TOut>>
	{
		readonly ValueInlet<TIn1> inlet1 = new ValueInlet<TIn1>();
		readonly ValueInlet<TIn2> inlet2 = new ValueInlet<TIn2>();
		readonly ValueInlet<TIn3> inlet3 = new ValueInlet<TIn3>();
		readonly ValueInlet<TIn4> inlet4 = new ValueInlet<TIn4>();
		readonly PushValueOutlet<TOut> outlet = new PushValueOutlet<TOut>();

		public FunctionInOutNode(FunctionInOutDefinition<TIn1, TIn2, TIn3, TIn4, TOut> functionDefinition) : base(functionDefinition) { }

		public override ExecutionResults Execute()
		{
			outlet.PushValue(functionDefinition.Method(inlet1.PullValue(), inlet2.PullValue(), inlet3.PullValue(), inlet4.PullValue()));

			return base.Execute();
		}

		public override ValueInlet GetValueInlet(int index)
		{
			switch (index)
			{
				default:
				case 0:
					return inlet1;
				case 1:
					return inlet2;
				case 2:
					return inlet3;
				case 3:
					return inlet4;
			}
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return outlet;
		}
	}

	public class FunctionInOutNode<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> : FunctionNodeBase<FunctionInOutDefinition<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>>
	{
		readonly ValueInlet<TIn1> inlet1 = new ValueInlet<TIn1>();
		readonly ValueInlet<TIn2> inlet2 = new ValueInlet<TIn2>();
		readonly ValueInlet<TIn3> inlet3 = new ValueInlet<TIn3>();
		readonly ValueInlet<TIn4> inlet4 = new ValueInlet<TIn4>();
		readonly ValueInlet<TIn5> inlet5 = new ValueInlet<TIn5>();
		readonly PushValueOutlet<TOut> outlet = new PushValueOutlet<TOut>();

		public FunctionInOutNode(FunctionInOutDefinition<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> functionDefinition) : base(functionDefinition) { }

		public override ExecutionResults Execute()
		{
			outlet.PushValue(functionDefinition.Method(inlet1.PullValue(), inlet2.PullValue(), inlet3.PullValue(), inlet4.PullValue(), inlet5.PullValue()));

			return base.Execute();
		}

		public override ValueInlet GetValueInlet(int index)
		{
			switch (index)
			{
				default:
				case 0:
					return inlet1;
				case 1:
					return inlet2;
				case 2:
					return inlet3;
				case 3:
					return inlet4;
				case 4:
					return inlet5;
			}
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return outlet;
		}
	}
}
