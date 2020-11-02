using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
 * SavedObject
 * Author:          Andrew Potisk
 * Finalized on:    --/--/----
 * 
 * Purpose:
 * This class represents a game object as it is stored in memory.
 * This can be anything from an item to an NPC.  It is also usd to store the player character's attributes.
 * 
 * Notes:
 * For each list, certain specific values shall represent specific object and character attributes,
 * common across everything they represent.  These are:
 * ints[0] = the object's health
 * ints[1] = the object's maximum health
 * ints[2] = the object's speed modifier
 * ints[3] = the object's ammo count (if it is a gun)
 * floats[0] = the object's weight
 * floats[1] = the object's temperature
 * strings[0] = the object's in-game model, used for drawing it from the resources folder
 * strings[1] = the object's name
 * bools[0] = the object's state of being invincible (whether it will take damage at all)
 * bools[1] = the object's state of being on or off (if the object is a switch, light source or something similar)
 * objects[1] - objects[10] = a character's toolbelt (hotbar)
 */
[System.Serializable]
public class SavedObject
{ 
    public float rotation_x, rotation_y, rotation_z;

    public float position_x, position_y, position_z;

    public List<int> ints;

    public List<float> floats;

    public List<string> strings;

    public List<bool> bools;

    public List<SavedObject> objects;
}