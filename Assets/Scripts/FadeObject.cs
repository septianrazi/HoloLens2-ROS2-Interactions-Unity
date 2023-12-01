using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class FadeObject : MonoBehaviour
{
    public float waitTime = 2f; // Time to wait before fading starts

    [SerializeField] TextMeshPro text;
    [SerializeField] GameObject quipObject;

    private void Start()
    {
        quipObject.SetActive(false);
    }

    public void TextToForward()
    {
        text.text = "Forward";
        StartCoroutine(FadeOut());
    }
    public void TextToBackward()
    {
        text.text = "Backward";
        StartCoroutine(FadeOut());
    }

    public void TextToLeft()
    {
        text.text = "Left";
        StartCoroutine(FadeOut());
    }

    public void TextToRight()
    {
        text.text = "Right";
        StartCoroutine(FadeOut());
    }

    public void TextToStop()
    {
        text.text = "Stop";
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        quipObject.SetActive(true);
        yield return new WaitForSeconds(waitTime);


        for (float t = 0; t < waitTime; t += Time.deltaTime)
        {
            yield return null;
        }

        quipObject.SetActive(false);

    }
}
