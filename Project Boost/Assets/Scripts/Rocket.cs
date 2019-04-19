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

  bool isTransitioning = false;

  bool collisionsDisabled = false;

  private void Start() {
    myRigidbody = GetComponent<Rigidbody>();
    audioSource = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update() {
    if (!isTransitioning) {
      RespondToThrustInput();
      RespondToRotateInput();
    }

    if (Debug.isDebugBuild) {
      RespondToDebugKeys();
    }
  }

  private void RespondToDebugKeys() {
    if (Input.GetKeyDown(KeyCode.L)) {
      LoadNextLevel();
    }
    else if (Input.GetKeyDown(KeyCode.C)) {
      collisionsDisabled = !collisionsDisabled;
    }
  }

  private void OnCollisionEnter(Collision collision) {
    if (isTransitioning || collisionsDisabled) { return; }

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
    isTransitioning = true;
    audioSource.Stop();
    audioSource.PlayOneShot(successSFX);
    successParticles.Play();
    Invoke("LoadNextLevel", levelLoadDelay);
  }

  private void StartDeathSequence() {
    isTransitioning = true;
    audioSource.Stop();
    audioSource.PlayOneShot(deathSFX);
    deathParticles.Play();
    Invoke("LoadFirstLevel", levelLoadDelay);
  }

  private void LoadNextLevel() {
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
    SceneManager.LoadScene(nextSceneIndex);
  }

  private void LoadFirstLevel() {
    SceneManager.LoadScene(0);
  }

  private void RespondToThrustInput() {
    if (Input.GetKey(KeyCode.Space)) {
      ApplyThrust();
    }
    else {
      StopApplyingThrust();
    }
  }

  private void StopApplyingThrust() {
    audioSource.Stop();
    mainEngineParticles.Stop();
  }

  private void ApplyThrust() {
    myRigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
    if (!audioSource.isPlaying) {
      audioSource.PlayOneShot(mainEngine);
    }
    mainEngineParticles.Play();
  }

  private void RespondToRotateInput() {
    if (Input.GetKey(KeyCode.A)) {
      RotateManually(-rcsThrust * Time.deltaTime);
    }
    else if (Input.GetKey(KeyCode.D)) {
      RotateManually(rcsThrust * Time.deltaTime);
    }
  }

  private void RotateManually(float rotationThisFrame) {
    myRigidbody.freezeRotation = true;
    transform.Rotate(-Vector3.forward * rotationThisFrame);
    myRigidbody.freezeRotation = false;
  }
}
