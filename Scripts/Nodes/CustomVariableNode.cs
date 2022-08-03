using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	public abstract class CustomVariableNode : VariableNode
	{
		[SerializeField, Label("Name")] protected string variableName;
	}
}