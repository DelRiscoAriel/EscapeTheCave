using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Player")]
    public Controller playerController;
    public Transform player;
    public Transform playerMoveTo;
    public bool followPlayer;
    public float timeToPlayer = 0f;

    [Header("1st Gate")]
    public Gates gate1;
    public float timeforExit1 = 2f;

    [Header("2nd Gate")]
    public Gates gate;
    public Transform door;
    public float timeforExit = 2f;
    bool secondGate = false;
    public float timeforExit2 = 2f;

    [Header("3rd Gate")]
    public Gates gate3;
    public float timeforExit3 = 2f;

    [Header("General")]
    public float speed = 20f;
    public Transform viewDoors;

    // Update is called once per frame
    void Update()
    {
        if (followPlayer)
        {
            timeToPlayer -= Time.deltaTime;
            if (timeToPlayer > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerMoveTo.position, speed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(player.position.x, player.position.y, -10);
                playerController.EnabledMove = true;
            }
                
        }

        //Second Gates
        if (gate.move == true)
        {
            playerController.EnabledMove = false;
            followPlayer = false;
            if (!secondGate)
            {
                transform.position = Vector3.MoveTowards(transform.position, door.position, speed * Time.deltaTime);
            }           
            timeforExit -= Time.deltaTime;
            if (timeforExit <= 0)
            {
                secondGate = true;
            }
        }
        if (secondGate)
        {
            transform.position = Vector3.MoveTowards(transform.position, viewDoors.position, speed * Time.deltaTime);
            timeforExit2 -= Time.deltaTime;
            if (timeforExit2 <= 0)
            {
                gate.move = false;
                followPlayer = true;
                timeToPlayer = 0.6f;
                secondGate = false;
            }
        }

        //Theird Gate
        if (gate3.move == true)
        {
            playerController.EnabledMove = false;
            followPlayer = false;
            transform.position = Vector3.MoveTowards(transform.position, viewDoors.position, speed * Time.deltaTime);
            timeforExit3 -= Time.deltaTime;
            if (timeforExit3 <= 0)
            {
                gate3.move = false;
                followPlayer = true;
                timeToPlayer = 0.6f;
            }
        }

        //First gate
        if (gate1.move == true)
        {
            playerController.EnabledMove = false;
            followPlayer = false;
            transform.position = Vector3.MoveTowards(transform.position, viewDoors.position, speed * Time.deltaTime);
            timeforExit1 -= Time.deltaTime;
            if (timeforExit1 <= 0)
            {
                gate1.move = false;
                followPlayer = true;
                timeToPlayer = 0.6f;
            }
        }
    }
}
