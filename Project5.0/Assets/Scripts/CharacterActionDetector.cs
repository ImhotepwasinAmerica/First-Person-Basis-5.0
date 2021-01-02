using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionDetector : MonoBehaviour
{
    public bool move_left, move_right, move_forward, move_backward, jump, jump_higher, squat, lean_left, lean_right, speed_toggle, item_rotate, general_action;
    public float mouse_x, mouse_y;

    public void Awake()
    {
        DoOnAwake();
    }

    // Start is called before the first frame update
    void Start()
    {
        DoOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        DoOnUpdate();
    }

    void FixedUpdate()
    {
        DoOnFixedUpdate();
    }

    public virtual void DoOnAwake() { }

    public virtual void DoOnStart() { }

    public virtual void DoOnUpdate() { }

    public virtual void DoOnFixedUpdate() { }
}
