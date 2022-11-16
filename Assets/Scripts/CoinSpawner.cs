using System;
using System.Collections;
using System.Collections.Generic;
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

    public CoinList Coins
    {
        get { return coinList; }
    }

    public static CoinSpawner instance;
    public Action<Coin> onCoinSpawn, onCoinDespawn;

    private List<int> coinsInUse = new List<int>();

    private float counter = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        maxSpawnPos = spawnArea.position + (Vector3)(spawnArea.rect.max);
        minSpawnPos = spawnArea.position + (Vector3)(spawnArea.rect.min);
        StartCoroutine(SpawnCoin());
    }

    private IEnumerator SpawnCoin()
    {
        while (true)
        {
            GameObject coin = Instantiate(menuCoin, spawnArea);
            
            coin.transform.position = GetSpawnPos();
            
            // TODO:Handle which coin to spawn
            int coinIndex = Utils.GetRandomCoin(coinList.coins.Count, coinsInUse);

            MenuCoin menuC = coin.GetComponent<MenuCoin>();
            menuC.Coin = coinList.coins[coinIndex];
            menuC.UpdateSpeed = Random.Range(10, 30);
            menuC.Initilaze();
            onCoinSpawn.Invoke(menuC.Coin);
            
            yield return new WaitForSecondsRealtime(spawnTime);
            
        }
    }

    // TODO: coin dispose system for: keep coins in the portfolio if you have them in wallet, else wait for its time.
    private void DespawnCoin(Coin coin)
    {
        if (coinsInUse.Count < 15)
            return;
        
        MenuCoin m_coin = spawnArea.GetChild(0).GetComponent<MenuCoin>();
        onCoinDespawn.Invoke(m_coin.Coin);
        Destroy(spawnArea.GetChild(0));

    }

    private Vector3 GetSpawnPos()
    {
        Vector3 spawnpos = spawnArea.position;

        float posX = (int)Random.Range(0, 2) == 0 ? minSpawnPos.x : maxSpawnPos.x;
        float posY = Random.Range(minSpawnPos.y, maxSpawnPos.y);

        spawnpos = new Vector2(posX, posY);

        return spawnpos;
    }

    private void OnCoinSpawned(Coin coin)
    {
        coinsInUse.Add(coin.id);
    }

    private void OnCoinDespawned(Coin coin)
    {
        coinsInUse.Remove(coin.id);
    }
    
    private void OnEnable()
    {
        onCoinSpawn += OnCoinSpawned;
        onCoinSpawn += DespawnCoin;
        onCoinDespawn += OnCoinDespawned;
    }
    
    private void OnDisable()
    {
        onCoinSpawn -= OnCoinSpawned;
        onCoinSpawn -= DespawnCoin;
        onCoinDespawn -= OnCoinDespawned;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        coinsInUse.Clear();
    }
}
