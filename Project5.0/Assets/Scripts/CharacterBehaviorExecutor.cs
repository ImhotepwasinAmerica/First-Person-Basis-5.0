using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviorExecutor : MonoBehaviour
{
    CharacterActionDetector action_detector;

    public GameObject usage_target, held_thing, camera, held_object_anchor, data_container;
    public float speed, jump_takeoff_speed, height_standing, height_squatting, lean_distance, distance_to_ground, speed_multiplier_squatting, sensitivity, smoothing, reach;

    private Vector3 velocity, velocity_endgoal;
    private Vector2 mouse_look, smooth_v, md;
    private float angular_speed, gravity_fake, time_fake, acceleration;
    private bool is_squatting, is_walking, current_grounded, previous_grounded;
    private Quaternion lean, held_thing_rotation;

    private Transform transformation;
    private CharacterController controller;
    private CapsuleCollider collider;
    private SavedObject guy;

    public void Awake()
    {
        action_detector = this.gameObject.GetComponent<CharacterActionDetector>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        smoothing = 1;

        usage_target = null;
        held_thing = null;
        current_grounded = true;
        previous_grounded = true;

        mouse_look.y = guy.rotation_x;
        mouse_look.x = guy.rotation_y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        previous_grounded = current_grounded;
        current_grounded = IsGrounded();
    }

    private bool IsGrounded()
    {
        if (controller.isGrounded)
        {
            return true;
        }

        Vector3 bottom = controller.transform.position - new Vector3(0, controller.height / 2, 0);

        RaycastHit hit;

        if (Physics.Raycast(bottom, new Vector3(0, -1, 0), out hit, 0.5f)
            && !action_detector.jump_higher
            && !action_detector.jump)
        {
            if (current_grounded && previous_grounded)
            {
                controller.Move(new Vector3(0, -hit.distance, 0));
                return true;
            }
        }

        return false;
    }

    private bool IsBeneathSomething()
    {
        return Physics.Raycast(transform.position, Vector3.up, ((controller.height / 2) + 0.3f));
    }

    private void Jump()
    {
        if (action_detector.jump_higher
            && IsGrounded())
        {
            velocity_endgoal.y += (jump_takeoff_speed * time_fake);
        }
    }

    private void BetterMovement()
    {
        // Better jumping and falling
        if (velocity_endgoal.y < -0.00327654 && Time.timeScale > 0.1f)
        {
            velocity_endgoal.y += gravity_fake * time_fake;
        }
        else if (velocity_endgoal.y > -0.00327654 && !(action_detector.jump_higher))
        {
            velocity_endgoal.y += (0.5f * gravity_fake * time_fake);
        }
    }

    private void WalkRun()
    {
        if (action_detector.speed_toggle)
        {
            if (is_walking)
            {
                is_walking = false;
            }
            else if (!is_walking)
            {
                is_walking = true;
            }
        }

        if (is_walking)
        {
            velocity_endgoal.x *= 0.5f;
            velocity_endgoal.z *= 0.5f;
        }
    }

    private void GeneralAction()
    {
        if (action_detector.general_action)
        {
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

        RaycastHit hit;

        Physics.Raycast(transform.position, transform.forward, out hit, reach);

        if (hit.collider.gameObject != null)
        {
            thing = hit.collider.gameObject;
        }

        return thing;
    }
}
