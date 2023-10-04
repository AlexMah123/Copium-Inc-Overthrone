using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

//created by Vinn, editted by Alex
namespace NightmareEchoes.Grid
{
    public class OverlayTileManager : MonoBehaviour
    {
        private int childCount = 0;
        public static OverlayTileManager Instance;

        public OverlayTile overlaytilePrefab;
        public GameObject overlayContainer;

        //Dicitonary to store overlay tile position from grid pos
        public Dictionary<Vector2Int, OverlayTile> gridDictionary;

        private Camera cam;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
            
            InitOverlayTiles();
            
            cam = Camera.main;
        }

        void InitOverlayTiles()
        {
            //generating the grid
            var tileMap = gameObject.GetComponentInChildren<Tilemap>();
            gridDictionary = new Dictionary<Vector2Int, OverlayTile>();
            //getting the bounds of the map
            BoundsInt bounds = tileMap.cellBounds;

            //For looping to get the bounds of our map (Starting with Z first as its height then x or y either does not matter) (Z-- since its going down)
            for (int z = bounds.max.z; z >= bounds.min.z; z--)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    for (int x = bounds.min.x; x < bounds.max.x; x++)
                    {
                        //Capturing tile location
                        var tileLocation = new Vector3Int(x, y, z);
                        var TileKey = new Vector2Int(x, y);

                        //Make sure the tilemap has the tile we are looking for
                        if (tileMap.HasTile(tileLocation) && !gridDictionary.ContainsKey(TileKey))
                        {
                            //Instantiating the tileprefab over the grid and storing the position in the girdContainer
                            var overlayTile = Instantiate(overlaytilePrefab, overlayContainer.transform);
                            overlayTile.name = $"{childCount} {tileLocation}";

                            //Getting WorldPos (Vector 3) to Tile Position (Vector3int)
                            var cellWorldPos = tileMap.GetCellCenterWorld(tileLocation);

                            //Placing overlay tile to where they are supposed to be
                            //Instead of setting the position straigh tto the cellWorldPos we are offsetting it to one cell higher on the z axis so it can be seen on screen
                            overlayTile.transform.position = new Vector3(cellWorldPos.x, cellWorldPos.y, cellWorldPos.z);
                            //overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
                            overlayTile.gridLocation = tileLocation;
                            childCount++;
                            gridDictionary.Add(TileKey, overlayTile);
                        }
                    }
                }
            }
        }
        
        public List<OverlayTile> GetNeighbourTiles(OverlayTile currentOverlayTile , List<OverlayTile> LimitTiles)
        {
            var map = gridDictionary;

            //Dictionary for the tileRange
            Dictionary<Vector2Int, OverlayTile> tileToSearch = new Dictionary<Vector2Int, OverlayTile> ();

            //if statement to limit the tiles
            if (LimitTiles.Count > 0)
            {
                foreach (var item in LimitTiles)
                {
                    tileToSearch.Add(new Vector2Int(item.gridLocation.x, item.gridLocation.y), item);
                }
            }
            else
            {
                tileToSearch = map;
            }


            List<OverlayTile> neighbours = new List<OverlayTile>();

            //Top
            Vector2Int LocToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y + 1);

            if (tileToSearch.ContainsKey(LocToCheck))
            {
                neighbours.Add(tileToSearch[LocToCheck]);
            }

            //Bottom
            LocToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y - 1);

            if (tileToSearch.ContainsKey(LocToCheck))
            {
                neighbours.Add(tileToSearch[LocToCheck]);
            }

            //Right
            LocToCheck = new Vector2Int(currentOverlayTile.gridLocation.x + 1, currentOverlayTile.gridLocation.y);

            if (tileToSearch.ContainsKey(LocToCheck))
            {
                neighbours.Add(tileToSearch[LocToCheck]);
            }

            //Left
            LocToCheck = new Vector2Int(currentOverlayTile.gridLocation.x - 1, currentOverlayTile.gridLocation.y);

            if (tileToSearch.ContainsKey(LocToCheck))
            {
                neighbours.Add(tileToSearch[LocToCheck]);
            }

            return neighbours;
        }

        #region Utility for Getting Specific Tiles based on pos
        public List<OverlayTile> TrimOutOfBounds(List<Vector2Int> list)
        {
            var tileRange = new List<OverlayTile>();
            
            foreach (var coord in list.Where(coord => gridDictionary.ContainsKey(coord)))
            {
                if (gridDictionary.TryGetValue(coord, out var tile))
                    tileRange.Add(tile);
            }

            return tileRange;
        }

        public OverlayTile GetOverlayTileOnMouseCursor()
        {
            var hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Overlay Tile"));
            if (!hit) return null;
            var target = hit.collider.gameObject.GetComponent<OverlayTile>();
            return !target ? null : target;
        }

        public OverlayTile GetOverlayTileOnWorldPosition(Vector3 pos)
        {
            var hit = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Overlay Tile"));
            if (!hit) return null;
            var target = hit.collider.gameObject.GetComponent<OverlayTile>();
            return !target ? null : target;
        }
        
        public OverlayTile GetOverlayTile(Vector2Int pos)
        {
            return gridDictionary.ContainsKey(pos) ? gridDictionary[pos] : null;
        }

        #endregion
    }
}


