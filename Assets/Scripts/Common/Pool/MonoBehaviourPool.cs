using UnityEngine;
using UnityEngine.Pool;

namespace Common.Pool
{
    public class MonoBehaviourPool<T> where T : MonoBehaviour
    {
        private readonly ObjectPool<T> _pool;
        private readonly T _prefab;
        private readonly Transform _parent;

        public MonoBehaviourPool(T prefab, Transform parent, int defaultCount = 4)
        {
            _parent = parent;
            _prefab = prefab;
            _pool = new ObjectPool<T>(Create, OnGet, OnRelease, defaultCapacity: defaultCount);
        }

        private void OnGet(T item)
        {
            item.gameObject.SetActive(true);
        }

        private void OnRelease(T item)
        {
            item.gameObject.SetActive(false);
        }

        public T Get() => _pool.Get();

        public void Release(T item) => _pool.Release(item);

        private T Create()
        {
            var newInstance = Object.Instantiate(_prefab, _parent);
            newInstance.gameObject.SetActive(false);
            return newInstance;
        }
    }
}