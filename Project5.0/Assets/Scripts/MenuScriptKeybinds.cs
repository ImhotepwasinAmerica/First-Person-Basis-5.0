using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MenuScriptKeybinds : MonoBehaviour
{
    public GameObject keybindings_menu, options_menu, data_container, content;

    private GameObject current_button;

    public string[] action_array;

    private string current_action, current_key;

    //public Text move_forward, move_backward, move_left, move_right,
    //    jump, squat, lean_left, lean_right, speed_toggle, action_general,
    //    attack_primary, attack_secondary, attack_tertiary, kick, block,
    //    toolbelt_one, toolbelt_two, toolbelt_three, toolbelt_four, toolbelt_five,
    //    toolbelt_six, toolbelt_seven, toolbelt_eight, toolbelt_nine, toolbelt_ten,
    //    toolbelt_scroll_up, toolbelt_scroll_down, drop,
    //    quicksave, quickload, pause, log, objectives, stats,
    //    gamma_up, gamma_down, item_rotate;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("General Action"))
        {
            DeveloperPreferences.Keybinds();
        }

        //action_general.text = PlayerPrefs.GetString("General Action");
        //toolbelt_one.text = PlayerPrefs.GetString("Toolbelt One");
        //toolbelt_two.text = PlayerPrefs.GetString("Toolbelt Two");
        //toolbelt_three.text = PlayerPrefs.GetString("Toolbelt Three");
        //toolbelt_four.text = PlayerPrefs.GetString("Toolbelt Four");
        //toolbelt_five.text = PlayerPrefs.GetString("Toolbelt Five");
        //toolbelt_six.text = PlayerPrefs.GetString("Toolbelt Six");
        //toolbelt_seven.text = PlayerPrefs.GetString("Toolbelt Seven");
        //toolbelt_eight.text = PlayerPrefs.GetString("Toolbelt Eight");
        //toolbelt_nine.text = PlayerPrefs.GetString("Toolbelt Nine");
        //toolbelt_ten.text = PlayerPrefs.GetString("Toolbelt Ten");
        //attack_primary.text = PlayerPrefs.GetString("Primary Attack");
        //attack_secondary.text = PlayerPrefs.GetString("Secondary Attack");
        //attack_tertiary.text = PlayerPrefs.GetString("Tertiary Attack");
        //kick.text = PlayerPrefs.GetString("Kick");
        //block.text = PlayerPrefs.GetString("Block");
        //move_forward.text = PlayerPrefs.GetString("Move Forward");
        //move_backward.text = PlayerPrefs.GetString("Move Backward");
        //move_left.text = PlayerPrefs.GetString("Move Left");
        //move_right.text = PlayerPrefs.GetString("Move Right");
        //jump.text = PlayerPrefs.GetString("Jump");
        //squat.text = PlayerPrefs.GetString("Squat");
        //speed_toggle.text = PlayerPrefs.GetString("Speed Toggle");
        //toolbelt_scroll_up.text = PlayerPrefs.GetString("Toolbelt Scroll Up");
        //toolbelt_scroll_down.text = PlayerPrefs.GetString("Toolbelt Scroll Down");
        //drop.text = PlayerPrefs.GetString("Drop");
        //quicksave.text = PlayerPrefs.GetString("Quicksave");
        //quickload.text = PlayerPrefs.GetString("Quickload");
        //pause.text = PlayerPrefs.GetString("Pause");
        //log.text = PlayerPrefs.GetString("Log");
        //objectives.text = PlayerPrefs.GetString("Objectives");
        //stats.text = PlayerPrefs.GetString("Stats");
        //gamma_up.text = PlayerPrefs.GetString("Gamma Up");
        //gamma_down.text = PlayerPrefs.GetString("Gamma Down");
        //item_rotate.text = PlayerPrefs.GetString("Item Rotate");
        //lean_left.text = PlayerPrefs.GetString("Lean Left");
        //lean_right.text = PlayerPrefs.GetString("Lean Right");

        action_array = new string[] { "General Action", "Toolbelt One", "Toolbelt Two", "Toolbelt Three", "Toolbelt Four", "Toolbelt Five", "Toolbelt Six", "Toolbelt Seven",
            "Toolbelt Eight", "Toolbelt Nine","Toolbelt Ten", "Primary Attack", "Secondary Attack", "Tertiary Attack", "Kick", "Block", "Move Forward", "Move Backward", "Move Left", "Move Right",
            "Jump", "Squat", "Speed Toggle", "Toolbelt Scroll Up", "Toolbelt Scroll Down", "Drop", "Quicksave", "Quickload", "Pause", "Log", "Objectives", "Stats", "Gamma Up", "Gamma Down",
            "Item Rotate", "Lean Left", "Lean Right", "Look Up","Look Down","Look Left","Look Right"};

        float y_position = 0;

        foreach (string action in action_array)
        {
            GameObject keybinder = Instantiate(Resources.Load<GameObject>("Keybind Template"), content.GetComponent<RectTransform>().position, new Quaternion(0, 0, 0, 0));

            keybinder.transform.parent = content.transform;

            keybinder.GetComponent<RectTransform>().position += new Vector3(241.5f, -21.65f - y_position, 0);

            y_position += 50.0f;

            keybinder.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = action;

            keybinder.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString(action);

            keybinder.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate () { DoBoth(action, keybinder.transform.GetChild(1).gameObject); });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if (current_button != null) // set when a button is clicked
        {
            Event e = Event.current;
            if (e.isKey)
            {
                KeyEntry(e.keyCode.ToString());
            }
            else if (e.shift)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    KeyEntry(KeyCode.LeftShift.ToString());
                }
                else if (Input.GetKey(KeyCode.RightShift))
                {
                    KeyEntry(KeyCode.RightShift.ToString());
                }
            }
            else if (e.isMouse)
            {
                if (e.button == 0)
                {
                    KeyEntry(KeyCode.Mouse0.ToString());
                }
                else if (e.button == 1)
                {
                    KeyEntry(KeyCode.Mouse1.ToString());
                }
                else if (e.button == 2)
                {
                    KeyEntry(KeyCode.Mouse2.ToString());
                }
            }
            else if (e.isScrollWheel)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    KeyEntry("MouseWheelUp");
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    KeyEntry("MouseWheelDown");
                }
            }
        }
    }

    public void KeyEntry(string entry)
    {
        current_button.transform.GetChild(0).GetComponent<Text>().text = entry;
        current_button = null;

        PlayerPrefs.SetString(current_action,entry);
    }

    public void ChangeKey(GameObject clicked)
    {
        current_button = clicked;
    }

    public void SetCurrentAction(string action)
    {
        current_action = action;
    }

    public void DoBoth(string action, GameObject clicked)
    {
        ChangeKey(clicked);
        SetCurrentAction(action);

    }

    public void Open_Menu_Page(GameObject page)
    {
        page.SetActive(true);
        keybindings_menu.SetActive(false);
    }

    public void Open_Options()
    {
        Open_Menu_Page(options_menu);
    }
}
