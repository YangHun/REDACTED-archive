using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMe : MonoBehaviour
{
    public float after;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float k = 0;
    // Update is called once per frame
    void Update()
    {
        k += Time.deltaTime;
        if (after < k)
        {
            Destroy(this.gameObject);
        }
    }
}
