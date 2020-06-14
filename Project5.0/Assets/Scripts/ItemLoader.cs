using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class ItemLoader : MonoBehaviour
{
    public GameObject data_container, item_spawner;
    private string[] files;
    private int index;
    private bool has_been_created;
    // Start is called before the first frame update
    void Start()
    {
        has_been_created = false;

        if (Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/"
            + PlayerPrefs.GetString("saved_game_slot")
            + "/" + SceneManager.GetActiveScene().name))
        {
            files = Directory.GetFiles(Application.persistentDataPath + "/saves/savedgames/"
            + PlayerPrefs.GetString("saved_game_slot")
            + "/" + SceneManager.GetActiveScene().name
            + "/items");

            if (files.Length > 0)
            {
                index = files.Length-1; // Off-by-one error avoided \o/

                while (index >= 0)
                {
                    item_spawner.GetComponent<ItemSpawner>().item_stack.Push(Serialization.Load<SavedObject>(
                        Application.persistentDataPath + "/saves/savedgames/"
                    + PlayerPrefs.GetString("saved_game_slot")
                    + "/" + SceneManager.GetActiveScene().name
                    + "/items/" + files[index]));

                    index--;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
