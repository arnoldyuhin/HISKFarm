using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HISK {
	public class TileMaster : MonoBehaviour {
		public static TileMaster Instance;

		public Tilemap GroundTileMap;
		public TileBase GrassTileBase;
		public TileBase FieldTileBase;
		public TileBase DitchTileBase;
		public TileBase WaterTileBase;

		private float waterFlowCooldown = 1f;
		private float waterFlowCounter = 0f;

		private void Awake() {
			if (Instance == null) {
				Instance = this;
			} else {
				GameObject.Destroy(this.gameObject);
			}
		}

		private void Update() {
			waterFlowCounter += Time.deltaTime;
			if (waterFlowCounter >= waterFlowCooldown) {
				waterFlowCounter -= waterFlowCooldown;
				WaterFlow();
			}
		}

		private void WaterFlow() {
			List<Vector3Int> ditchTileCoor = new List<Vector3Int>();
			BoundsInt cellBounds = GroundTileMap.cellBounds;
			TileBase[] allTiles = GroundTileMap.GetTilesBlock(cellBounds);


			for (int x = cellBounds.xMin; x < cellBounds.xMax; x++) {
				for (int y = cellBounds.yMin; y < cellBounds.yMax; y++) {
					for (int z = cellBounds.zMin; z < cellBounds.zMax; z++) {
						//modify later
						Vector3Int coor = new Vector3Int(x, y, z);
						TileBase tb = GroundTileMap.GetTile(coor);
						if (tb == null) continue;
						if (tb.name == "RT_Ditch") {
							ditchTileCoor.Add(coor);
						}
					}
				}
			}

			List<Vector3Int> waterFlowTileCoor = new List<Vector3Int>();

			for (int i = 0; i < ditchTileCoor.Count; i++) {
				//modify later
				TileBase upTile = GroundTileMap.GetTile(ditchTileCoor[i] + Vector3Int.up);
				TileBase rightTile = GroundTileMap.GetTile(ditchTileCoor[i] + Vector3Int.right);
				TileBase downTile = GroundTileMap.GetTile(ditchTileCoor[i] + Vector3Int.down);
				TileBase leftTile = GroundTileMap.GetTile(ditchTileCoor[i] + Vector3Int.left);

				if ((upTile != null && upTile.name == "RT_Water") ||
					(rightTile != null && rightTile.name == "RT_Water")||
					(downTile != null && downTile.name == "RT_Water") ||
					(leftTile != null && leftTile.name == "RT_Water")) { 
					waterFlowTileCoor.Add(ditchTileCoor[i]);
				}
			}

			for (int i = 0; i < waterFlowTileCoor.Count; i++) {
				GroundTileMap.SetTile(waterFlowTileCoor[i], WaterTileBase);
			}
		}
	}
}