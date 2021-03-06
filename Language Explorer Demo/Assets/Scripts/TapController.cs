﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // This will create a RigidBody2D component on anything the script is made a part of (almost like Typescript static types)
public class TapController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public GameManager gameInstance;
    public float tapForce = 200;
    public float tiltSmooth = 5;
    public Vector3 starPos;
    public AudioSource tapSound;
    public AudioSource scoreSound;
    public AudioSource dieSound;


    private Rigidbody2D rigidBody; // you dont need to explicitly say private
    Quaternion downRotation;
    Quaternion forwardRotation;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -80);
        forwardRotation = Quaternion.Euler(0, 0, 35);
        gameInstance = GameManager.Instance;
        rigidBody.simulated = false;
    }

    private void Update()
    {
        if (gameInstance.GameOver) return;
        if (Input.GetMouseButtonDown(0))
        {
            tapSound.Play();
            transform.rotation = forwardRotation;
            rigidBody.velocity = Vector2.zero; // If you dont add this the tap has to fight against the current velocity and wont do enough. This makes the velocity 0 and the bounce more snappy
            rigidBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force); // The second argument of Force mode has an impulse option as well. Force is the default and is optional
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);

    }


    private void OnEnable()
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
        GameManager.OnGameStarted += OnGameStarted;
    }
        private void OnDisable()
    {
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
        GameManager.OnGameStarted -= OnGameStarted;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = starPos;
        transform.rotation = Quaternion.identity;
    }

    void OnGameStarted()
    {
        rigidBody.velocity = Vector2.zero;
        rigidBody.simulated = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "ScoreZone")
        {
            scoreSound.Play();
            OnPlayerScored(); // event sent to GameManager
        }     
        if(collision.tag == "DeadZone")
        {
            dieSound.Play();
            rigidBody.simulated = false; // freeze the character
            OnPlayerDied(); // event sent to GameManager  register dead event
        }
    }

}
