using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {

    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if(hasProgress == null ) {
            Debug.LogError($"GameObject {hasProgress} does not have a componen that implements IHasProgress!.");
        }

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        barImage.fillAmount = 0;
        Hide();
    }

    private void OnDestroy() {
        hasProgress.OnProgressChanged -= HasProgress_OnProgressChanged;
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        barImage.fillAmount = e.ProgressNormalized;
        if(e.ProgressNormalized == 0f || e.ProgressNormalized == 1f ) { 
            Hide();
        }
        else {
            Show();
        }
    }

    private void Show() => gameObject.SetActive(true);
    private void Hide() => gameObject.SetActive(false);

}
