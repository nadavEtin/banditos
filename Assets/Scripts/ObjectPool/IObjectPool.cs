using UnityEngine;

namespace ObjectPool
{
    public interface IObjectPool
    {
        void AddObjectToPool(GameObject obj, ObjectTypes type);

        GameObject GetObjectFromPool(ObjectTypes type);
    }
}
