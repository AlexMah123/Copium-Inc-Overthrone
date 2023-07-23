using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using NightmareEchoes.UI;

//created by Jian Hua, editted by Vinn and Terrence and Alex
namespace NightmareEchoes.Grid
{
    public class TileMapManager : MonoBehaviour
    {
        [Header("Singleton stuff")]
        private static TileMapManager _instance; 
        public static TileMapManager Instance { get { return _instance; } }

        [Header("Grid")]
        [SerializeField] public int width;
        [SerializeField] public int length;
        [SerializeField] public TileBase testTile;

        
        public Tilemap tilemap;
        public Vector3Int TilePos;
        private TilemapRenderer tilemapRenderer;
        private Vector3 spawnPos;
        private Vector3 endPos;

        Vector3Int prevTilePos;

        [Header("Pathfinding")]
        [SerializeField] private GameObject StartPos;
        [SerializeField] private GameObject EndPos;
        [SerializeField] public GameObject spawnTest;
        public TileData tileData;
        public float TransformPosZOffset;
        private PathfindingScript pathfinder;

        public Dictionary<Vector3Int, TileData> MAP;

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
            tilemapRenderer = GetComponent<TilemapRenderer>();
            StartPos.SetActive(false);
            EndPos.SetActive(false);

            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            { 
                _instance = this;
            }
        }

        public void Start()
        {
           MAP = new Dictionary<Vector3Int, TileData>();
            pathfinder = new PathfindingScript();
        }

        public void Update()
        {
            if (UIManager.Instance.gameIsPaused)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                GetMouseTilePos();
            }
            if (Input.GetMouseButtonDown(1))
            {
                GetMouseTilePos2();
            }
        }

        //Source: https://blog.unity.com/engine-platform/procedural-patterns-you-can-use-with-tilemaps-part-1
        public int[,] GenerateArray(int width, int length, bool empty)
        {
            var map = new int[width, length];
            for (var x = 0; x < map.GetUpperBound(0); x++)
            {
                for (var y = 0; y < map.GetUpperBound(1); y++)
                {
                    if (empty)
                    {
                        map[x, y] = 0;
                    }
                    else if (!empty )
                    {
                        map[x, y] = 1;
                    }
                }
            }
            return map;
        }
        
        public void RenderMap(int[,] map, Tilemap tilemap, TileBase tile)
        {
            //Clear the map (ensures we dont overlap)
            tilemap.ClearAllTiles();
            
            //Loop through the width of the map
            for (int x = 0; x < map.GetUpperBound(0) ; x++)
            {
                //Loop through the height of the map
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    var tileKey = new Vector3Int(x,y,0);
                    var tilelocation = new Vector3Int(x,y,0);
                    // 1 = tile, 0 = no tile
                    if (map[x, y] == 1 && !MAP.ContainsKey(tileKey))
                    {
                        tilemap.SetTile(tilelocation, tile);
                        tileData.GridLocation = tilelocation;
                        MAP.Add(tileKey,tileData);
                    }
                }
            }
        }



        public void UpdateMap(int[,] map, Tilemap tilemap) //Takes in our map and tilemap, setting null tiles where needed
        {
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    //We are only going to update the map, rather than rendering again
                    //This is because it uses less resources to update tiles to null
                    //As opposed to re-drawing every single tile (and collision data)
                    if (map[x, y] == 0)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), null);
                    }
                }
            }
        }

        public void GetMouseTilePos()
        {
            Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TilePos = tilemap.WorldToCell(MousePos);

            if (tilemap.GetTile(TilePos))
            {
                if (prevTilePos != null)
                {
                    tilemap.SetTileFlags(prevTilePos, TileFlags.None);
                    tilemap.SetColor(prevTilePos, Color.white);
                }

                tilemap.SetTileFlags(TilePos, TileFlags.None);
                tilemap.SetColor(TilePos, Color.red);

                prevTilePos = TilePos;

                if (StartPos.activeSelf == false)
                {
                    StartPos.SetActive(true);
                    spawnPos = new Vector3(tilemap.CellToLocal(TilePos).x, tilemap.CellToLocal(TilePos).y - 2, 1);
                    StartPos.transform.position = spawnPos;
                }
                else
                {
                    endPos = new Vector3(tilemap.CellToLocal(TilePos).x, tilemap.CellToLocal(TilePos).y - 2, 1);
                    
                    
                    //StartPos.transform.position = spawnPos;
/*                    var path = pathfinder.FindPath(StartPos,endPos);*/
                    Debug.Log("Second Mouse Click is on" + spawnPos);
                }
            }
        }

        //this section by Terrence, spawning on tiles proof of concept
        public void GetMouseTilePos2()
        {
            Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TilePos = tilemap.WorldToCell(MousePos);
            spawnPos = new Vector3(tilemap.CellToLocal(TilePos).x, tilemap.CellToLocal(TilePos).y - 2, 1);
            
            if (tilemap.GetTile(TilePos))
            {
                if (prevTilePos != null)
                {
                    tilemap.SetTileFlags(prevTilePos, TileFlags.None);
                    tilemap.SetColor(prevTilePos, Color.white);
                }

                tilemap.SetTileFlags(TilePos, TileFlags.None);
                tilemap.SetColor(TilePos, Color.cyan);

                prevTilePos = TilePos;

                if (EndPos.activeSelf == false)
                {
                    EndPos.SetActive(true);
                    spawnPos = new Vector3(tilemap.CellToLocal(TilePos).x, tilemap.CellToLocal(TilePos).y - 2, 1);
                    EndPos.transform.position = spawnPos;
                }
                else if (EndPos.activeSelf == true)
                {
                    spawnPos = new Vector3(tilemap.CellToLocal(TilePos).x, tilemap.CellToLocal(TilePos).y - 2, 1);
                    EndPos.transform.position = spawnPos;
                }
            }
        }
    }
}
