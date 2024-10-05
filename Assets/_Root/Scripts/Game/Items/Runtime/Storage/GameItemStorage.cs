﻿using System;
using Pancake.Common;
using Sirenix.OdinInspector;
using Soul.Serializers.Runtime;
using Soul.Storages.Runtime;

namespace _Root.Scripts.Game.Items.Runtime.Storage
{
    [Serializable]
    public class GameItemStorage : IntStorage<GameItem> 
    {
        public string appendKey = "_string_int";
        public override string StorageKey => $"{Guid}{appendKey}";
        
        [Button]
        public void Load() => LoadData(Guid);

        [Button]
        public void Save() => SaveData();

        public override void LoadData(string guid)
        {
            Guid = guid;
            var datas= Data.Load(StorageKey,ToStringPair(DefaultData));
            SetData(ToGameItemPair(datas));
        }

        public override void SaveData(Pair<GameItem, int>[] data)
        {
            Pair<string, int>[] stringIntData = ToStringPair(data);
            Data.Save(StorageKey, stringIntData);
        }

        public Pair<string, int>[] ToStringPair(Pair<GameItem, int>[] datas)
        {
            var result = new Pair<string, int>[datas.Length];
            for (var i = 0; i < datas.Length; i++)
            {
                var data = datas[i];
                result[i] = new Pair<string, int>(data.Key, data.Value);
            }

            return result;
        }

        public Pair<GameItem, int>[] ToGameItemPair(Pair<string, int>[] datas)
        {
            var result = new Pair<GameItem, int>[datas.Length];
            for (var i = 0; i < datas.Length; i++)
            {
                var data = datas[i];
                result[i] = new Pair<GameItem, int>(AllGameItemSingletonScriptable.Instance[data.Key], data.Value);
            }

            return result;
        }

        public override void ClearStorage() => Data.DeleteKey(StorageKey);
    }
}