using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universals : MonoBehaviour
{
    public static void Teleport_V1(GameObject thing, Vector3 new_position, Vector3 new_rotation)
    {
        if (thing.TryGetComponent<CharacterController>(out CharacterController character_controller))
        {

        }
        else
        {
            thing.transform.rotation = Quaternion.Euler(new_rotation);

            thing.transform.position = new_position;
        }
    }

    public static void RememberImportantThings()
    {
        GameObject data_container = GameObject.FindGameObjectWithTag("DataContainer");
    }
}
