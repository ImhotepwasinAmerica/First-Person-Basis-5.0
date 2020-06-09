using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlsMiscellaneous : MonoBehaviour
{
    public static bool paused = false;
    public GameObject data_container, pause_UI, main_menu, options_menu, keybindings_menu, graphics_menu, sound_menu, gameplay_menu, game_files_menu, loading_screen;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");
    }

    // Update is called once per frame
    void Update()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        if (Input.GetButtonDown(PlayerPrefs.GetString("Pause")))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (Input.GetButtonDown(PlayerPrefs.GetString("Quicksave")))
        {
            SaveButton("quicksave");
            Debug.Log("Saved Quickly!");
        }

        if (Input.GetButtonDown(PlayerPrefs.GetString("Quickload")))
        {
            SetSlot("quicksave");
            LoadInfo();
            SetScene();
            LoadScene();
        }

        if (Input.GetButtonDown(PlayerPrefs.GetString("Log")))
        {

        }

        if (Input.GetButtonDown(PlayerPrefs.GetString("Objectives")))
        {

        }

        if (Input.GetButtonDown(PlayerPrefs.GetString("Stats")))
        {

        }

        if (Input.GetButtonDown(PlayerPrefs.GetString("Gamma Up")))
        {

        }

        if (Input.GetButtonDown(PlayerPrefs.GetString("Gamma Down")))
        {

        }
    }

    public void Resume()
    {
        pause_UI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        pause_UI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SaveButton(string slot)
    {
        PlayerPrefs.SetString("saved_game_slot", slot);

        // If a saved game has files stored in a different slot than what is being saved to, those files will be transferred over to the new slot, in their equivalent directories.
        if (data_container.GetComponent<DataContainer>().saved_game_slot != "new game")
        {
            UnityEditor.FileUtil.CopyFileOrDirectory(
                Application.persistentDataPath + "/saves/savedgames/" + data_container.GetComponent<DataContainer>().saved_game_slot,
                Application.persistentDataPath + "/saves/savedgames/" + slot);
        }
        else
        {
            data_container.GetComponent<DataContainer>().saved_game_slot = slot;
        }

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

        Serialization.Save<Character>(data_container.GetComponent<DataContainer>().character,
            Application.persistentDataPath + "/saves/savedgames/"
            + PlayerPrefs.GetString("saved_game_slot") + "/character.dat");

        // The scene essential data is saved
        Serialization.Save<Scene>(data_container.GetComponent<DataContainer>().scene,
            Application.persistentDataPath + "/saves/savedgames/"
            + PlayerPrefs.GetString("saved_game_slot")
            + "/" + SceneManager.GetActiveScene().name + "/scene.dat");

        // The items in the current scene are saved
        GameEvents.current.SaveAllItems();

        //Debug.Log(data_container.GetComponent<DataContainer>().character.position_x);
        //Debug.Log(data_container.GetComponent<DataContainer>().game.current_scene_name);
    }

    public void SetSlot(string slot)
    {
        //data_container.GetComponent<DataContainer>().saved_game_slot = slot;
        PlayerPrefs.SetString("saved_game_slot", slot);
    }

    public void LoadInfo()
    {
        data_container.GetComponent<DataContainer>().game =
            Serialization.Load<Game>(Application.persistentDataPath + "/saves/savedgames/" +
            PlayerPrefs.GetString("saved_game_slot") + "/game.dat");

        //data_container.GetComponent<DataContainer>().character =
        //    Serialization.Load<Character>(Application.persistentDataPath + "/saves/savedgames/" +
        //    data_container.GetComponent<DataContainer>().saved_game_slot + "/character.dat");

        //Debug.Log(data_container.GetComponent<DataContainer>().character.position_x);
        //Debug.Log(data_container.GetComponent<DataContainer>().character.max_health);
        //Debug.Log(data_container.GetComponent<DataContainer>().game.current_scene_name);
    }

    public void SetScene()
    {
        //data_container.GetComponent<DataContainer>().saved_game_scene = data_container.GetComponent<DataContainer>().game.current_scene_name;
    }

    public void LoadScene()
    {
        StartCoroutine(LoadAsynchronously(data_container.GetComponent<DataContainer>().game.current_scene_name));
    }

    IEnumerator LoadAsynchronously(string scene_name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene_name);

        loading_screen.SetActive(true);

        while (operation.isDone == false)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}
