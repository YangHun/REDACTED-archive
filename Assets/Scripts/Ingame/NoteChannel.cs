using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteChannel : MonoBehaviour
{

    public delegate TaikoNote Spawner();
    public delegate void Despawner(GameObject obj, System.Action onComplete = null);

    private Spawner SpawnNote;
    private Despawner DespawnNote;

    private System.Action onMiss;


    [SerializeField]
    private List<Note> data;


    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private RectTransform bar;

    
    private float min = Mathf.Infinity;

    public float noteSpeed;

    private int cid = -1;

    

    public void Init (int channelId, List<Note> data, Spawner spawner, Despawner despawner, System.Action onMiss) {
        this.data = data;
        this.SpawnNote = spawner;
        this.DespawnNote = despawner;
        this.onMiss = onMiss;
        this.cid = channelId;

        transform.localScale = Vector3.one;
        gameObject.SetActive(true);
    }
    
    public void Flush () {
        foreach (Transform note in spawnPoint) {
            DespawnNote (note.gameObject);
        }
        Destroy (this.gameObject);
    }
    
    public void OnUpdate(float velocity) {
        float current = SoundModule.Instance.BGM.time;
        for (int i = 0; i < data.Count; i++) {
            if ( ((float)data[0].timing / 1000.0f - SoundModule.Instance.GetTiming) * velocity >= 1 ) break;
            TaikoNote note = SpawnNote();
            note.transform.SetParent(this.spawnPoint);
            note.Init(data[0], bar.rect.width, onMiss, ()=>{ DespawnNote (note.gameObject);}, this.cid);
            data.RemoveAt(0);
        }

        foreach (Transform n in spawnPoint) {
            if (n.gameObject.activeSelf) n.GetComponent<TaikoNote>().OnUpdate(velocity);
        }
    }



    public TaikoNote GetNearest () {
        float distance = Mathf.Infinity;
        Transform result = null;
        foreach (Transform note in spawnPoint) {
            if (!note.gameObject.activeSelf) continue;
            float d = Vector3.Distance(note.position, spawnPoint.position);
            if (distance > d) {
                distance = d;
                result = note;
            }
        }

        if (result == null) return null;
        else return result.GetComponent<TaikoNote>();
    }






/*
    
    private void TryPass () {
        TaikoNote target = GetNearest();
        if (target == null) {
            Debug.Log ("null!!");
            return;
        }

        if (min > Vector3.Distance(target.transform.position, JudgePoint.position)) min = Vector3.Distance(target.transform.position, JudgePoint.position);
        dummyText.text = string.Format("distance:{0}\nMin dist:{1}", target.transform.position.x - JudgePoint.position.x, min);

        int score = JudgeScore(target.transform.position);

        if (score == 0) {
            target.PlayBadEffect();
        }
        else if (score == judgeScore) {
            target.PlayNormalEffect();
        }
        else if (score == exactScore) {
            target.PlayExactEffect();
        }
    }

    private void TryReject () {
        //todo
    }


    private int JudgeScore (Vector3 targetPosition) {
        float distance = Vector3.Distance(targetPosition, JudgePoint.position);
        
        if (distance <= exactDistance) return judgeScore;
        else if (distance <= judgeDistance) return exactScore; 
        else return 0;
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

*/
}
