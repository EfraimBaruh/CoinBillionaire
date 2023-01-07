using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private float spawnTime = 5f;
    [SerializeField] private GameObject menuCoin;
    [SerializeField] private CoinList coinList;
    [Header("Spawn Settings")]
    [SerializeField] private RectTransform spawnArea;
    [SerializeField] private Vector2 maxSpawnPos;
    [SerializeField] private Vector2 minSpawnPos;
    [SerializeField] private int coinSize;

    public CoinList Coins
    {
        get { return coinList; }
    }

    public static CoinSpawner instance;

    #region Publishers
    public Action<Coin> onCoinSpawn, onCoinDespawn;
    #endregion

    private List<Coin> coinsInUse = new();
    private Queue<Coin> _spawn = new();
    private Queue<Coin> _deSpawn = new();


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _spawn = new Queue<Coin>(coinList.coins);
        StartCoroutine(Core());
    }

    private IEnumerator Core()
    {
        yield return null;
        
        while (true)
        {
            var currentCoinCount = _deSpawn.Count + coinsInUse.Count;

            /*Debug.LogError($"Despawn Queue: {_deSpawn.Count}");
            Debug.LogError($"Coins in use Count: {coinsInUse.Count}");*/

            if (currentCoinCount < coinSize)
                SpawnCoin();
            if (currentCoinCount >= coinSize)
                DespawnCoin();

            yield return new WaitForSeconds(spawnTime);
        }
    }

    private void SpawnCoin()
    {
        // TODO: Spawn will be edited.
        Coin spawnCoin = _spawn.Dequeue();
        GameObject coin = Instantiate(menuCoin, spawnArea);

        coin.transform.position = GetSpawnPos();

        MenuCoin menuC = coin.GetComponent<MenuCoin>();
        menuC.Coin = spawnCoin;
        menuC.UpdateSpeed = Random.Range(10, 30);
        menuC.Initialize();

        onCoinSpawn.Invoke(menuC.Coin);

    }

    // TODO: coin dispose system for: keep coins in the portfolio if you have them in wallet, else wait for its time.
    private void DespawnCoin()
    {
        Coin coin = _deSpawn.Dequeue();
        onCoinDespawn.Invoke(coin);
    }

    private Vector3 GetSpawnPos()
    {
        Vector3 spawnpos;

        float posX = Random.Range(0, 2) == 0 ? minSpawnPos.x : maxSpawnPos.x;
        float posY = Random.Range(minSpawnPos.y, maxSpawnPos.y);

        spawnpos = new Vector2(posX, posY);

        return spawnpos;
    }

    public void DOOnCoinUse(Coin coin)
    {
        if(coinsInUse.Contains(coin))
            return;
        
        coinsInUse.Add(coin);
        // Test
        List<Coin> coins = _deSpawn.ToList();
        coins.Remove(coin);
        _deSpawn.Clear();
        _deSpawn = new Queue<Coin>(coins);
    }

    public void DOOnCoinNoUse(Coin coin)
    {
        coinsInUse.Remove(coin);
        _deSpawn.Enqueue(coin);
    }

    private void OnCoinSpawned(Coin coin)
    {
        _deSpawn.Enqueue(coin);
    }

    private void OnCoinDespawned(Coin coin)
    {
        _spawn.Enqueue(coin);
    }
    
    private void OnEnable()
    {
        onCoinSpawn += OnCoinSpawned;
        onCoinDespawn += OnCoinDespawned;
    }
    
    private void OnDisable()
    {
        onCoinSpawn -= OnCoinSpawned;
        onCoinDespawn -= OnCoinDespawned;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        coinsInUse.Clear();
    }
}
