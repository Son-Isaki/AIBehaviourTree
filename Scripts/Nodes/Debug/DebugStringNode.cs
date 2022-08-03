using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Debug")]
	public class DebugStringNode : DecoratorNode
	{
		[SerializeField, HideInInspector] protected NodePort stringValue;

		public override void Initialize()
		{
			base.Initialize();
			stringValue = AddInput("debugValue", "String", typeof(string));
		}

		protected override State Execute()
		{
			Debug.Log($"{Target.name} ({GetName()}) : {GetInputValue<string>(stringValue)}");
			return base.Execute();
		}
	}
}