using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Node
{
    public class DebugNode : ActionNode
	{
		public string message;

		protected override void OnStart()
		{

		}

		protected override void OnStop()
		{

		}

		protected override State OnUpdate()
		{
			Debug.Log($"{message}");
			return State.Success;
		}
	}
}