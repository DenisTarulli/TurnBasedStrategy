using System;

public interface IInteractable
{
    void Interact(Action onInteractionComplete); 
    SoundManager.SoundType GetInteractSound();
}
