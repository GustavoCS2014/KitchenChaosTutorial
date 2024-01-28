using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO _recipeSO) {
        recipeNameText.text = _recipeSO.RecipeName;

        foreach(Transform _child in iconContainer) {
            if(_child == iconTemplate) continue;
            Destroy(_child);
        }

        foreach(KitchenObjectSO _kitchenObjectSO in _recipeSO.KitchenObjectSOList) {
            Transform _iconTransform = Instantiate(iconTemplate, iconContainer);
            _iconTransform.gameObject.SetActive(true);
            _iconTransform.GetComponent<Image>().sprite = _kitchenObjectSO.Icon;
        }
    }

}
