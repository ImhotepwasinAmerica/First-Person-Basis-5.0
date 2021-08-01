using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Events;
/*
 * LevelManager
 * Author:          Andrew Potisk
 * Finalized on:    --/--/----
 * 
 * Purpose:
 * This script handles a variety of loading processes, most notably the character's data.
 * 
 * Notes:
 * 
 * Bugs:
 * If the 'auxiliary' save slot has not been filled with the proper data, this entire operation will not work.
 */
public class LevelManager : MonoBehaviour
{
    public GameObject data_container, character;
    public string character_model_name;
    public Vector3 start_location_default;

    private int counter;
    private Vector3 character_position, character_rotation;
    private SavedObject guy;
    private Camera camera;

    public void Awake()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        // If the directory which holds the current scene's data does not exist,
        // it is created.
        if (!Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/auxiliary/" + SceneManager.GetActiveScene().name))
        {
            Serialization.CreateDirectory(Application.persistentDataPath + "/saves/savedgames/auxiliary/" + SceneManager.GetActiveScene().name);
        }

        // Just in case either the 'items' or 'presentitems' directories do not exist for the current scene in the 'auxiliary' save slot,
        // they are created here.
        if (!Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/auxiliary/" + SceneManager.GetActiveScene().name
            + "/items"))
        {
            Serialization.CreateDirectory(Application.persistentDataPath + "/saves/savedgames/auxiliary/" + SceneManager.GetActiveScene().name
            + "/items");
        }

        if (!Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/auxiliary/" + SceneManager.GetActiveScene().name
            + "/presentitems"))
        {
            Serialization.CreateDirectory(Application.persistentDataPath + "/saves/savedgames/auxiliary/" + SceneManager.GetActiveScene().name
            + "/presentitems");
        }

        //Debug.Log("Items affected: " + Directory.GetFiles(Application.persistentDataPath + "/saves/savedgames/auxiliary/" + SceneManager.GetActiveScene().name
        //    + "/presentitems").Length);

        if (Serialization.SaveExists(Application.persistentDataPath + "/saves/savedgames/auxiliary/game.dat"))
        {
            data_container.GetComponent<DataContainer>().game =
                Serialization.Load<Game>(Application.persistentDataPath + "/saves/savedgames/auxiliary/game.dat");
        }
        else
        {
            data_container.GetComponent<DataContainer>().game = new Game();
            Serialization.Save<Game>(data_container.GetComponent<DataContainer>().game, Application.persistentDataPath + "/saves/savedgames/auxiliary/game.dat");
        }

        if (Serialization.SaveExists(Application.persistentDataPath + "/saves/savedgames/auxiliary/" + SceneManager.GetActiveScene().name + "/scene.dat"))
        {
            data_container.GetComponent<DataContainer>().scene =
                Serialization.Load<Scene>(Application.persistentDataPath + "/saves/savedgames/auxiliary/" + SceneManager.GetActiveScene().name + "/scene.dat");
        }
        else
        {
            data_container.GetComponent<DataContainer>().scene = new Scene();
            Serialization.Save<Scene>(data_container.GetComponent<DataContainer>().scene, Application.persistentDataPath + "/saves/savedgames/auxiliary/" + SceneManager.GetActiveScene().name + "/scene.dat");
        }

        if (Serialization.SaveExists(Application.persistentDataPath + "/saves/savedgames/auxiliary/character.dat"))
        {
            data_container.GetComponent<DataContainer>().character =
                Serialization.Load<SavedObject>(Application.persistentDataPath + "/saves/savedgames/auxiliary/character.dat");
        }
        else
        {
            data_container.GetComponent<DataContainer>().character = new SavedObject();
            Serialization.Save<SavedObject>(data_container.GetComponent<DataContainer>().character, Application.persistentDataPath + "/saves/savedgames/auxiliary/character.dat");
        }

        data_container.GetComponent<DataContainer>().game.current_scene_name = SceneManager.GetActiveScene().name;
    }

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;

        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        Cursor.lockState = CursorLockMode.Locked;

        guy = data_container.GetComponent<DataContainer>().character;

        data_container.GetComponent<DataContainer>().game.current_scene_name = SceneManager.GetActiveScene().name;

        if (guy.position_x == 0 && guy.position_y == 0 && guy.position_z == 0)
        {
            character = GameObject.Instantiate(Resources.Load<GameObject>(character_model_name),
                        new Vector3(start_location_default.x, start_location_default.y, start_location_default.z),
                        Quaternion.Euler(guy.rotation_x, guy.rotation_y, guy.rotation_z));
        }
        else
        {
            character = GameObject.Instantiate(Resources.Load<GameObject>(character_model_name),
                        new Vector3(guy.position_x, guy.position_y, guy.position_z),
                        Quaternion.Euler(guy.rotation_x, guy.rotation_y, guy.rotation_z));
        }

        // The scene's backup camera is deleted, so that the player character's camera can work.
        GameObject.FindGameObjectWithTag("CameraBackup").SetActive(false);

        // The camera is instantiated
        camera = Resources.Load<Camera>("CameraPrefab");

        // The player character has the camera attached to the main body's camera anchor  
        Universals.BuildCharacter(character, camera);
    }

    // Update is called once per frame
    void Update()
    {
        if (counter < 1000000)
        {
            counter++;
        }
        
        if (counter == 1)
        {
            EventSystemFixer();
        }
    }

    void FixedUpdate()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        //camera = GameObject.FindGameObjectWithTag("MainCamera");

        if (character != null)
        {
            character_position = character.transform.position;
            //character_rotation = camera.transform.eulerAngles;

            //guy.position_x = character_position.x;
            //guy.position_y = character_position.y;
            //guy.position_z = character_position.z;

            //guy.rotation_x = character_rotation.x;
            //guy.rotation_y = character_rotation.y;
            //guy.rotation_z = character_rotation.z;
        }
    }

    private void EventSystemFixer()
    {
        EventSystem[] systems = GameObject.FindObjectsOfType<EventSystem>();

        if(systems.Length > 1)
        {
            for (int i = 0; i < systems.Length; i++)
            {
                if (systems[i] != EventSystem.current)
                {
                    GameObject.Destroy(systems[i]);
                }
                
            }
        }
        else if(systems.Length == 0)
        {
            GameObject eventsystem = new GameObject("EventSystem");
            eventsystem.AddComponent<EventSystem>();
            eventsystem.AddComponent<StandaloneInputModule>();
        }
    }
}



















