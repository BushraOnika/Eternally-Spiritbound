using System;
using Pancake;
using Soul2.Storages.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul2.Items.Runtime
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Item", menuName = "Soul2/Item")]
    public class Item : ScriptableObject, IItem
    {
        [Guid] string guid;
        public string itemName;
        [TextArea(3, 10)] public string description;
        public AssetReferenceGameObject assetReferenceGameObject;
        public Sprite icon;
        public int maxStack;
        public bool consumable;

        public string Guid => guid;
        public string ItemName => itemName;
        public string Description => description;
        public AssetReferenceGameObject AssetReferenceGameObject => assetReferenceGameObject;
        public Sprite Icon => icon;
        public bool Consumable => consumable;
        public bool IsStackable => maxStack > 1;
        public int MaxStack => maxStack;

        public virtual bool TryPick(GameObject picker, IStorageBase<IItem> storageBase, int amount = 1)
        {
            return storageBase.TryAdd(this, amount, out var added);
        }

        public virtual bool TryUse(GameObject user, IStorageBase<IItem> storageBase, int amount = 1)
        {
            return Consumable && storageBase.TryRemove(this, amount, out int removed);
        }

        public virtual bool TryDrop(GameObject dropper, IStorageBase<IItem> storageBase, int amount = 1)
        {
            return storageBase.TryRemove(this, amount, out int removed);
        }
    }
}