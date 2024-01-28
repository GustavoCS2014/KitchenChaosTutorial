using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress {


    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs {
        public State _state;
    }

    public enum State {
        Idle,
        Frying,
        Fried,
        Burned,
        Done,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {

        if(HasKitchenObject()) {
            switch(state){
                case State.Idle:
                    break;
                case State.Frying:
                    Frying();
                    if(!(fryingTimer > fryingRecipeSO.FryingTimerMax)) break;

                    state = State.Fried;
                    burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    burningTimer = 0f;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { _state = state });

                    break;
                case State.Fried:
                    //? If can't burn, skip and turn off the stove
                    if(!CanBurn(GetKitchenObject().GetKitchenObjectSO())) {
                        state = State.Done;

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { ProgressNormalized = 0f });
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { _state = state });
                        return;
                    }

                    Burning();
                    if(!(burningTimer > burningRecipeSO.BurningTimerMax)) break;

                    state = State.Burned;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { ProgressNormalized = 0f });
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { _state = state });
                    break;
                case State.Burned:
                    break;
                case State.Done:
                    break;
            }
        }
    }

    private void Frying() {
        fryingTimer += Time.deltaTime;
        //? Sending Event
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            ProgressNormalized = fryingTimer / fryingRecipeSO.FryingTimerMax
        });

        if(!(fryingTimer > fryingRecipeSO.FryingTimerMax)) return;

        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(fryingRecipeSO.Output, this);
    }

    private void Burning() {
        burningTimer += Time.deltaTime;
        //? Sending Event
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            ProgressNormalized = burningTimer / burningRecipeSO.BurningTimerMax
        });

        if(!(burningTimer > burningRecipeSO.BurningTimerMax)) return;

        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(burningRecipeSO.Output, this);
    }

    public override void Interact(Player _player) {
        //! Counter and Player are holding object.
        if(_player.HasKitchenObject() && HasKitchenObject()) {

            //? Picking the object if the player has a plate.
            if(_player.GetKitchenObject().TryGetPlate(out PlateKitchenObject _plateKitchenObject)) {
                if(!(_plateKitchenObject.TryAddIngridient(GetKitchenObject().GetKitchenObjectSO()))) return;
                GetKitchenObject().DestroySelf();
                state = State.Idle;

                //? Events
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { ProgressNormalized = 0f });
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { _state = state });
            }
            return;
        }

        //! Counter has object and player not.
        if(!_player.HasKitchenObject() && HasKitchenObject()) {
            //? Give the item.
            GetKitchenObject().SetKitchenObjectParent(_player);
            state = State.Idle;

            //? Events
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { ProgressNormalized = 0f });
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { _state = state });
            return;
        }

        //! Counter has object.
        if(HasKitchenObject()) {

            return;
        }

        //! Player is Holding something and counter not.
        if(_player.HasKitchenObject() && !HasKitchenObject()) {
            //? if item has a recipe, place it.
            if(HasRecipieWithInput(_player.GetKitchenObject().GetKitchenObjectSO())) {
                _player.GetKitchenObject().SetKitchenObjectParent(this);

                //? Get the recipe and start Frying
                fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                state = State.Frying;
                fryingTimer = 0;

                //? Events
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { _state = state });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    ProgressNormalized = (float)fryingTimer / fryingRecipeSO.FryingTimerMax
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

    //? Helping Methods

    private bool HasRecipieWithInput(KitchenObjectSO _inputKitchenObjectSO) {
        FryingRecipeSO _fryingRecipeSO = GetFryingRecipeSOWithInput(_inputKitchenObjectSO);
        return _fryingRecipeSO != null ? true : false;
    }   

    private bool CanBurn(KitchenObjectSO _inputKitchenObjectSO) {
        BurningRecipeSO _burningRecipeSO = GetBurningRecipeSOWithInput(_inputKitchenObjectSO);
        return _burningRecipeSO != null ? true : false;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO _inputKitchenObjectSO) {
        FryingRecipeSO _fryingRecipeSO = GetFryingRecipeSOWithInput(_inputKitchenObjectSO);
        return _fryingRecipeSO != null ? _fryingRecipeSO.Output : null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO _inputKitchenObjectSO) {
        foreach(FryingRecipeSO _fryingRecipeSO in fryingRecipeSOArray) {
            if(_fryingRecipeSO.Input == _inputKitchenObjectSO)
                return _fryingRecipeSO;
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO _inputKitchenObjectSO) {
        foreach(BurningRecipeSO _burningRecipeSO in burningRecipeSOArray) {
            if(_burningRecipeSO.Input == _inputKitchenObjectSO)
                return _burningRecipeSO;
        }
        return null;
    }

    public bool IsFried() => state == State.Fried;

}
