using System;
using Pseudo.Internal.Editor;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class PopupAttribute : CustomAttributeBase
	{
		public string arrayName;
		public string onChangeCallback = "";

		public PopupAttribute(string arrayName)
		{
			this.arrayName = arrayName;
		}

		public PopupAttribute(string arrayName, string onChangeCallback)
		{
			this.arrayName = arrayName;
			this.onChangeCallback = onChangeCallback;
		}
	}
}