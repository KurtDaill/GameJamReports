using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horde_Control : MonoBehaviour
{
    LinkedList<Horde_Member> hordeMembers;
    bool IsLeft;
    public float maxSpeed;
    //Momentum indicates how fast each step should start, and scales up per step.
    float Momentum;
    //Current Speed is literally how fast the horde is moving this frame;
    float currentSpeed;
    public float stepDegradeFactor;
    public GameObject controler;
    public AudioSource leftDrum, rightDrum, leftFoot, rightFoot;
    public FreeParallax parallax;
    // Start is called before the first frame update
    void Start()
    {
        hordeMembers = new LinkedList<Horde_Member>();
        foreach (Transform child in transform)
        {
            hordeMembers.AddLast(child.GetComponent<Horde_Member>());
        }
        IsLeft = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Did the Player hit the drum this frame
        if (Input.GetButtonDown("Left Hand") || Input.GetButtonDown("Right Hand"))
        {
            //Which Drum?
            if (Input.GetButton("Left Hand"))
            {
                leftDrum.Play();               
            }
            else //I guess it's the right drum then eh?
            {
                rightDrum.Play();
            }

            if (controler.GetComponent<Timer>().CheckBeat())
            {
                if (IsLeft)
                {
                    leftFoot.Play();
                }
                else
                {
                    rightFoot.Play();
                }
                foreach(Horde_Member member in hordeMembers)
                {
                    member.Step();
                }
                Momentum = Mathf.Lerp(currentSpeed, maxSpeed, 0.33F);
                currentSpeed = Momentum;
            }
            IsLeft = !IsLeft;
        }
        transform.Translate(new Vector3(currentSpeed * Time.deltaTime, 0, 0));
        parallax.Speed = -1F * currentSpeed * Time.deltaTime;
        currentSpeed = Mathf.Lerp(currentSpeed, 0, stepDegradeFactor * Time.deltaTime);
    }
}
