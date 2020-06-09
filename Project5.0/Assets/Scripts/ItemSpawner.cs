using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(item_stack.Count != 0)
        {
            item = item_stack.Pop();

            thang = GameObject.Instantiate(Resources.Load<GameObject>(item.saved_thing),
                    new Vector3(item.position_x, item.position_y, item.position_z),
                    Quaternion.Euler(item.rotation_x, item.rotation_y, item.rotation_z));

            thang.GetComponent<ObjectBehaviorDefault>().SetObjectData(item);
        }
    }
}
