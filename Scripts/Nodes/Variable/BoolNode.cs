using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Variable")]
	public class BoolNode : VariableNode
	{
		[SerializeField] bool value;

		public override void Initialize()
		{
			base.Initialize();
			AddVariableOutput(typeof(bool));
		}

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			return State.Success;
		}

		public override object GetValue()
		{
			return value;
		}
	}
}