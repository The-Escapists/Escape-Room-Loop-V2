using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class SwitchNode : LogicNode
{

	[Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict, backingValue = ShowBackingValue.Never)]
	public bool trigger;
	[Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict, backingValue = ShowBackingValue.Never)]
	public bool state;

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		if (GetInputValue<bool>("trigger"))
			state = !state;

		return state;
	}

    public override void Reset()
    {
		state = false;
    }
}