﻿using System;
using System.Collections.Generic;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Model.Items.Runtime
{
    [CreateAssetMenu(fileName = "AllGameItem", menuName = "Scriptable/GameItem/New ALL")]
    public class AllGameItem : ScriptableSettings<AllGameItem>
    {
        public List<GameItem> gameItems;
        private readonly Dictionary<string, GameItem> _dictionary = new();

        private void OnEnable()
        {
            foreach (var item in gameItems) _dictionary.TryAdd(item.Guid, item);
        }

        public GameItem this[string key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }

        private void OnValidate()
        {
            _dictionary.Clear();
            foreach (var item in gameItems) _dictionary.TryAdd(item.Guid, item);
        }


#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void CaptureGuid()
        {
            gameItems.Clear();
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:GameItem");
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var item = UnityEditor.AssetDatabase.LoadAssetAtPath<GameItem>(path);
                gameItems.Add(item);
            }
        }
#endif
    }
}