using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePotion : MonoBehaviour
{
    //[SerializeField] protected Sprite spriteVisual;
    [SerializeField] protected Color potionColorUI;

    public abstract void ConsumePotion();
    public Color GetColor()
    {
        return potionColorUI;
    }
}
