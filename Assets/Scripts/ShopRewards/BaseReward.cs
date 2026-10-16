using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseReward : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private Color spriteColor;

    public abstract void Behaviour();

    public Sprite GetSprite()
    {
        return sprite;
    }

    public Color GetSpriteColor()
    {
        return spriteColor;
    }
}
