using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour {

    private AudioSource audioSource;
    [SerializeField] private StoveCounter stoveCounter;

    private float warningSoundTimer;
    bool playWarningSound;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        float _burnShowProgressAmount = .5f;
        playWarningSound = stoveCounter.IsFried() && e.ProgressNormalized >= _burnShowProgressAmount; 
    }

    private void OnDestroy() {

        stoveCounter.OnStateChanged -= StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        bool playSound = e._state == StoveCounter.State.Frying || e._state == StoveCounter.State.Fried;
        if (playSound) {
            audioSource.Play();
        }
        else {
            audioSource.Pause();
        }
    }

    private void Update() {
        if(playWarningSound) {
            warningSoundTimer -= Time.deltaTime;
            if(!(warningSoundTimer < 0)) return;
            float _warningSoundTimerMax = .2f;
            warningSoundTimer = _warningSoundTimerMax;

            SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position, 2f);
        }
    }
}
