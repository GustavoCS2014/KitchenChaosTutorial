using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> platesVisualsList;

    private void Awake() {
        platesVisualsList = new List<GameObject>();
    }

    private void Start() {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void OnDestroy() {
        platesCounter.OnPlateSpawned -= PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved -= PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e) {
        GameObject _plateGameObject = platesVisualsList[platesVisualsList.Count - 1];
        platesVisualsList.Remove(_plateGameObject);
        Destroy(_plateGameObject);
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e) {
        Transform _plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        float _plateOffsetY = 0.1f;
        _plateVisualTransform.localPosition = new Vector3(0, _plateOffsetY * platesVisualsList.Count, 0);
        platesVisualsList.Add(_plateVisualTransform.gameObject);
    }
}
