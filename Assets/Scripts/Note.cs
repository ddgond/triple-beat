using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteInfo info;
    public float speed;

    public bool inGoodHitbox = false;
    public bool inPerfectHitbox = false;

    private HitManager hitManager;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
        if (transform.localPosition.y < -2)
        {
            Miss();
        }
    }

    public void Hit()
    {
        ScoreCounter scoreCounter = FindObjectOfType<ScoreCounter>();
        ComboCounter comboCounter = FindObjectOfType<ComboCounter>();
        if (hitManager != null)
        {
            hitManager.DeregisterNote(this);
        }
        if (inPerfectHitbox)
        {
            scoreCounter.AddToScore(CurrentSongInfo.pointsPerNote);
            ResultsInfo.perfectHits++;
            GetClosestSuccessIndicator().OnPerfect();
        }
        else if (inGoodHitbox)
        {
            scoreCounter.AddToScore(CurrentSongInfo.pointsPerNote * 0.5f);
            ResultsInfo.goodHits++;
            GetClosestSuccessIndicator().OnGood();
        }
        else
        {
            Miss();
            return;
        }
        comboCounter.IncrementCombo();
        enabled = false;
        AudioSource audioSource = GetComponentInChildren<AudioSource>();
        audioSource.gameObject.transform.SetParent(transform.parent);
        audioSource.Play();
        Destroy(audioSource.gameObject, 2f);
        Destroy(gameObject);
    }

    public void Miss()
    {
        GetClosestSuccessIndicator().OnMiss();
        ComboCounter comboCounter = FindObjectOfType<ComboCounter>();
        if (hitManager != null)
        {
            hitManager.DeregisterNote(this);
        }
        comboCounter.ResetCombo();
        ResultsInfo.misses++;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        hitManager = other.GetComponentInParent<HitManager>();
        if (other.name.Contains("Miss"))
        {
            hitManager.RegisterNote(this);
        }
        if (other.name.Contains("Good"))
        {
            inGoodHitbox = true; ;
        }
        if (other.name.Contains("Perfect"))
        {
            inPerfectHitbox = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Good"))
        {
            Miss();
        }
        if (other.name.Contains("Perfect"))
        {
            inPerfectHitbox = false;
        }
    }

    private SuccessIndicator GetClosestSuccessIndicator()
    {
        SuccessIndicator[] indicators = FindObjectsOfType<SuccessIndicator>();
        SuccessIndicator closestIndicator = indicators[0];
        foreach (SuccessIndicator indicator in indicators)
        {
            if ((indicator.transform.position - transform.position).magnitude < (closestIndicator.transform.position - transform.position).magnitude)
            {
                closestIndicator = indicator;
            }
        }
        return closestIndicator;
    }
}
