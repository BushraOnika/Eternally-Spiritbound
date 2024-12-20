﻿using System;
using System.Collections.Generic;
using Soul.QuickPickup.Runtime;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.QuickPickup.Runtime.Handlers
{
    [Serializable]
    public class PickupRecycleHandlerBase<T> : PickupHandlerBase<PickupContainer<T>>
    {
        public List<Pair<PickupContainer<T>, float>> controllers;
        public int delay;
        public Func<PickupContainer<T>, bool> onRecycle;

        public override void Handle(PickupContainer<T> responsibility)
        {
            if (onRecycle.Invoke(responsibility))
            {
                responsibility.transform.gameObject.SetActive(false);
                controllers.Add(new Pair<PickupContainer<T>, float>(responsibility, delay));
            }
        }

        public override void Process()
        {
            for (var index = controllers.Count - 1; index >= 0; index--)
            {
                var controller = controllers[index];
                controller.Second -= Time.deltaTime;
                if (controller.Second <= 0)
                {
                    controllers.RemoveAt(index);
                    HandleNext(controller.First);
                }
            }
        }

        public override void Dispose()
        {
            controllers.Clear();
        }

#if UNITY_EDITOR

        [SerializeField] protected Color gizmoColor = Color.yellow;

        public override void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            foreach (var controller in controllers)
            {
                Gizmos.DrawWireSphere(controller.First.transform.position, 1);
            }
        }

#endif
    }
}