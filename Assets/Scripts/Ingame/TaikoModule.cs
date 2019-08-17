using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TaikoModule : MonoBehaviour
{
    
    private int score;
    
    public int Score {
        get { return this.score; }
        set { 
            this.score = value;
            GameUI.Instance.UpdateScoreText(this.score);
        }
    }
    
    private float life;

    public float Life {
        get { return this.life; }
        set {
            this.life = value;
            if (this.life <= 0) GameOver();
            GameUI.Instance.UpdateLifeBar (this.life);
        }
    }

    private Song song;
    public int Bpm
    {
        get { return song.Bpm; }
    }
    
    [SerializeField]
    List <NoteChannel> channels;


    [SerializeField]
    GameObject notebase;
    [SerializeField]
    GameObject channelbase;


    [SerializeField]
    Transform notePool;
    [SerializeField]
    Transform JudgePoint;
    [SerializeField]
    Transform channelRoot;


    [SerializeField]
    private List <GameObject> waited = new List<GameObject>();
    [SerializeField]
    private List <GameObject> spawned = new List<GameObject>();


    public float velocity = 5.0f;


    private int totalCount;

    #region dummy UI    
    public Text dummyText;
    #endregion

    void Start()
    {
        Debug.Log("Starting Taiko Module");
        if (Song.currentSong != null)
        {
            Init(Song.currentSong);
        }
        else
        {
            Init(Song.LoadSong("freerider"));
        }
    }

    public void Init (Song song)
    {
        this.song = song;
        
        if (channels == null) channels = new List <NoteChannel>();
        else channels.Clear();

        this.notebase.SetActive(false);
        this.channelbase.SetActive(false);
        this.Score = 0;
        this.Life = 1.0f;

        totalCount = 0;
        for (int i = 0; i < song.Channels.Count; ++i) {
            NoteChannel c = GameObject.Instantiate(channelbase).GetComponent<NoteChannel>();
            c.transform.SetParent(this.channelRoot, false);
            c.Init(i, song.Channels[i], Spawn, Despawn, AutoJudgeMiss);
            channels.Add(c);
            totalCount += song.Channels[i].Count;
        }


        InputModule.onLeftMouseClicked += JudgeChannelLeftClick;
        InputModule.onRightMouseClicked += JudgeChannelRightClick;
        Play();   
    }

    private void Play () {
        StopAllCoroutines();
        StartCoroutine (OnPlay());
    }

    public void Flush () {
        foreach (NoteChannel c in this.channels) {
            c.Flush();
        }    
        channels.Clear();
        
        InputModule.onLeftMouseClicked -= JudgeChannelLeftClick;
        InputModule.onRightMouseClicked -= JudgeChannelRightClick;
        StopAllCoroutines();
    }

    private void ExpandPool () {
        if (this.waited.Count == 0) {
            for (int i = 0; i < 30; i++) {
                GameObject obj = GameObject.Instantiate (this.notebase, this.gameObject.transform, false);
                obj.transform.SetParent(notePool, false);
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

    private void Despawn (GameObject obj, System.Action onComplete = null) {
        obj.SetActive(false);
        obj.transform.SetParent(notePool);
        this.spawned.Remove (obj);
        this.waited.Add(obj);
        onComplete?.Invoke();
    }

    private IEnumerator OnPlay () {
        /*
        while (!SoundModule.Instance.BGM.isPlaying) {
            yield return null;
        }
        */
        SoundModule.Instance.PlayBGM(song.Clip);
        float length = SoundModule.Instance.BGM.clip.length;
        while (SoundModule.Instance.BGM.isPlaying) {
            float current = SoundModule.Instance.BGM.time;
            foreach (NoteChannel c in channels) {
                c.OnUpdate(velocity);
            }
            

            yield return null;
        }
    }

    private void JudgeChannelLeftClick () {
       // use channels [0]
        TaikoNote target = GetNearestGlobal();
        if (target == null || (target != null && target.IsJudged))  return;
        JudgeTouch (target, 0);        
    }

    private void JudgeChannelRightClick () {
        // use channels [1]
        TaikoNote target = GetNearestGlobal();
        if (target == null || (target != null && target.IsJudged)) return;
        JudgeTouch (target, 1, true);        
    
    }

    private TaikoNote GetNearestGlobal () {
        TaikoNote n0 = channels[0].GetNearest();
        TaikoNote n1 = channels[1].GetNearest();

        if (n0 == null && n1 != null) return n1;
        else if (n1 == null && n0 != null) return n0;
        else if (n0 != null && n1 != null) return ( Vector3.Distance (n0.transform.position, JudgePoint.position) < Vector3.Distance (n1.transform.position, JudgePoint.position))? n0 : n1;
        else return null;
    }

    private void JudgeTouch(TaikoNote target, int targetChannel, bool changeText = false) {
        float distance = Vector3.Distance (target.transform.position, JudgePoint.position);
        Debug.Log (distance);
        if (distance >= GameConstant.JUDGE_OFFSET_ENTRY) {
            // not reached judge entry
            return;            
        }

        else if (distance >= GameConstant.JUDGE_OFFSET_NORMAL) {
            // miss
            target.SetJudged();
            target.PlayMissTouchEffect();
            this.Life = Mathf.Max (0.0f, this.life - GameConstant.JUDGE_MISS_LIFE_PENALTY);
            GameUI.Instance.UpdateJudgeText ("Miss!");
            return;
        }
        else {
            if (target.channel != targetChannel) {
                // miss
                target.SetJudged();
                target.PlayMissTouchEffect();
                this.Life = Mathf.Max (0.0f, this.life - GameConstant.JUDGE_MISS_LIFE_PENALTY);
                GameUI.Instance.UpdateJudgeText ("Miss!");
                return;
            }

            if (distance >= GameConstant.JUDGE_OFFSET_EXACT) {
                // normal touch
                target.SetJudged();
                target.PlayNormalTouchEffect();
                this.Score += GameConstant.JUDGE_SCORE_0;
                this.Life = Mathf.Min (1.0f, this.life + GameConstant.JUDGE_SUCCESS_LIFE_PRICE);
                GameUI.Instance.UpdateJudgeText ("Good!");
                if (changeText) target.ChangeText();
                return;
            }
            else if (distance <= GameConstant.JUDGE_OFFSET_EXACT) {
                //exact touch
                target.SetJudged();
                target.PlayExactTouchEffect();
                this.Score += GameConstant.JUDGE_SCORE_1;
                this.Life = Mathf.Min (1.0f, this.life + GameConstant.JUDGE_SUCCESS_LIFE_PRICE);
                GameUI.Instance.UpdateJudgeText ("Exact!");
                if (changeText) target.ChangeText();
                return;
            }
        }
        
        //Despawn (target.gameObject);
    }

    private void AutoJudgeMiss () {
        this.Life = Mathf.Max (0.0f, this.life - GameConstant.JUDGE_MISS_LIFE_PENALTY);
        GameUI.Instance.UpdateJudgeText ("Miss!");
    }

    private void GameOver()
    {
        SoundModule.Instance.StopBGM();
        SceneManager.LoadScene("GameOver");
    }


}
