using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class AndNode : LogicNode {

	[Input(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict)]
	public bool input;

	[Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict)]
	public bool output;

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {

		if(port.fieldName == "output")
        {
			bool[] inputs = GetInputValues<bool>("input", input);

			bool state = inputs[0];
			for(int i = 1; i < inputs.Length; i++)
            {
				state = state && inputs[i];
            }

			return state;
        }

		return null; // Replace this
	}

    public override void Reset()
    {
        
    }
}