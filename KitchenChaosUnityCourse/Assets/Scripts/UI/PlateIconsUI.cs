using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }
    private void Start() {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void OnDestroy() {
        plateKitchenObject.OnIngredientAdded -= PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach(Transform child in transform) {
            if(child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach(KitchenObjectSO _kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
            Transform _iconTransform = Instantiate(iconTemplate, transform);
            _iconTransform.gameObject.SetActive(true);
            _iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(_kitchenObjectSO);
        }
    }
}
