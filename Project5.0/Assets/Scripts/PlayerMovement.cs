using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject rotation_thing, data_container;
    public float speed, jump_takeoff_speed, height_standing, height_squatting, lean_distance, distance_to_ground, speed_multiplier_squatting;

    private Vector3 velocity, velocity_endgoal;
    private float angular_speed, gravity_fake, time_fake;
    private bool is_squatting, is_walking, current_grounded, previous_grounded;
    private Quaternion lean;

    private Transform transformation;
    private CharacterController controller;
    private CapsuleCollider collider;
    private Character guy;

    // Start is called before the first frame update
    void Start()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        guy = data_container.GetComponent<DataContainer>().character;

        transformation = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
        collider = GetComponent<CapsuleCollider>();

        current_grounded = true;
        previous_grounded = true;
        
        angular_speed = Mathf.Sqrt((speed * speed / 2.0f));

        time_fake = 0.01666f;
        gravity_fake = Physics.gravity.y * time_fake;

        lean = Quaternion.Euler(0,0,0);

        is_squatting = false;

        is_walking = false;
    }

    // Update is called once per frame
    void Update()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        Walk();

        BetterMovement();

        ApplyGravity();

        Jump();

        CycaBlyat();

        MovementLean();

        ControlLean();

        WalkRun();

        velocity_endgoal = transformation.rotation * velocity_endgoal;

        previous_grounded = current_grounded;
        current_grounded = IsGrounded();
    }

    void FixedUpdate()
    {
        velocity.x = Mathf.Lerp(velocity.x, velocity_endgoal.x, 0.15f);
        velocity.z = Mathf.Lerp(velocity.z, velocity_endgoal.z, 0.15f);
        velocity.y = velocity_endgoal.y;

        // The velocity value shall be changed by standing on moving platforms
        // which shall be done here.

        controller.Move(velocity);
    }

    private void Walk()
    {
        if (Input.GetButton(PlayerPrefs.GetString("Move Forward"))
            && !Input.GetButton(PlayerPrefs.GetString("Move Backward")))
        {
            velocity_endgoal.z = time_fake;
        }
        else if (!Input.GetButton(PlayerPrefs.GetString("Move Forward"))
            && Input.GetButton(PlayerPrefs.GetString("Move Backward")))
        {
            velocity_endgoal.z = -time_fake;
        }
        else
        {
            velocity_endgoal.z = 0;
        }

        if (Input.GetButton(PlayerPrefs.GetString("Move Right"))
            && !Input.GetButton(PlayerPrefs.GetString("Move Left")))
        {
            velocity_endgoal.x = time_fake;
        }
        else if (!Input.GetButton(PlayerPrefs.GetString("Move Right"))
            && Input.GetButton(PlayerPrefs.GetString("Move Left")))
        {
            velocity_endgoal.x = -time_fake;
        }
        else
        {
            velocity_endgoal.x = 0;
        }

        if ((Input.GetButton(PlayerPrefs.GetString("Move Forward")) && Input.GetButton(PlayerPrefs.GetString("Move Left")))
            || (Input.GetButton(PlayerPrefs.GetString("Move Forward")) && Input.GetButton(PlayerPrefs.GetString("Move Right")))
            || (Input.GetButton(PlayerPrefs.GetString("Move Backward")) && Input.GetButton(PlayerPrefs.GetString("Move Left")))
            || (Input.GetButton(PlayerPrefs.GetString("Move Backward")) && Input.GetButton(PlayerPrefs.GetString("Move Right"))))
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

    private void BetterMovement()
    {
        // Better jumping and falling
        if (velocity_endgoal.y < -0.00327654 && Time.timeScale > 0.1f)
        {
            velocity_endgoal.y += gravity_fake * time_fake;
        }
        else if (velocity_endgoal.y > -0.00327654 && !(Input.GetButton(PlayerPrefs.GetString("Jump"))))
        {
            velocity_endgoal.y += (0.5f * gravity_fake * time_fake);
        }
    }

    private void ApplyGravity()
    {
        if (IsGrounded())
        {
            velocity_endgoal.y = (gravity_fake * time_fake);
        }
        else
        {
            if (Time.timeScale > 0.1f)
            {
                velocity_endgoal.y += (gravity_fake * time_fake);
            }
        }
    }

    private bool IsGrounded()
    {
        if (controller.isGrounded)
        {
            return true;
        }

        Vector3 bottom = controller.transform.position - new Vector3(0, controller.height/2, 0);

        RaycastHit hit;

        if (Physics.Raycast(bottom, new Vector3(0, -1, 0), out hit, 0.5f)
            && !(Input.GetButton(PlayerPrefs.GetString("Jump")))
            && !Input.GetButtonDown(PlayerPrefs.GetString("Jump")))
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
        return Physics.Raycast(transform.position, Vector3.up, controller.height-distance_to_ground+0.3f);
    }

    private void Jump()
    {
        if (Input.GetButtonDown(PlayerPrefs.GetString("Jump"))
            && IsGrounded())
        {
            velocity_endgoal.y += (jump_takeoff_speed * time_fake);
        }
    }

    private void MovementLean()
    {
        lean.z = Mathf.Lerp(lean.z, -velocity_endgoal.x/5, 0.09f);
        rotation_thing.transform.localRotation = lean;
    }

    private void ControlLean()
    {
        if (Input.GetButton(PlayerPrefs.GetString("Lean Left"))
            && Input.GetButton(PlayerPrefs.GetString("Lean Right")))
        {
            lean.z = Mathf.Lerp(lean.z, 0, 0.09f);
        }
        else if (Input.GetButton(PlayerPrefs.GetString("Lean Left"))
            && !Input.GetButton(PlayerPrefs.GetString("Lean Right")))
        {
            lean.z = Mathf.Lerp(lean.z, 0.15f, 0.09f);
        }
        else if (Input.GetButton(PlayerPrefs.GetString("Lean Right"))
            && !Input.GetButton(PlayerPrefs.GetString("Lean Left")))
        {
            lean.z = Mathf.Lerp(lean.z, -0.15f, 0.09f);
        }

        
        rotation_thing.transform.localRotation = lean;
    }
    
    private void CycaBlyat()
    {
        if (PlayerPrefs.GetString("togglehold_squat") == "hold")
        {
            if (Input.GetButton(PlayerPrefs.GetString("Squat")))
            {
                velocity_endgoal.x *= speed_multiplier_squatting;
                velocity_endgoal.z *= speed_multiplier_squatting;

                if (collider.height > height_squatting)
                {
                    collider.height -= 0.1f;
                    controller.height -= 0.1f;
                    distance_to_ground -= 0.05f;
                    transformation.localScale -= new Vector3(0, 0.05f, 0);
                }
            }
            else if (!Input.GetButton(PlayerPrefs.GetString("Squat"))
                && collider.height < height_standing
                && !IsBeneathSomething())
            {
                collider.height += 0.1f;
                controller.height += 0.1f;
                distance_to_ground += 0.05f;
                transformation.localScale += new Vector3(0, 0.05f, 0);
            }
        }
        else if (PlayerPrefs.GetString("togglehold_squat") == "toggle")
        {
            if (Input.GetButtonDown(PlayerPrefs.GetString("Squat")))
            {
                if (!is_squatting)
                {
                    is_squatting = true;
                }
                else if (is_squatting)
                {
                    is_squatting = false;
                }
            }

            if (!is_squatting
                && collider.height < height_standing
                && !IsBeneathSomething())
            {
                collider.height += 0.1f;
                controller.height += 0.1f;
                distance_to_ground += 0.05f;
                transformation.localScale += new Vector3(0, 0.05f, 0);
            }
            else if (is_squatting)
            {
                velocity_endgoal.x *= speed_multiplier_squatting;
                velocity_endgoal.z *= speed_multiplier_squatting;

                if (collider.height > height_squatting)
                {
                    collider.height -= 0.1f;
                    controller.height -= 0.1f;
                    distance_to_ground -= 0.05f;
                    transformation.localScale -= new Vector3(0, 0.05f, 0);
                }
            }
        }
    }

    private void WalkRun()
    {
        if (Input.GetButtonDown(PlayerPrefs.GetString("Speed Toggle")))
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
}
