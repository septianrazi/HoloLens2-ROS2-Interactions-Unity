using System;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// Config file allowing us to change parameters of componenets within the app without having to rebuild or redeploy
/// </summary>
public class ConfigFile : MonoBehaviour
{
    [SerializeField] bool enable = true;    // whether we allow the config file to be read and established
    [SerializeField] APIManager apiManager;
    private async void Awake()
    {
        if (!enable) return;

        if (apiManager == null)
            if (!TryGetComponent<APIManager>(out apiManager))
                apiManager = GameObject.Find("APIManager").GetComponent<APIManager>();

        // If config available, read, otherwise use this default config file
        var config = await ReadOrInitConfig(new Config
        {
             robotArmURI= "http://192.168.100.3:5000/",
             turtlebotURI = "http://192.168.100.3:5000/",
             defaultServerURI = "http://192.168.100.3:5000/",
        });

        //Apply Config file to components
        apiManager.robotArmURI = config.robotArmURI;
        apiManager.turtlebotURI = config.turtlebotURI;
        apiManager.defaultServerURI = config.defaultServerURI;
    }

    static async Task<Config> ReadOrInitConfig(Config defaultConfig)
    {
        try
        {
            var configFileRaw = await System.IO.File.ReadAllTextAsync($"{Application.persistentDataPath}/config.json");
            return JsonUtility.FromJson<Config>(configFileRaw);
        }
        catch
        {
            var defaultConfigRaw = JsonUtility.ToJson(defaultConfig, true);
            await System.IO.File.WriteAllTextAsync($"{Application.persistentDataPath}/config.json", defaultConfigRaw);
            return defaultConfig;
        }
    }
}

/// <summary>
/// Config file contents
/// </summary>
[Serializable]
public class Config : ISerializationCallbackReceiver
{
    public string robotArmURI;
    public string turtlebotURI;
    public string defaultServerURI;
   
    public void OnBeforeSerialize()
    {
        return;
    }

    public void OnAfterDeserialize()
    {
        return;
    }
}
