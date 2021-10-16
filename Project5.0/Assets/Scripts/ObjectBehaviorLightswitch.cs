using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehaviorLightswitch : ObjectBehaviorSwitch
{
    public GameObject cylinder;

    public override void DoOnUpdate() 
    {
        base.DoOnUpdate();
        
        if (cylinder != null)
        {
            if (this.object_data.bools[0])
            {
                cylinder.transform.localRotation = Quaternion.Euler(0,-90, 72);
            }
            else
            {
                cylinder.transform.localRotation = Quaternion.Euler(0, -90, 0);
            }
        }
    }
}
