using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CharacterBehaviorExecutor
 * Author:          Andrew Potisk
 * Finalized on:    --/--/----
 * 
 * Purpose:
 * This script reads input from a 'CharacterActionDetector' script attached  to the same entity,
 * and executes behavior accordingly.
 * The behavior in question is common to all characters: NPC or PC.
 * 
 * Notes:
 * 
 * Bugs:
 * Detection of input from 'CharacterActionDetector' scripts can be poorly synchronized with the other script itself,
 * resulting in inconsistent player input.
 */
public class CharacterBehaviorExecutor : MonoBehaviour
{
    protected CharacterActionDetector action_detector;

    public GameObject usage_target, camera, held_object_anchor, data_container;
    public float speed, jump_takeoff_speed, smoothing, reach, acceleration, deceleration;
    public Vector2 md;

    private Vector2 mouse_look, smooth_v;
    private float acceleration_true, accelerator_x, accelerator_y, accelerator_z;
    private bool is_walking, current_grounded, previous_grounded, general_action_this;
    private Quaternion held_thing_rotation;

    protected Vector3 velocity, velocity_endgoal;
    protected CharacterController controller;
    protected SavedObject guy;

    public void Awake()
    {
        action_detector = this.gameObject.GetComponent<CharacterActionDetector>();

        DoOnAwake();
    }

    // Start is called before the first frame update
    void Start()
    {
        //data_container = GameObject.FindGameObjectWithTag("DataContainer");
        data_container = Universals.GetDataContainer();

        guy = data_container.GetComponent<DataContainer>().character;

        Cursor.lockState = CursorLockMode.Locked;

        accelerator_x = 0;
        accelerator_y = 0;
        accelerator_z = 0;

        md = new Vector2();

        controller = GetComponent<CharacterController>();

        usage_target = null;
        current_grounded = true;
        previous_grounded = true;
        is_walking = false;

        mouse_look.y = transform.localRotation.x;//data_container.character.rotation_y;//-transform.localRotation.x;
        mouse_look.x = guy.rotation_y;//character.transform.localRotation.y;

        DoOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.1f)
        {
            Walk();

            WalkRun();

            // I'm not sure yet how to make a character move along with whatever its standing on.
            // This could cause problems.
            velocity_endgoal = CorrectForRotation(velocity_endgoal);

            ApplyGravity();

            Jump();


            //AccelerationSet();
            //MovementLerpUnifiedXZ();

            //MovementLerpRealXZ();
            //AcceleratorScale();

            MovementSetY();

            BetterMovement();

            GeneralAction();

            SpinHeldThing();

            //ApplyTime();

            controller.Move(velocity * Time.deltaTime);

            previous_grounded = current_grounded;
            current_grounded = IsGrounded();
        }

        GetCameraMovement();

        DoOnUpdate();
    }

    void FixedUpdate()
    {
        AccelerationSet();
        MovementLerpUnifiedXZ();
        //velocity = velocity_endgoal;

        DoOnFixedUpdate();
    }

    private void ApplyTime()
    {
        velocity.x *= Time.deltaTime;
        velocity.y *= Time.deltaTime;
        velocity.z *= Time.deltaTime;
    }

    private Vector3 CorrectForRotation(Vector3 direction_original)
    {
        return transform.localRotation * direction_original;
    }

    private void GetSomeInputs()
    {
        general_action_this = action_detector.general_action;
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
            velocity_endgoal.z *= Universals.GetAngularSpeed(speed);
            velocity_endgoal.x *= Universals.GetAngularSpeed(speed);
        }
        else
        {
            velocity_endgoal.z *= speed;
            velocity_endgoal.x *= speed;
        }

        //velocity_endgoal.z *= Time.deltaTime;
        //velocity_endgoal.x *= Time.deltaTime;
    }

    private void MovementLerpX()
    {
        velocity.x = Universals.LerpBetter(velocity.x, velocity_endgoal.x, acceleration * Time.deltaTime);
    }

    private void MovementLerpZ()
    {
        velocity.z = Universals.LerpBetter(velocity.z, velocity_endgoal.z, acceleration * Time.deltaTime);
    }

    private void MovementLerpUnifiedXZ()
    {
        //Debug.Log(velocity_endgoal);
        
        float ratio;
        
        if(Mathf.Abs(velocity_endgoal.x-velocity.x)
            > Mathf.Abs(velocity_endgoal.z-velocity.z))
        {
            ratio = Mathf.Abs(velocity_endgoal.z - velocity.z) / Mathf.Abs(velocity_endgoal.x - velocity.x);
            velocity.x = Universals.LerpBetter(velocity.x, velocity_endgoal.x, acceleration_true * Universals.GetTimeFake());
            velocity.z = Universals.LerpBetter(velocity.z, velocity_endgoal.z, acceleration_true * ratio * Universals.GetTimeFake());
        }
        else
        {
            ratio = Mathf.Abs(velocity_endgoal.x - velocity.x) / Mathf.Abs(velocity_endgoal.z - velocity.z);
            velocity.z = Universals.LerpBetter(velocity.z, velocity_endgoal.z, acceleration_true * Universals.GetTimeFake());
            velocity.x = Universals.LerpBetter(velocity.x, velocity_endgoal.x, acceleration_true * ratio * Universals.GetTimeFake());
        }
    }

    private void MovementLerpY()
    {
        velocity.y = Universals.LerpBetter(velocity.y, velocity_endgoal.y, acceleration * Time.deltaTime);
    }

    private void MovementLerpRealXZ()
    {
        velocity.x = Mathf.Lerp(0, velocity_endgoal.x, accelerator_x);
        velocity.z = Mathf.Lerp(0, velocity_endgoal.z, accelerator_z);
    }

    private void AcceleratorScale()
    {
        if ((Mathf.Abs(velocity.x) < Mathf.Abs(velocity_endgoal.x) && Mathf.Sign(velocity.x) == Mathf.Sign(velocity_endgoal.x))
            || accelerator_x == 0)
        {
            Universals.LerpBetter(accelerator_x, 1f, acceleration_true * Time.deltaTime);
        }
        else
        {
            Universals.LerpBetter(accelerator_x, 0, acceleration_true * Time.deltaTime);
        }

        if ((Mathf.Abs(velocity.y) < Mathf.Abs(velocity_endgoal.y) && Mathf.Sign(velocity.y) == Mathf.Sign(velocity_endgoal.y))
            || accelerator_y == 0)
        {
            Universals.LerpBetter(accelerator_y, 1f, acceleration_true * Time.deltaTime);
        }
        else
        {
            Universals.LerpBetter(accelerator_y, 0, acceleration_true * Time.deltaTime);
        }

        if ((Mathf.Abs(velocity.z) < Mathf.Abs(velocity_endgoal.z) && Mathf.Sign(velocity.z) == Mathf.Sign(velocity_endgoal.z))
            || accelerator_z == 0)
        {
            Universals.LerpBetter(accelerator_z, 1f, acceleration_true * Time.deltaTime);
        }
        else
        {
            Universals.LerpBetter(accelerator_z, 0, acceleration_true * Time.deltaTime);
        }
    }

    private void MovementSetY()
    {
        velocity.y = velocity_endgoal.y;
    }

    private void ApplyGravity()
    {
        if (IsGrounded())
        {
            velocity_endgoal.y = (Physics.gravity.y * Time.deltaTime); //* Time.deltaTime);
        }
        else
        {
            velocity_endgoal.y += (Physics.gravity.y * Time.deltaTime * 2);// * Time.deltaTime * 2);
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
        if ((action_detector.jump_higher)
            && IsGrounded())
        {
            velocity_endgoal.y = jump_takeoff_speed; //* Universals.GetTimeFake();
        }
    }

    private void BetterMovement()
    {
        // Better jumping and falling
        if (velocity_endgoal.y < -0.00327654 && Time.timeScale > 0.1f)
        {
            //velocity_endgoal.y += (Physics.gravity.y * Time.deltaTime * Time.deltaTime); // Increased gravity
        }
        else if (velocity_endgoal.y > -0.00327654 && !(action_detector.jump_higher)) // Reactive jumping
        {
            velocity_endgoal.y += (0.5f * (Physics.gravity.y * Time.deltaTime));
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

    public virtual void GetCameraMovement()
    {
        if (!action_detector.item_rotate)
        {
            md.x = action_detector.mouse_x * (Universals.GetNPCSensitivity() * 50) * Time.deltaTime;
            md.y -= action_detector.mouse_y * (Universals.GetNPCSensitivity() * 50) * Time.deltaTime;

            md.y = Mathf.Clamp(md.y, -90f, 90f);

            camera.transform.localRotation = Quaternion.Euler(md.y, 0,0);
            transform.Rotate(Vector3.up * md.x);
        }
    }

    private void SpinHeldThing()
    {
        if (action_detector.item_rotate)
        {
            held_object_anchor.transform.Rotate(action_detector.mouse_x * Vector3.right);
            held_object_anchor.transform.Rotate(action_detector.mouse_y * Vector3.down);
        }
    }

    private void HeldThingReset()
    {
        held_object_anchor.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void HeldThingSet(Quaternion quat)
    {
        held_object_anchor.transform.rotation = quat;
    }

    private void AccelerationSet()
    {
        if (action_detector.move_forward
            || action_detector.move_backward
            || action_detector.move_left
            || action_detector.move_right)
        {
            acceleration_true = acceleration;
        }
        else
        {
            acceleration_true = deceleration;
        }
    }

    public virtual void DoOnAwake() { }

    public virtual void DoOnStart() { }

    public virtual void DoOnUpdate() { }

    public virtual void DoOnFixedUpdate() { }
}
