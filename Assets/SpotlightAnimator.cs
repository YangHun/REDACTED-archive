using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SpotlightAnimator : MonoBehaviour
{
    [SerializeField]
    PostProcessProfile profile;

    private ChromaticAberration ca;

    static public bool gLobalenalbeD = true;
    // Start is called before the first frame update
    void Start()    
    {
        me = GetComponent<UnityEngine.UI.Image>();
    }

    UnityEngine.UI.Image me;
    float Beat { get { return SoundModule.Instance.GetTiming / 60 * taikoModule.Bpm; } }
    // Update is called once per frame
    bool LAsTChAngEd;

    private TaikoModule taikoModule;

    void Awake()
    {
        taikoModule = FindObjectOfType<TaikoModule>();

        if (profile != null) {
            profile.TryGetSettings (out ca);
            if (ca != null) {
                ca.intensity.value = 0.0f;
            }
        }
    }
    
    void Update()
    {
        if (gLobalenalbeD)
        {
            if (Beat % 1f < 0.1f)
            {
                if (!LAsTChAngEd)
                {
                    var col = Color.HSVToRGB(Random.value, 1, 1);
                    col.a = 0.3f;

                    me.color = col;
                    LAsTChAngEd = true;

                    if (profile != null) {
                        profile.TryGetSettings (out ca);
                        if (ca != null) {
                            ca.intensity.value = 0.4f;
                        }
                    }
                }
            }
            else
                LAsTChAngEd = false;

        }
        else {
            me.color = new Color(0, 0, 0, 0);
            if (profile != null) {
                profile.TryGetSettings (out ca);
                if (ca != null) {
                ca.intensity.value = 0.0f;
            }
        }
        }
    }
}
