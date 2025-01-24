using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private readonly T prefab;
    private readonly Transform parent;
    private readonly Queue<T> pool = new Queue<T>();

    public ObjectPool(T prefab, Transform parent = null, int initialSize = 0)
    {
        this.prefab = prefab;
        this.parent = parent;
        
        for (int i = 0; i < initialSize; i++)
        {
            T obj = CreateObject();
            ReturnToPool(obj);
        }
    }
    
    public T GetFromPool()
    {
        if (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        
        return CreateObject();
    }
    
    public void ReturnToPool(T obj)
    {
        obj.transform.SetParent(parent);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
    
    public int Count => pool.Count;
    
    private T CreateObject()
    {
        T obj = Object.Instantiate(prefab, parent);
        obj.gameObject.SetActive(false);
        return obj;
    }
}