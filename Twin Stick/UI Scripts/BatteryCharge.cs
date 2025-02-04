using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryCharge : MonoBehaviour
{
    public Slider batterySlider;
    public void SetMaxCharge(float charge)
    {
        batterySlider.maxValue = charge;
        batterySlider.value = charge;
    }
    public void SetCharge(float charge)
    {
        batterySlider.value = charge;
    }
}
