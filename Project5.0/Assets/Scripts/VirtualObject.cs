using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VirtualObject
{
    public string saved_thing;

    public Vector2 mass;

    public int health, max_health;

    public float weight;

    public bool invincible, switch_on, weapon_ammo_count;

    public ArrayList inventory;
}
