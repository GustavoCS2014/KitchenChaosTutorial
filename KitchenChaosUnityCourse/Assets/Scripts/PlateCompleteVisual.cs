using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour {

    [Serializable]
    public struct KitchenObjectSO_GameObject {
        public KitchenObjectSO _kitchenObjectSO;
        public GameObject _gameObject;
    }
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;    
    
    [SerializeField] private PlateKitchenObject plateKitchenObjectSO;

    private void Start() {
        plateKitchenObjectSO.OnIngredientAdded += PlateKitchenObjectSO_OnIngredientAdded;

        foreach(KitchenObjectSO_GameObject _kitchenObjectSOGameObject in kitchenObjectSOGameObjectList) {
            _kitchenObjectSOGameObject._gameObject.SetActive(false);
        }
    }

    private void OnDestroy() {
        plateKitchenObjectSO.OnIngredientAdded -= PlateKitchenObjectSO_OnIngredientAdded;
    }

    private void PlateKitchenObjectSO_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        foreach(KitchenObjectSO_GameObject _kitchenObjectSOGameObject in kitchenObjectSOGameObjectList) {
            if(_kitchenObjectSOGameObject._kitchenObjectSO == e._kitchenObjectSO) {
                _kitchenObjectSOGameObject._gameObject.SetActive(true);
            }
        }
    }
}

