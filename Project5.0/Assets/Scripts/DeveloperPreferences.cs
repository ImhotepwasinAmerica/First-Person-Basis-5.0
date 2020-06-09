using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperPreferences : MonoBehaviour
{
    public static void Keybinds()
    {
        PlayerPrefs.SetString("General Action","E");
        PlayerPrefs.SetString("Toolbelt One", "Alpha1");
        PlayerPrefs.SetString("Toolbelt Two", "Alpha2");
        PlayerPrefs.SetString("Toolbelt Three", "Alpha3");
        PlayerPrefs.SetString("Toolbelt Four", "Alpha4");
        PlayerPrefs.SetString("Toolbelt Five", "Alpha5");
        PlayerPrefs.SetString("Toolbelt Six", "Alpha6");
        PlayerPrefs.SetString("Toolbelt Seven", "Alpha7");
        PlayerPrefs.SetString("Toolbelt Eight", "Alpha8");
        PlayerPrefs.SetString("Toolbelt Nine", "Alpha9");
        PlayerPrefs.SetString("Toolbelt Ten", "Alpha0");
        PlayerPrefs.SetString("Primary Attack", "Mouse0");
        PlayerPrefs.SetString("Secondary Attack", "Mouse1");
        PlayerPrefs.SetString("Tertiary Attack", "Mouse2");
        PlayerPrefs.SetString("Kick", "F");
        PlayerPrefs.SetString("Block", "C");
        PlayerPrefs.SetString("Move Forward", "W");
        PlayerPrefs.SetString("Move Backward", "S");
        PlayerPrefs.SetString("Move Left", "A");
        PlayerPrefs.SetString("Move Right", "D");
        PlayerPrefs.SetString("Jump", "Space");
        PlayerPrefs.SetString("Squat", "LeftControl");
        PlayerPrefs.SetString("Speed Toggle", "LeftShift");
        PlayerPrefs.SetString("Toolbelt Scroll Up", "MouseWheelUp");
        PlayerPrefs.SetString("Toolbelt Scroll Down", "MouseWheelDown");
        PlayerPrefs.SetString("Drop", "Q");
        PlayerPrefs.SetString("Quicksave", "KeypadPlus");
        PlayerPrefs.SetString("Quickload", "KeypadMinus");
        PlayerPrefs.SetString("Pause", "Escape");
        PlayerPrefs.SetString("Log", "L");
        PlayerPrefs.SetString("Objectives", "O");
        PlayerPrefs.SetString("Stats", "P");
        PlayerPrefs.SetString("Gamma Up", "Equals");
        PlayerPrefs.SetString("Gamma Down", "Minus");
        PlayerPrefs.SetString("Item Rotate", "G");
        PlayerPrefs.SetString("Lean Left", "Z");
        PlayerPrefs.SetString("Lean Right","X");
    }

    public static void Graphics()
    {
        PlayerPrefs.SetInt("width",1920);
        PlayerPrefs.SetInt("height",1080);
        PlayerPrefs.SetInt("fullscreen",1);
        PlayerPrefs.SetInt("vsync",0);
        PlayerPrefs.SetInt("bilinear_filtering",0);
        PlayerPrefs.SetInt("bloom",0);
        PlayerPrefs.SetInt("flares",1);
        PlayerPrefs.SetInt("fov",110);
        PlayerPrefs.SetInt("frame_cap",120);
        PlayerPrefs.SetInt("pixelization",1);
        PlayerPrefs.SetInt("gamma",100);
    }

    public static void Sound()
    {
        PlayerPrefs.SetInt("sound_universal",100);
        PlayerPrefs.SetInt("sound_music",100);
        PlayerPrefs.SetInt("sound_dialogue",100);
        PlayerPrefs.SetInt("sound_environment",100);
    }

    public static void Gameplay()
    {
        PlayerPrefs.SetString("togglehold_carry", "toggle");
        PlayerPrefs.SetString("togglehold_squat", "hold");
        PlayerPrefs.SetInt("enemy_health_percentage", 100);
        PlayerPrefs.SetInt("enemy_damage_percentage",100);
    }
}
