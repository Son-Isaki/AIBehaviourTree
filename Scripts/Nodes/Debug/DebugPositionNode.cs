using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Debug")]
	public class DebugPositionNode : DecoratorNode
	{
		[SerializeField, HideInInspector] protected NodePort debugValue;

		public override void Initialize()
		{
			base.Initialize();
			debugValue = AddInput("debugValue", "Vector3", typeof(Vector3));
		}

		protected override State Execute()
		{
			Debug.Log($"{Target.name} ({GetName()}) : {GetInputValue<Vector3>(debugValue)}");
			return base.Execute();
		}
	}
}