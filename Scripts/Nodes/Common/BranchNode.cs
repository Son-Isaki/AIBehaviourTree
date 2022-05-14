using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviourTree.Node
{
	[Category("Logic")]
	public class BranchNode : DecoratorNode
	{
		NodePort condition, outputTrue, outputFalse;

		public override void Initialize()
		{
			base.Initialize();
			ClearOutputs();
			condition = AddInput("check", "Condition", typeof(bool), UnityEditor.Experimental.GraphView.Port.Capacity.Single);
			outputTrue = AddOutput("true", "True");
			outputFalse = AddOutput("false", "False");
		}

		protected override State Execute()
		{
			if (GetValue<bool>(condition))
			{
				return GetLinkedNode(outputTrue).Update();
			}
			else
			{
				return GetLinkedNode(outputFalse).Update();
			}
		}
	}
}