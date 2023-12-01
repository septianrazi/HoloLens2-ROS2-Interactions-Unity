using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeText : MonoBehaviour
{

    [SerializeField] TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        if (text == null)
            text = GetComponent<TextMeshPro>();
    }

    public void ShowStop()
    {
        text.text = "Stop";
    }

    public void ShowForward()
    {
        text.text = "Forward";
    }
}
