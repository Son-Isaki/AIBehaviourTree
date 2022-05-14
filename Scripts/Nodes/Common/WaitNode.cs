using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Common")]
	public class WaitNode : ActionNode
    {
        public float duration = 1f;
        float startTime;

		protected override void OnStart()
		{
			Debug.Log($"{GetName()} wait {duration} seconds");
			startTime = Time.time;
		}

		protected override State Execute()
		{
			if (Time.time < startTime + duration)
			{
				return State.Success;
			}
			return State.Running;
		}
	}
}