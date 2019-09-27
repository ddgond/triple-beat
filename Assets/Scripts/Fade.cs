using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float duration = 2f;
    public bool fadeOut = true;

    private MeshRenderer renderer;

    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        StartCoroutine(DoFade());
    }

    IEnumerator DoFade()
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            if (fadeOut)
            {
                renderer.material.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, (Time.time - startTime) / duration);
            }
            else
            {
                renderer.material.color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), (Time.time - startTime) / duration);
            }
            yield return null;
        }
        yield return null;
    }
}
