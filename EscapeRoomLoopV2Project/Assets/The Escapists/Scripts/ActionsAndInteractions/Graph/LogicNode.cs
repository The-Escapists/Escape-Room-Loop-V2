using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNode;



public abstract class LogicNode : Node
    {
        protected override void Init()
        {
            base.Init();
        }

    public abstract void Reset();
    }

