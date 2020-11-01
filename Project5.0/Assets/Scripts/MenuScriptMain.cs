using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScriptMain : MonoBehaviour
{
    public GameObject data_container, main_menu, loading_screen, options_menu, load_game_menu, manual_menu, some_controls;
    public Text newresume_text, quitmainmenu_text;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            // In the case that the game was terminated unexpectedly,
            // the auxiliary save file may still be there. (it should be deleted on game exit)
            // This is a precautionary measure. 
            Serialization.DeleteDirectory(Application.persistentDataPath + "/saves/savedgames/auxiliary");
        }

        if (!PlayerPrefs.HasKey("General Action"))
        {
            DeveloperPreferences.Keybinds();
        }

        if (!PlayerPrefs.HasKey("fullscreen"))
        {
            DeveloperPreferences.Graphics();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            newresume_text.text = "New Game";
            quitmainmenu_text.text = "Quit Game";
        }
        else
        {
            newresume_text.text = "Resume Game";
            quitmainmenu_text.text = "Quit to Main Menu";
        }
    }

    public void NewResumeAction()
    {
        // Save_game_slot equalling "new game"
        // indicates that the game has not been saved to a save slot yet.
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            PlayerPrefs.SetString("saved_game_slot","new game");
            Serialization.CreateDirectory(Application.persistentDataPath + "/saves/savedgames/auxiliary");
            
            Serialization.Save<Game>(new Game(), Application.persistentDataPath + "/saves/savedgames/auxiliary/game.dat");

            SavedObject character = new SavedObject();
            character.position_x = 6;
            character.position_y = 1.1f;
            character.position_z = 4;

            Serialization.Save<SavedObject>(character, Application.persistentDataPath + "/saves/savedgames/auxiliary/character.dat");

            LoadLevel01();
        }
        else
        {
            GameEvents.current.ResumeGame();
        }
    }

    public void QuitMainMenuAction()
    {
        Time.timeScale = 1f;

        Serialization.DeleteDirectory(Application.persistentDataPath + "/saves/savedgames/auxiliary");

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            QuitGame();
        }
        else
        {
            LoadMainMenu();
        }

        
    }

    public void LoadLevel01()
    {
        StartCoroutine(LoadAsynchronously("SampleScene"));
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadAsynchronously("MainMenu"));
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

    public void QuitGame()
    {

        Application.Quit();
        Debug.Log("Quit");
    }

    public void OpenMenuPage(GameObject page)
    {
        page.SetActive(true);
        main_menu.SetActive(false);
    }

    public void Open_Options()
    {
        OpenMenuPage(options_menu);
    }

    public void Open_Load_Level()
    {
        OpenMenuPage(load_game_menu);
    }

    public void Open_Manual()
    {
        OpenMenuPage(manual_menu);
    }
}
