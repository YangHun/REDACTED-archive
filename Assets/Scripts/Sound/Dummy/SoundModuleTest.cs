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
            var channels = Parser.ParseString(mapfile.text);
            module.Init(channels);
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
