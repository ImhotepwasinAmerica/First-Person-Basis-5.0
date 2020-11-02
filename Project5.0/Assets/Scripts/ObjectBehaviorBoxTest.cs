using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ObjectBehaviorBoxTest
 * Author:          Andrew Potisk
 * Finalized on:    --/--/----
 * 
 * Purpose:
 * This script defines behavior exclusive to the object 'BoxTest'.
 * 
 * Notes:
 * 
 * Bugs:
 */
public class ObjectBehaviorBoxTest : ObjectBehaviorDefault
{
    public GameObject held_object_anchor;

    public void DestroyOnDeath()
    {
        int health = object_data.health;

        if (health < 0)
        {
            Object.Destroy(object_in_question);
        }
    }

    public override void UseDefault(GameObject new_anchor)
    {
        if (held_object_anchor == null)
        {
            held_object_anchor = new_anchor;
            held_object_anchor.transform.localRotation = new Quaternion(0, 0, 0, 0);

            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().freezeRotation = true;
        }
        else
        {
            held_object_anchor.transform.localRotation = new Quaternion(0, 0, 0, 0);
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<Rigidbody>().freezeRotation = false;
            held_object_anchor = null;
        }
    }

    public override void MoveAugment()
    {
        if (held_object_anchor != null && Input.GetButton(PlayerPrefs.GetString("Item Rotate")))
        {
            transform.Rotate(Input.GetAxisRaw("Mouse X") * Vector3.right);
            transform.Rotate(Input.GetAxisRaw("Mouse Y") * Vector3.down);
        }

        if (held_object_anchor != null)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, held_object_anchor.transform.position, 20f * Time.deltaTime);
        }

        if (held_object_anchor != null &&
            Vector3.Distance(this.transform.position, held_object_anchor.transform.position) > 2)
        {
            held_object_anchor.transform.localRotation = new Quaternion(0, 0, 0, 0);

            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<Rigidbody>().freezeRotation = false;
            held_object_anchor = null;
        }
    }
}
