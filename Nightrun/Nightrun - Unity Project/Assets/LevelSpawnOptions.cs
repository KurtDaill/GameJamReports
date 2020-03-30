using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnOptions : MonoBehaviour
{
    public GameObject shrine;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool spawnShrine(float procent)
    {   
        shrine.SetActive(procent > 0.8F);
        return (procent > 0.8F);
    }
}
