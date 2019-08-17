using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaikoModule : MonoBehaviour
{
    

    public Text dummyText;
    private float min = Mathf.Infinity;

    public float velocity = 5.0f;
    public float judgeDistance = 100;
    public float judgeOffset = 100;
    public float noteSpeed;

    [HideInInspector]
    public List<Note> data;


    [SerializeField]
    private List <GameObject> waited = new List<GameObject>();
    [SerializeField]
    private List <GameObject> spawned = new List<GameObject>();


    public GameObject note;
    public Transform SpawnPoint;
    public Transform JudgePoint;

    public RectTransform bar;

    public void Init (List<Note> data) {
        if (data != null) this.data = data;
        else {
            this.data = new List<Note>();
            for (int i = 0; i < 50; i ++) {
                Note n = new Note();
                n.timing = i * 500;
                n.charactor = ((char)((int)'a' + i)).ToString();
                this.data.Add (n);
            }
        }
        
        this.note.SetActive(false);
        ExpandPool();


        InputModule.onLeftMouseClicked += TryPass;
        InputModule.onRightMouseClicked += TryReject;
        

        Play();
    }

    public void Flush (bool disableNotes = false) {
        InputModule.onLeftMouseClicked -= TryPass;
        InputModule.onRightMouseClicked -= TryReject;
        
        if (disableNotes) {
            foreach (GameObject obj in spawned) {
                Despawn (obj);
            }
        }
        this.data.Clear();
        
        StopAllCoroutines();
    }

    private void Play () {
        StopAllCoroutines();
        StartCoroutine (OnPlay());
    }

    private void ExpandPool () {
        if (this.waited.Count == 0) {
            for (int i = 0; i < 30; i++) {
                GameObject obj = GameObject.Instantiate (this.note, this.gameObject.transform, false);
                this.waited.Add (obj.gameObject);
            }
        }
    }

    private TaikoNote Spawn () {
        if (this.waited.Count == 0) {
            ExpandPool ();
        }

        TaikoNote target = this.waited [0].GetComponent<TaikoNote>();
        this.spawned.Add (target.gameObject);
        this.waited.RemoveAt (0);
        return target;
    }

    private void Despawn (GameObject obj) {
        obj.SetActive(false);
        this.spawned.Remove (obj);
        this.waited.Add(obj);
    }

    private IEnumerator OnPlay () {
        while (!SoundModule.Instance.BGM.isPlaying) {
            yield return null;
        }
        float length = SoundModule.Instance.BGM.clip.length;
        while (SoundModule.Instance.BGM.isPlaying) {
            float current = SoundModule.Instance.BGM.time;

            for (int i = 0; i < data.Count; ++i) {
                if (  ((float)data [0].timing / 1000.0f - current) * velocity >= 1) break;
                TaikoNote note = Spawn();
                note.Init(SpawnPoint, data[0], bar, ()=>{ Despawn (note.gameObject); });
                data.RemoveAt(0);
            }

            foreach (Transform n in SpawnPoint) {
                if (n.gameObject.activeSelf) n.GetComponent<TaikoNote>().OnUpdate(velocity);
            }

            yield return null;
        }
    }




    private void TryPass () {
        TaikoNote target = GetNearest();
        if (target == null) {
            Debug.Log ("null!!");
            return;
        }

        if (min > Vector3.Distance(target.transform.position, JudgePoint.position)) min = Vector3.Distance(target.transform.position, JudgePoint.position);
        dummyText.text = string.Format("distance:{0}\nMin dist:{1}", target.transform.position.x - JudgePoint.position.x, min);

        if (Vector3.Distance(target.transform.position, JudgePoint.position) > judgeDistance) {
            
            Debug.LogError ("Failed!");
             //todo : handle this case
            return;
        }

        else {
            Debug.LogError ("Success!");
        }


        

        //todo : check this note to be passed



    }

    private void TryReject () {
        //todo
    }


    private TaikoNote GetNearest () {
        float distance = Mathf.Infinity;
        Transform result = null;
        foreach (Transform n in SpawnPoint) {
            if (!n.gameObject.activeSelf) continue;
            float d = Vector3.Distance(n.position, JudgePoint.position);
            if (distance > d) {
                distance = d;
                result = n;
            }
        }

        if (result == null) return null;
        else return result.GetComponent<TaikoNote>();
    }
}