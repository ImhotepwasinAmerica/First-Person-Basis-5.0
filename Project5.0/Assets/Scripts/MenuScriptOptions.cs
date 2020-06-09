using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScriptOptions : MonoBehaviour
{
    public GameObject options_menu, main_menu, keybindings_menu, graphics_menu, sound_menu, gameplay_menu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMenuPage(GameObject page)
    {
        page.SetActive(true);
        options_menu.SetActive(false);
    }

    public void OpenMain()
    {
        OpenMenuPage(main_menu);
    }

    public void OpenKeybindings()
    {
        OpenMenuPage(keybindings_menu);
    }

    public void OpenGraphics()
    {
        OpenMenuPage(graphics_menu);
    }

    public void OpenSound()
    {
        OpenMenuPage(sound_menu);
    }

    public void OpenGameplay()
    {
        OpenMenuPage(gameplay_menu);
    }
}
