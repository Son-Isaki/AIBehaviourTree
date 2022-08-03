using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Debug")]
	public class DebugIntNode : DecoratorNode
	{
		[SerializeField, HideInInspector] protected NodePort debugValue;

		public override void Initialize()
		{
			base.Initialize();
			debugValue = AddInput("debugValue", "Int", typeof(int));
		}

		protected override State Execute()
		{
			Debug.Log($"{Target.name} ({GetName()}) : {GetInputValue<int>(debugValue)}");
			return base.Execute();
		}
	}
}