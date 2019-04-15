using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
  // Config
  [SerializeField] float rcsThrust = 100f;
  [SerializeField] float mainThrust = 100f;

  // Cached component references
  Rigidbody myRigidbody;
  AudioSource audioSource;

  private void Start() {
    myRigidbody = GetComponent<Rigidbody>();
    audioSource = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update() {
    Thrust();
    Rotate();
  }

  private void OnCollisionEnter(Collision collision) {
    switch (collision.gameObject.tag) {
      case "Friendly":
        break;
      default:
        break;
    }
  }

  private void Thrust() {
    if (Input.GetKey(KeyCode.Space)) {
      myRigidbody.AddRelativeForce(Vector3.up * mainThrust);
      if (!audioSource.isPlaying) {
        audioSource.Play();
      }
    }
    else {
      audioSource.Stop();
    }
  }

  private void Rotate() {
    myRigidbody.freezeRotation = true;

    float rotationThisFrame = rcsThrust * Time.deltaTime;

    if (Input.GetKey(KeyCode.A)) {
      transform.Rotate(Vector3.forward * rotationThisFrame);
    }
    else if (Input.GetKey(KeyCode.D)) {
      transform.Rotate(-Vector3.forward * rotationThisFrame);
    }

    myRigidbody.freezeRotation = false;
  }
}
