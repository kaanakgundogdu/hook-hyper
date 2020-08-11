using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Hook : MonoBehaviour
{
    public Transform hookedTransform;

    private Camera _mainCamera;
    private int _length;
    private int _strength;
    private int _fishCount;

    private Collider2D _coll;

    private bool _canMove =true;
    private List<Fish> _hookedFishes;
    private Tweener _cameraTween;

	private void Awake()
	{
        _mainCamera = Camera.main;
        _coll = GetComponent<Collider2D>();
        _hookedFishes = new List<Fish>();
		
	}

	void Update()
	{
		if(_canMove && Input.GetMouseButton(0))
		{
            Vector3 myVector = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;
            position.x = myVector.x;
            transform.position = position;
		}
	}

    public void StartFishing()
	{
        _length = IdleManager.instance.length -20; 
        _strength = IdleManager.instance.strength;
        _fishCount = 0;

        float _time = (-_length) * 0.1f;

		_cameraTween = _mainCamera.transform.DOMoveY(_length, 1 + _time * 0.25f, false).OnUpdate(delegate
         {
             if (_mainCamera.transform.position.y <= -11)
             {
				 transform.SetParent(_mainCamera.transform);
             }
         }).OnComplete(delegate
		 {
			 _coll.enabled = true;
			 _cameraTween = _mainCamera.transform.DOMoveY(0, _time * 5, false).OnUpdate(delegate
            {
                if (_mainCamera.transform.position.y >= -25f)
				{
					StopFishing();
				}

			});
		 });

        //Screen(game)
        ScreenManager.instance.ChangeScreen(Screens.GAME);
        _coll.enabled = false;
        _canMove = true;
        _hookedFishes.Clear();

	}
    private void StopFishing()
    {
        _canMove = false;
        _cameraTween.Kill(false);
        _cameraTween = _mainCamera.transform.DOMoveY(0, 2, false).OnUpdate(delegate
        {
            if (_mainCamera.transform.position.y >= -11)
            {
                transform.SetParent(null);
                transform.position = new Vector2(transform.position.x, -6);
            }
        }).OnComplete(delegate
        {
            transform.position = Vector2.down * 6;
            _coll.enabled = true;
            int _num = 0;
            for (int i =0; i< _hookedFishes.Count; i++)
			{
                _hookedFishes[i].transform.SetParent(null);
                _hookedFishes[i].ResetFish();
                _num += _hookedFishes[i].Type.price;
			}
            //IdleManager totalgain = num;
            IdleManager.instance.totalGain = _num;
            //SceenManager End Screen
            ScreenManager.instance.ChangeScreen(Screens.END);
        });

    }

	private void OnTriggerEnter2D(Collider2D target)
	{
		if(target.CompareTag("Fish") && _fishCount != _strength)
		{
            _fishCount++;
            Fish component = target.GetComponent<Fish>();
            component.Hooked();
            _hookedFishes.Add(component);
            target.transform.SetParent(transform);
            target.transform.position = hookedTransform.position;
            target.transform.rotation = hookedTransform.rotation;
            target.transform.localScale = Vector3.one;

            target.transform.DOShakeRotation(5, Vector3.forward * 45, 10, 90, false).SetLoops(1, LoopType.Yoyo).OnComplete(delegate
            {
                target.transform.rotation = Quaternion.identity;
            });
            if (_fishCount == _strength)
			{
                StopFishing();
			}

		}
	}
}
