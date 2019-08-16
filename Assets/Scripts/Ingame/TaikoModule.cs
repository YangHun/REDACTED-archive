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

#region Refactored Later!!
    public List<Note> data;
#endregion


    private List <GameObject> waited = new List<GameObject>();
    private List <GameObject> spawned = new List<GameObject>();


    public GameObject note;
    public Transform SpawnPoint;
    public Transform JudgePoint;

    public void Init (List<Note> data) {
        if (data != null) this.data = data;
        
        this.note.SetActive(false);
        ExpandPool();


        InputModule.onLeftMouseClicked += TryPass;
        InputModule.onRightMouseClicked += TryReject;
        

        Play();
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
            if ( data.Count > 0 && (float) (data [0].timing) / 1000.0f < time ) {
                TaikoNote note = Spawn(data[0]);
                note.Init(SpawnPoint, data[0], ()=>{ Despawn (note.gameObject); });
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
        dummyText.text = string.Format ("distance:{0}\nMin dist:{1}",Vector3.Distance(target.transform.position, JudgePoint.position), min);

        if (Vector3.Distance(target.transform.position, JudgePoint.position) > judgeDistance) {
            
            Debug.LogError (" so far from judge line! :"+ Vector3.Distance(target.transform.position, JudgePoint.position));
             //todo : handle this case
            return;
        }

        else {
            Debug.Log ("distance:"+ Vector3.Distance(target.transform.position, JudgePoint.position));
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