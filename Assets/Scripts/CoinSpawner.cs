using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinSpawner : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private float spawnTime = 5f;
    [SerializeField] private float despawnTime = 30f;
    [SerializeField] private GameObject menuCoinPrefab;
    [SerializeField] private CoinList coinList;
    [SerializeField] private CloudyMessagesScriptable cloudyMessages;
    
    [Header("Spawn Settings")]
    [SerializeField] private RectTransform spawnArea;
    [SerializeField] private RectTransform walletArea;
    [SerializeField] private Transform walletEntrance;
    [SerializeField] private Vector2 maxSpawnPos;
    [SerializeField] private Vector2 minSpawnPos;
    [SerializeField] private int maxSpawnCount = 5;
    #endregion

    #region Properties
    public static CoinSpawner Instance { get; private set; }
    public Transform WalletEntrance => walletEntrance;
    public Transform WalletArea => walletArea;
    public Transform SpawnArea => spawnArea;
    public CoinList Coins => coinList;
    #endregion

    #region Events
    public Action<MenuCoin> onCoinSpawn;
    public Action<Coin> onCoinDespawn;
    #endregion

    #region Private Fields
    private List<MenuCoin> _coinsInUse = new();
    private Queue<Coin> _spawnQueue = new();
    private Queue<MenuCoin> _despawnQueue = new();
    #endregion

    #region Unity Lifecycle Methods
    private void Awake() => Instance = this;

    private void Start()
    {
        _spawnQueue = new Queue<Coin>(coinList.coins.FindAll(coin => coin.isUnlocked));
        StartCoroutine(SpawnRoutine());
        StartCoroutine(DespawnRoutine());
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
        _coinsInUse.Clear();
    }
    #endregion

    #region Spawn Management
    /// <summary>
    /// Main coroutine that manages coin spawning based on coin size limit
    /// </summary>
    private IEnumerator SpawnRoutine()
    {
        yield return null;
        
        while (true)
        {
            int currentActiveCoins = _coinsInUse.Count + _despawnQueue.Count;
            
            if (currentActiveCoins < maxSpawnCount && _spawnQueue.Count > 0)
            {
                SpawnCoin();
                Debug.Log($"Active coins: {currentActiveCoins + 1}/{maxSpawnCount}");
            }
            else if (_spawnQueue.Count == 0)
            {
                // Refresh spawn queue with unlocked coins
                _spawnQueue = new Queue<Coin>(coinList.coins.FindAll(coin => coin.isUnlocked));
                Debug.Log($"Refreshed spawn queue. {_spawnQueue.Count} coins available to spawn.");
            }
            else
            {
                Debug.Log($"Max spawn count reached ({maxSpawnCount}). Waiting for coins to despawn. " +
                         $"Coins in queue: {_spawnQueue.Count}");
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }

    /// <summary>
    /// Spawns a single coin with random position and message
    /// </summary>
    private void SpawnCoin()
    {
        if (_spawnQueue.Count == 0) return;

        Coin spawnCoin = _spawnQueue.Dequeue();
        GameObject coinObj = Instantiate(menuCoinPrefab, spawnArea);
        coinObj.transform.position = GetRandomSpawnPosition();

        MenuCoin menuCoin = coinObj.GetComponent<MenuCoin>();
        menuCoin.Coin = spawnCoin;
        menuCoin.Initialize(GetRandomCloudyMessage());

        Debug.Log($"Spawned coin: {spawnCoin.Name} (ID: {spawnCoin.id})");
        onCoinSpawn?.Invoke(menuCoin);
    }

    /// <summary>
    /// Returns a random spawn position within the defined bounds
    /// </summary>
    private Vector3 GetRandomSpawnPosition()
    {
        float posX = Random.Range(minSpawnPos.x, maxSpawnPos.x);
        float posY = Random.Range(minSpawnPos.y, maxSpawnPos.y);
        return new Vector2(posX, posY);
    }

    /// <summary>
    /// Returns a random message from the cloudy messages list
    /// </summary>
    private string GetRandomCloudyMessage() => 
        cloudyMessages.Messages[Random.Range(0, cloudyMessages.Messages.Count)];

    private IEnumerator DespawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(despawnTime);
            
            if (_despawnQueue.Count > 0)
            {
                MenuCoin menuCoinToDespawn = _despawnQueue.Dequeue();
                if (menuCoinToDespawn != null)
                {
                    Debug.Log($"Despawning coin: {menuCoinToDespawn.Coin.Name} (ID: {menuCoinToDespawn.Coin.id})");
                    onCoinDespawn?.Invoke(menuCoinToDespawn.Coin);
                }
            }
        }
    }
    #endregion

    #region Coin State Management
    /// <summary>
    /// Handles coin usage state - removes from despawn queue when in use
    /// </summary>
    public void DOOnCoinUse(Coin coin)
    {
        MenuCoin menuCoin = _despawnQueue.FirstOrDefault(mc => mc.Coin == coin);
        if (menuCoin != null && !_coinsInUse.Contains(menuCoin))
        {
            _coinsInUse.Add(menuCoin);
            RemoveCoinFromDespawnQueue(menuCoin);
            Debug.Log($"Coin in use: {coin.Name} (ID: {coin.id})");
        }
    }

    /// <summary>
    /// Handles coin release state - adds back to despawn queue when no longer in use
    /// </summary>
    public void DOOnCoinNoUse(Coin coin)
    {
        MenuCoin menuCoin = _coinsInUse.FirstOrDefault(mc => mc.Coin == coin);
        if (menuCoin != null)
        {
            _coinsInUse.Remove(menuCoin);
            _despawnQueue.Enqueue(menuCoin);
            Debug.Log($"Coin released from use: {coin.Name} (ID: {coin.id})");
        }
    }

    public void AddCoinToSpawnQueue(Coin coin)
    {
        _spawnQueue.Enqueue(coin);
        Debug.Log($"Asserted coin to the spawn queue: {coin.id}");
    }

    private void RemoveCoinFromDespawnQueue(MenuCoin menuCoin)
    {
        var coins = _despawnQueue.ToList();
        if (coins.Remove(menuCoin))
        {
            _despawnQueue = new Queue<MenuCoin>(coins);
            Debug.Log($"Removed from despawn queue: {menuCoin.Coin.Name} (ID: {menuCoin.Coin.id})");
        }
    }

    private void OnCoinSpawned(MenuCoin menuCoin) => _despawnQueue.Enqueue(menuCoin);

    private void OnCoinDespawned(Coin coin)
    {
        _spawnQueue.Enqueue(coin);
        Debug.Log($"Added back to spawn queue: {coin.Name} (ID: {coin.id})");
    }
    #endregion
}
