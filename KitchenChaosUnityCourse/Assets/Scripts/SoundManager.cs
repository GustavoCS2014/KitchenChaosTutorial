using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private float volume = .3f;

    private void Awake() {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void OnDestroy() {
        DeliveryManager.Instance.OnRecipeSuccess -= DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed -= DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut -= CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething -= Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere -= BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed -= TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e) {
        TrashCounter _trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.Trash, _trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e) {
        BaseCounter _baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.ObjectDrop, _baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e) {
        PlaySound(audioClipRefsSO.ObjectPickup, Player.Instance.transform.position, 2);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e) {
        CuttingCounter _cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.Chop, _cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e) {
        DeliveryCounter _deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.DeliveryFail, _deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e) {
        DeliveryCounter _deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.DeliverySuccess, _deliveryCounter.transform.position);
    }
    private void PlaySound(AudioClip[] _audioClips, Vector3 _position, float _volumeMultiplier = 1f) {
        int _randomClip = Random.Range(0, _audioClips.Length);
        PlaySound(_audioClips[_randomClip], _position, _volumeMultiplier * volume);
    }

    private void PlaySound(AudioClip _audio, Vector3 _position, float _volumeMultiplier = 1f) {
        AudioSource.PlayClipAtPoint(_audio, _position, _volumeMultiplier * volume);
    }

    public void PlayFootstepsSound(Vector3 _position, float _volumeMultiplier = 1f) {
        PlaySound(audioClipRefsSO.FootStep, _position, _volumeMultiplier * volume);
    }

    public void PlayCountdownSound() {
        PlaySound(audioClipRefsSO.Warning, Vector3.zero, 2f);
    }

    public void PlayWarningSound(Vector3 _position, float _volumeMultiplier = 1f) {
        PlaySound(audioClipRefsSO.Warning, _position, _volumeMultiplier * volume);
    }

    public void ChangeVolume() {
        volume += .1f;
        if(volume > 1f) {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() {
        return volume;
    }
}
