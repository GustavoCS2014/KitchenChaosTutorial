using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
        public KitchenObjectSO _kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake() {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngridient(KitchenObjectSO _kitchenObjectSO) {
        //? Not a valid ingredient
        if(!validKitchenObjectSOList.Contains(_kitchenObjectSO)) return false;
        //? Already has that ingredient
        if(kitchenObjectSOList.Contains(_kitchenObjectSO)) return false;

        kitchenObjectSOList.Add(_kitchenObjectSO);

        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { _kitchenObjectSO = _kitchenObjectSO});
        return true;
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList() => kitchenObjectSOList;
}
