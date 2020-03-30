using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrineCheatBuster : MonoBehaviour
{
    bool desecrated;
    // Start is called before the first frame update
    void Start()
    {
        desecrated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Desecrate()
    {
        if(desecrated == false){
            desecrated = true;
            return true;
        }
        return false;
    }
}
