using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongInfo
{
    public string title;
    public string artist;
    public string mapper;
    public string[] difficulties;
    public string folder;
    public float bpm;
    public float startOffset;

    public SongInfo(string title, string artist, string mapper, string[] difficulties, string folder, float bpm, float startOffset)
    {
        this.title = title;
        this.artist = artist;
        this.mapper = mapper;
        this.difficulties = (string[]) difficulties.Clone();
        this.folder = folder;
        this.bpm = bpm;
        this.startOffset = startOffset;
    }
}
