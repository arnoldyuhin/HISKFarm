using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace HISK {
	public class CameraShaker : MonoBehaviour {
		private static CameraShaker instance;
		public static CameraShaker Instance {
			get {
				if (instance == null) {
					CameraShaker cs = Camera.main.GetComponent<CameraShaker>();
					if (cs != null) {
						instance = cs;
					} else {
						instance = Camera.main.gameObject.AddComponent<CameraShaker>();
					}
				}
				return instance;
			}
		}

		private Camera mainCamera;
		public Camera MainCamera {
			get {
				if (mainCamera == null) {
					mainCamera = Camera.main;
				}
				return mainCamera;
			}
		}

		public void Shake(float duration, float strength) {
			MainCamera.DOShakePosition(duration, strength);
		}
	}
}
