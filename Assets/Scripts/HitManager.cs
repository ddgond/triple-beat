using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    public KeyCode key;
    public GameObject targetTriangle;
    public Color hitColor;

    private IEnumerator lerpCoroutine;
    private List<Note> hittableNotes = new List<Note>();

    public void Start()
    {
        //TODO: Scale hitbox size with note speed
        float scaleFactor = 2f / CurrentSongInfo.spawnToHitTimeDelta;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * scaleFactor, transform.localScale.z);
    }

    public void Update()
    {
        if (Input.GetKeyDown(key) && hittableNotes.Count > 0)
        {
            hittableNotes[0].Hit();
        }

        if (Input.GetKey(key))
        {
            if (lerpCoroutine != null)
            {
                StopCoroutine(lerpCoroutine);
            }
            lerpCoroutine = LerpTargetTriangleColor();
            StartCoroutine(lerpCoroutine);
        }
    }

    public void RegisterNote(Note note)
    {
        hittableNotes.Add(note);
    }

    public void DeregisterNote(Note note)
    {
        hittableNotes.Remove(note);
    }

    IEnumerator LerpTargetTriangleColor()
    {
        for (float i = 0; i <= 30; i++)
        {
            targetTriangle.GetComponent<MeshRenderer>().material.color = Color.Lerp(hitColor, Color.white, i/30);
            yield return null;
        }
    }
}
