﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
 * 
 * It has proven difficult to change the location of the character;
 * If I try to change it manually, the character just teleports back to its previous location.
 * A trick is used here where, in order to set the character's spawn location when a new scene or game is loaded,
 * the character is literally deleted and re-spawned at the desired location.
 * This could cause problems when/if the character is given a unique model,
 * or if a teleportation mechanic is introduced into the game.
 * These problems might also apply to NPCs.
 */
public class LevelManager : MonoBehaviour
{
    public GameObject data_container, character, camera;

    private Vector3 character_position, character_rotation;
    private SavedObject guy;

    public void Awake()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        //// If the sub-directory corresponding to the scene exists in the save file,
        //// it is understood that the scene has been visited in the ongoing game.
        //// It is therefore necessary to affect the scene according to the changes made to it.
        //// If the sub-directory exists but the 'presentitems' sub-directory within that is empty,
        //// it is presumed that the directory was created without entering and thereby saving the scene.
        //if (Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/auxiliary/"
        //    + SceneManager.GetActiveScene().name)
        //    &&
        //    Directory.GetFiles(Application.persistentDataPath + "/saves/savedgames/auxiliary/" 
        //    + SceneManager.GetActiveScene().name + "/presentitems").Length 
        //    != 0)
        //{
        //    Debug.Log("Items affected appropriately, so far.");

        //    // This *CANNOT* run when the 'presentitems' directory is empty, otherwise all items in the scene will disappear!
        //    //GameEvents.current.DeleteSmartly();
        //    GameEvents.current.DeleteSmartly();
        //}
        //else
        //{
        //    Debug.Log("No items affected.");
        //}

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

        Debug.Log("Items affected: " + Directory.GetFiles(Application.persistentDataPath + "/saves/savedgames/auxiliary/" + SceneManager.GetActiveScene().name
            + "/presentitems").Length);

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
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        Cursor.lockState = CursorLockMode.Locked;

        guy = data_container.GetComponent<DataContainer>().character;

        data_container.GetComponent<DataContainer>().game.current_scene_name = SceneManager.GetActiveScene().name;

        // If a saved game has been loaded and the character has been assigned a location and rotation,
        // the character object in the current scene is destroyed and another is created
        // at he location specified in the saved data
        character = GameObject.Instantiate(Resources.Load<GameObject>("Character 1"),
                        new Vector3(guy.position_x, guy.position_y, guy.position_z),
                        Quaternion.Euler(guy.rotation_x, guy.rotation_y, guy.rotation_z));

        GameObject.FindGameObjectWithTag("CameraBackup").SetActive(false);

        camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.GetComponent<PlayerLooking>().ex = guy.rotation_x;
        camera.GetComponent<PlayerLooking>().why = guy.rotation_y;
        camera.GetComponent<PlayerLooking>().zee = guy.rotation_z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        camera = GameObject.FindGameObjectWithTag("MainCamera");

        if (character != null)
        {
            character_position = character.transform.position;
            character_rotation = camera.transform.eulerAngles;

            guy.position_x = character_position.x;
            guy.position_y = character_position.y;
            guy.position_z = character_position.z;

            guy.rotation_x = character_rotation.x;
            guy.rotation_y = character_rotation.y;
            guy.rotation_z = character_rotation.z;
        }
        
    }
}