using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(Action onInteractionComplete);
}
