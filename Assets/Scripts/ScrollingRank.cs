using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollingRank : MonoBehaviour
{
    public float scrollDuration = 1f;

    private TextMeshPro tmp;
    private Rank value;
    private Rank displayedValue;
    private float scrollStartValue = 0f;
    private float scrollStartTime = 0;

    void Start()
    {
        tmp = GetComponent<TextMeshPro>();
        StartCoroutine(ScrollRank());
    }

    private void Update()
    {
        tmp.text = displayedValue.ToString();
    }

    public void SetValue(Rank rank)
    {
        scrollStartValue = (float) value;
        value = rank;
        scrollStartTime = Time.time;
    }


    IEnumerator ScrollRank()
    {
        while (true)
        {
            displayedValue = (Rank) Mathf.Round(Mathf.Lerp(scrollStartValue, (float) value, (Time.time - scrollStartTime) / scrollDuration));
            yield return null;
        }
    }
}
