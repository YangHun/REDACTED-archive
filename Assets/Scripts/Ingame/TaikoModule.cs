using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaikoModule : MonoBehaviour
{
    
    public float velocity = 5.0f;

#region Refactored Later!!
    public List<Note> data;
#endregion


    private List <GameObject> waited = new List<GameObject>();
    private List <GameObject> spawned = new List<GameObject>();


    public GameObject note;
    public Transform SpawnPoint;
    public Transform WaitPoint;


    void Start () {
        this.note.SetActive(false);
        Init(data);
    }

    public void Init (List<Note> data) {
        this.data = data;
        this.note.SetActive(false);
        Play();
    }

    public void Play () {
        StartCoroutine (OnPlay());
    }

    private TaikoNote Spawn (Note data) {
        if (this.waited.Count == 0) {
            for (int i = 0; i < 10; i++) {
                GameObject obj = GameObject.Instantiate (this.note, this.gameObject.transform, false);
                //todo: init spawned note
                this.waited.Add (obj.gameObject);
            }
        }

        TaikoNote target = this.waited [0].GetComponent<TaikoNote>();
        this.spawned.Add (target.gameObject);
        this.waited.RemoveAt (0);
        return target;
    }

    private void Despawn (GameObject obj) {
        obj.SetActive(false);
        this.spawned.Remove (obj);
        this.spawned.Add(obj);
    }

    private IEnumerator OnPlay () {
        while (!SoundModule.Instance.BGM.isPlaying) {
            yield return null;
        }
        float length = SoundModule.Instance.BGM.clip.length;
        while (SoundModule.Instance.BGM.isPlaying) {
            float time = SoundModule.Instance.BGM.time;
            Debug.Log ("playing?");
            if ( data.Count > 0 && (float) (data [0].timing) / 1000.0f < time ) {
                TaikoNote note = Spawn(data[0]);
                note.Init(SpawnPoint, data[0], ()=>{ Despawn (note.gameObject); });
                data.RemoveAt(0);
                Debug.Log ("Spawned at:" + time);
            } 

            foreach (Transform n in SpawnPoint) {
                if (n.gameObject.activeSelf) n.GetComponent<TaikoNote>().OnUpdate(velocity);
            }

            yield return null;
        }
    }
}