using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviourTree.Node
{
	[Category("Logic")]
	public class BranchNode : DecoratorNode
	{
		[SerializeField, HideInInspector] NodePort condition, outputTrue, outputFalse;

		public override void Initialize()
		{
			base.Initialize();
			ClearOutputs();
			condition = AddInput("condition", "Condition", typeof(bool));
			outputTrue = AddOutput("true", "True");
			outputFalse = AddOutput("false", "False");
		}

		protected override State Execute()
		{
			if (GetValue<bool>(condition))
			{
				return GetLinkedNode(outputTrue)?.Update() ?? State.Failure;
			}
			else
			{
				return GetLinkedNode(outputFalse)?.Update() ?? State.Failure;
			}
		}
	}
}