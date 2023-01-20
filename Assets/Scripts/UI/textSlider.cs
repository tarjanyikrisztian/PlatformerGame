using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class textSlider : MonoBehaviour
{
    public TextMeshProUGUI sliderText;

    public Slider slider;

    void Start()
    {
        sliderText.text = slider.value.ToString();
    }

    public void UpdateText(float value)
    {
        sliderText.text = value.ToString();
    }

}
