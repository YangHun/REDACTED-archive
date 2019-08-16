using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaikoModule : MonoBehaviour
{
    
    public float velocity = 5.0f;
    public float judgeDistance = Screen.width / 1280 * 5.0f;

#region Refactored Later!!
    public List<Note> data;
#endregion


    private List <GameObject> waited = new List<GameObject>();
    private List <GameObject> spawned = new List<GameObject>();


    public GameObject note;
    public Transform SpawnPoint;
    public RectTransform JudgePoint;


    void Start () {
        Init(data);
    }

    public void Init (List<Note> data) {
        this.data = data;
        
        this.note.SetActive(false);
        ExpandPool();


        InputModule.onLeftMouseClicked += TryPass;
        InputModule.onRightMouseClicked += TryReject;
        

        Play();
    }

    public void Play () {
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

    private TaikoNote Spawn (Note data) {
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




    private void TryPass () {
        TaikoNote target = GetNearest();
        if (target == null) return;

        if (Vector2.Distance(target.GetComponent<RectTransform>().localPosition, JudgePoint.localPosition) > judgeDistance) {
            
            Debug.LogError (" out of range :"+Vector2.Distance(target.GetComponent<RectTransform>().localPosition, JudgePoint.localPosition));
             //todo : handle this case
            return;
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
            float d = Vector3.Distance(n.GetComponent<RectTransform>().localPosition, JudgePoint.localPosition);
            if (distance > d) {
                distance = d;
                result = n;
            }
        }

        if (result == null) return null;
        else return result.GetComponent<TaikoNote>();
    }
}