using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayWalkerBehaviour : MonoBehaviour
{
    bool amIDead;
    // Start is called before the first frame update
    void Start()
    {
        amIDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death()
    {
        this.GetComponent<Animator>().SetBool("die", true);
        GameObject.Destroy(this);
    }

    public bool KillMe()
    {
        if(amIDead == false)
        {
            amIDead = true;
            return true;
        }
        return false;
    }
}
