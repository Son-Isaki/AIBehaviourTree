using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Variable")]
	public class StringNode : VariableNode
	{
		[SerializeField] string value;

		public override void Initialize()
		{
			base.Initialize();
			output = AddValueOutput(typeof(string));
		}

		public override object GetValue()
		{
			return value;
		}
	}
}