using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance;

    private void Awake() {
        Instance = this;
    }

    public override void Interact(Player _player) {
        if(!_player.HasKitchenObject()) return;

        if(_player.GetKitchenObject().TryGetPlate(out PlateKitchenObject _plateKitchenObject)) {
            DeliveryManager.Instance.DeliverRecipe(_plateKitchenObject);
            _player.GetKitchenObject().DestroySelf();
        }
    }
}
