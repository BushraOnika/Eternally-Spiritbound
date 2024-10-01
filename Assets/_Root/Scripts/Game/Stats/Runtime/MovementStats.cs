﻿using System;

namespace _Root.Scripts.Game.Stats.Runtime
{
    [Serializable]
    public class MovementStats<T>
    {
        public T speed;
        public T pickupRange;
        public T detectRange;
    }
}