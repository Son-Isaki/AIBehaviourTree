using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Variable")]
	public class IntNode : VariableNode
	{
		[SerializeField] int value;

		public override void Initialize()
		{
			base.Initialize();
			output = AddValueOutput(typeof(int));
		}

		public override object GetValue()
		{
			return value;
		}
	}
}