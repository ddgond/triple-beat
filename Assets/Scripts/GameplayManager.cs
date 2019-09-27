using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public GameObject notePrefab;
    public Material redNoteMat;
    public Material greenNoteMat;
    public Material blueNoteMat;
    public GameObject beatMarkerPrefab;
    public Material redMarkerMat;
    public Material greenMarkerMat;
    public Material blueMarkerMat;

    public GameObject topGameObject;
    public GameObject leftGameObject;
    public GameObject rightGameObject;
    public GameObject areYouReadyParent;
    public GameObject areYouReadyPrefab;
    public GameObject fadePrefab;
    public Transform fadeSpawn;

    public AudioSource mainAudioSource;

    private float noteSpeed;
    private float spawnToHitDistance = 8.5f; // Effectively a constant
    private float traingleHitDistance = 1.06215f; // Also a constant
    private float beatsSpawned = -32;
    private float spawnToHitTimeDelta = 5.5f; // Effectively time notes coming from left and right are on-screen (roughly 5/8 for top), NJS
    private AudioClip songClip;
    private float songStartTime;
    private bool loadingSong = false;
    private List<NoteInfo> unspawnedNoteInfos = new List<NoteInfo>();
    private List<NoteInfo> spawnedNoteInfos = new List<NoteInfo>();

    void Start()
    {
        ResultsInfo.Clear();

        Fade fadeIn = Instantiate(fadePrefab, fadeSpawn).GetComponent<Fade>();
        fadeIn.duration = 0.5f;
        fadeIn.fadeOut = false;

        unspawnedNoteInfos = new List<NoteInfo>(CurrentSongInfo.noteInfos);
        spawnToHitTimeDelta = CurrentSongInfo.spawnToHitTimeDelta;
        noteSpeed = spawnToHitDistance / spawnToHitTimeDelta;
        StartCoroutine(PlaySong());
    }

    IEnumerator QueueEndLoadingSongFlag(float duration)
    {
        yield return new WaitForSeconds(duration);
        loadingSong = false;
    }

    // Handles spawning notes and beat markers
    IEnumerator PlaySong()
    {
        // Song loading first
        loadingSong = true;
        string audioClipPath = "File://" + Path.Combine(Application.streamingAssetsPath, "CustomSongs", CurrentSongInfo.songFolder, "song.ogg");
        UnityWebRequest unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(audioClipPath, AudioType.OGGVORBIS);
        yield return unityWebRequest.SendWebRequest();
        songClip = DownloadHandlerAudioClip.GetContent(unityWebRequest);
        mainAudioSource.clip = songClip;

        // Are you ready?
        float ayrDuration = 4;
        FlyInAnimation ayr = Instantiate(areYouReadyPrefab, areYouReadyParent.transform).GetComponent<FlyInAnimation>();
        ayr.duration = ayrDuration;
        yield return new WaitForSeconds(ayrDuration); // Pauses song until Are You Ready phase ends

        // Preparing synchronized song start
        float songStartDelay = BeatsInTime(4);
        mainAudioSource.PlayDelayed(songStartDelay);
        mainAudioSource.time = CurrentSongInfo.songInfo.startOffset;
        songStartTime = Time.time + songStartDelay;
        StartCoroutine(QueueEndLoadingSongFlag(songStartDelay + 1)); // Adding 1 to this to avoid weird overlap issues, this flag is needed to be active until at least when the song starts playing

        // Beat marker spawning
        float markerSpeed = noteSpeed;
        while (beatsSpawned < TimeInBeats(mainAudioSource.clip.length))
        {
            float timeToHit = songStartTime + BeatsInTime(beatsSpawned);
            float spawnDistance = markerSpeed * (timeToHit - Time.time);
            BeatMarker marker = Instantiate(beatMarkerPrefab, leftGameObject.transform).GetComponent<BeatMarker>();
            marker.speed = markerSpeed;
            marker.transform.position += Vector3.up * (spawnDistance + traingleHitDistance);
            marker.GetComponent<MeshRenderer>().material = redMarkerMat;
            
            marker = Instantiate(beatMarkerPrefab, topGameObject.transform).GetComponent<BeatMarker>();
            marker.speed = markerSpeed;
            marker.transform.position += Vector3.up * (spawnDistance + traingleHitDistance);
            marker.GetComponent<MeshRenderer>().material = greenMarkerMat;
            
            marker = Instantiate(beatMarkerPrefab, rightGameObject.transform).GetComponent<BeatMarker>();
            marker.speed = markerSpeed;
            marker.transform.position += Vector3.up * (spawnDistance + traingleHitDistance);
            marker.GetComponent<MeshRenderer>().material = blueMarkerMat;
            
            beatsSpawned++;
        }

        //if (songProgress * (CurrentSongInfo.bpm / 60) >= beatsSpawned)
        //{
        //    float beatHitTime = songStartTime + BeatsInTime(beatsSpawned + 0);
        //}

        // Gameplay object spawning
        while (!IsSongOver())
        {
            float songProgress = Time.time - songStartTime;

            // Note spawning, notes are in order in beat files and so we can just loop through notes at the start
            while (unspawnedNoteInfos.Count > 0 && unspawnedNoteInfos[0].IsReadyToSpawn(spawnToHitTimeDelta, songProgress))
            {
                Note note = null;
                NoteColor noteColor = unspawnedNoteInfos[0].color;
                switch(noteColor)
                {
                    case NoteColor.Red:
                        note = Instantiate(notePrefab, leftGameObject.transform).GetComponent<Note>();
                        note.GetComponent<MeshRenderer>().material = blueNoteMat;
                        break;
                    case NoteColor.Green:
                        note = Instantiate(notePrefab, topGameObject.transform).GetComponent<Note>();
                        note.GetComponent<MeshRenderer>().material = redNoteMat;
                        break;
                    case NoteColor.Blue:
                        note = Instantiate(notePrefab, rightGameObject.transform).GetComponent<Note>();
                        note.GetComponent<MeshRenderer>().material = greenNoteMat;
                        break;
                }
                note.info = unspawnedNoteInfos[0];
                note.info.spawned = true;
                note.transform.position += Vector3.up * (spawnToHitDistance * ((note.info.hitTime - songProgress) / spawnToHitTimeDelta) + traingleHitDistance);
                note.speed = noteSpeed;
                spawnedNoteInfos.Add(note.info);
                unspawnedNoteInfos.RemoveAt(0);
            }
            yield return null;
        }

        // Song ended, wrap up scene
        StartCoroutine(WrapUpScene());
    }

    public float TimeAtNBeatsFromTime(float time, float numBeats)
    {
        return ((int) Mathf.Floor((time - songStartTime) / BeatsInTime(1f))) * BeatsInTime(1f) + BeatsInTime(numBeats);
    }

    public float TimeInBeats(float time)
    {
        return time * CurrentSongInfo.bpm / 60f;
    }

    public float BeatsInTime(float beats)
    {
        return beats * 60f / CurrentSongInfo.bpm;
    }

    IEnumerator WrapUpScene()
    {
        yield return new WaitForSeconds(0.5f); // Wait a bit in case song ends suddenly.
        float fadeDuration = 2f;
        Fade fadeOut = Instantiate(fadePrefab, fadeSpawn.transform).GetComponent<Fade>();
        fadeOut.duration = fadeDuration;
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene("ResultsScene");
    }

    private bool IsSongOver()
    {
        return !(mainAudioSource.isPlaying || loadingSong);
    }
}
