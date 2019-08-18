using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<UnityEngine.UI.Text>().text = TaikoModule.LastScore + "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
