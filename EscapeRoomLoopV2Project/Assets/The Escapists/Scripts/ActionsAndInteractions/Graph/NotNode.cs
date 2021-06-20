using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class NotNode : LogicNode
{

	[Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
	public bool input;

	[Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict)]
	public bool output;

	// Use this for initialization
	protected override void Init()
	{
		base.Init();

	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{

		if (port.fieldName == "output")
		{
			return !GetInputValue<bool>("input", input);
		}

		return null; // Replace this
	}

	public override void Reset()
	{

	}
}