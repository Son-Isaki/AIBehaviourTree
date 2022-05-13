using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Variable")]
	public class Vector3Node : VariableNode
	{
		[SerializeField] Vector3 value;

		public override void Initialize()
		{
			base.Initialize();
			AddVariableOutput(typeof(Vector3));
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