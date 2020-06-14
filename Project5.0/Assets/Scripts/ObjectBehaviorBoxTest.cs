using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (PlayerPrefs.GetString("togglehold_carry") == "toggle")
        {
            if (held_object_anchor == null)
            {
                held_object_anchor = new_anchor;
                held_object_anchor.transform.localRotation = new Quaternion(0, 0, 0, 0);

                this.GetComponent<Rigidbody>().useGravity = false;
                this.GetComponent<Rigidbody>().freezeRotation = true;
                Debug.Log("Attach " + held_object_anchor);
            }
            else
            {
                held_object_anchor.transform.localRotation = new Quaternion(0, 0, 0, 0);

                this.GetComponent<Rigidbody>().useGravity = true;
                this.GetComponent<Rigidbody>().freezeRotation = false;
                held_object_anchor = null;
                Debug.Log("Detach " + held_object_anchor);
            }
        }
    }

    public override void UseDefaultHold(GameObject new_anchor)
    {
        if (PlayerPrefs.GetString("togglehold_carry") == "hold")
        {
            if (held_object_anchor == null)
            {
                held_object_anchor = new_anchor;
                held_object_anchor.transform.localRotation = new Quaternion(0, 0, 0, 0);

                this.GetComponent<Rigidbody>().useGravity = false;
                this.GetComponent<Rigidbody>().freezeRotation = true;
                Debug.Log("Attached " + held_object_anchor);
            }
        }
    }

    public override void UseDefaultHoldRelease()
    {
        if (held_object_anchor != null)
        {
            held_object_anchor.transform.localRotation = new Quaternion(0, 0, 0, 0);

            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<Rigidbody>().freezeRotation = false;
            held_object_anchor = null;
            Debug.Log("Detach " + held_object_anchor);
        }
    }

    public override void MoveAugment()
    {
        if (held_object_anchor != null)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, held_object_anchor.transform.position, 20f * Time.deltaTime);

            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, held_object_anchor.transform.localRotation, 20f * Time.deltaTime);
        }
    }
}
