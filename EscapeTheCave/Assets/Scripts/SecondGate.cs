using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondGate : MonoBehaviour
{
    public Gates gate;
    public Transform[] patrolPoints;
    public float speed;
    bool startTime = false;
    public float time = 2f;

    // Update is called once per frame
    void Update()
    {
        if (gate.move == true)
        {
            startTime = true;                    
        }
        if (startTime)
        {
            time -= Time.deltaTime;
        }
        if (time <= 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, speed * Time.deltaTime);
        }
    }
}
