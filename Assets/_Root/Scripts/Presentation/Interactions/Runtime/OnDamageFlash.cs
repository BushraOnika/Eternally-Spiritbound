﻿using _Root.Scripts.Game.Consmatics.Runtime;
using _Root.Scripts.Game.GameEntities.Runtime;
using _Root.Scripts.Game.GameEntities.Runtime.Healths;
using Pancake.Common;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Presentation.Interactions.Runtime
{
    public class DamageFlash : MonoBehaviour<FlashConfigScript>
    {
        private FlashConfigScript _flashConfigScript;
        private Renderer _targetRenderer;
        private IHealth _health;
        private EntityStatsComponent _entityStatsComponent;
        private Material _defaultMaterial;
        private DelayHandle _delayHandle;

        protected override void Init(FlashConfigScript argument)
        {
            _flashConfigScript = argument;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _health = GetComponent<IHealth>();
            _entityStatsComponent = GetComponent<EntityStatsComponent>();
            _targetRenderer = GetComponentInChildren<Renderer>();
            _defaultMaterial = _targetRenderer.material;
        }

        public void OnEnable()
        {
            _defaultMaterial = _targetRenderer.material;
            _health.HealthReference.current.OnChange += OnHealthChange;
            _entityStatsComponent.entityStats.vitality.health.current.OnChange += OnHealthChange;
        }
        
        public void OnDisable()
        {
            _health.HealthReference.current.OnChange -= OnHealthChange;
            _entityStatsComponent.entityStats.vitality.health.current.OnChange -= OnHealthChange;
            _delayHandle?.Cancel();
            Restore();
        }

        private void OnHealthChange(float old, float current)
        {
            _flashConfigScript.Flash(_targetRenderer);
            _delayHandle = App.Delay(_flashConfigScript.flashDuration, Restore, useRealTime: true);
        }

        private void Restore() => _flashConfigScript.Restore(_targetRenderer, _defaultMaterial);
    }
}