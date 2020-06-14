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

    private void Awake()
    {
        GameEvents.current.SmartDelete += DestroyOrChange; // This must run before DeleteSmartly is called
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.DeleteAllTheThings += Destroy;
        GameEvents.current.SaveAllTheThings += SaveItem;

        id = this.gameObject.transform.position.sqrMagnitude;

        is_original = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.gameObject != null
            && data_container != null
            && object_data != null)
        {
            RecordRotation();
            RecordPosition();
        }

        MoveAugment();
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
        object_data.health += hit_strength;
    }

    public void Destroy()
    {
        GameObject.Destroy(object_in_question);
    }

    public void SaveItem()
    {
        Debug.Log("Saving this item");

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

    public void DestroyOrChange()
    {
        if (Serialization.SaveExists(
            Application.persistentDataPath + "/saves/savedgames/"
            + PlayerPrefs.GetString("saved_game_slot")
            + "/" + SceneManager.GetActiveScene().name
            + "/presentitems/" + this.gameObject.transform.position.sqrMagnitude + ".dat"))
        {
            Debug.Log("Objectchanged");

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
            Debug.Log(SceneManager.GetActiveScene().name);

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
}
