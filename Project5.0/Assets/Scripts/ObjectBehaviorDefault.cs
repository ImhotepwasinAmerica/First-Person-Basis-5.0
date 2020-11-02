using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectBehaviorDefault : MonoBehaviour
{
    public GameObject object_in_question, data_container;
    public SavedObject object_data;
    public bool has_been_interacted, is_original;
    public float id;

/*
 * ObjectBehaviorDefault
 * Author:          Andrew Potisk
 * Finalized on:    --/--/----
 * 
 * Purpose:
 * This script defines the functions that are common to all objects in the entire game, 
 * such as saving and loading procedures as it concerns them.
 * 
 * Notes:
 * 
 * Bugs:
 * If the GameEvents script is not executed before this one, the game loading system having to do with DestroyOrChange() will not work.
*/
    private void Awake()
    {
        try // Make sure the GameEvents script is placed earlier in the script execution order than this.
        {
            Debug.Log("Is this called before DeleteSmartly, or after?");
            GameEvents.current.SmartDelete += DestroyOrChange; // This must run before DeleteSmartly is called
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log("Event system not found.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        GameEvents.current.DeleteAllTheThings += Destroy;
        GameEvents.current.SaveAllTheThings += SaveItem;
        GameEvents.current.SaveAllTheThingsAux += SaveItemAux;

        id = this.gameObject.transform.position.sqrMagnitude;

        is_original = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        if (this.gameObject != null
            && data_container != null
            && object_data != null)
        {
            RecordRotation();
            RecordPosition();
        }

        MoveAugment();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            OnCollideWithPlayer(collision.gameObject);
        }

        OnCollideWithAnything(collision.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnCollideWithPlayer(other.gameObject);
        }
    }

    public void SetObjectData(SavedObject new_object_data)
    {
        object_data = new_object_data;
    }

    public SavedObject GetObjectData()
    {
        return object_data;
    }

    public void RecordRotation()
    {
        object_data.rotation_x = this.gameObject.transform.rotation.x;
        object_data.rotation_y = this.gameObject.transform.rotation.y;
        object_data.rotation_z = this.gameObject.transform.rotation.z;
    }

    public void RecordPosition()
    {
        object_data.position_x = this.gameObject.transform.position.x;
        object_data.position_y = this.gameObject.transform.position.y;
        object_data.position_z = this.gameObject.transform.position.z;
    }

    public void SpawnProceedings()
    {
        object_data = new SavedObject();
    }

    public void HealthChange(int hit_strength)
    {
        object_data.ints[0] += hit_strength;
    }

    public void Destroy()
    {
        GameObject.Destroy(object_in_question);
    }

    // The item's data (represented as 'object_data') is saved, either to the 'items' or 'presentitems' folder
    public void SaveItem()
    {
        if (is_original)
        {
            Serialization.Save<SavedObject>(object_data,
                Application.persistentDataPath + "/saves/savedgames/"
                + PlayerPrefs.GetString("saved_game_slot")
                + "/" + SceneManager.GetActiveScene().name
                + "/presentitems/" + this.id + ".dat"); // The instance ID serves as the name of the object data file in memory
        }
        else
        {
            Serialization.Save<SavedObject>(object_data,
                Application.persistentDataPath + "/saves/savedgames/"
                + PlayerPrefs.GetString("saved_game_slot")
                + "/" + SceneManager.GetActiveScene().name
                + "/items/" + this.id + ".dat");
        }
    }

    public void SaveItemAux()
    {
        if (is_original)
        {
            Serialization.Save<SavedObject>(object_data,
                Application.persistentDataPath + "/saves/savedgames/auxiliary/" 
                + SceneManager.GetActiveScene().name
                + "/presentitems/" + this.id + ".dat"); // The instance ID serves as the name of the object data file in memory
        }
        else
        {
            Serialization.Save<SavedObject>(object_data,
                Application.persistentDataPath + "/saves/savedgames/auxiliary/" 
                + SceneManager.GetActiveScene().name
                + "/items/" + this.id + ".dat");
        }

        Debug.Log("Item saved: " + this.gameObject.transform.position.sqrMagnitude);
    }

    public void DestroyOrChange()
    {
        Debug.Log("Item affected: " + this.gameObject.transform.position.sqrMagnitude);

        if (Serialization.SaveExists(
            Application.persistentDataPath + "/saves/savedgames/"
            + PlayerPrefs.GetString("saved_game_slot")
            + "/" + SceneManager.GetActiveScene().name
            + "/presentitems/" + this.gameObject.transform.position.sqrMagnitude + ".dat"))
        {
            object_data = Serialization.Load<SavedObject>(Application.persistentDataPath + "/saves/savedgames/"
            + PlayerPrefs.GetString("saved_game_slot")
            + "/" + SceneManager.GetActiveScene().name
            + "/presentitems/" + this.gameObject.transform.position.sqrMagnitude + ".dat");

            this.gameObject.transform.rotation = Quaternion.Euler(
                object_data.rotation_x,
                object_data.rotation_y,
                object_data.rotation_z);

            this.gameObject.transform.position = new Vector3(
                object_data.position_x,
                object_data.position_y,
                object_data.position_z);
        }
        else
        {
            GameObject.Destroy(object_in_question);
        }

        
    }

    public virtual void UseDefault(GameObject thing) { }

    public virtual void UseDefault() { }

    public virtual void UseDefaultHold(GameObject thing) { }

    public virtual void UseDefaultHold() { }

    public virtual void UseDefaultHoldRelease() { }

    public virtual void MoveAugment() { }

    public virtual void MakeVirtual() { }

    public virtual void InstantiateObjectData()
    {
        if (this.object_data == null)
        {
            this.object_data = new SavedObject();
        }

        if (this.object_data.ints == null)
        {
            this.object_data.ints = new List<int>();
        }

        if (this.object_data.floats == null)
        {
            this.object_data.floats = new List<float>();
        }

        if (this.object_data.strings == null)
        {
            this.object_data.strings = new List<string>();
        }

        if (this.object_data.bools == null)
        {
            this.object_data.bools = new List<bool>();
        }

        if (this.object_data.objects == null)
        {
            this.object_data.objects = new List<SavedObject>();
        }
    }

    public virtual void OnCollideWithPlayer(GameObject collided) { }

    public virtual void OnCollideWithAnything(GameObject collided) { }
}
