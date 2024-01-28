using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContainerCounter : BaseCounter{

    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player _player) {
        if(_player.HasKitchenObject()) return;
        KitchenObject.SpawnKitchenObject(kitchenObjectSO, _player);
        
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}
