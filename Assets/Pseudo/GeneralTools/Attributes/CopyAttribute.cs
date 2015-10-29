using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Delegate | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public sealed class CopyAttribute : Attribute
{
}
