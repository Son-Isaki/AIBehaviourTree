using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Variable")]
	public class Vector2Node : CustomVariableNode
	{
		[SerializeField] Vector2 value;

		public override void Initialize()
		{
			base.Initialize();
			output = AddValueOutput(typeof(Vector2));
		}

		public override object GetValue()
		{
			return value;
		}
	}
}