using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universals : MonoBehaviour
{
    //public static void Teleport_V1(GameObject thing, Vector3 new_position, Vector3 new_rotation)
    //{
    //    if (thing.TryGetComponent<CharacterController>(out CharacterController character_controller))
    //    {

    //    }
    //    else
    //    {
    //        thing.transform.rotation = Quaternion.Euler(new_rotation);

    //        thing.transform.position = new_position;
    //    }
    //}

    public static void RememberImportantThings()
    {
        GameObject data_container = GameObject.FindGameObjectWithTag("DataContainer");
    }

    public static GameObject GetDataContainer()
    {
        return GameObject.FindGameObjectWithTag("DataContainer");
    }

    public static GameObject FindChild(GameObject parent, string name)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).gameObject.name == name)
            {
                return parent.transform.GetChild(i).gameObject;
            }
            else
            {
                return FindChild(parent.transform.GetChild(i).gameObject, name);
            }
        }

        return null;
    }

    public static GameObject FindChildByTag(GameObject parent, string tag)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).gameObject.tag == tag)
            {
                return parent.transform.GetChild(i).gameObject;
            }
            else
            {
                return FindChild(parent.transform.GetChild(i).gameObject, tag);
            }
        }

        return null;
    }

    public static int DiceRoll(int number_of_dice, int dice_level)
    {
        int result = 0;

        for (int i = 0; i < number_of_dice; i++)
        {
            result += Random.Range(1, dice_level + 1);
        }

        return result;
    }

    public static int GetModifier(int stat)
    {
        switch (stat)
        {
            case 3:
                return -3;
                break;
            case 4:
                return -2;
                break;
            case 5:
                return -2;
                break;
            case 6:
                return -1;
                break;
            case 7:
                return -1;
                break;
            case 8:
                return -1;
                break;
            case 13:
                return 1;
                break;
            case 14:
                return 1;
                break;
            case 15:
                return 1;
                break;
            case 16:
                return 2;
                break;
            case 17:
                return 2;
                break;
            case 18:
                return 3;
                break;
            default:
                return 0;
                break;
        }
    }

    public static void BuildCharacter(GameObject body, Camera camera)
    {
        GameObject camera_anchor_this = Universals.FindChild(body, "CameraAnchor");

        camera = Instantiate(camera, camera_anchor_this.transform.position, camera_anchor_this.transform.rotation);

        camera.transform.parent = camera_anchor_this.transform;
    }

    public static float LerpBetter(float starting_point, float end_point, float rate)
    {
        if (starting_point < end_point)
        {
            if((end_point - starting_point) < rate)
            {
                return starting_point + (end_point - starting_point);
            }
            else
            {
                return starting_point + rate;
            }
        }
        else if (starting_point > end_point)
        {
            if ((starting_point - end_point) < rate)
            {
                return starting_point - (starting_point - end_point);
            }
            else
            {
                return starting_point - rate;
            }
        }
        else
        {
            return end_point;
        }
    }

    public static void SetTargetFramerate(int rate)
    {
        Application.targetFrameRate = rate;
    }

    public static float GetNPCSensitivity()
    {
        return 10;
    }

    public static float GetSmoothing()
    {
        return 4;
    }

    public static float GetAngularSpeed(float speed)
    {
        return Mathf.Sqrt((speed * speed / 2.0f));
    }

    public static float GetTimeFake()
    {
        return 0.000966f;
    }

    public static float GetGravityFake()
    {
        return Physics.gravity.y * Universals.GetTimeFake();
    }

    public static float GetCharacterMaxSpeed()
    {
        return 100f;
    }

    public static float Pythagoras(float a, float b)
    {
        return Mathf.Sqrt((a * a) + (b * b));
    }
}
