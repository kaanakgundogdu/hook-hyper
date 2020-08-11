using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
	private void Awake()
	{
		for (int i =0; i < _fishTypes.Length; i++)
		{
			int _num = 0;
			while(_num < _fishTypes[i].fishCount)
			{
				Fish _fish = UnityEngine.Object.Instantiate<Fish>(_fishPrefab);
				_fish.Type = _fishTypes[i];
				_fish.ResetFish();
				_num++;
			}
		}
	}

	[SerializeField]
	private Fish _fishPrefab;
	[SerializeField]
	private Fish.FishType[] _fishTypes;



}
