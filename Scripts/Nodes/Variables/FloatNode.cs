using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Variable")]
	public class FloatNode : VariableNode
	{
		[SerializeField] float value;

		public override void Initialize()
		{
			base.Initialize();
			AddVariableOutput(typeof(float));
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