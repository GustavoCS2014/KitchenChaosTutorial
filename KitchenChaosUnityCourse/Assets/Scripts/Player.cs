using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IKitchenObjectParent {

    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private float movementSpeed = 7;
    [SerializeField] private float rotationSpeed = 15;
    [SerializeField] private LayerMask CountersLayerMask;
    [SerializeField] private Transform KitchenObjectHoldPoint;

    private KitchenObject kitchenObject;
    private BaseCounter selectedCounter;
    private Vector3 moveDirection;
    private Vector3 lastMoveDirection;
    private bool isWalking;

    private void Awake() {
        if(Instance != null) {
            Debug.LogError("More than one instance of Player");
        }
        else {
            Instance = this;
        }
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void OnDestroy() {
        gameInput.OnInteractAction -= GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction -= GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
        if(!GameManager.Instance.IsGamePlaying()) return;

        if(selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if(!GameManager.Instance.IsGamePlaying()) return;

        if(selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    private void Update() {
        MovementHandler();
        InteractionHandler();

        if(moveDirection != Vector3.zero) 
            lastMoveDirection = moveDirection;
    }

    private void InteractionHandler() {
        float _interactionDistance = 2f;
        
        if(!Physics.Raycast(transform.position, lastMoveDirection, out RaycastHit _raycastHit, _interactionDistance, CountersLayerMask)) {
            SetSelectecCounter(null);
            return;   
        }

        //?TryGetComponent checks if there is the desired component and returns it.
        if(!_raycastHit.transform.TryGetComponent(out BaseCounter _counter)) {
            SetSelectecCounter(null);
            return;
        }

        if(_counter != selectedCounter) {
            SetSelectecCounter(_counter);
        }
    }

    private void SetSelectecCounter(BaseCounter _newSelectedCounter) {
        selectedCounter = _newSelectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }

    private void MovementHandler() {

        Vector2 _inputVector = gameInput.GetMovementVectorNormalized();

        moveDirection = new Vector3(_inputVector.x, 0, _inputVector.y);

        if(!CanMove(moveDirection))
            SlideAlong(moveDirection);

        if(CanMove(moveDirection))
            transform.position += moveDirection * movementSpeed * Time.deltaTime;


        isWalking = moveDirection != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, lastMoveDirection, Time.deltaTime * rotationSpeed);
    }

    private void SlideAlong(Vector3 _moveDirection) {
        Vector3 _moveDirectionX = new Vector3(_moveDirection.x, 0, 0).normalized;
        Vector3 _moveDirectionY = new Vector3(0, 0, _moveDirection.z).normalized;

        float _frictionPenalty = .7f;

        if(CanMove(_moveDirectionX)) {
            transform.position += _moveDirectionX * movementSpeed * _frictionPenalty * Time.deltaTime;
            return;
        }

        if(CanMove(_moveDirectionY)) {
            transform.position += _moveDirectionY * movementSpeed * _frictionPenalty * Time.deltaTime;
            return;
        }

        return;
    }

    private bool CanMove(Vector3 _facingDirection) {
        float _moveDistance = movementSpeed * Time.deltaTime;
        float _playerRadius = .7f;
        float _playerHeight = 1.5f;
        Vector3 _headPosition = transform.position + Vector3.up * _playerHeight;

        return !Physics.CapsuleCast(transform.position, _headPosition, _playerRadius, _facingDirection, _moveDistance);
    }

    public bool IsWalking() => isWalking;


    public Transform GetKitchenObjectFollowTransform() => KitchenObjectHoldPoint;

    public void SetKitchenObject(KitchenObject _kitchenObject) {
        kitchenObject = _kitchenObject;

        if(kitchenObject != null) 
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
    }

    public KitchenObject GetKitchenObject() => kitchenObject;

    public void ClearKitchenObject() => kitchenObject = null;

    public bool HasKitchenObject() => kitchenObject != null;
}
