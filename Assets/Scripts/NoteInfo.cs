using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteInfo
{
    public NoteColor color;
    public float hitTime;
    public bool spawned = false;
    public bool hitOrMissed = false;

    public NoteInfo(NoteColor color, float hitTime)
    {
        this.color = color;
        this.hitTime = hitTime;
    }

    public bool IsReadyToSpawn(float spawnToHitTimeDelta, float songProgress)
    {
        float spawnTime = hitTime - spawnToHitTimeDelta;
        return songProgress > spawnTime && songProgress < hitTime;
    }
}
