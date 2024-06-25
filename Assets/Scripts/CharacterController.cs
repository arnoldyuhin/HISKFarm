using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

namespace HISK {
	public class CharacterController : MonoBehaviour {

		[SerializeField] private bool isMoveDiagonal = true;
		[SerializeField] private bool isMoveInGrid = false;
		private bool isMoving = false;

		[SerializeField] private float moveSpeed = 1f;
		[SerializeField] private Rigidbody2D rigidbody;
		private Vector3 facingDirection = Vector3.down;


		[SerializeField] private GameObject gridIndicator;
		private Vector3Int indicatorPosInt;
		private Vector3 indicatorPos;

		private ToolType currentToolType = ToolType.Axe;

		private void OnEnable() {
			PlayerInput.Instance.MovementInputFixedUpdateAction += MoveInDirection;
			PlayerInput.Instance.InteractInputDownAction += CheckInteract;
		}

		private void OnDisable() {
			PlayerInput.Instance.MovementInputFixedUpdateAction -= MoveInDirection;
			PlayerInput.Instance.InteractInputDownAction -= CheckInteract;
		}

		private void Update() {
			//move to player input later
			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				currentToolType = ToolType.Axe;
			}
			if (Input.GetKeyDown(KeyCode.Alpha2)) {
				currentToolType = ToolType.Shovel;
			}
			if (Input.GetKeyDown(KeyCode.Alpha3)) {
				currentToolType = ToolType.Hoe;
			}
			if (Input.GetKeyDown(KeyCode.Alpha4)) {
				currentToolType = ToolType.Sickle;
			}
			if (Input.GetKeyDown(KeyCode.Alpha5)) {
				currentToolType = ToolType.Hammer;
			}


			IndicateCloestGrid();
		}

		public void MoveInDirection(Vector2 direction) {
			if (isMoveDiagonal == false) {
				if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y)) {
					direction.y = 0f;
				} else {
					direction.x = 0f;
				}
				direction.Normalize();
			}
			Vector2 currentPos = new Vector2(this.transform.position.x, this.transform.position.y);
			Vector2 targetPos = Vector2.zero;
			if (isMoveInGrid == false) {
				targetPos = currentPos + moveSpeed * direction * Time.deltaTime;
				rigidbody.MovePosition(targetPos);
			} else {
				if (isMoving == true) return;
				isMoving = true;
				targetPos = currentPos + direction;
				targetPos.x = Mathf.RoundToInt(targetPos.x);
				targetPos.y = Mathf.RoundToInt(targetPos.y);
				rigidbody.DOMove(targetPos, 1f / moveSpeed)
					.OnComplete(() => {
						isMoving = false;
					});
			}
			facingDirection = (Vector3)direction;
			facingDirection.Normalize();
		}

		public void OnTriggerEnter2D(Collider2D collision) {
			CollectableObject collectableObject = collision.gameObject.GetComponent<CollectableObject>();
			if (collectableObject != null) {
				if (collectableObject.IsCollectable == false) return;
				collectableObject.OnCollected(this);
			}
		}

		public void OnTriggerStay2D(Collider2D collision) {
			CollectableObject collectableObject = collision.gameObject.GetComponent<CollectableObject>();
			if (collectableObject != null) {
				if (collectableObject.IsCollectable == false) return;
				collectableObject.OnCollected(this);
			}
		}

		private void IndicateCloestGrid() {
			Vector3 targetPos = this.transform.position + facingDirection * 0f;

			indicatorPosInt = new Vector3Int(Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y), Mathf.RoundToInt(targetPos.z));
			Tilemap groundTileMap = TileMaster.Instance.GroundTileMap;
			indicatorPos = groundTileMap.CellToWorld(indicatorPosInt);

			gridIndicator.transform.position = indicatorPos;
		}

		private void CheckInteract() {
			switch (currentToolType) {
				case ToolType.Axe:
					Chop();
					break;
				case ToolType.Shovel:
					Dig();
					break;
				case ToolType.Hoe:
					Hoe();
					break;
				case ToolType.Hammer:
					Destroy();
					break;
			}
		}

		private void Chop() {
			RaycastHit2D[] hits = Physics2D.BoxCastAll(indicatorPos, Vector2.one, 0f, Vector3.zero);
			for (int i = 0; i < hits.Length; i++) {
				InteractableObject io = hits[i].collider.GetComponent<InteractableObject>();
				if (io == null) continue;
				io.Interact();
			}
		}

		private void Dig() {
			Tilemap groundTileMap = TileMaster.Instance.GroundTileMap;
			TileBase tile = groundTileMap.GetTile(indicatorPosInt);
			if (tile == null) return;
			if (tile.name == "RT_Ditch") {
				float possibilityToWater = 0.33f;
				float randomSeed = Random.Range(0f, 1f);
				if (randomSeed < possibilityToWater) {
					groundTileMap.SetTile(indicatorPosInt, TileMaster.Instance.WaterTileBase);
				} else {
					groundTileMap.SetTile(indicatorPosInt, TileMaster.Instance.DitchTileBase);
				}

			} else {
				groundTileMap.SetTile(indicatorPosInt, TileMaster.Instance.DitchTileBase);
			}
		}

		private void Hoe() {
			Tilemap groundTileMap = TileMaster.Instance.GroundTileMap;
			groundTileMap.SetTile(indicatorPosInt, TileMaster.Instance.FieldTileBase);
		}

		private void Destroy() {
			Tilemap groundTileMap = TileMaster.Instance.GroundTileMap;
			groundTileMap.SetTile(indicatorPosInt, TileMaster.Instance.GrassTileBase);

		}

	}
}
