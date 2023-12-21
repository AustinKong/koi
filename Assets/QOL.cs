using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QOL : MonoBehaviour
{

    public GameObject rocks;
    public GameObject deco;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rocks.SetActive(!rocks.activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            deco.SetActive(!deco.activeInHierarchy);
        }
    }
}
