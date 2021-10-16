using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehaviorSwitch : ObjectBehaviorDefault
{
    public bool on_off;

    public override void DoOnUpdate()
    {
        base.DoOnUpdate();

        this.object_data.bools[0] = on_off;
    }

    public override void UseDefault(GameObject new_anchor)
    {
        if (on_off)
        {
            on_off = false;
        }
        else
        {
            on_off = true;
        }
    }

    public bool OnOrOff()
    {
        return this.object_data.bools[1];
    }
}
