using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class CategoryAttribute : Attribute
{
	public readonly string category;

	public CategoryAttribute(string _category)
	{
		category = _category;
	}
}
