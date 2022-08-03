using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Debug")]
	public class DebugFloatNode : DecoratorNode
	{
		[SerializeField, HideInInspector] protected NodePort debugValue;

		public override void Initialize()
		{
			base.Initialize();
			debugValue = AddInput("debugValue", "Float", typeof(float));
		}

		protected override State Execute()
		{
			Debug.Log($"{Target.name} ({GetName()}) : {GetInputValue<float>(debugValue)}");
			return base.Execute();
		}
	}
}