﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooking : MonoBehaviour
{
    public float sensitivity, smoothing, reach;

    public GameObject held_object_anchor, character, data_container;

    private RaycastHit hit;
    private GameObject usage_target, held_thing;
    private Quaternion held_thing_rotation;
    private Vector2 mouse_look, smooth_v, md;
    private string general_action, attack_primary, attack_secondary, attack_tertiary;
    public float ex, why, zee;

    // Start is called before the first frame update
    void Start()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        GameEvents.current.LoadCharacterRotation += LoadRotation;

        Cursor.lockState = CursorLockMode.Locked;

        usage_target = null;
        held_thing = null;
        
        LoadRotation();
    }

    // Update is called once per frame
    void Update()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        GeneralAction();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (!Input.GetButton(PlayerPrefs.GetString("Item Rotate")))//else
        {
            md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smooth_v.x = Mathf.Lerp(smooth_v.x, md.x, 1f / smoothing);
            smooth_v.y = Mathf.Lerp(smooth_v.y, md.y, 1f / smoothing);
            mouse_look += smooth_v;

            mouse_look.y = Mathf.Clamp(mouse_look.y, -90f, 90f);

            transform.localRotation = Quaternion.AngleAxis(-mouse_look.y, Vector3.right); // up and down
            character.transform.localRotation = Quaternion.Euler(0, why+mouse_look.x, 0); // left and right
        }
    }
    
    private void GeneralAction()
    {
        if (Input.GetButtonDown(PlayerPrefs.GetString("General Action")))
        {
            Debug.Log(Application.persistentDataPath);

            try
            {
                usage_target = ReturnUsableObject();
                usage_target.GetComponent<ObjectBehaviorDefault>().UseDefault(held_object_anchor);

                usage_target = null;
            }
            catch (System.NullReferenceException e)
            {
                usage_target = null;
                Debug.Log("No object found");
            }
        }
        else
        {
            if (usage_target != null)
            {
                usage_target = null;
            }
        }
    }

    private GameObject ReturnUsableObject()
    {
        GameObject thing = null;

        Physics.Raycast(transform.position, transform.forward, out hit, reach);

        if (hit.collider.gameObject != null)
        {
            thing = hit.collider.gameObject;
        }

        return thing;
    }

    private void LoadRotation()
    {
        ex = data_container.GetComponent<DataContainer>().character.rotation_x;
        why = data_container.GetComponent<DataContainer>().character.rotation_y;
        zee = data_container.GetComponent<DataContainer>().character.rotation_z;
    }
}
