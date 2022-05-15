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
			return (GetValue<bool>(cond1) && GetValue<bool>(cond2));
		}

		public override string GetName() => "Or";
	}
}