using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
  // Config
  [SerializeField] float rcsThrust = 100f;
  [SerializeField] float mainThrust = 100f;
  [SerializeField] float levelLoadDelay = 2f;

  [SerializeField] AudioClip mainEngine;
  [SerializeField] AudioClip deathSFX;
  [SerializeField] AudioClip successSFX;

  [SerializeField] ParticleSystem mainEngineParticles;
  [SerializeField] ParticleSystem deathParticles;
  [SerializeField] ParticleSystem successParticles;

  // Cached component references
  Rigidbody myRigidbody;
  AudioSource audioSource;

  enum State { Alive, Dying, Transcending }
  State state = State.Alive;

  private void Start() {
    myRigidbody = GetComponent<Rigidbody>();
    audioSource = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update() {
    if (state == State.Alive) {
      RespondToThrustInput();
      RespondToRotateInput();
    }
  }

  private void OnCollisionEnter(Collision collision) {
    // ignore collisions when dead
    if (state != State.Alive) { return; }

    switch (collision.gameObject.tag) {
      case "Friendly":
        break;
      case "Finish":
        StartSuccessSequence();
        break;
      default:
        StartDeathSequence();
        break;
    }
  }

  private void StartSuccessSequence() {
    state = State.Transcending;
    audioSource.Stop();
    audioSource.PlayOneShot(successSFX);
    successParticles.Play();
    Invoke("LoadNextLevel", levelLoadDelay);
  }

  private void StartDeathSequence() {
    state = State.Dying;
    audioSource.Stop();
    audioSource.PlayOneShot(deathSFX);
    deathParticles.Play();
    Invoke("LoadFirstLevel", levelLoadDelay);
  }

  private void LoadNextLevel() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  private void LoadFirstLevel() {
    SceneManager.LoadScene(0);
  }

  private void RespondToThrustInput() {
    if (Input.GetKey(KeyCode.Space)) {
      ApplyThrust();
    }
    else {
      audioSource.Stop();
      mainEngineParticles.Stop();
    }
  }

  private void ApplyThrust() {
    myRigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
    if (!audioSource.isPlaying) {
      audioSource.PlayOneShot(mainEngine);
    }
    mainEngineParticles.Play();
  }

  private void RespondToRotateInput() {
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
