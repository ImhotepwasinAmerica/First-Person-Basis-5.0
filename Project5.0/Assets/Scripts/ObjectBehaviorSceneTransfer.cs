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
 */
public class ObjectBehaviorSceneTransfer : ObjectBehaviorDefault
{
    public string scene_name, activate_or_collide;
    public Vector3 spawn_location;
    private GameObject slider;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadAsynchronously(string scene_name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene_name);

        slider.gameObject.SetActive(true);

        while (operation.isDone == false)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.GetComponent<Slider>().value = progress;
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
            LoadNewLevel();
        }
    }

    public override void OnCollideWithPlayer(GameObject player)
    {
        if (activate_or_collide == "collide")
        {
            LoadNewLevel();
        }
    }

    // In the instance that a game has been started, and the player runs through a scene without saving,
    // any changed data concerning that scene will be lost.
    public void SaveAuxiliary()
    {
        PlayerPrefs.SetString("saved_game_slot", "auxiliary");

        // If the directory to which the data must be saved does not exist,
        // said directory is created.
        Serialization.CreateDirectory(Application.persistentDataPath + "/saves/savedgames/"
            + PlayerPrefs.GetString("saved_game_slot")
            + "/" + SceneManager.GetActiveScene().name
            + "/presentitems");

        Serialization.CreateDirectory(Application.persistentDataPath + "/saves/savedgames/"
            + PlayerPrefs.GetString("saved_game_slot")
            + "/" + SceneManager.GetActiveScene().name
            + "/items");

        // The essential data of the game is saved.
        Serialization.Save<Game>(data_container.GetComponent<DataContainer>().game,
            Application.persistentDataPath + "/saves/savedgames/"
            + data_container.GetComponent<DataContainer>().saved_game_slot + "/game.dat");

        Serialization.Save<SavedObject>(data_container.GetComponent<DataContainer>().character,
            Application.persistentDataPath + "/saves/savedgames/"
            + PlayerPrefs.GetString("saved_game_slot") + "/character.dat");

        // The scene essential data is saved
        Serialization.Save<Scene>(data_container.GetComponent<DataContainer>().scene,
            Application.persistentDataPath + "/saves/savedgames/"
            + PlayerPrefs.GetString("saved_game_slot")
            + "/" + SceneManager.GetActiveScene().name + "/scene.dat");

        // The items in the current scene are saved
        GameEvents.current.SaveAllItemsAux();
    }
}
