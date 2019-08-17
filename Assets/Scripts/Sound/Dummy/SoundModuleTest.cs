using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundModuleTest : MonoBehaviour
{
    public TextAsset mapfile;

    public TaikoModule module;

    bool bgm = false;
    bool sfx = false;
    public void ToggleBGM (Text text) {
        if (this.bgm) {
            SoundModule.Instance.StopBGM();
            text.text = "bgm\nOff"; 
            module.Flush();
        }
        else {
            SoundModule.Instance.PlayBGM(0, ()=> {
                text.text = "bgm\nOff";
            });
            if (Song.currentSong == null)
            {
                module.Init(Song.currentSong);
            }
            else
            {
                module.Init(Song.LoadSong("프리라이더"));
            }
            text.text = "bgm\nOn";
        }
        this.bgm = !this.bgm;
    }

    public void ToggleSFX(Text text) {
        if (this.sfx) {
            return;
        }
        else {
            SoundModule.Instance.SetBGMVolume(0.0f);
            SoundModule.Instance.PlaySFX(0, ()=> {
                text.text = "sfx\nOff";
                this.sfx = !this.sfx;
                SoundModule.Instance.SetBGMVolume(1.0f);
            });

            text.text = "sfx\nOn";
        }
        this.sfx = !this.sfx;
    }
}
