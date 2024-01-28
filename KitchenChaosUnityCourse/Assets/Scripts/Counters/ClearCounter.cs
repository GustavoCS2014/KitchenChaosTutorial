using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ClearCounter : BaseCounter{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player _player) {

        //! Counter and Player are holding object.
        if(_player.HasKitchenObject() && HasKitchenObject()) {

            //? Picking the object if the player has a plate.
            if(_player.GetKitchenObject().TryGetPlate(out PlateKitchenObject _plateKitchenObject)) {
                if(!(_plateKitchenObject.TryAddIngridient(GetKitchenObject().GetKitchenObjectSO()))) return;
                GetKitchenObject().DestroySelf();

                return;
            }
            //? Placing object if there's a plate on the counter.
            if(GetKitchenObject().TryGetPlate(out _plateKitchenObject)) {
                if(!(_plateKitchenObject.TryAddIngridient(_player.GetKitchenObject().GetKitchenObjectSO()))) return;
                _player.GetKitchenObject().DestroySelf();

                return;
            }
            return;
        }

        //! Counter has object and player not.
        if(!_player.HasKitchenObject() && HasKitchenObject()) {

            //? Picking object.
            GetKitchenObject().SetKitchenObjectParent(_player);
            return;
        }

        //! Counter has object.
        if(HasKitchenObject()) {

            return;
        }

        //! Player is Holding something and counter not.
        if(_player.HasKitchenObject() && !HasKitchenObject()) {

            //? Placing object.
            if(_player.HasKitchenObject()) {
                _player.GetKitchenObject().SetKitchenObjectParent(this);
            }
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
}