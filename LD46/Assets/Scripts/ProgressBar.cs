using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image fillBar;
    
    public void SetFill(float percent) {
        fillBar.fillAmount = percent;
    }
}
