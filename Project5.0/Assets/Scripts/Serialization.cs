using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class Serialization : MonoBehaviour
{
    public static void Save<T>(T thing, string path)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = File.Create(path);
        formatter.Serialize(stream, thing);
        stream.Close();
    }

    public static T Load<T>(string path)
    {
        if (File.Exists(path))
        {
            T return_value = default(T);
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = File.Open(path, FileMode.Open);
            return_value = (T)formatter.Deserialize(stream);
            stream.Close();

            return return_value;
        }
        else
        {
            return default(T); // Check if the returned object equals default(T); if so, count it as a failure
        }
    }

    public static bool SaveExists(string path)
    {
        return File.Exists(path);
    }

    public static bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

    public static void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    // A directory is deleted, and everything in it.
    public static void DeleteDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(path, false);
        }
    }

    // A single file is deleted.
    public static void DeleteFile(string path)
    {
        File.Delete(path);
    }

    // Eliminates all files and directories having to do with the game's saved data
    // Only to be used in emergencies
    public static void EmergencyPurge()
    {
        string path = Application.persistentDataPath + "/saves/";

        string[] files = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);

        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        foreach (string dir in dirs)
        {
            DeleteDirectory(dir);
        }

        Directory.Delete(path, false);
    }
}
