﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundModuleTest : MonoBehaviour
{
    bool bgm = false;
    bool sfx = false;
    public void ToggleBGM (Text text) {
        if (this.bgm) {
            SoundModule.Instance.StopBGM();
            text.text = "bgm\nOff";
        }
        else {
            SoundModule.Instance.PlayBGM(0, ()=> {
                text.text = "bgm\nOff";
            });

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