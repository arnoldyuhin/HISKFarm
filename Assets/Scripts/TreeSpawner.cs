using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace HISK {
	public class TreeSpawner : MonoBehaviour {
		[SerializeField] private float spawnCooldown = 3f;
		private float counter = 0f;
		[SerializeField] private int maxAmount = 4;
		[SerializeField] private TreeObject treePrefab;

		private List<TreeObject> activeTree = new List<TreeObject>();
		private List<TreeObject> inactiveTree = new List<TreeObject>();

		private void Awake() {
			for (int i = 0; i < maxAmount; i++) {
				SpawnTree();
			}
		}

		private void Update() {
			counter += Time.deltaTime;
			if (counter >= spawnCooldown) {
				counter -= spawnCooldown;
				SpawnTree();
			}
		}

		private void SpawnTree() {
			if (activeTree.Count >= maxAmount) return;
			TreeObject tree = GetTreeObject();
			tree.transform.position = GetRandomSpawnPosition();
			tree.transform.localScale = Vector3.zero;
			float scaleUpDuration = 0.3f;
			tree.Reset();
			tree.transform.DOScale(1f, scaleUpDuration).SetEase(Ease.OutBack)
				.OnComplete(() => {
					//coin.IsCollectable = true;
					tree.IsInteractable = true;
				});
			activeTree.Add(tree);
		}

		private TreeObject GetTreeObject() {
			TreeObject t;
			if (inactiveTree.Count == 0) {
				t = Instantiate<TreeObject>(treePrefab, Vector3.zero, Quaternion.identity);
				t.OnDiedAction += (TreeObject t) => {
					activeTree.Remove(t);
					inactiveTree.Add(t);
				};
			} else {
				t = inactiveTree[0];
				inactiveTree.Remove(t);
			}
			t.gameObject.SetActive(true);
			return t;
		}

		private Vector3 GetRandomSpawnPosition() {
			Vector3 spawnPos = Vector3.zero;
			Vector3 screenPos = new Vector3(Random.Range(0f, 1f) * 0.8f, Random.Range(0f, 1f) * 0.9f, 0f);
			spawnPos = Camera.main.ViewportToWorldPoint(screenPos);
			spawnPos.z = 0f;
			bool isChecking = true;
			while (isChecking) {
				bool isChanged = false;
				for (int i = 0; i < activeTree.Count; i++) {
						//Debug.Log(Vector3.Distance(spawnPos, activeTree[i].transform.position));
					if (Vector3.Distance(spawnPos, activeTree[i].transform.position) < 2.1f) {
						spawnPos += Random.insideUnitSphere;
						Vector3 viewportPoint = Camera.main.WorldToViewportPoint(spawnPos);
						bool isOutbounded = false;
						if (viewportPoint.x < 0.15f) {
							viewportPoint.x = 0.85f - viewportPoint.x;
							isOutbounded = true;
						}
						if (viewportPoint.x > 0.85f) {
							viewportPoint.x -= 0.85f;
							isOutbounded = true;
						}
						if (viewportPoint.y < 0.15f) {
							viewportPoint.y = 0.85f - viewportPoint.y;
							isOutbounded = true;
						}
						if (viewportPoint.y > 0.85f) {
							viewportPoint.y -= 0.85f;
							isOutbounded = true;
						}
						if (isOutbounded) {
							spawnPos = Camera.main.ViewportToWorldPoint(viewportPoint);
						}
						spawnPos.z = 0f;
						isChanged = true;
					}
				}
				if (isChanged) {
					isChecking = true;
				} else {
					isChecking = false;
				}
			}
			spawnPos.x = Mathf.RoundToInt(spawnPos.x);
			spawnPos.y = Mathf.RoundToInt(spawnPos.y) + 0.2f;
			spawnPos.z = 0f;
			return spawnPos;
		}

	}
}