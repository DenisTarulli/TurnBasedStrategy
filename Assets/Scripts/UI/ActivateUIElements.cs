using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateUIElements : MonoBehaviour
{
    [SerializeField] private GameObject[] elementToActivateArray;

    private void Awake()
    {
        for (int i = 0; i < elementToActivateArray.Length; i++)
        {
            elementToActivateArray[i].gameObject.SetActive(true);
        }
    }
}
