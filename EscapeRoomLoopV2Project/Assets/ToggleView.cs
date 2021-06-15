using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleView : MonoBehaviour
{
    public GameObject view;

    private void Start()
    {
        view.SetActive(false);
    }

    public void Toggle()
    {
        view.SetActive(!view.activeSelf);
    }
}
