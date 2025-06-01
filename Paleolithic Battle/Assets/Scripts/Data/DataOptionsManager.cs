using UnityEngine;

public class DataOptionsManager
{
    private string filePath => Application.persistentDataPath + "/options.json";

    public void SaveOptions(OptionsData optionsData)
    {
        string json = JsonUtility.ToJson(optionsData, true);
        System.IO.File.WriteAllText(filePath, json);
        Debug.Log("Options saved to: " + filePath);
    }

    public OptionsData LoadOptions()
    {
        if (System.IO.File.Exists(filePath))
        {
            Debug.Log("Loading options from: " + filePath);
            string json = System.IO.File.ReadAllText(filePath);
            return JsonUtility.FromJson<OptionsData>(json);
        }
        return new OptionsData(); // Return default options if file does not exist
    }
}