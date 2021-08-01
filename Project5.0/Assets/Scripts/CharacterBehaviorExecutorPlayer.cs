using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterBehaviorExecutorPlayer : CharacterBehaviorExecutor
{
    public GameObject rotation_thing;

    public float speed_multiplier_squatting, height_standing, height_squatting, sensitivity, 
        cont_lean_distance, move_lean_distance, lean_speed_scalar;

    private float squat_toggle_timer, lean_goal;
    private bool is_squatting;
    private Quaternion lean;

    private CapsuleCollider collider;

    public override void DoOnStart()
    {
        base.DoOnStart();

        GameEvents.current.SaveAllTheThings += SaveSelf;
        GameEvents.current.SaveAllTheThingsAux += SaveSelfAux;

        collider = GetComponent<CapsuleCollider>();

        lean = Quaternion.Euler(0, 0, 0);
    }

    public override void DoOnUpdate()
    {
        base.DoOnUpdate();

        if (Time.timeScale > 0.1f)
        {
            lean_goal = LeanGoalGenerate();

            lean.z = Universals.LerpBetter(lean.z, lean_goal, lean_speed_scalar * Time.deltaTime);

            LeanSet();

            //MovementLean();

            //ControlLean();

            CycaBlyat();
        }

        RecordPosRotData();
    }

    private float LeanGoalGenerate()
    {
        //lean_goal = 0;

        if (action_detector.move_left
            && !action_detector.move_right)
        {
            return move_lean_distance;
        }
        else if (action_detector.move_right
            && !action_detector.move_left)
        {
            return -move_lean_distance;
        }

        if (action_detector.lean_left
            && !action_detector.lean_right)
        {
            return cont_lean_distance;
        }
        else if (action_detector.lean_right
            && !action_detector.lean_left)
        {
            return -cont_lean_distance;
        }

        return 0;
    }

    private void MovementLean()
    {
        if ((action_detector.move_left && action_detector.move_right) || (!action_detector.move_left && !action_detector.lean_right))
        {
            if (lean.z < -0.001)
            {
                lean.z += Time.deltaTime;
            }
            else if (lean.z > 0.001)
            {
                lean.z -= Time.deltaTime;
            }
        }
        else if (action_detector.move_left
            && !action_detector.move_right)
        {
            if (lean.z < 0.15f)
            {
                lean.z += Time.deltaTime;
            }
        }
        else if (action_detector.move_right
            && !action_detector.move_left)
        {
            if (lean.z > -0.15f)
            {
                lean.z -= Time.deltaTime;
            }
        }
    }

    private void ControlLean()
    {
        if ((action_detector.lean_left && action_detector.lean_right) || (!action_detector.lean_left && !action_detector.lean_right))
        {
            if (lean.z < -0.001)
            {
                lean.z += Time.deltaTime;
            }
            else if (lean.z > 0.001)
            {
                lean.z -= Time.deltaTime;
            }
        }
        else if (action_detector.lean_left
            && !action_detector.lean_right)
        {
            if (lean.z < 0.15f)
            {
                lean.z += Time.deltaTime;
            }
        }
        else if (action_detector.lean_right
            && !action_detector.lean_left)
        {
            if (lean.z > -0.15f)
            {
                lean.z -= Time.deltaTime;
            }
        }
    }

    private void CycaBlyat()
    {
        if (PlayerPrefs.GetString("togglehold_squat") == "hold")
        {
            if (action_detector.squat_hold)
            {
                velocity_endgoal.x *= speed_multiplier_squatting;
                velocity_endgoal.z *= speed_multiplier_squatting;

                collider.height = height_squatting;
                controller.height = height_squatting;
                //transform.localScale.Set(1, 0.5f, 1);
            }
            else if (!action_detector.squat_hold
                && !IsBeneathSomething())
            {
                collider.height = height_standing;
                controller.height = height_standing;
                //transform.localScale.Set(1, 1, 1);
            }
        }
        else if (PlayerPrefs.GetString("togglehold_squat") == "toggle")
        {
            if (action_detector.squat)
            {
                if (squat_toggle_timer == null)
                {
                    squat_toggle_timer = Time.time;
                }
                else if (squat_toggle_timer < Time.time - 0.3)
                {
                    if (!is_squatting)
                    {
                        is_squatting = true;
                    }
                    else if (is_squatting)
                    {
                        is_squatting = false;
                    }

                    squat_toggle_timer = Time.time;
                }
            }

            if (!is_squatting
                && !IsBeneathSomething())
            {
                collider.height = 2f;
                controller.height = 2f;
                transform.localScale.Set(1,1,1);
            }
            else if (is_squatting)
            {
                velocity_endgoal.x *= speed_multiplier_squatting;
                velocity_endgoal.z *= speed_multiplier_squatting;

                collider.height = 1f;
                controller.height = 1f;
                transform.localScale.Set(1, 0.5f, 1);
            }
        }
    }

    public override void GetCameraMovement()
    {
        if (!action_detector.item_rotate)
        {
            md.x = action_detector.mouse_x * (sensitivity * 50) * Time.deltaTime;
            md.y -= action_detector.mouse_y * (sensitivity * 50) * Time.deltaTime;

            md.y = Mathf.Clamp(md.y, -90f, 90f);

            camera.transform.localRotation = Quaternion.Euler(md.y, 0, 0);
            transform.Rotate(Vector3.up * md.x);
        }
    }

    private bool IsBeneathSomething()
    {
        return Physics.Raycast(transform.position, Vector3.up, ((controller.height / 2) + 0.3f));
    }

    private void RecordPosRotData()
    {
        guy.position_x = transform.position.x;
        guy.position_y = transform.position.y;
        guy.position_z = transform.position.z;

        guy.rotation_x = transform.rotation.x;
        guy.rotation_y = transform.rotation.y;
        guy.rotation_z = transform.rotation.z;
    }

    private void LeanCorrection()
    {
        if (lean.z > -0.001 && lean.z < 0.001)
        {
            lean.z = 0;
        }
    }

    private void LeanSet()
    {
        rotation_thing.transform.localRotation = lean;
    }

    public void SaveSelf()
    {
        Serialization.Save<SavedObject>(guy,
            Application.persistentDataPath + "/saves/savedgames/"
            + PlayerPrefs.GetString("saved_game_slot")
            + "/character.dat");
    }

    public void SaveSelfAux()
    {
        Serialization.Save<SavedObject>(guy,
            Application.persistentDataPath + "/saves/savedgames/auxiliary/"
            + "/character.dat");
    }
}
