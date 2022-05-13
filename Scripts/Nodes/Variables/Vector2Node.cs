using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Variable")]
	public class Vector2Node : VariableNode
	{
		[SerializeField] Vector2 value;

		public override void Initialize()
		{
			base.Initialize();
			AddVariableOutput(typeof(Vector2));
		}

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State Execute()
		{
			return State.Success;
		}

		public override object GetValue()
		{
			return value;
		}
	}
}