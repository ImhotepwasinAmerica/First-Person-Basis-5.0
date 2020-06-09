using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataContainer : MonoBehaviour
{
    public GameObject this_thing;

    //  When a game is loaded, these shall comprise the saved file location.
    //  "savedgames/saved_game_slot/saved_game_scene/..."
    public string saved_game_slot, saved_game_scene;

    public Game game;
    public Character character;
    public Scene scene;

    private GameObject[] alla_deeze;

    // Start is called before the first frame update
    void Start()
    {
        alla_deeze = GameObject.FindGameObjectsWithTag("DataContainer");

        for (int i = 0; i < alla_deeze.Length; i++)
        {
            if (alla_deeze[i] != this.gameObject)
            {
                // No variables yet
                //this.saved_game_slot = alla_deeze[i].GetComponent<DataContainer>().saved_game_slot;
                //this.saved_game_scene = alla_deeze[i].GetComponent<DataContainer>().saved_game_scene;
                //this.game = alla_deeze[i].GetComponent<DataContainer>().game;
                //this.character = alla_deeze[i].GetComponent<DataContainer>().character;
                //this.scene = alla_deeze[i].GetComponent<DataContainer>().scene;

                //Destroy(alla_deeze[i]);
            }
        }

        //Object.DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
