using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent {

    public static event EventHandler OnAnyObjectPlacedHere;
    [SerializeField] private Transform counterTopPoint;
    private KitchenObject kitchenObject;

    public virtual void Interact(Player _player) {
        Debug.LogError("BaseCounter.Interact()");

        //! Counter and Player are holding object.
        if(_player.HasKitchenObject() && HasKitchenObject()) {

            return;
        }

        //! Counter has object and player not.
        if(!_player.HasKitchenObject() && HasKitchenObject()) {

            return;
        }

        //! Counter has object.
        if(HasKitchenObject()) {

            return;
        }

        //! Player is Holding something and counter not.
        if(_player.HasKitchenObject() && !HasKitchenObject()) {

            return;
        }

        //! Player isn't holding anything and counter not.
        if(!_player.HasKitchenObject() && !HasKitchenObject()) {

            return;
        }

        //! Counter is clean.
        if(!HasKitchenObject()) {

            return;
        }

    }
    public virtual void InteractAlternate(Player _player) {
    }

    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;

    public void SetKitchenObject(KitchenObject _kitchenObject) {
        kitchenObject = _kitchenObject;
        if(kitchenObject != null)
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
    }

    public KitchenObject GetKitchenObject() => kitchenObject;

    public void ClearKitchenObject() => kitchenObject = null;

    public bool HasKitchenObject() => kitchenObject != null;
}
