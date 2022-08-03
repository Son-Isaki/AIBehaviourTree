using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviourTree.Node
{
	[Category("Logic")]
	public class OrNode : VariableNode
	{
		[SerializeField, HideInInspector] NodePort cond1, cond2;

		public override void Initialize()
		{
			base.Initialize();
			ClearPorts();
			cond1 = AddInput("condition", "Cond", typeof(bool));
			cond2 = AddInput("condition", "Cond", typeof(bool));
			output = AddValueOutput(typeof(bool));
		}

		public override object GetValue()
		{
			return (GetInputValue<bool>(cond1) && GetInputValue<bool>(cond2));
		}

		public override string GetName() => "Or";
	}
}