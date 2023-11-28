using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.Events;
using UnityEditor.PackageManager;
using System.Text;

public class APIManager : MonoBehaviour
{
    static readonly HttpClient client = new HttpClient();

    public static string robotArmURI = "";
    public static string turtlebotURI = "";

    public string defaultServerURI = "http://192.168.100.3:5000/";

    [SerializeField] TextMeshPro textIndicator;



    // Start is called before the first frame update
    void Start()
    {
        if (turtlebotURI == null || turtlebotURI == ""){
            turtlebotURI = defaultServerURI;
        }
    }

    async Task PostAsync(string uri)
    {
        //if (textIndicator != null)
        //{
        //    textIndicator.gameObject.SetActive(true);
        //    textIndicator.text = "Polling from " + uri;
        //}

        Debug.Log("Polling from " + uri);
        // Call asynchronous network methods in a try/catch block to handle exceptions.

        try
        {
            //var content = new StringContent("{\"key\":\"value\"}", Encoding.UTF8, "application/json");
            //using HttpResponseMessage response = await client.PostAsync(uri, content);
            Debug.Log("trying");

            using HttpResponseMessage response = await client.GetAsync(uri);
            Debug.Log(response.StatusCode);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);

            //textIndicator.gameObject.SetActive(false);

        }
        catch (HttpRequestException e)
        {
            textIndicator.text = "Exception " + e.InnerException.Message;
        }
    }

    [ContextMenu("Forward Turtlebot")]
    public void ForwardTurtlebot()
    {
        string uri = turtlebotURI + "move_forward";
        PostAsync(uri);
    }

    [ContextMenu("Backward Turtlebot")]
    public void BackwardTurtlebot()
    {
        string uri = turtlebotURI + "move_backward";
        PostAsync(uri);
    }

    [ContextMenu("Left Turtlebot")]
    public void TurnLeftTurtlebot()
    {
        string uri = turtlebotURI + "turn_left";
        PostAsync(uri);
    }

    [ContextMenu("Right Turtlebot")]
    public void TurnRightTurtlebot()
    {
        string uri = turtlebotURI + "turn_right";
        PostAsync(uri);
    }

    [ContextMenu("Stop Turtlebot")]
    public void StopTurtlebot()
    {
        string uri = turtlebotURI + "stop";
        PostAsync(uri);
    }
}
