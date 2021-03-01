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
        DoOnAwake();

        action_detector = this.gameObject.GetComponent<CharacterActionDetector>();
    }

    // Start is called before the first frame update
    void Start()
    {
        DoOnStart();

        //data_container = GameObject.FindGameObjectWithTag("DataContainer");
        data_container = Universals.GetDataContainer();

        guy = data_container.GetComponent<DataContainer>().character;

        Cursor.lockState = CursorLockMode.Locked;

        smoothing = 1;
        angular_speed = Mathf.Sqrt((speed * speed / 2.0f));
        time_fake = 0.01666f;
        gravity_fake = Physics.gravity.y * time_fake;

        lean = Quaternion.Euler(0, 0, 0);

        transformation = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();

        usage_target = null;
        held_thing = null;
        current_grounded = true;
        previous_grounded = true;
        is_squatting = false;
        is_walking = false;

        mouse_look.y = transform.localRotation.x;//data_container.character.rotation_y;//-transform.localRotation.x;
        mouse_look.x = guy.rotation_y;//character.transform.localRotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        DoOnUpdate();

        //MovementLerpNotFixedUpdate();
    }

    void FixedUpdate()
    {
        DoOnFixedUpdate();

        Walk();

        ApplyGravity();

        BetterMovement();

        Jump();

        WalkRun();

        velocity_endgoal = transformation.rotation * velocity_endgoal;

        previous_grounded = current_grounded;
        current_grounded = IsGrounded();

        //velocity.x = Mathf.Lerp(velocity.x, velocity_endgoal.x, 0.15f);
        //velocity.z = Mathf.Lerp(velocity.z, velocity_endgoal.z, 0.15f);
        //velocity.y = velocity_endgoal.y;
        MovementLerp();

        controller.Move(velocity);

        GetCameraMovement();

        GeneralAction();

        
    }

    private void Walk()
    {
        if (action_detector.move_forward
            && !action_detector.move_backward)
        {
            velocity_endgoal.z = 1;
        }
        else if (!action_detector.move_forward
            && action_detector.move_backward)
        {
            velocity_endgoal.z = -1;
        }
        else
        {
            velocity_endgoal.z = 0;
        }

        if (action_detector.move_right
            && !action_detector.move_left)
        {
            velocity_endgoal.x = 1;
        }
        else if (!action_detector.move_right
            && action_detector.move_left)
        {
            velocity_endgoal.x = -1;
        }
        else
        {
            velocity_endgoal.x = 0;
        }

        if ((action_detector.move_forward && action_detector.move_left)
            || (action_detector.move_forward && action_detector.move_right)
            || (action_detector.move_backward && action_detector.move_left)
            || (action_detector.move_backward && action_detector.move_right))
        {
            velocity_endgoal.z *= angular_speed;
            velocity_endgoal.x *= angular_speed;
        }
        else
        {
            velocity_endgoal.z *= speed;
            velocity_endgoal.x *= speed;
        }
    }

    private void MovementLerp()
    {
        velocity.x = Mathf.Lerp(velocity.x, velocity_endgoal.x * time_fake, 0.15f);
        velocity.z = Mathf.Lerp(velocity.z, velocity_endgoal.z * time_fake, 0.15f);
        velocity.y = velocity_endgoal.y * time_fake;
    }

    private void MovementLerpNotFixedUpdate()
    {
        velocity.x = Mathf.Lerp(velocity.x, velocity_endgoal.x * Time.deltaTime, 0.1f);
        velocity.z = Mathf.Lerp(velocity.z, velocity_endgoal.z * Time.deltaTime, 0.1f);
        velocity.y = velocity_endgoal.y * Time.deltaTime;
    }

    private void ApplyGravity()
    {
        if (IsGrounded())
        {
            velocity_endgoal.y = (gravity_fake);
        }
        else
        {
            if (Time.timeScale > 0.1f)
            {
                velocity_endgoal.y += (gravity_fake);
            }
        }
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
            velocity_endgoal.y += (jump_takeoff_speed);
        }
    }

    private void BetterMovement()
    {
        // Better jumping and falling
        if (velocity_endgoal.y < -0.00327654 && Time.timeScale > 0.1f)
        {
            velocity_endgoal.y += gravity_fake;
        }
        else if (velocity_endgoal.y > -0.00327654 && !(action_detector.jump_higher))
        {
            velocity_endgoal.y += (0.5f * gravity_fake);
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
        if (action_detector.general_action
            || action_detector.general_action_hold)
        {
            try
            {
                usage_target = ReturnUsableObject();
                Debug.Log("Interacting object: " + usage_target);
                usage_target.GetComponent<ObjectBehaviorDefault>().UseDefault(held_object_anchor);

                usage_target = null;
            }
            catch (System.NullReferenceException e)
            {
                usage_target = null;
                Debug.Log("No object detected");
            }
            
            usage_target = null;
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

        Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, reach);
        
        
        if (hit.collider.gameObject != null)
        {
            thing = hit.collider.gameObject;
        }
       

        return thing;
    }

    private void AlterAcceleration()
    {
        if (IsGrounded())
        {
            acceleration = 0.15f;
        }
        else
        {
            acceleration = 0.1f;
        }
    }

    private void GetCameraMovement()
    {
        if (!action_detector.item_rotate)
        {
            md = new Vector2(action_detector.mouse_x, action_detector.mouse_y);

            md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smooth_v.x = Mathf.Lerp(smooth_v.x, md.x, 1f / smoothing);
            smooth_v.y = Mathf.Lerp(smooth_v.y, md.y, 1f / smoothing);
            mouse_look += smooth_v;

            mouse_look.y = Mathf.Clamp(mouse_look.y, -90f, 90f);

            camera.transform.localRotation = Quaternion.AngleAxis(-mouse_look.y, Vector3.right); // up and down
            transform.localRotation = Quaternion.Euler(0, mouse_look.x, 0); // left and right
        }
    }

    public virtual void DoOnAwake() { }

    public virtual void DoOnStart() { }

    public virtual void DoOnUpdate() { }

    public virtual void DoOnFixedUpdate() { }
}
