using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universals : MonoBehaviour
{
    //public static void Teleport_V1(GameObject thing, Vector3 new_position, Vector3 new_rotation)
    //{
    //    if (thing.TryGetComponent<CharacterController>(out CharacterController character_controller))
    //    {

    //    }
    //    else
    //    {
    //        thing.transform.rotation = Quaternion.Euler(new_rotation);

    //        thing.transform.position = new_position;
    //    }
    //}

    public static void RememberImportantThings()
    {
        GameObject data_container = GameObject.FindGameObjectWithTag("DataContainer");
    }

    public static GameObject GetDataContainer()
    {
        return GameObject.FindGameObjectWithTag("DataContainer");
    }

    public static GameObject FindChild(GameObject parent, string name)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).gameObject.name == name)
            {
                return parent.transform.GetChild(i).gameObject;
            }
            else
            {
                return FindChild(parent.transform.GetChild(i).gameObject, name);
            }
        }

        return null;
    }

    public static GameObject FindChildByTag(GameObject parent, string tag)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).gameObject.tag == tag)
            {
                return parent.transform.GetChild(i).gameObject;
            }
            else
            {
                return FindChild(parent.transform.GetChild(i).gameObject, tag);
            }
        }

        return null;
    }
}
