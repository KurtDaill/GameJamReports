using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horde_Member : MonoBehaviour
{
    float marchHeight;
    float baseHeight;
    bool marchUp;
    // Start is called before the first frame update
    void Start()
    {
        marchHeight = (Random.value * 0.13F) + 0.05F;
        baseHeight = this.transform.position.y;
        //FOR TESTING: SET TO FALSE LATER?
        marchUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (marchUp)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, (new Vector3(this.transform.position.x, marchHeight + baseHeight, this.transform.position.z)), Time.deltaTime);
        }
    }

    public float GetHeight()
    {
        return transform.position.y;
    }

    public void ToggleMarch()
    {
        marchUp = !marchUp;
    }

    public void Step()
    {
        this.transform.position = new Vector3(this.transform.position.x, baseHeight, this.transform.position.z);
    }
}
