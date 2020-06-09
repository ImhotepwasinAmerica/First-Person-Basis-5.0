using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MenuScriptGraphics : MonoBehaviour
{
    public GameObject options_menu, graphics_menu, data_container;

    public Text res_text, fov_text, frame_cap_text, pixelization_text, gamma_text;

    public Toggle fullscreen_button, vsync_button, bilinear_filtering_button, bloom_button, flares_button;

    public Slider fov_slider, frame_cap_slider, pixelization_slider, gamma_slider;

    int res_array_pointer;
    Resolution[] res_array;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("fullscreen"))
        {
            DeveloperPreferences.Graphics();
        }

        res_array = new Resolution[]
        {
            new Resolution(320,200),
            new Resolution(400,300),
            new Resolution(512,384),
            new Resolution(640,400),
            new Resolution(800,600),
            new Resolution(1024,768),
            new Resolution(1152,864),
            new Resolution(1280,600),
            new Resolution(1360,768),
            new Resolution(1400,1050),
            new Resolution(1440,900),
            new Resolution(1680,1050),
            new Resolution(1920,1080)
        };

        if (!PlayerPrefs.HasKey("width") || 
            PlayerPrefs.GetInt("width") == 1920) { res_array_pointer = res_array.Length - 1; }
        else if (PlayerPrefs.GetInt("width") == 320) { res_array_pointer = 0; }
        else if (PlayerPrefs.GetInt("width") == 400) { res_array_pointer = 1; }
        else if (PlayerPrefs.GetInt("width") == 512) { res_array_pointer = 2; }
        else if (PlayerPrefs.GetInt("width") == 640) { res_array_pointer = 3; }
        else if (PlayerPrefs.GetInt("width") == 800) { res_array_pointer = 4; }
        else if (PlayerPrefs.GetInt("width") == 1024) { res_array_pointer = 5; }
        else if (PlayerPrefs.GetInt("width") == 1152) { res_array_pointer = 6; }
        else if (PlayerPrefs.GetInt("width") == 1280) { res_array_pointer = 7; }
        else if (PlayerPrefs.GetInt("width") == 1360) { res_array_pointer = 8; }
        else if (PlayerPrefs.GetInt("width") == 1400) { res_array_pointer = 9; }
        else if (PlayerPrefs.GetInt("width") == 1440) { res_array_pointer = 10; }
        else if (PlayerPrefs.GetInt("width") == 1680) { res_array_pointer = 11; }
        else { res_array_pointer = res_array.Length - 1; }

        fullscreen_button.isOn = (PlayerPrefs.GetInt("fullscreen") == 1);
        vsync_button.isOn = (PlayerPrefs.GetInt("vsync") == 1);
        bilinear_filtering_button.isOn = (PlayerPrefs.GetInt("bilinear_filtering") == 1);
        bloom_button.isOn = (PlayerPrefs.GetInt("bloom") == 1);
        flares_button.isOn = (PlayerPrefs.GetInt("flares") == 1);

        fov_slider.value = PlayerPrefs.GetInt("fov");
        frame_cap_slider.value = PlayerPrefs.GetInt("frame_cap");
        pixelization_slider.value = PlayerPrefs.GetInt("pixelization");
        gamma_slider.value = PlayerPrefs.GetInt("gamma");
    }

    // Update is called once per frame
    void Update()
    {
        ResSet(res_array_pointer);

        PlayerPrefs.SetInt("height", (res_array[res_array_pointer].height));
        PlayerPrefs.SetInt("width", (res_array[res_array_pointer].width));

        res_text.text = (res_array[res_array_pointer].width) + " x " + (res_array[res_array_pointer].height);

        if (fullscreen_button.isOn)
        { PlayerPrefs.SetInt("fullscreen", 1); }
        else
        { PlayerPrefs.SetInt("fullscreen", 0); }

        if (vsync_button.isOn)
        { PlayerPrefs.SetInt("vsync", 1); }
        else
        { PlayerPrefs.SetInt("vsync", 0); }

        if (bilinear_filtering_button.isOn)
        { PlayerPrefs.SetInt("bilinear_filtering", 1); }
        else
        { PlayerPrefs.SetInt("bilinear_filtering", 0); }

        if (bloom_button.isOn)
        { PlayerPrefs.SetInt("bloom", 1); }
        else
        { PlayerPrefs.SetInt("bloom", 0); }

        if (flares_button.isOn)
        { PlayerPrefs.SetInt("flares", 1); }
        else
        { PlayerPrefs.SetInt("flares", 0); }

        PlayerPrefs.SetInt("fov", ((int)fov_slider.value));
        PlayerPrefs.SetInt("frame_cap", ((int)frame_cap_slider.value));
        PlayerPrefs.SetInt("pixelization", ((int)pixelization_slider.value));
        PlayerPrefs.SetInt("gamma", ((int)gamma_slider.value));
        
        fov_text.text = PlayerPrefs.GetInt("fov") + "";
        frame_cap_text.text = PlayerPrefs.GetInt("frame_cap") + "";
        pixelization_text.text = PlayerPrefs.GetInt("pixelization") + "";
        gamma_text.text = PlayerPrefs.GetInt("gamma") + "%";
    }

    public void Open_Options()
    {
        Open_Menu_Page(options_menu);
    }

    public void Res_Scroll_Down()
    {
        if (res_array_pointer > 0)
        {
            res_array_pointer -= 1;
        }
    }

    public void Res_Scroll_Up()
    {
        if (res_array_pointer < (res_array.Length - 1))
        {
            res_array_pointer += 1;
        }
    }

    public void ResSet(int index)
    {
        if ((res_array_pointer < (res_array.Length - 1)) && (res_array_pointer > 0))
        {
            res_array_pointer = index;
        }
    }

    public void Open_Menu_Page(GameObject page)
    {
        page.SetActive(true);
        graphics_menu.SetActive(false);
    }

    public class Resolution
    {
        public int width;
        public int height;

        public Resolution(int new_width, int new_height)
        {
            height = new_height;
            width = new_width;
        }
    }
}
