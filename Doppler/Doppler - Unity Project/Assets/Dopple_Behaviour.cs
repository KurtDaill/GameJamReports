using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dopple_Behaviour : MonoBehaviour
{
    public float scaleFactorPerSec;
    float currentScaling;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale *= 1 + scaleFactorPerSec * Time.deltaTime;
        currentScaling = this.transform.localScale.magnitude;
        print(currentScaling);
        if (this.transform.localScale.magnitude >= 1.5)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameStateHandler>().EndGame();
        }
    }
}
