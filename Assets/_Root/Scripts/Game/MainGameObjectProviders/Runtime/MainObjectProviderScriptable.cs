﻿using System;
using System.Collections.Generic;
using _Root.Scripts.Game.Inputs.Runtime;
using _Root.Scripts.Game.UiLoaders.Runtime;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.MainGameObjectProviders.Runtime
{
    public class MainObjectProviderScriptable : ScriptableObject
    {
        public AssetReferenceGameObject mainGameObjectAssetReference;
        public GameObject mainGameObjectInstance;
        public LayerMask layerMask;
        public CinemachineCamera virtualCamera;
        public Camera mainCamera;

        [FormerlySerializedAs("uISpawnPointTransform")]
        public Transform uISpawnTransform;

        [Header("Input Actions")] public InputActionReference moveAction;
        public GameObject lastFocusedGameObject;

        private IMoveInputConsumer _lastMoveInputConsumer;
        private Action<GameObject> _spawnedGameObjectCallBack;
        private IUIProvider _lastUiProvider;
        private readonly Dictionary<AssetReferenceGameObject, GameObject> _activeUiElements = new();

        public void SpawnMainGameObject(Camera camera,
            CinemachineCamera cinemachineCamera,
            Action<GameObject> gameObjectCallBack, Transform uiSpawnPoint)
        {
            mainCamera = camera;
            _spawnedGameObjectCallBack = gameObjectCallBack;
            virtualCamera = cinemachineCamera;
            uISpawnTransform = uiSpawnPoint;
            Addressables.InstantiateAsync(mainGameObjectAssetReference).Completed += OnCompletedInstantiate;
        }

        void OnCompletedInstantiate(AsyncOperationHandle<GameObject> handle)
        {
            mainGameObjectInstance = handle.Result;
            ProvideTo(mainGameObjectInstance, mainCamera, virtualCamera, uISpawnTransform);
            _spawnedGameObjectCallBack?.Invoke(mainGameObjectInstance);
            _spawnedGameObjectCallBack = null;
        }

        public void ProvideTo(GameObject gameObject, Camera camera, CinemachineCamera cinemachineCamera,
            Transform uiSpanPoint)
        {
            UnlinkGameObject();
            mainGameObjectInstance = gameObject;
            mainCamera = camera;
            virtualCamera = cinemachineCamera;
            uISpawnTransform = uiSpanPoint;
            AssignVirtualCamera(gameObject, camera, cinemachineCamera);
            AssignGameObject(gameObject);
            AssignMoveInput(gameObject);
            AssignUI(gameObject);
        }

        private void AssignUI(GameObject gameObject)
        {
            gameObject.GetComponent<IUIProvider>().EnableUI(_activeUiElements, uISpawnTransform, gameObject);
        }


        private void AssignVirtualCamera(GameObject gameObject, Camera camera, CinemachineCamera cinemachineCamera)
        {
            if (cinemachineCamera) cinemachineCamera.Follow = mainGameObjectInstance.transform;
            var mainCameraProvider = gameObject.GetComponent<IMainCameraProvider>();
            if (mainCameraProvider == null) return;
            mainCameraProvider.MainCamera = camera;
        }

        private void AssignGameObject(GameObject gameObject)
        {
            if (lastFocusedGameObject != null) lastFocusedGameObject.layer &= ~(layerMask);
            lastFocusedGameObject = gameObject;
            lastFocusedGameObject.layer |= 1 << layerMask;
        }

        private void AssignMoveInput(GameObject gameObject)
        {
            _lastMoveInputConsumer?.DisableMoveInput(moveAction);
            var inputConsumer = gameObject.GetComponent<IMoveInputConsumer>();
            if (inputConsumer == null) return;
            _lastMoveInputConsumer = inputConsumer;
            _lastMoveInputConsumer.EnableMoveInput(moveAction);
        }


        private void UnlinkGameObject()
        {
            _lastMoveInputConsumer?.DisableMoveInput(moveAction);
            if (lastFocusedGameObject != null) lastFocusedGameObject.layer &= ~(1 << layerMask);
            if (lastFocusedGameObject != null) _lastUiProvider.DisableUI(lastFocusedGameObject);
        }

        public void Forget()
        {
            foreach (var activeUiElement in _activeUiElements)
            {
                Addressables.ReleaseInstance(activeUiElement.Value);
            }
            _activeUiElements.Clear();
            
            _lastUiProvider = null;
            _lastMoveInputConsumer = null;
            lastFocusedGameObject = null;
        }
    }
}