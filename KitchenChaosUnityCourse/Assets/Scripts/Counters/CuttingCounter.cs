using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CuttingCounter : BaseCounter, IHasProgress {

    public static event EventHandler OnAnyCut;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player _player) {
        //! Counter and Player are holding object.
        if(_player.HasKitchenObject() && HasKitchenObject()) {

            //? Picking the object if the player has a plate.
            if(_player.GetKitchenObject().TryGetPlate(out PlateKitchenObject _plateKitchenObject)) {
                if(!(_plateKitchenObject.TryAddIngridient(GetKitchenObject().GetKitchenObjectSO()))) return;
                GetKitchenObject().DestroySelf();
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

            //? Placing object if there's a recipie.
            if(HasRecipieWithInput(_player.GetKitchenObject().GetKitchenObjectSO())) {
                _player.GetKitchenObject().SetKitchenObjectParent(this);
                cuttingProgress = 0;

                CuttingRecipeSO _cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    ProgressNormalized = (float)cuttingProgress / _cuttingRecipeSO.CuttingProgressMax
                });
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

    public override void InteractAlternate(Player _player) {
        if(HasKitchenObject() && HasRecipieWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            cuttingProgress++;
            
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO _cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                ProgressNormalized = (float)cuttingProgress / _cuttingRecipeSO.CuttingProgressMax
            });
            
            if(!(cuttingProgress >= _cuttingRecipeSO.CuttingProgressMax)) return;

            KitchenObjectSO _cuttingOuputSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(_cuttingOuputSO, this);
        }
    }

    private bool HasRecipieWithInput(KitchenObjectSO _inputKitchenObjectSO) {
        CuttingRecipeSO _cuttingRecipeSO = GetCuttingRecipeSOWithInput(_inputKitchenObjectSO);
        return _cuttingRecipeSO != null ? true : false;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO _inputKitchenObjectSO) {
        CuttingRecipeSO _cuttingRecipeSO = GetCuttingRecipeSOWithInput(_inputKitchenObjectSO);
        return _cuttingRecipeSO != null ? _cuttingRecipeSO.Output : null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO _inputKitchenObjectSO) {
        foreach(CuttingRecipeSO _cuttingRecipeSO in cuttingRecipeSOArray) {
            if(_cuttingRecipeSO.Input == _inputKitchenObjectSO)
                return _cuttingRecipeSO;
        }
        return null;
    }
}
