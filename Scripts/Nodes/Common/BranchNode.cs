using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviourTree.Node
{
	[Category("Logic")]
	public class BranchNode : DecoratorNode
	{
		public override void Initialize()
		{
			base.Initialize();
			ClearOutputs();
			AddInput("check", "Condition", typeof(bool), UnityEditor.Experimental.GraphView.Port.Capacity.Single);
			AddOutput("true", "True", GetNodePortType());
			AddOutput("false", "False", GetNodePortType());
		}

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			return State.Running;
		}
	}
}