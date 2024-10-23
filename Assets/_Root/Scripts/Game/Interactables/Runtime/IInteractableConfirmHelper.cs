﻿using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public interface IInteractableConfirmHelper
    {
        public GameObject GameObject { get; }
        public void Active(IInteractable interactable);
        public void Hide();
    }
}