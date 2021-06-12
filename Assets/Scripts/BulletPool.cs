using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;
    [SerializeField]
    private GameObject bulletPrefab;
    public GameObject poolUsing;
    Queue<GameObject> bulletQueue = new Queue<GameObject>();
    public int bulletCnt = 100;

    private void Awake()
    {
        Instance = this;
        Initialize(bulletCnt);
    }

    private void Initialize(int initCount)
    {
        for(int i=0; i< initCount; i++)
        {
            bulletQueue.Enqueue(CreateNewBullet());
        }
    }

    private GameObject CreateNewBullet()
    {
        GameObject newObj = Instantiate(bulletPrefab);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static GameObject GetBullet()
    {
        if(Instance.bulletQueue.Count > 0)
        {
            GameObject obj = Instance.bulletQueue.Dequeue();
            obj.transform.SetParent(Instance.poolUsing.transform);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instance.CreateNewBullet();
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(Instance.poolUsing.transform);
            return obj;
        }
    }

    public static void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.bulletQueue.Enqueue(obj);
    }
}
