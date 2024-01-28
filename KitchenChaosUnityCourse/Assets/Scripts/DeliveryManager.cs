using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance;

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;

    private int successfulRecipesAmount = 0;

    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if(!(waitingRecipeSOList.Count < waitingRecipesMax && GameManager.Instance.IsGamePlaying())) return;

            int _randomIndice = UnityEngine.Random.Range(0, recipeListSO.RecipeSOList.Count);
            RecipeSO _waitingRecipeSO = recipeListSO.RecipeSOList[_randomIndice];
            waitingRecipeSOList.Add(_waitingRecipeSO);
            OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
        }
    }

    public void DeliverRecipe(PlateKitchenObject _plateKitchenObject) {
        List<KitchenObjectSO> _plateObjects = _plateKitchenObject.GetKitchenObjectSOList();
        //? Loop all the current orders and check if player is correct.
        foreach(RecipeSO _waitingRecipe in waitingRecipeSOList) {
            //? if the current recipe doesn't match the amount of objects in the plate, stop.
            if(_waitingRecipe.KitchenObjectSOList.Count != _plateObjects.Count)
                continue;
            //? If the current recipe match the one in the plate remove it from the list.
            if(PlateContainsRecipe(_waitingRecipe.KitchenObjectSOList, _plateObjects)) {
                //?Player delivered correct recipe!
                waitingRecipeSOList.Remove(_waitingRecipe);
                successfulRecipesAmount++;
                OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                return;
            }
        }
        //? Player delivered incorrect recipe!
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }
    private bool PlateContainsRecipe(List<KitchenObjectSO> _recipeObjects, List<KitchenObjectSO> _plateObjects) {
        //? Are all the recipe objects on the plate?
        bool _allRecipeItemsOnPlate = _recipeObjects.All(_object => _plateObjects.Contains(_object));
        //? Are all the plate objects on the recipe?
        bool _allPlateItemsInRecipe = _plateObjects.All(_object => _recipeObjects.Contains(_object));
        return _allRecipeItemsOnPlate && _allPlateItemsInRecipe;
    }

    public List<RecipeSO> GetWaitingRecipeSOList() {
        return waitingRecipeSOList;
    }

    private void DeprecatedFunciton(PlateKitchenObject _plateKitchenObject) {

        //TODO Code to refactor!!!!
        foreach(RecipeSO _waitingRecipe in waitingRecipeSOList) {
            if(_waitingRecipe.KitchenObjectSOList.Count == _plateKitchenObject.GetKitchenObjectSOList().Count) {
                //? Has the same number of ingredients.
                bool _plateContentsMatchRecipe = true;
                foreach(KitchenObjectSO _recipeKitchenObjectSO in _waitingRecipe.KitchenObjectSOList) {
                    //? Cycling through all ingredients in the recipe
                    bool _ingredientFound = false;
                    foreach(KitchenObjectSO _plateKitchenObjectSO in _plateKitchenObject.GetKitchenObjectSOList()) {
                        //? Cycling through all ingredients in the plate
                        if(_plateKitchenObjectSO == _recipeKitchenObjectSO) {
                            _ingredientFound = true;
                            break;
                        }
                    }
                    if(!_ingredientFound) {
                        //? this recipe ingredient was not found on the plate.
                        _plateContentsMatchRecipe = false;
                    }
                }
                if(_plateContentsMatchRecipe) {
                    //?Player delivered correct recipe!
                    Debug.Log("Player Delivered correct recipe!");
                    waitingRecipeSOList.Remove(_waitingRecipe);
                    return;
                }
            }
        }

        //? no match found
        Debug.Log("PLAYER STUPID!!");
    }

    public int GetSuccessfulRecipesAmount() => successfulRecipesAmount;
}
