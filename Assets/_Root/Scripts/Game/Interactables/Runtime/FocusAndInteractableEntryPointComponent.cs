﻿using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    [DisallowMultipleComponent]
    [SelectionBase]
    public class FocusAndInteractableEntryPointComponent : MonoBehaviour, IFocusEntryPoint
    {
        [SerializeField] private bool isMain;
        [SerializeField] private bool isFocused;
        
        [FormerlySerializedAs("focusProcessor")] [SerializeField]
        private FocusProcessorScript focusProcessorScript;
        [SerializeField] private FocusManagerScript focusManagerScript;

        public event Action<GameObject> OnPushFocus;
        public event Action<GameObject> OnRemoveFocus;

        public bool IsMain => isMain;
        public GameObject GameObject => gameObject;

        public bool IsFocused
        {
            get => isFocused;
            set
            {
                if (isFocused == value) return;
                isFocused = value;
                if (value) FocusSelf();
                else RemoveFocus(GameObject);
            }
        }

        public void PushFocus(FocusReferences focusReferences)
        {
            isFocused = true;
            OnPushFocus?.Invoke(gameObject);
            focusProcessorScript.SetFocus(focusReferences);
        }

        public void RemoveFocus(GameObject targetGameObject)
        {
            isFocused = false;
            focusProcessorScript.OnFocusLost(targetGameObject);
            OnPushFocus?.Invoke(gameObject);
        }

        private void FocusSelf()
        {
            focusManagerScript.PushFocus(this);
        }
    }
}