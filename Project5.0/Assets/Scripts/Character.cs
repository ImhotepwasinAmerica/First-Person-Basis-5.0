using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public float rotation_x, rotation_y, rotation_z;

    public float position_x, position_y, position_z;

    public int health, max_health;

    public float weight;

    public bool invincible, weapon_ammo_count;

    public ArrayList inventory;

    public VirtualObject[] toolbelt;
}
