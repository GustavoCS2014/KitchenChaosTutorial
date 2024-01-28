using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {

    public static GameInput Instance { get; private set; }

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum Bindings {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate,
        Pause,
        GamepadInteract,
        GamepadInteractAlternate,
        GamepadPause,
    }

    private PlayerInputActions playerInputActions;

    private void Awake() {
        Instance = this;

        playerInputActions = new PlayerInputActions();

        if(PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy() {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        //? el ?.Invoke funciona igual que un if verificando si el evento no tiene suscriptores.
        OnInteractAction?.Invoke(this,EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 _inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        return _inputVector.normalized;
    }

    public string GetBindingText(Bindings _binding) {
        switch (_binding) {
            default:
            case Bindings.MoveUp:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Bindings.MoveDown:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Bindings.MoveLeft:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Bindings.MoveRight:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Bindings.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Bindings.InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Bindings.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
            case Bindings.GamepadInteract:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Bindings.GamepadInteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Bindings.GamepadPause:
                return playerInputActions.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Bindings _binding, Action _onActionRebound) {
        playerInputActions.Player.Disable();

        InputAction _inputAction;
        int _bindingIndex;

        switch(_binding) {
            default:
            case Bindings.MoveUp:
                _inputAction = playerInputActions.Player.Move;
                _bindingIndex = 1;
                break;
            case Bindings.MoveDown:
                _inputAction = playerInputActions.Player.Move;
                _bindingIndex = 2;
                break;
            case Bindings.MoveLeft:
                _inputAction = playerInputActions.Player.Move;
                _bindingIndex = 3;
                break;
            case Bindings.MoveRight:
                _inputAction = playerInputActions.Player.Move;
                _bindingIndex = 4;
                break;
            case Bindings.Interact:
                _inputAction = playerInputActions.Player.Interact;
                _bindingIndex = 0;
                break;
            case Bindings.InteractAlternate:
                _inputAction = playerInputActions.Player.InteractAlternate;
                _bindingIndex = 0;
                break;
            case Bindings.Pause:
                _inputAction = playerInputActions.Player.Pause;
                _bindingIndex = 0;
                break;
            case Bindings.GamepadInteract:
                _inputAction = playerInputActions.Player.Interact;
                _bindingIndex = 1;
                break;
            case Bindings.GamepadInteractAlternate:
                _inputAction = playerInputActions.Player.InteractAlternate;
                _bindingIndex = 1;
                break;
            case Bindings.GamepadPause:
                _inputAction = playerInputActions.Player.Pause;
                _bindingIndex = 1;
                break;
        }

        _inputAction.PerformInteractiveRebinding(_bindingIndex)
            .OnComplete(callback => {
                callback.Dispose();
                playerInputActions.Player.Enable();
                _onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);

            }).Start();

    }
}
