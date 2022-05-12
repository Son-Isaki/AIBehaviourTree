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

        public static Color ToColor(string hex)
        {
            hex = hex.Replace("0x", "");
            hex = hex.Replace("#", "");
            byte a = 255;
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            if (hex.Length == 8) a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, a);
        }

        public static string ToHex(Color color)
        {
            Color32 c = color;
            var hex = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", c.r, c.g, c.b, c.a);
            return hex;
        }
    }
}