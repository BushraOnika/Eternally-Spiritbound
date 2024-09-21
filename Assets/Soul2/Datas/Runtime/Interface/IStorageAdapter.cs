﻿namespace Soul2.Datas.Runtime.Interface
{
    public interface IStorageAdapter<in T> : IStorageIdentifier
    {
        void LoadData(string guid);
        void SetData(T data);
        void SaveData(T data);
        void SaveData();
    }
}