using System;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

public class DataHandler
{
    //Save Data
    public static void saveData(string dataToSave, string dataFileName)
    {
        // take application data path
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName);
        
        
        //Convert To Json then to bytes
        string jsonData = JsonConvert.SerializeObject(dataToSave);
        byte[] jsonByte = Encoding.ASCII.GetBytes(jsonData);

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
        }
        //Debug.Log(path);

        try
        {
            File.WriteAllBytes(tempPath, jsonByte);
            //Debug.Log("Saved Data to: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To PlayerInfo Data to: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }

    //Load Data
    public static string loadData(string dataFileName)
    {
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName);
        tempPath = tempPath.Replace("/", "\\");
        
        //Exit if Directory or File does not exist after creating it
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            //Debug.Log($"{tempPath}");
            Debug.LogWarning("Directory does not exist");
            Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
            return null;
        }

        if (!File.Exists(tempPath))
        {
            //Debug.Log($"{tempPath}");
            //Debug.Log("File does not exist");
            return null;
        }

        //Load saved Json
        byte[] jsonByte = null;
        try
        {
            jsonByte = File.ReadAllBytes(tempPath);
            //Debug.Log("Loaded Data from: " + tempPath.Replace("/", "\\"));
            //Debug.Log(jsonByte);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Data from: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        //Convert to json string
        string jsonData = Encoding.ASCII.GetString(jsonByte);
        jsonData = jsonData.Replace("\\", "");
        jsonData = jsonData.Remove(0,1);
        jsonData = jsonData.Remove(jsonData.Length-1,1);
        //Debug.Log(jsonData);

        //Convert to Object
        return jsonData;
    }

    public static bool deleteData(string dataFileName)
    {
        bool success = false;

        //Load Data
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName);
        tempPath = tempPath.Replace("/", "\\");

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            //Debug.LogWarning("delete cant work : Directory");
            return false;
        }

        Debug.Log($"{tempPath}");
        if (!File.Exists(tempPath))
        {
            //Debug.Log("delete cant work : File");
            return false;
        }

        try
        {
            File.Delete(tempPath);
            //Debug.Log("Data deleted from: " + tempPath.Replace("/", "\\"));
            success = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Delete Data: " + e.Message);
        }

        return success;
    }
}