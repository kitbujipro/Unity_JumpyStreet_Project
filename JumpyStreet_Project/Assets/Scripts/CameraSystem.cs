﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraSystem: MonoBehaviour
{
    public GameObject aircraft;

    [SerializeField] float defaultSpeed = 0;
    [SerializeField] float fastSpeed = 0;
    [SerializeField] float loseDistance = 0;
    [SerializeField] float speedUpDistance = 0;
    [SerializeField] bool isMoving;
    [SerializeField] Transform playerTransform = null;
    float moveSpeed;
    Vector3 deathoffset;

    [HideInInspector] public PlayerScore PS;
    [HideInInspector] public PlayerMovement PM;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        PS = FindObjectOfType<PlayerScore>();
        PM = FindObjectOfType<PlayerMovement>();
        deathoffset = new Vector3(0, 0.5f, loseDistance);
    }

    // Update is called once per frame
    void Update()
    {
        StartCameraChecker();
        if (isMoving)
        {
            MoveCamera();
            if(playerTransform != null)
            {
                DistanceChecker();
            }
        }
        //Debug.Log(moveSpeed);
    }

    void MoveCamera()
    {
        if (playerTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(playerTransform.position.x, transform.position.y, transform.position.z), .02f);
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
        }   
    }

    void DistanceChecker()
    {
        float distanceToPlayer = playerTransform.position.z -  transform.position.z;
        //Debug.Log(distanceToPlayer);

        if (distanceToPlayer > speedUpDistance)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, fastSpeed, Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, defaultSpeed, Time.deltaTime);
        }

        if( distanceToPlayer < loseDistance && PM.onLog == false)
        {
            StartCoroutine(LoseByDistance());
        }
    }
    IEnumerator LoseByDistance()
    {
        Vector3 aircraftOffset = new Vector3(playerTransform.position.x + 0.5f, 2f, playerTransform.position.z + 12f);
        isMoving = false;
        PM.isDead = true;
        Instantiate(aircraft, aircraftOffset, aircraft.transform.rotation);
        FindObjectOfType<AudioManager>().Play("Aircraft");
        yield return new WaitForSeconds(1.6f);
        PS.GameOver();
        FindObjectOfType<AudioManager>().Play("Death");
    }

    void StartCameraChecker()
    {
        if (playerTransform != null && PM.isDead == false)
        {
            float playerZPosition = playerTransform.position.z;
            if (playerZPosition > 0.5f)
            {
                isMoving = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.right) * 5;
        Gizmos.DrawRay(transform.position + deathoffset, direction);
        Vector3 direction2 = transform.TransformDirection(Vector3.left) * 5;
        Gizmos.DrawRay(transform.position + deathoffset, direction2);
    }

}
