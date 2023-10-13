using System.Collections.Generic;
using UnityEngine;

namespace Common.Pool
{
    public class MultiInstancePool<TKey, TItem> where TItem : MonoBehaviour
    {
        private readonly Dictionary<TKey, MonoBehaviourPool<TItem>> _pools = new();
        private readonly Transform _parent;

        public MultiInstancePool(Transform parent)
        {
            _parent = parent;
        }

        public TItem Get(TKey key, TItem prefab)
        {
            if (!_pools.ContainsKey(key))
            {
                _pools[key] = new MonoBehaviourPool<TItem>(prefab, _parent);
            }

            return _pools[key].Get();
        }

        public void Release(TKey key, TItem item) 
            => _pools[key].Release(item);
    }
}