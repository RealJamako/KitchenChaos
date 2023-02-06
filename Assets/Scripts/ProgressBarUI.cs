using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityRandom = UnityEngine.Random;

[DisallowMultipleComponent]
public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private float progressSmoothing;
    [SerializeField] private GameObject hasProgressGameobject;
    [SerializeField] private GameObject progressBarGameObject;
    [SerializeField] private Image progressBarImage;

    private IHasProgress hasProgress;

    private void Awake()
    {
        progressBarImage.fillAmount = 0f;
    }

    private void Start()
    {
        hasProgress = hasProgressGameobject.GetComponent<IHasProgress>();
        if (hasProgress == null) { Debug.LogError("The gameobject supplied does not have IHasProgress assigned"); }
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged; ;
        ShowOrHideVisual(false);
    }

    private void HasProgress_OnProgressChanged(object sender, OnCounterProgressChangedArgs e)
    {
        StartCoroutine(LerpAndSetProgress(e.ProgressNormalized, progressSmoothing));
        ShowOrHideVisual(e.ProgressNormalized > 0f);
    }

    private void OnDisable()
    {
        hasProgress.OnProgressChanged -= HasProgress_OnProgressChanged;
    }

    private IEnumerator LerpAndSetProgress(float progress, float smoothing)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / smoothing;
            if (t > 1) t = 1;
            progressBarImage.fillAmount = progress >= 1 ? 0 : Mathf.Lerp(progressBarImage.fillAmount, progress, t);
            yield return null;
        }
    }

    private void ShowOrHideVisual(bool set)
    {
        progressBarGameObject.SetActive(set);
    }
}