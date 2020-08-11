using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Fish : MonoBehaviour
{
    public Fish.FishType fishType;
    private CircleCollider2D _collider;
    private SpriteRenderer _renderer;
    private float _screeenLeft;
    private Tweener _tweener;


    public Fish.FishType Type
	{
		get
		{
            return fishType; //eğer fishtype varsa return ediyoruz.
		}
		set
		{ //yoksa alıp bunları FishType'dan yeniden atıyoruz.
            fishType = value;
            _collider.radius = fishType.colliderRadius;
            _renderer.sprite = fishType.sprite;
		}
	}

	private void Awake()
	{
        _collider = GetComponent<CircleCollider2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _screeenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;

    }


    public void ResetFish()
	{
        if(_tweener !=null)
		{
            _tweener.Kill(false);
		}
        //burası vardiye aralığını patrol uzunlupunu ayarlama
        float num = UnityEngine.Random.Range(fishType.minLength, fishType.maxLength);
        _collider.enabled = true;

        Vector3 newPosition = transform.position;

        newPosition.y = num;
        newPosition.x = _screeenLeft;
        transform.position = newPosition;

        //Hareket etme burada
        float num2 = 1;
        float yPos = UnityEngine.Random.Range(num , num + num2);
        Vector2 newVector = new Vector2(-newPosition.x, yPos);

        float num3 = 3;
        float delay = UnityEngine.Random.Range(0, 2 * num3);
        _tweener = transform.DOMove(newVector, num3, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(delay).OnStepComplete(delegate
        {
            // Balığın hareketlerini ve ters harekette scale'i tersleme işi yapılıyor.
            Vector3 localScale = transform.localScale;
            localScale.x = -localScale.x;
            transform.localScale = localScale;

        });
	}

    public void Hooked()
	{
        _collider.enabled = false;
        _tweener.Kill(false);
	}



    [Serializable]
    public class FishType
    {
        public int price;
        public float fishCount;
        public float minLength;
        public float maxLength;
        public float colliderRadius;
        public Sprite sprite;


    }
}
