using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Common")]
	public class TargetNode : VariableNode
	{
		public override void Initialize()
		{
			base.Initialize();
			output = AddValueOutput(typeof(GameObject));
			SetDescription("GameObject the tree is attached to");
		}

		public override object GetValue()
		{
			return Target;
		}
	}
}