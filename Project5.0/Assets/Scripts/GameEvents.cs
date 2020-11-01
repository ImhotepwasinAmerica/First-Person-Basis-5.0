using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    public event Action DeleteAllTheThings, SaveAllTheThings, SaveAllTheThingsAux, SmartDelete, LoadCharacterStance, LoadCharacterRotation, Pause, Resume;
    public event Action<int> DoorwayOpen, DoorwayClose;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        if (Pause != null)
        {
            Pause();
        }
    }

    public void ResumeGame()
    {
        if (Resume != null)
        {
            Resume();
        }
    }

    public void DeleteAllItems()
    {
        if (DeleteAllTheThings != null)
        {
            DeleteAllTheThings();
        }
    }

    public void SaveAllItems()
    {
        if (SaveAllTheThings != null)
        {
            SaveAllTheThings();
        }
    }

    public void SaveAllItemsAux()
    {
        if (SaveAllTheThingsAux != null)
        {
            SaveAllTheThingsAux();
        }
    }

    public void OpenTheDoor(int num)
    {
        if(DoorwayOpen != null)
        {
            DoorwayOpen(num);
        }
    }

    public void CloseTheDoor(int num)
    {
        if (DoorwayClose != null)
        {
            DoorwayClose(num);
        }
    }

    public void DeleteSmartly()
    {
        if (SmartDelete != null)
        {
            Debug.Log("Got this far...");
            SmartDelete();
        }
    }

    public void LoadDaGuy()
    {
        if (LoadCharacterStance != null)
        {
            LoadCharacterStance();
        }
    }

    public void LoadDaRot()
    {
        if (LoadCharacterRotation !=null)
        {
            LoadCharacterRotation();
        }
    }
}
