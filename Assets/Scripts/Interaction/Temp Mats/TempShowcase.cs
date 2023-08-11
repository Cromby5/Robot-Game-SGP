using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempShowcase : MonoBehaviour
{
    [SerializeField]GameObject buttonObj;
    [SerializeField] GameObject pressurePlateObj;
    [SerializeField] GameObject leverObj;

    public void ButtonTest()
    {
        buttonObj.SetActive(!buttonObj.activeSelf);
    }

    public void PressurePlateTest()
    {
        pressurePlateObj.SetActive(!pressurePlateObj.activeSelf);
    }

    public void LeverTest()
    {
        leverObj.SetActive(!leverObj.activeSelf);
    }
}
