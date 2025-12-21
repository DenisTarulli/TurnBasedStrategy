using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseReward : MonoBehaviour
{
    [SerializeField] private Image sprite;
    [SerializeField] private Color spriteColor;

    public abstract void Behaviour();

    public Image GetImage()
    {
        return sprite;
    }

    public Color GetSpriteColor()
    {
        return spriteColor;
    }
}
