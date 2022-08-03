using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Debug")]
	public class DebugTextNode : DecoratorNode
	{
		public string message;

		protected override State Execute()
		{
			Debug.Log($"{Target.name} ({GetName()}) : {message}");
			return base.Execute();
		}
	}
}