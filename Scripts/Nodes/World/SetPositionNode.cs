using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviourTree.Node
{
	[Category("World")]
	public class SetPositionNode : DecoratorNode
	{
		[SerializeField, HideInInspector] NodePort gameObject, position;

		public override void Initialize()
		{
			base.Initialize();
			gameObject = AddInput("gameObject", null, typeof(GameObject));
			position = AddInput("position", null, typeof(Vector3));
			SetDescription("Set the world position of a GameObject");
		}

		protected override State Execute()
		{
			var go = GetInputValue<GameObject>(gameObject);
			if (go)
			{
				go.transform.position = GetInputValue<Vector3>(position);
			}
			return base.Execute();
		}
	}
}