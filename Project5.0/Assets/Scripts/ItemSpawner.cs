using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ItemSpawner
 * Author:          Andrew Potisk
 * Finalized on:    --/--/----
 * 
 * Purpose:
 * This script spawns items into the current scene from a stack of SavedObject objects,
 * which has been filled by the the 'ItemLoader' script
 * as a part of the game-loading functionality.
 * 
 * Notes:
 * Due to the way in which this script's functionality continues to run on every frame,
 * there is technically no need for the 'ItemLoader' script to be placed prior to it in the load order.
 * The whole operation will behave as it should either way.
 * 
 * Bugs:
*/
public class ItemSpawner : MonoBehaviour
{
    public Stack<SavedObject> item_stack;
    private SavedObject item;
    private GameObject thang;
    
    // Start is called before the first frame update
    void Start()
    {
        item_stack = new Stack<SavedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(item_stack.Count != 0)
        //{
        //    item = item_stack.Pop();

        //    thang = GameObject.Instantiate(Resources.Load<GameObject>(item.strings[0]),
        //            new Vector3(item.position_x, item.position_y, item.position_z),
        //            Quaternion.Euler(item.rotation_x, item.rotation_y, item.rotation_z));

        //    thang.GetComponent<ObjectBehaviorDefault>().SetObjectData(item);
        //}

        while (item_stack.Count != 0)
        {
            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        item = item_stack.Pop();

        thang = GameObject.Instantiate(Resources.Load<GameObject>(item.strings[0]),
                new Vector3(item.position_x, item.position_y, item.position_z),
                Quaternion.Euler(item.rotation_x, item.rotation_y, item.rotation_z));

        thang.GetComponent<ObjectBehaviorDefault>().SetObjectData(item);
    }
}
