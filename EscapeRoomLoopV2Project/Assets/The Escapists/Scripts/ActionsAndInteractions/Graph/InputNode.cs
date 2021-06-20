using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheEscapists.ActionsAndInteractions;
using TheEscapists.Core;
using UnityEngine;
using XNode;

public class InputNode : LogicNode
{
	public NotifyType allowedNotifyType;

	public int changes;

	[Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict, backingValue = ShowBackingValue.Never)]
	public bool state;

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

	public void Notify(NotifyType type)
    {
		if(allowedNotifyType == type)
        {
			changes++;
        }
    }

	public void Update()
    {
		if (changes > 0)
		{
			changes--;
			state = true;
		}
		else
			state = false;
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
			return state;
	}

	public override void Reset()
	{
		changes = 0;
		state = false;
	}
}