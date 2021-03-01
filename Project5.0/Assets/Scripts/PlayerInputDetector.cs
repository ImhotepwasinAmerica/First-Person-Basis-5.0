using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputDetector : CharacterActionDetector
{
    public override void DoOnFixedUpdate()
    {
        base.DoOnFixedUpdate();

        GetInputs();
    }

    public override void DoOnUpdate()
    {
        base.DoOnUpdate();

        //GetInputs();
    }

    private void GetInputs()
    {
        mouse_x = Input.GetAxisRaw("Mouse X");

        mouse_y = Input.GetAxisRaw("Mouse Y");

        general_action = Input.GetButtonDown(PlayerPrefs.GetString("General Action"));

        general_action_hold = Input.GetButton(PlayerPrefs.GetString("General Action"));

        item_rotate = Input.GetButton(PlayerPrefs.GetString("Item Rotate"));

        speed_toggle = Input.GetButtonDown(PlayerPrefs.GetString("Speed Toggle"));

        squat = Input.GetButtonDown(PlayerPrefs.GetString("Squat"));

        lean_left = Input.GetButton(PlayerPrefs.GetString("Lean Left"));

        lean_right = Input.GetButton(PlayerPrefs.GetString("Lean Right"));

        jump = Input.GetButtonDown(PlayerPrefs.GetString("Jump"));

        jump_higher = Input.GetButton(PlayerPrefs.GetString("Jump"));

        move_backward = Input.GetButton(PlayerPrefs.GetString("Move Backward"));

        move_forward = Input.GetButton(PlayerPrefs.GetString("Move Forward"));

        move_left = Input.GetButton(PlayerPrefs.GetString("Move Left"));

        move_right = Input.GetButton(PlayerPrefs.GetString("Move Right"));
    }
}
