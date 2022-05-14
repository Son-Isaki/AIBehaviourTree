using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Variable")]
	public class Vector3Node : VariableNode
	{
		[SerializeField] Vector3 value;

		public override void Initialize()
		{
			base.Initialize();
			AddValueOutput(typeof(Vector3));
		}

		public override object GetValue()
		{
			return value;
		}
	}
}