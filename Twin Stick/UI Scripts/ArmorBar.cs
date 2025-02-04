using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorBar : MonoBehaviour
{
    public Slider armorSlider;
    public void SetMaxArmor(int armor)
    {
        armorSlider.maxValue = armor;
        armorSlider.value = armor;
    }
    public void SetArmor(int armor)
    {
        armorSlider.value = armor;
    }
}
