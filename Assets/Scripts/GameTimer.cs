using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    private void Update()
    {
        fillImage.fillAmount = GameManager.Instance.CountDownTimerNormalized;
    }
}
