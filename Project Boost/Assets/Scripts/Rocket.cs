using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
  // Cached component references
  Rigidbody myRigidbody;
  AudioSource audioSource;

  private void Start() {
    myRigidbody = GetComponent<Rigidbody>();
    audioSource = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update() {
    ProcessInput();
  }

  private void ProcessInput() {
    if (Input.GetKey(KeyCode.Space)) {
      myRigidbody.AddRelativeForce(Vector3.up);
      if (!audioSource.isPlaying) {
        audioSource.Play();
      }
    }
    else {
      audioSource.Stop();
    }

    if (Input.GetKey(KeyCode.A)) {
      transform.Rotate(Vector3.forward);
    }
    else if (Input.GetKey(KeyCode.D)) {
      transform.Rotate(-Vector3.forward);
    }  
  }
}
