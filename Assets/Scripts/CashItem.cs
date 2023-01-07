using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CashItem : MonoBehaviour
{
    public CashAble cashAble;

    [SerializeField] private AudioClip clip;
    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void StartMovement()
    {
        transform.position = cashAble._locker.position + RandomOffset();
        StartCoroutine(MoveCash());
    }

    private IEnumerator MoveCash()
    {
        yield return new WaitForSeconds(RandomWaitTime());
        transform.DOMove(cashAble._target.position, 0.8f).OnComplete(() =>
        {
            _source.PlayOneShot(clip);
        });
    }

    private Vector3 RandomOffset()
    {
        var size = new Vector2(1f, 1f);
        float x = Random.Range(-size.x / 2, size.x / 2);
        float y = Random.Range(-size.y / 2, size.y / 2);
        return new Vector2(x,y);
    }

    private float RandomWaitTime()
    {
        float t = Random.Range(0.1f, 0.5f);
        return t;
    }
    
}

public class CashAble
{
    public int _stock;
    public Transform _locker;
    public Transform _target;

    public CashAble(int stock, Transform locker, Transform target)
    {
        _stock = stock;
        _locker = locker;
        _target = target;
    }
}
