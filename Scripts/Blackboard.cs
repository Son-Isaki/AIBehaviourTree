using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    [CreateAssetMenu(menuName = "Isaki/AI Behaviour Tree/Blackboard", order = 10)]
    public class Blackboard : ScriptableObject
    {
        [Header("Blackboard")]
        [SerializeField] public Vector3 targetPosition;

		public Blackboard Clone()
		{
			Blackboard blackboard = Instantiate(this);
			return blackboard;
		}
	}
}