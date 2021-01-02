using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviorExecutor : MonoBehaviour
{
    CharacterActionDetector action_detector;

    public GameObject rotation_thing, data_container;
    public float speed, jump_takeoff_speed, height_standing, height_squatting, lean_distance, distance_to_ground, speed_multiplier_squatting;

    private Vector3 velocity, velocity_endgoal;
    private float angular_speed, gravity_fake, time_fake, acceleration;
    private bool is_squatting, is_walking, current_grounded, previous_grounded;
    private Quaternion lean;

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

    private void MovementLean()
    {
        lean.z = Mathf.Lerp(lean.z, -velocity_endgoal.x / 5, 0.09f);
        rotation_thing.transform.localRotation = lean;
    }

    private void ControlLean()
    {
        if (action_detector.lean_left
            && action_detector.lean_right)
        {
            lean.z = Mathf.Lerp(lean.z, 0, 0.09f);
        }
        else if (action_detector.lean_left
            && !action_detector.lean_right)
        {
            lean.z = Mathf.Lerp(lean.z, 0.15f, 0.09f);
        }
        else if (action_detector.lean_right
            && !action_detector.lean_left)
        {
            lean.z = Mathf.Lerp(lean.z, -0.15f, 0.09f);
        }


        rotation_thing.transform.localRotation = lean;
    }
}
