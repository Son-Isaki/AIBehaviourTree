using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Variable")]
	public class FloatNode : VariableNode
	{
		[SerializeField] float value;

		public override void Initialize()
		{
			base.Initialize();
			output = AddValueOutput(typeof(float));
		}

		public override object GetValue()
		{
			return value;
		}
	}
}