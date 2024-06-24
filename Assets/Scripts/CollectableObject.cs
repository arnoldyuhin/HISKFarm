using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace HISK {
	public class CollectableObject : MonoBehaviour {
		public bool IsCollectable = false;
		private float collectDuration = 0.5f;

		public Action<CollectableObject> OnCollectedAction;

		public void OnCollected(CharacterController character) {
			PlayCollectAnimationInto(character);
		}

		public void PlayCollectAnimationInto(CharacterController character) {
			IsCollectable = false;
			float upOffset = 1f;
			this.transform.DOMoveY(this.transform.position.y + upOffset, collectDuration * 0.5f)
				.OnComplete(() => {
					StartCoroutine(FlyToCollectorCoroutine(character));
				});
			OnCollectedAction?.Invoke(this);
		}

		public IEnumerator FlyToCollectorCoroutine(CharacterController character) {
			float duration = collectDuration * 0.5f;
			float counter = 0f;
			Vector3 startPos = this.transform.position;
			Vector3 targetPos = character.transform.position;
			while (counter < duration) {
				counter += Time.deltaTime;
				targetPos = character.transform.position;
				this.transform.position = startPos + (targetPos - startPos) * (counter / duration);
				yield return null;
			}
			GameObject.Destroy(this.gameObject);
		}
	}
}