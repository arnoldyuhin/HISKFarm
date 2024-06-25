using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HISK {
	public class PlayerInput : MonoBehaviour {

		private static PlayerInput instance;
		public static PlayerInput Instance {
			get {
				if (instance == null) {
					GameObject go = new GameObject();
					go.name = "PlayerInput";
					PlayerInput playerInput = go.AddComponent<PlayerInput>();
					instance = playerInput;
				}
				return instance;
			}
		}

		private Vector2 inputAxis = Vector2.zero;

		public Action<Vector2> MovementInputUpdateAction;
		public Action<Vector2> MovementInputFixedUpdateAction;
		public Action InteractInputDownAction;

		private void Update() {
			inputAxis = Vector2.zero;
			//Debug.Log(Input.GetKey(KeyCode.UpArrow));
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
				inputAxis += Vector2.up;
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
				inputAxis += Vector2.down;
			}
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
				inputAxis += Vector2.left;
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
				inputAxis += Vector2.right;
			}
			if (inputAxis != Vector2.zero) {
				inputAxis.Normalize();
				MovementInputUpdateAction?.Invoke(inputAxis);
			}
			if (Input.GetKeyDown(KeyCode.Space)) {
				InteractInputDownAction?.Invoke();
			}
		}

		private void FixedUpdate() {
			inputAxis = Vector2.zero;
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
				inputAxis += Vector2.up;
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
				inputAxis += Vector2.down;
			}
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
				inputAxis += Vector2.left;
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
				inputAxis += Vector2.right;
			}
			if (inputAxis != Vector2.zero) {
				inputAxis.Normalize();
				MovementInputFixedUpdateAction?.Invoke(inputAxis);
			}
		}


	}
}