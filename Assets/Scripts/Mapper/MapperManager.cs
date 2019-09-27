using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

namespace Mapper
{
    public class MapperManager : MonoBehaviour
    {
        public TextMeshProUGUI cursorResolutionDisplay;
        public Transform leftNoteSpawn;
        public Transform topNoteSpawn;
        public Transform rightNoteSpawn;
        public GameObject notePrefab;
        public Material redNoteMat;
        public Material greenNoteMat;
        public Material blueNoteMat;
        public GameObject beatMarkerPrefab;
        public float beatMarkerOffset = 2f;

        public AudioSource mainAudioSource;

        private bool playing = false;
        private Camera mainCamera;
        private float cursorTime = 0f;
        private int cursorResolutionNumerator = 1;
        private int cursorResolutionDenominator = 1; // Use inversely
        private AudioClip songClip;
        private List<NoteInfo> noteInfos;

        void Start()
        {
            mainCamera = Camera.main;
            StartCoroutine(LoadMap());
            // TODO: Display a "Loading" box while loading map
        }

        void Update()
        {
            if (!playing)
            {
                HandleNotePlacement();
                HandleNoteDeletion();
                HandleCursorResolution();
                HandleCursorMovement();
            }
            HandlePlayback();
        }

        void HandleNotePlacement()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    PlacementHitbox hitbox = hit.collider.GetComponent<PlacementHitbox>();
                    if (hitbox != null)
                    {
                        InsertNoteInfo(new NoteInfo(hitbox.color, cursorTime));
                    }
                }
            }
        }

        void HandleNoteDeletion()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    MapperNote note = hit.collider.GetComponent<MapperNote>();
                    if (note != null)
                    {
                        RemoveNoteInfo(note);
                    }
                }
            }
        }

        void HandleCursorResolution()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (cursorResolutionDenominator > 1)
                {
                    cursorResolutionDenominator /= 2;
                }
                else
                {
                    cursorResolutionNumerator *= 2;
                    if (cursorResolutionNumerator > 32)
                    {
                        cursorResolutionNumerator = 32;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (cursorResolutionNumerator > 1)
                {
                    cursorResolutionNumerator /= 2;
                }
                else
                {
                    cursorResolutionDenominator *= 2;
                    if (cursorResolutionDenominator > 32)
                    {
                        cursorResolutionDenominator = 32;
                    }
                }
            }
            cursorResolutionDisplay.text = cursorResolutionNumerator + "/" + cursorResolutionDenominator;
        }

        void HandleCursorMovement()
        {
            float beatSkip = cursorResolutionNumerator / (float) cursorResolutionDenominator; // How many beats to move
            float timeSkip = BeatsInTime(beatSkip); // How many seconds to move
            float newTime = cursorTime;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                newTime += 0.0001f;
                newTime = Mathf.Floor(cursorTime / timeSkip) * timeSkip + timeSkip;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                newTime += 0.0001f;
                newTime = Mathf.Floor(cursorTime / timeSkip) * timeSkip - timeSkip;
            }
            SetCursorTime(newTime);
        }

        void HandlePlayback()
        {
            if (!mainAudioSource.isPlaying)
            {
                playing = false;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playing = !playing;
                if (playing)
                {
                    mainAudioSource.time = cursorTime;
                    mainAudioSource.Play();
                }
                else
                {
                    mainAudioSource.Stop();
                }
            }
        }

        public void InsertNoteInfo(NoteInfo info)
        {
            int insertionIndex = 0;
            while (insertionIndex < noteInfos.Count && noteInfos[insertionIndex].hitTime <= info.hitTime)
            {
                if (Mathf.Abs(noteInfos[insertionIndex].hitTime - cursorTime) < 0.001f && noteInfos[insertionIndex].color == info.color)
                {
                    return; // Do not want to write duplicate notes, so we return without doing anything
                }
                insertionIndex++;
            }
            noteInfos.Insert(insertionIndex, info);

            MapperNote note = null;
            NoteColor noteColor = info.color;
            switch (noteColor)
            {
                case NoteColor.Red:
                    note = Instantiate(notePrefab, leftNoteSpawn).GetComponent<MapperNote>();
                    note.GetComponent<MeshRenderer>().material = blueNoteMat;
                    break;
                case NoteColor.Green:
                    note = Instantiate(notePrefab, topNoteSpawn).GetComponent<MapperNote>();
                    note.GetComponent<MeshRenderer>().material = redNoteMat;
                    break;
                case NoteColor.Blue:
                    note = Instantiate(notePrefab, rightNoteSpawn).GetComponent<MapperNote>();
                    note.GetComponent<MeshRenderer>().material = greenNoteMat;
                    break;
            }
            note.info = info;
            note.transform.position += Vector3.up * beatMarkerOffset * TimeInBeats(info.hitTime);
        }

        public void RemoveNoteInfo(MapperNote note)
        {
            noteInfos.Remove(note.info);
            Destroy(note.gameObject);
        }

        public void SetCursorTime(float time)
        {
            cursorTime = time;
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, beatMarkerOffset * TimeInBeats(time), mainCamera.transform.position.z);
        }

        private IEnumerator LoadMap()
        {
            noteInfos = new List<NoteInfo>(CurrentSongInfo.noteInfos);

            // Song loading first
            string audioClipPath = "File://" + Path.Combine(Application.streamingAssetsPath, "CustomSongs", CurrentSongInfo.songFolder, "song.ogg");
            UnityWebRequest unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(audioClipPath, AudioType.OGGVORBIS);
            yield return unityWebRequest.SendWebRequest();
            songClip = DownloadHandlerAudioClip.GetContent(unityWebRequest);
            mainAudioSource.clip = songClip;
            float numBeats = TimeInBeats(songClip.length);

            // Placing beat markers
            for (int beatCounter = 0; beatCounter < numBeats; beatCounter++)
            {
                GameObject beatMarker = Instantiate(beatMarkerPrefab);
                beatMarker.transform.position += Vector3.up * beatMarkerOffset * beatCounter;
                for (int subBeatCounter = 1; subBeatCounter <= 3; subBeatCounter++)
                {
                    GameObject subBeatMarker = Instantiate(beatMarkerPrefab);
                    Color baseMarkerColor = subBeatMarker.GetComponent<MeshRenderer>().material.color;
                    subBeatMarker.GetComponent<MeshRenderer>().material.color = new Color(baseMarkerColor.r, baseMarkerColor.g, baseMarkerColor.b, baseMarkerColor.a / 2f);
                    subBeatMarker.transform.position += Vector3.up * beatMarkerOffset * (beatCounter + subBeatCounter / 4f);
                    subBeatMarker.GetComponentInChildren<TextMeshPro>().enabled = false;
                }
                beatMarker.GetComponentInChildren<TextMeshPro>().text = beatCounter.ToString();
            }

            // Placing notes
            foreach (NoteInfo noteInfo in noteInfos)
            {
                MapperNote note = null;
                NoteColor noteColor = noteInfo.color;
                switch (noteColor)
                {
                    case NoteColor.Red:
                        note = Instantiate(notePrefab, leftNoteSpawn).GetComponent<MapperNote>();
                        note.GetComponent<MeshRenderer>().material = blueNoteMat;
                        break;
                    case NoteColor.Green:
                        note = Instantiate(notePrefab, topNoteSpawn).GetComponent<MapperNote>();
                        note.GetComponent<MeshRenderer>().material = redNoteMat;
                        break;
                    case NoteColor.Blue:
                        note = Instantiate(notePrefab, rightNoteSpawn).GetComponent<MapperNote>();
                        note.GetComponent<MeshRenderer>().material = greenNoteMat;
                        break;
                }
                note.info = noteInfo;
                note.transform.position += Vector3.up * beatMarkerOffset * TimeInBeats(noteInfo.hitTime);
            }
            StartCoroutine(HandleAutoMoveCursor());
        }

        private IEnumerator HandleAutoMoveCursor()
        {
            while (true)
            {
                if (cursorTime < 0)
                {
                    SetCursorTime(0);
                }
                if (cursorTime > songClip.length)
                {
                    SetCursorTime(songClip.length);
                }
                if (playing)
                {
                    SetCursorTime(mainAudioSource.time);
                }
                yield return null;
            }
        }

        public float TimeInBeats(float time)
        {
            return time * CurrentSongInfo.bpm / 60f;
        }

        public float BeatsInTime(float beats)
        {
            return beats * 60f / CurrentSongInfo.bpm;
        }

        // Called by button
        public void ReturnToSongInfo()
        {
            SceneManager.LoadScene("SongInfoScene");
        }

        // Called by button
        public void SaveMap()
        {
            string infoFileContents = "songName:" + CurrentSongInfo.songName;
            infoFileContents += "\n" + "spawnToHitTime:" + CurrentSongInfo.spawnToHitTimeDelta; // TODO: modify spawnToHitTime in editor
            infoFileContents += "\n" + "bpm:" + CurrentSongInfo.bpm;

            foreach (NoteInfo info in noteInfos)
            {
                infoFileContents += "\n" + ColorToString(info.color) + ":" + info.hitTime;
            }

            string infoFilePath = Path.Combine(Application.streamingAssetsPath, "CustomSongs", CurrentSongInfo.songFolder, CurrentSongInfo.songDifficulty + ".beat");
            using (StreamWriter sw = File.CreateText(infoFilePath))
            {
                sw.Write(infoFileContents);
            }
        }

        private string ColorToString(NoteColor color)
        {
            switch (color)
            {
                case NoteColor.Red:
                    return "red";
                case NoteColor.Green:
                    return "green";
                case NoteColor.Blue:
                    return "blue";
                default:
                    return "red";
            }
        }
    }
}