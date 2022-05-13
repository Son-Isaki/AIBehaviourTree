using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviourTree.Node
{
	[Category("Logic")]
	public class BranchNode : DecoratorNode
	{
		NodePort condition;

		public override void Initialize()
		{
			base.Initialize();
			ClearOutputs();
			condition = AddInput("check", "Condition", typeof(bool), UnityEditor.Experimental.GraphView.Port.Capacity.Single);
			AddOutput("true", "True", GetNodePortType());
			AddOutput("false", "False", GetNodePortType());
		}

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State Execute()
		{

			return State.Running;
		}
	}
}