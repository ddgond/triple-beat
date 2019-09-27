using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyInAnimation : MonoBehaviour
{
    public float duration;
    public float driftDistance;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FlyIn());
    }

    // Update is called once per frame
    IEnumerator FlyIn()
    {
        float startTime = Time.time;
        transform.localPosition += Vector3.right * 20f;

        while (Time.time - startTime < duration)
        {
            if (Time.time - startTime < duration / 4f)
            {
                transform.localPosition = Vector3.Lerp(Vector3.right * 20f, Vector3.right * driftDistance / 2, (Time.time - startTime) / (duration / 4f));
            }
            if (Time.time - startTime >= duration / 4f && Time.time - startTime <= 3 * duration / 4f)
            {
                transform.localPosition = Vector3.Lerp(Vector3.right * driftDistance / 2, Vector3.left * driftDistance / 2, (Time.time - startTime - duration / 4f) / (duration / 2f));
            }
            if (Time.time - startTime > 3 * duration / 4f)
            {
                transform.localPosition = Vector3.Lerp(Vector3.left * driftDistance / 2, Vector3.left * 20f, (Time.time - startTime - 3 * duration / 4f) / (duration / 4f));
            }
            yield return null;
        }
        Destroy(gameObject);
    }
}
