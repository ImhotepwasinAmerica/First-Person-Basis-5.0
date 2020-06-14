using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/*
 * MenuScriptFiles
 * Author:          Andrew Potisk
 * Finalized on:    --/--/----
 * Purpose:
 * These scripts handle all operations on the game files menu.
 * 
 * Notes:
 * This is how the game's files are organized:
 * 
 * Savegame data not related to scenes:
 * #Application.persistentDataPath#/saves/savedgames/#saveslot#/character
 * #Application.persistentDataPath#/saves/savedgames/#saveslot#/game
 * 
 * Savegame data regarding individual scenes:
 * #Application.persistentDataPath#/saves/savedgames/#saveslot#/#scene#/scene
 * 
 * Savegame data regarding item and NPCs in scenes:
 * #Application.persistentDataPath#/saves/savedgames/#saveslot#/#scene#/items/#item#
 * 
 * Savegame data regarding items and NPCs which were already in the scene, and therefore need not be destroyed
 * #Application.persistentDataPath#/saves/savedgames/#saveslot#/#scene#/presentitems/#item#
*/
public class MenuScriptFiles : MonoBehaviour
{
    public GameObject data_container, loading_screen, game_files_menu, main_menu;

    public Text quicksave, save1, save2, save3, save4, save5, save6, save7, save8, save9, save10;
    public GameObject button_save_0, button_save_1, button_save_2, button_save_3, button_save_4, button_save_5, button_save_6, button_save_7, button_save_8, button_save_9, button_save_10,
        button_load_0, button_load_1, button_load_2, button_load_3, button_load_4, button_load_5, button_load_6, button_load_7, button_load_8, button_load_9, button_load_10;

    private Game[] local_data;

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        InitiateSaveSlot(quicksave,Application.persistentDataPath + "/saves/savedgames/quicksave/game",0);
        InitiateSaveSlot(save1, Application.persistentDataPath + "/saves/savedgames/save1/game", 1);
        InitiateSaveSlot(save2, Application.persistentDataPath + "/saves/savedgames/save2/game", 2);
        InitiateSaveSlot(save3, Application.persistentDataPath + "/saves/savedgames/save3/game", 3);
        InitiateSaveSlot(save4, Application.persistentDataPath + "/saves/savedgames/save4/game", 4);
        InitiateSaveSlot(save5, Application.persistentDataPath + "/saves/savedgames/save5/game", 5);
        InitiateSaveSlot(save6, Application.persistentDataPath + "/saves/savedgames/save6/game", 6);
        InitiateSaveSlot(save7, Application.persistentDataPath + "/saves/savedgames/save7/game", 7);
        InitiateSaveSlot(save8, Application.persistentDataPath + "/saves/savedgames/save8/game", 8);
        InitiateSaveSlot(save9, Application.persistentDataPath + "/saves/savedgames/save9/game", 9);
        InitiateSaveSlot(save10, Application.persistentDataPath + "/saves/savedgames/save10/game", 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            button_save_0.SetActive(false);
            button_save_1.SetActive(false);
            button_save_2.SetActive(false);
            button_save_3.SetActive(false);
            button_save_4.SetActive(false);
            button_save_5.SetActive(false);
            button_save_6.SetActive(false);
            button_save_7.SetActive(false);
            button_save_8.SetActive(false);
            button_save_9.SetActive(false);
            button_save_10.SetActive(false);
        }

        button_load_0.SetActive(Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/quicksave"));
        button_load_1.SetActive(Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/save1"));
        button_load_2.SetActive(Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/save2"));
        button_load_3.SetActive(Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/save3"));
        button_load_4.SetActive(Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/save4"));
        button_load_5.SetActive(Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/save5"));
        button_load_6.SetActive(Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/save6"));
        button_load_7.SetActive(Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/save7"));
        button_load_8.SetActive(Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/save8"));
        button_load_9.SetActive(Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/save9"));
        button_load_10.SetActive(Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/save10"));
    }

    public void InitiateSaveSlot(Text text, string directory, int index)
    {
        if (Serialization.DirectoryExists(directory))
        {
            local_data[index] = Serialization.Load<Game>(directory + "/game");

            text.text = local_data[index].saveslot_label;
        }
    }

    public void DeleteDirectory(string directory_partial)
    {
        Serialization.DeleteDirectory(Application.persistentDataPath + "/saves/savedgames/" + directory_partial);
    }

    public void DeleteText(Text text)
    {
        text.text = "";
    }

    public void FindDate(Text text)
    {
        text.text = System.DateTime.Now.ToLongTimeString() + System.DateTime.Now.ToLongDateString();
    }

    public void SetSlot(string slot)
    {
        //data_container.GetComponent<DataContainer>().saved_game_slot = slot;
        PlayerPrefs.SetString("saved_game_slot", slot);
    }

    public void SetScene()
    {
        //data_container.GetComponent<DataContainer>().saved_game_scene = data_container.GetComponent<DataContainer>().game.current_scene_name;
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

    public void LoadMainMenu()
    {
        StartCoroutine(LoadAsynchronously("MainMenu"));
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

    public void Open_Menu_Page(GameObject page)
    {
        page.SetActive(true);
        game_files_menu.SetActive(false);
    }

    public void Open_Options()
    {
        Open_Menu_Page(main_menu);
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

    public void EstablishDate(Text text)
    {

    }
}
