using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day1Outcomes : Outcomes
{
    void Start()
    {

    }

    void Update()
    {

    }

    public override void Outcome(int outcomeID)
    {
        // put outcome switch here
        switch (outcomeID)
        {
            case 0:
                // do outcome 0 stuff here
                break;
            case 1:
                // do outcome 1 stuff here
                break;
            default:
                // do outcome 2 stuff here
                break;
        }
    }
}
