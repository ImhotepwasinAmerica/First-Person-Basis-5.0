using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScriptGameplay : MonoBehaviour
{
    public GameObject gameplay_menu, options_menu;
    public Text squat_text, carry_text, enemy_health_text, enemy_damage_text;
    public Slider enemy_health, enemy_damage;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("togglehold_carry"))
        {
            DeveloperPreferences.Gameplay();
        }

        squat_text.text = PlayerPrefs.GetString("togglehold_squat");
        carry_text.text = PlayerPrefs.GetString("togglehold_carry");

        enemy_health.value = PlayerPrefs.GetInt("enemy_health_percentage") + 0.01f;
        enemy_damage.value = PlayerPrefs.GetInt("enemy_damage_percentage") + 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetString("togglehold_squat", squat_text.text);
        PlayerPrefs.SetString("togglehold_carry", carry_text.text);

        PlayerPrefs.SetInt("enemy_health_percentage", (int)enemy_health.value);
        PlayerPrefs.SetInt("enemy_damage_percentage", (int)enemy_damage.value);

        enemy_health_text.text = (int)enemy_health.value + "";
        enemy_damage_text.text = (int)enemy_damage.value + "";
    }

    public void ButtonTextSet(Text text)
    {
        if (text.text == "toggle")
        {
            text.text = "hold";
        }
        else if(text.text == "hold")
        {
            text.text = "toggle";
        }
    }

    public void Open_Menu_Page(GameObject page)
    {
        page.SetActive(true);
        gameplay_menu.SetActive(false);
    }

    public void Open_Options()
    {
        Open_Menu_Page(options_menu);
    }
}
