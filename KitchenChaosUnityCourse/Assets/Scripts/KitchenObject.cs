using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public KitchenObjectSO GetKitchenObjectSO() => kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public void SetKitchenObjectParent(IKitchenObjectParent _kitchenObjectParent) {
        if(kitchenObjectParent != null) 
            kitchenObjectParent.ClearKitchenObject();
        
        kitchenObjectParent = _kitchenObjectParent;

        if(kitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("kitchenObjectParent Has an object!");
        }

        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = _kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent() => kitchenObjectParent;

    public void DestroySelf() {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject _plateKitchenObject) {
        if(this is PlateKitchenObject) {
            _plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else {
            _plateKitchenObject = null;
            return false;
        }
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO _kitchenObjectSO, IKitchenObjectParent _kitchenObjectParent) {
        Transform _kitchenObjectTransform = Instantiate(_kitchenObjectSO.Prefab);
        KitchenObject _kitchenObject = _kitchenObjectTransform.GetComponent<KitchenObject>();
        _kitchenObject.SetKitchenObjectParent(_kitchenObjectParent); 

        return _kitchenObject;
    }
}

