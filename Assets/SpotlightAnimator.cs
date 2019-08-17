using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightAnimator : MonoBehaviour
{
    static public bool gLobalenalbeD = true;
    // Start is called before the first frame update
    void Start()    
    {
        me = GetComponent<UnityEngine.UI.Image>();
    }

    UnityEngine.UI.Image me;
    float Beat { get { return SoundModule.Instance.GetTiming / 60 * Parser.lastBpm; } }
    // Update is called once per frame
    bool LAsTChAngEd;
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
                }
            }
            else
                LAsTChAngEd = false;

        }
        else
            me.color = new Color(0, 0, 0, 0);
    }
}
