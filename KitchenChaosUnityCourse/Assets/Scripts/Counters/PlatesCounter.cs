using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter {

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimerMax = 4f;
    private float spawnPlateTimer;
    private int plateSpawnedAmount;
    private int plateSpawnedAmountMax = 4;

    private void Update() {
        spawnPlateTimer += Time.deltaTime;
        if(!(spawnPlateTimer > spawnPlateTimerMax && GameManager.Instance.IsGamePlaying())) return;
        spawnPlateTimer = 0;

        if(plateSpawnedAmount < plateSpawnedAmountMax) {
            plateSpawnedAmount++;

            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
        }
    }

    public override void Interact(Player _player) {
        if(_player.HasKitchenObject()) return;

        if(plateSpawnedAmount > 0) {
            plateSpawnedAmount--;

            KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, _player);
            OnPlateRemoved?.Invoke(this, EventArgs.Empty);
        }
    }

}
