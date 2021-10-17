using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehaviorLamp : ObjectBehaviorDefault
{
    public GameObject lightswitch, lightbulb, lightsource;

    public Material light_off, light_on;

    public override void DoOnUpdate()
    {
        base.DoOnUpdate();

        try
        {
            if (lightswitch.GetComponent<ObjectBehaviorSwitch>().OnOrOff())
            {
                lightbulb.GetComponent<MeshRenderer>().material = light_on;

                lightsource.SetActive(true);
            }
            else
            {
                lightbulb.GetComponent<MeshRenderer>().material = light_off;

                lightsource.SetActive(false);
            }
        }
        catch(System.NullReferenceException e) { }
    }
}
