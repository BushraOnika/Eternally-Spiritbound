﻿using JetBrains.Annotations;
using Pancake;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Model.Assets.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Asset/AssetScript", fileName = "AssetScript")]
    public class AssetScript : StringConstant
    {
        [Guid] [SerializeField] protected string guid;
        [SerializeField] protected AssetReferenceGameObject assetReference;
        [TextArea(3, 10)] [SerializeField] protected string description;
        [CanBeNull] [SerializeField] protected Sprite icon;

        public string Guid => guid;
        public AssetReferenceGameObject AssetReference => assetReference;
        public string Description => description;
        public Sprite Icon => icon;

        public virtual void PresentInInventory(AssetScriptStorageComponent assetScriptStorageComponent, int amount)
        {
        }

        public virtual bool OnTryAddToInventory(
            AssetScriptStorageComponent assetScriptStorageComponent,
            int amount,
            out int addedAmount
        )
        {
            return assetScriptStorageComponent.AssetScriptStorage.TryAdd(this, amount, out addedAmount);
        }

        public virtual bool OnTryRemovedFromInventory(
            AssetScriptStorageComponent assetScriptStorageComponent,
            int amount,
            out int removedAmount
        )
        {
            return assetScriptStorageComponent.AssetScriptStorage.TryRemove(this, amount, out removedAmount);
        }

        public static implicit operator AssetReferenceGameObject(AssetScript assetScript)
        {
            return assetScript.assetReference;
        }
    }
}