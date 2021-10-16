using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

/*
 * ObjectBehaviorSceneTransfer
 * Author:          Andrew Potisk
 * Finalized on:    --/--/----
 * 
 * Purpose:
 * This script is used to trigger scene transitions, 
 * either by pressing'general action' on an object or by colliding with it.
 * 
 * Notes:
 * 
 * Bugs:
 * The script disappears from the object when a scene which has been loaded  before has been loaded again.
 */
public class ObjectBehaviorSceneTransfer : ObjectBehaviorDefault
{
    public string scene_name, activate_or_collide;
    public Vector3 spawn_location;
    private GameObject slider;
    
    //// Start is called before the first frame update
    //void Start()
    //{
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    IEnumerator LoadAsynchronously(string scene_name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene_name);

        //slider.gameObject.SetActive(true);

        while (operation.isDone == false)
        {
            //float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //slider.GetComponent<Slider>().value = progress;
            yield return null;
        }
    }

    public void LoadNewLevel()
    {
        slider = GameObject.FindGameObjectWithTag("LevelTransferSlider");
        StartCoroutine(LoadAsynchronously(scene_name));
    }

    public override void UseDefault(GameObject new_anchor)
    {
        if (activate_or_collide == "activate")
        {
            Debug.Log("Ostrich nipples");
            SetSpawnPoint();
            SaveAuxiliary();
            LoadNewLevel();
        }
    }

    public override void OnCollideWithPlayer(GameObject player)
    {
        if (activate_or_collide == "collide")
        {
            Debug.Log("Ostrich nipples");
            SetSpawnPoint();
            SaveAuxiliary();
            LoadNewLevel();
        }
    }

    public override void OnCollideWithAnything(GameObject anything)
    {
        Debug.Log("Ostrich nipples");
    }



    // In the instance that a game has been started, and the player runs through a scene without saving,
    // any changed data concerning that scene will be lost.
    private void SaveAuxiliary()
    {
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

        // The essential data of the game is saved.
        Serialization.Save<Game>(data_container.GetComponent<DataContainer>().game,
            Application.persistentDataPath + "/saves/savedgames/auxiliary/game.dat");

        Serialization.Save<SavedObject>(data_container.GetComponent<DataContainer>().character,
            Application.persistentDataPath + "/saves/savedgames/auxiliary/character.dat");

        // The scene essential data is saved
        Serialization.Save<Scene>(data_container.GetComponent<DataContainer>().scene,
            Application.persistentDataPath + "/saves/savedgames/auxiliary/" + SceneManager.GetActiveScene().name + "/scene.dat");

        // The items in the current scene are saved
        GameEvents.current.SaveAllItemsAux();
    }

    private void SetSpawnPoint()
    {
        data_container.GetComponent<DataContainer>().character.position_x = spawn_location.x;
        data_container.GetComponent<DataContainer>().character.position_y = spawn_location.y;
        data_container.GetComponent<DataContainer>().character.position_z = spawn_location.z;
    }
}
