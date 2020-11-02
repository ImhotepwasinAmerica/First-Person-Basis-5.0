using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Runtime.Serialization.Formatters.Binary;

public class ItemCaller : MonoBehaviour
{
    public GameObject data_container;

    // Start is called before the first frame update
    void Start()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        Debug.Log("Does the directory exist?");

        // If the sub-directory corresponding to the scene exists in the save file,
        // it is understood that the scene has been visited in the ongoing game.
        // It is therefore necessary to affect the scene according to the changes made to it.
        // If the sub-directory exists but the 'presentitems' sub-directory within that is empty,
        // it is presumed that the directory was created without entering and thereby saving the scene.
        if (Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/auxiliary/"
            + SceneManager.GetActiveScene().name)
            &&
            Directory.GetFiles(Application.persistentDataPath + "/saves/savedgames/auxiliary/"
            + SceneManager.GetActiveScene().name + "/presentitems").Length
            != 0)
        {
            Debug.Log("Items affected appropriately, so far.");

            // This *CANNOT* run when the 'presentitems' directory is empty, otherwise all items in the scene will disappear!
            // It must also run *AFTER* the 'ObjectBehaviorDefault' script and its children.
            GameEvents.current.DeleteSmartly();
        }
        else
        {
            Debug.Log("No items affected.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
