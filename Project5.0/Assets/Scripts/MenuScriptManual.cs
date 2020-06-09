using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScriptManual : MonoBehaviour
{

    public GameObject main_menu, manual_menu;

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
        manual_menu.SetActive(false);
    }

    public void Open_Main()
    {
        OpenMenuPage(main_menu);
    }
}
