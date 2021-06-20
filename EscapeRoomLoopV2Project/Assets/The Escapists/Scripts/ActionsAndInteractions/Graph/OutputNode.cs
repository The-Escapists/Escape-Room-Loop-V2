using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheEscapists.Core;
using UnityEngine;
using XNode;

public class OutputNode : LogicNode
{
	[Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict, backingValue = ShowBackingValue.Never)]
	public bool state;

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return GetInputValue<bool>("state");
	}
	//Return the state if the correct Actor
	public bool GetValue()
	{
		return GetInputValue<bool>("state");
	}
	public override void Reset()
	{
		state = false;
	}
}