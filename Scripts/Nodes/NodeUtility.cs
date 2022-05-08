using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Node
{
    public static class NodeUtility
    {
        public static string NicifyTypeName(Type type)
        {
            return ObjectNames.NicifyVariableName(type.Name.Replace("Node", ""));
        }
    }
}