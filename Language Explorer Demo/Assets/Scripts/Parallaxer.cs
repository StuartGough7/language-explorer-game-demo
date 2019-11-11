using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour
{
    public class PoolObject
    {
        public Transform transform;
        public bool inUse;
        public PoolObject(Transform transfromInstance) { transform = transfromInstance; }
        public void Use() { inUse = true; }
        public void Dispose() { inUse = false; }
    }

    [System.Serializable] // a struct
    public struct YSpawnRange
    {
        public float min;
        public float max;
    }

    public GameObject Prefab;
    GameManager game;
        
    public int poolSize;
    public float shiftSpeed;
    public float spawnRate;

    public YSpawnRange ySpawnRange;

    public Vector3 defaultSpawnPosition;
    public Vector3 immediateSpawnPosition;
    public Vector2 targetAspectRatio; // to account for different screen aspects ratios
    public bool spawnImmediate; // its like particle prewarm

    float spawnTimer;
    float targetAspect;
    PoolObject[] poolObjects;

    void Awake()
    {
        Configure();
    }        
    void Start() // called after awake
    {
        // game manager is defined within awake and so the instance must be called afterwards (ie in Start method)
        game = GameManager.Instance;
    }

    void OnEnable()
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable()
    {
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameOverConfirmed()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].Dispose();
            poolObjects[i].transform.position = Vector3.one * 1000;
        }
        Configure();
    }
    void Update()
    {
        if (game.GameOver) return;
        Shift();
        spawnTimer += Time.deltaTime;
        if(spawnTimer > spawnRate)
        {
            Spawn();
            spawnTimer = 0;
        }
    }

    void Configure()
    {
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        poolObjects = new PoolObject[poolSize]; // instantiate poolObjects
        for (int i = 0; i < poolObjects.Length; i++)
        {
            GameObject gameObject = Instantiate(Prefab) as GameObject; // spawn the prefab (public member of the class) and cast as GameObject type
            Transform t = gameObject.transform;
            t.SetParent(transform); // becaus ethe script was put on the original Game Objects this is almost like bind this in JS. Sets the parent to be the newly spawned GameObject
            t.position = Vector3.one * 1000;
            poolObjects[i] = new PoolObject(t);
        }
        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    }

    void Spawn()
    {
        Transform t = GetPoolObject();
        if (t == null) return; // if true this indicates that poolSize is too small
        Vector3 pos = Vector3.zero;
        pos.x = (defaultSpawnPosition.x * Camera.main.aspect) / targetAspect; // the x position changes based on the target aspect 
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        t.position = pos;
    }

    void SpawnImmediate()
    {
        Transform t = GetPoolObject();
        if (t == null) return; 
        Vector3 pos = Vector3.zero;
        pos.x = (immediateSpawnPosition.x * Camera.main.aspect) / targetAspect; // This will spawn another object off screen to the right
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        t.position = pos;
        Spawn();
    }
    void Shift()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].transform.localPosition += Vector3.right * shiftSpeed * Time.deltaTime;
            CheckDisposeObject(poolObjects[i]);
        }
    }

    void CheckDisposeObject(PoolObject poolObject)
    {
        if(poolObject.transform.position.x < (-defaultSpawnPosition.x * Camera.main.aspect) / targetAspect)
        {
            poolObject.Dispose();
            poolObject.transform.position = Vector3.one * 1000; // somewhere way off screen
        }
    }

    Transform GetPoolObject()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].inUse)
            {
                poolObjects[i].Use(); // same as poolObejcts[i].inUse = true;
                return poolObjects[i].transform;
            }
        }
        return null; // have to return a Transform from this method so null is the default
    }
    
}
