using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class UIControllerStartingScreen : MonoBehaviour
{

    public TMP_InputField IPAddressInputField;

    public static UIControllerStartingScreen instance;  // Singleton

    public void Awake()
    {
        instance = this;
    }

}
