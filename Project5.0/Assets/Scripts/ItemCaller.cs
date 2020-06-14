using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ItemCaller : MonoBehaviour
{
    public GameObject data_container;

    // Start is called before the first frame update
    void Start()
    {
        
        if (Serialization.DirectoryExists(Application.persistentDataPath + "/saves/savedgames/"
            + data_container.GetComponent<DataContainer>().saved_game_slot
            + "/" + SceneManager.GetActiveScene().name))
        {
            GameEvents.current.DeleteSmartly();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
