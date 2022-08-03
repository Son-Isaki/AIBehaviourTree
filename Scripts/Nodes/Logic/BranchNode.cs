using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
			if (GetInputValue<bool>(condition))
			{
				return GetOutputNodes(outputTrue).FirstOrDefault()?.Update() ?? State.Failure;
			}
			else
			{
				return GetOutputNodes(outputFalse).FirstOrDefault()?.Update() ?? State.Failure;
			}
		}
	}
}