using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace HISK {
	public class CoinSpawner : MonoBehaviour {
		[SerializeField] private float spawnCooldown = 1f;
		private float counter = 0f;
		[SerializeField] private int maxAmount = 10;

		[SerializeField] private CollectableObject coinPrefab;
		private List<CollectableObject> activeCoins = new List<CollectableObject>();

		private void Update() {
			counter += Time.deltaTime;
			if (counter >= spawnCooldown) {
				counter -= spawnCooldown;
				SpawnCoin();
			}
		}

		private void SpawnCoin() {
			if (activeCoins.Count >= maxAmount) return;
			CollectableObject coin = Instantiate<CollectableObject>(coinPrefab, GetRandomSpawnPosition(), Quaternion.identity);
			coin.transform.localScale = Vector3.zero;
			float scaleUpDuration = 0.3f;
			coin.transform.DOScale(1f, scaleUpDuration).SetEase(Ease.OutBack)
				.OnComplete(() => {
					coin.IsCollectable = true;
				});
			activeCoins.Add(coin);
			coin.OnCollectedAction += (CollectableObject obj) => {
				activeCoins.Remove(obj);
				coin.OnCollectedAction = null;
			};
		}

		private Vector3 GetRandomSpawnPosition() {
			Vector3 spawnPos = Vector3.zero;
			Vector3 screenPos = new Vector3(Random.Range(0f, 1f) * 0.9f, Random.Range(0f, 1f) * 0.9f, 0f);
			spawnPos = Camera.main.ViewportToWorldPoint(screenPos);
			spawnPos.z = 0f;
			return spawnPos;
		}

	}
}