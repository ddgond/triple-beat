using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollingNumber : MonoBehaviour
{
    public float scrollDuration = 1f;

    private TextMeshPro tmp;
    private int value = 0;
    private int displayedValue = 0;
    private int scrollStartValue = 0;
    private float scrollStartTime = 0;

    void Start()
    {
        tmp = GetComponent<TextMeshPro>();
        StartCoroutine(ScrollNumber());
    }

    void Update()
    {
        tmp.text = displayedValue.ToString();
    }

    public void SetValue(int newValue)
    {
        scrollStartValue = value;
        value = newValue;
        scrollStartTime = Time.time;
    }

    IEnumerator ScrollNumber()
    {
        while (true)
        {
            displayedValue = (int) Mathf.Round(Mathf.Lerp(scrollStartValue, value, (Time.time - scrollStartTime) / scrollDuration));
            yield return null;
        }
    }
}
