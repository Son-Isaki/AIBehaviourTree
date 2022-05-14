using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Debug")]
	public class DebugNode : ActionNode
	{
		public string message;

		protected override void OnStart()
		{

		}

		protected override void OnStop()
		{

		}

		protected override State Execute()
		{
			Debug.Log($"{GetName()} : {message}");
			return State.Success;
		}
	}
}