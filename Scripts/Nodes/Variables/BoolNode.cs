using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Variable")]
	public class BoolNode : VariableNode
	{
		[SerializeField] bool value;

		public override void Initialize()
		{
			base.Initialize();
			output = AddValueOutput(typeof(bool));
		}

		public override object GetValue()
		{
			return value;
		}
	}
}