using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI MoveUpKeyText;
    [SerializeField] private TextMeshProUGUI MoveDownpKeyText;
    [SerializeField] private TextMeshProUGUI MoveLeftKeyText;
    [SerializeField] private TextMeshProUGUI MoveRightKeyText;
    [SerializeField] private TextMeshProUGUI InteractKeyText;
    [SerializeField] private TextMeshProUGUI InteractAlternativeKeyText;
    [SerializeField] private TextMeshProUGUI PauseKeyText;
    [SerializeField] private TextMeshProUGUI GamepadInteractKeyText;
    [SerializeField] private TextMeshProUGUI GamepadInteractAlternativeKeyText;
    [SerializeField] private TextMeshProUGUI GamepadPauseKeyText;

    private void Start() {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        UpdateVisual();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if(GameManager.Instance.IsCountdownToStart()) {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        MoveUpKeyText.text = ChangeText(GameInput.Bindings.MoveUp);
        MoveDownpKeyText.text = ChangeText(GameInput.Bindings.MoveDown);
        MoveLeftKeyText.text = ChangeText(GameInput.Bindings.MoveLeft);
        MoveRightKeyText.text = ChangeText(GameInput.Bindings.MoveRight);
        InteractKeyText.text = ChangeText(GameInput.Bindings.Interact);
        InteractAlternativeKeyText.text = ChangeText(GameInput.Bindings.InteractAlternate);
        PauseKeyText.text = ChangeText(GameInput.Bindings.Pause);
        GamepadInteractKeyText.text = ChangeText(GameInput.Bindings.GamepadInteract);
        GamepadInteractAlternativeKeyText.text = ChangeText(GameInput.Bindings.GamepadInteractAlternate);
        GamepadPauseKeyText.text = ChangeText(GameInput.Bindings.GamepadPause);
    }

    private string ChangeText(GameInput.Bindings _binding) {
        string _output = GameInput.Instance.GetBindingText(_binding) == "Escape" ? "Esc" :
            GameInput.Instance.GetBindingText(_binding);
        return _output;
    }


    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}