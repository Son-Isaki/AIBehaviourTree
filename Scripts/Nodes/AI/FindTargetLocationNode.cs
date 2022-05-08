using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviourTree.Node
{
	public class FindTargetLocationNode : ActionNode
	{
		[SerializeField] float radius = 5f;
		[SerializeField] int maxAttempts = 30;

		int attempts = 0;

		protected override void OnStart()
		{
			attempts = 0;
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			/*for (attempts = 0; attempts < maxAttempts; attempts++)
			{
				if (FindRandomPosition(out Vector3 position))
				{
					Blackboard.targetPosition = position;
					return State.Success;
				}
			}*/

			return State.Running;			
		}

		private bool FindRandomPosition(out Vector3 position)
		{
			/*Vector3 initialPosition = Blackboard.self.transform.position;
			if (Blackboard.self.Spawner)
			{
				initialPosition = Blackboard.self.Spawner.transform.position;
			}
			Vector3 randomPoint = initialPosition + Random.insideUnitSphere * radius;

			if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1f, NavMesh.AllAreas))
			{
				position = hit.position;
				return true;
			}

			position = initialPosition;*/
			position = Vector3.zero;
			return false;
		}
	}
}