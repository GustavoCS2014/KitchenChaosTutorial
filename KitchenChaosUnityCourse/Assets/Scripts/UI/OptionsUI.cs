using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {
    public static OptionsUI Instance { get; private set; }

    public Action OnCloseOptionsMenu;


    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;

    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternativeText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAlternativeText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;

    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAlternativeButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadInteractAlternativeButton;
    [SerializeField] private Button gamepadPauseButton;

    [SerializeField] private Transform pressToRebindKeyTransform;
    private void Awake() {
        Instance = this;

        soundEffectsButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() => { 
            Hide();
            OnCloseOptionsMenu();
        });

        moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.MoveUp); });
        moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.MoveDown); });
        moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.MoveLeft); });
        moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.MoveRight); });
        interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Interact); });
        interactAlternativeButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.InteractAlternate); });
        pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Pause); });
        gamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.GamepadInteract); });
        gamepadInteractAlternativeButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.GamepadInteractAlternate); });
        gamepadPauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.GamepadPause); });
    }

    private void Start() {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

        UpdateVisual();
        Hide();
        HidePressToRebindKey();
    }

    private void OnDestroy() {
        GameManager.Instance.OnGameUnpaused -= GameManager_OnGameUnpaused;
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e) {
        Hide();
    }

    private void UpdateVisual() {
        soundEffectsText.text = "SOUND EFFECTS VOLUME: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "MUSIC VOLUME: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        moveUpText.text = ChangeText(GameInput.Bindings.MoveUp);
        moveDownText.text = ChangeText(GameInput.Bindings.MoveDown);
        moveLeftText.text = ChangeText(GameInput.Bindings.MoveLeft);
        moveRightText.text = ChangeText(GameInput.Bindings.MoveRight);
        interactText.text = ChangeText(GameInput.Bindings.Interact);
        interactAlternativeText.text = ChangeText(GameInput.Bindings.InteractAlternate);
        pauseText.text = ChangeText(GameInput.Bindings.Pause);
        gamepadInteractText.text = ChangeText(GameInput.Bindings.GamepadInteract);
        gamepadInteractAlternativeText.text = ChangeText(GameInput.Bindings.GamepadInteractAlternate);
        gamepadPauseText.text = ChangeText(GameInput.Bindings.GamepadPause);
    }


    private void RebindBinding(GameInput.Bindings _binding) {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(_binding,() => {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }

    private string ChangeText(GameInput.Bindings _binding) {
        string _output = GameInput.Instance.GetBindingText(_binding) == "Escape" ? "Esc" :
            GameInput.Instance.GetBindingText(_binding);
        return _output;
    }

    public void Show(Action _onCloseOptionMenu) {
        OnCloseOptionsMenu = _onCloseOptionMenu;

        gameObject.SetActive(true);

        soundEffectsButton.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey() => pressToRebindKeyTransform.gameObject.SetActive(true);
    private void HidePressToRebindKey() => pressToRebindKeyTransform.gameObject.SetActive(false);
}
