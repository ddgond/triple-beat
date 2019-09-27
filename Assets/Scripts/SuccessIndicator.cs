using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessIndicator : MonoBehaviour
{
    private Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if (transform.localScale.y > 0.00001f)
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Max(transform.localScale.y - 0.2f * Time.deltaTime, 0f), transform.localScale.z);
        }
        mat.color = Color.Lerp(mat.color, Color.white, 0.1f);
    }

    public void OnPerfect()
    {
        mat.color = new Color(0,0,1,1);
        transform.localScale = new Vector3(6f, 0.2f, 1f);
    }

    public void OnGood()
    {
        mat.color = new Color(0, 1, 0, 1);
        transform.localScale = new Vector3(6f, 0.2f, 1f);
    }

    public void OnMiss()
    {
        mat.color = new Color(1, 0, 0, 1);
        transform.localScale = new Vector3(6f, 0.2f, 1f);
    }
}
