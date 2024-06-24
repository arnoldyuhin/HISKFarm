using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HISK {
	public class TreeObject : InteractableObject {
		[SerializeField] private int health = 3;
		[SerializeField] private CollectableObject woodPrefab;
		[SerializeField] private Vector2Int spawnWoodRange = new Vector2Int(2, 4);

		public Action<TreeObject> OnDiedAction;

		public void Reset() {
			health = 3;
		}

		public override void Interact() {
			if (IsInteractable == false) return;
			if (health > 0) {
				health--;
				CameraShaker.Instance.Shake(0.05f, 0.15f);
			}
			if (health == 0) {
				SpawnWoods();
				Die();
			}
		}

		private void SpawnWoods() {
			int spawnAmount = Random.Range(spawnWoodRange.x, spawnWoodRange.y + 1);
			for (int i = 0; i < spawnAmount; i++) {
				CollectableObject wood = Instantiate<CollectableObject>(woodPrefab, this.transform.position, Quaternion.identity);
				float spawnDuration = 0.5f;
				Vector3 startPos = this.transform.position + Vector3.up * 1f;
				Vector3 targetPos = GetRandomWoodPosition();
				wood.IsCollectable = false;
				wood.transform.DOMove(startPos + (targetPos - startPos) * 0.5f + Vector3.up * 1f, spawnDuration * 0.5f)
					.SetEase(Ease.OutQuad);
				wood.transform.DOMove(targetPos, spawnDuration * 0.5f)
					.SetEase(Ease.InQuad)
					.SetDelay(spawnDuration * 0.5f)
					.OnComplete(() => {
						wood.IsCollectable = true;
					});
				wood.transform.DOScale(1f, spawnDuration * 0.8f);
			}
		}

		private Vector3 GetRandomWoodPosition() {
			Vector2 randomOffset = new Vector2(Random.Range(-1f, 1f), Random.Range(-0.25f, 0.25f));
			Vector3 spawnPos = this.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);
			return spawnPos;
		}

		private void Die() {
			OnDiedAction?.Invoke(this);
			this.gameObject.SetActive(false);
		}
	}
}