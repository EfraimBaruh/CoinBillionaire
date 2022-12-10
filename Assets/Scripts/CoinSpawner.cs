using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    private List<Coin> coinsInUse = new List<Coin>();
    private Queue<Coin> Spawn = new Queue<Coin>();
    private Queue<Coin> DeSpawn = new Queue<Coin>();


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        maxSpawnPos = spawnArea.position + (Vector3)(spawnArea.rect.max);
        minSpawnPos = spawnArea.position + (Vector3)(spawnArea.rect.min);

        Spawn = new Queue<Coin>(coinList.coins);
        StartCoroutine(Core());
    }

    private IEnumerator Core()
    {
        while (true)
        {
            var currentCoinCount = DeSpawn.Count + coinsInUse.Count;

            if (currentCoinCount < coinSize)
                SpawnCoin();
            if (currentCoinCount >= coinSize)
                DespawnCoin();

            yield return new WaitForSeconds(spawnTime);
        }
    }

    private void SpawnCoin()
    {
        Coin spawnCoin = Spawn.Dequeue();
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
        Coin coin = DeSpawn.Dequeue();
        onCoinDespawn.Invoke(coin);
    }

    private Vector3 GetSpawnPos()
    {
        Vector3 spawnpos = spawnArea.position;

        float posX = (int)Random.Range(0, 2) == 0 ? minSpawnPos.x : maxSpawnPos.x;
        float posY = Random.Range(minSpawnPos.y, maxSpawnPos.y);

        spawnpos = new Vector2(posX, posY);

        return spawnpos;
    }

    public void onCoinUse(Coin coin)
    {
        if(coinsInUse.Contains(coin))
            return;
        
        coinsInUse.Add(coin);
        // Test
        List<Coin> coins = DeSpawn.ToList();
        coins.Remove(coin);
        DeSpawn.Clear();
        DeSpawn = new Queue<Coin>(coins);
    }

    public void onCoinNoUse(Coin coin)
    {
        coinsInUse.Remove(coin);
        DeSpawn.Enqueue(coin);
    }

    private void OnCoinSpawned(Coin coin)
    {
        DeSpawn.Enqueue(coin);
    }

    private void OnCoinDespawned(Coin coin)
    {
        Spawn.Enqueue(coin);
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
