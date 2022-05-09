using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    public static class NodeUtility
    {
        public static string NicifyTypeName(Type type)
        {
            return ObjectNames.NicifyVariableName(type.Name.Replace("Node", "").Trim());
        }
    }
}