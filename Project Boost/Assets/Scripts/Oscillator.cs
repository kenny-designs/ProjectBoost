using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// only allows one script of this type on a GameObject
[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {
  // Config
  [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
  [SerializeField] float period = 2f;

  float movementFactor;
  Vector3 startingPos;

  // Start is called before the first frame update
  void Start() {
    startingPos = transform.position;
  }

  // Update is called once per frame
  void Update() {
    if (period <= Mathf.Epsilon) { return; }
    float cycles = Time.time / period;

    const float tau = 2 * Mathf.PI;
    float rawSinWave = Mathf.Sin(cycles * tau);

    movementFactor = rawSinWave / 2f + 0.5f;
    Vector3 offset = movementFactor * movementVector;
    transform.position = startingPos + offset;
  }
}
