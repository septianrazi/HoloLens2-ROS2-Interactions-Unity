using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.Events;
using System.Text;

public class APIManager : MonoBehaviour
{
    static readonly HttpClient client = new HttpClient();

    public string robotArmURI = "";
    public string turtlebotURI = "";
    public string defaultServerURI = "http://192.168.100.3:5000/";

    private bool _toTurtlebotFlag = true;
    public bool ToTurtlebotFlag
    {
        get { return _toTurtlebotFlag; }
        set { _toTurtlebotFlag = value; }
    }


    [SerializeField] TextMeshPro textIndicator;

    public UnityEvent forwardTurtlebotMiscEvents;
    public UnityEvent backwardTurtlebotMiscEvents;
    public UnityEvent leftTurtlebotMiscEvents;
    public UnityEvent rightTurtlebotMiscEvents;
    public UnityEvent stopTurtlebotMiscEvents;





    // Start is called before the first frame update
    void Start()
    {
        if (turtlebotURI == null || turtlebotURI == ""){
            turtlebotURI = defaultServerURI;
        }
    }

    async Task PostAsync(string uri, StringContent content)
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
            //StringContent content = new StringContent("{\"key\":\"value\"}", Encoding.UTF8, "application/json");
            
            Debug.Log("trying");
            using HttpResponseMessage response = await client.PostAsync(uri, content);
            //using HttpResponseMessage response = await client.GetAsync(uri);
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

    public static string DegreesToRadianStrings(List<int> degrees)
    {
        List<double> radians = new List<double>();
        foreach (int degree in degrees)
        {
            double radian = (Math.PI / 180) * degree;
            radians.Add(radian);
        }
        return string.Join(" ", radians);
    }

    private void MoveRobotArm(List<int> degreesList)
    {
        string uri = robotArmURI + "move";
        string radians = DegreesToRadianStrings(degreesList);
        StringContent content = new StringContent("{\"angles\":\"" + radians + "\"}", Encoding.UTF8, "application/json");
        //Debug.Log("ROBOTARN");

        Debug.Log(content);
        PostAsync(uri, content);

    }


    [ContextMenu("Forward Turtlebot")]
    public void ForwardTurtlebot()
    {
        if (!ToTurtlebotFlag)
        {
            MoveRobotArm(new List<int> { 4, -90, 73, -166, -93, 358 }); return;
        }
        else
        {
            string uri = turtlebotURI + "move_forward";
            PostAsync(uri, null);
        }
        forwardTurtlebotMiscEvents.Invoke();
    }


    [ContextMenu("Backward Turtlebot")]
    public void BackwardTurtlebot()
    {
        if (!ToTurtlebotFlag)
        {
            MoveRobotArm(new List<int> { 87,-78,73,-176,-90,10 }); return;
        }
        else
        {
            string uri = turtlebotURI + "move_backward";
            PostAsync(uri, null);
        }
        backwardTurtlebotMiscEvents.Invoke();
    }

    [ContextMenu("Left Turtlebot")]
    public void TurnLeftTurtlebot()
    {
        if (!ToTurtlebotFlag)
        {
            MoveRobotArm(new List<int> { 0,-97,-80,-183,-91,2 }); return;
        }
        else
        {
            string uri = turtlebotURI + "turn_left";
            PostAsync(uri, null);
        }
        leftTurtlebotMiscEvents.Invoke();
    }

    [ContextMenu("Right Turtlebot")]
    public void TurnRightTurtlebot()
    {
        if (!ToTurtlebotFlag)
        {
            MoveRobotArm(new List<int> { 90,-90,-90,4,95,2 }); return;
        }
        else
        {
            string uri = turtlebotURI + "turn_right";
            PostAsync(uri, null);
        }
        rightTurtlebotMiscEvents.Invoke();
    }

    [ContextMenu("Stop Turtlebot")]
    public void StopTurtlebot()
    {
        string uri = turtlebotURI + "stop";
        PostAsync(uri, null);
        stopTurtlebotMiscEvents.Invoke();
    }
}
