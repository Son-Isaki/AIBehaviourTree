using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Variable")]
	public class StringNode : VariableNode
	{
		[SerializeField] string value;

		public override void Initialize()
		{
			base.Initialize();
			AddVariableOutput(typeof(string));
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