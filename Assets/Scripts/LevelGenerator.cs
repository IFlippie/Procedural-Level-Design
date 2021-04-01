using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public enum TileType {
    Empty = 0,
    Player,
    Enemy,
    Wall,
    Door,
    Key,
    Dagger,
    End
}

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] tiles;

    protected void Start()
    {        
        int width = 64;
        int height = 64;
        TileType[,] grid = new TileType[height, width];
        //FillWhole(grid,0,0,64,64,TileType.Wall);
        //FillBlock(grid, 0, 0, width, height, TileType.Wall);
        //FillBlock(grid, 26, 26, 12, 12, TileType.Empty);
        //FillBlock(grid, 32, 28, 1, 1, TileType.Player);
        //FillBlock(grid, 30, 30, 1, 1, TileType.Dagger);
        //FillBlock(grid, 34, 30, 1, 1, TileType.Key);
        //FillBlock(grid, 32, 32, 1, 1, TileType.Door);
        //FillBlock(grid, 32, 36, 1, 1, TileType.Enemy);
        //FillBlock(grid, 32, 34, 1, 1, TileType.End);



        Debugger.instance.AddLabel(32, 26, "Room 1");

        //use 2d array (i.e. for using cellular automata)
        CreateTilesFromArray(grid);
    }

    //fill part of array with tiles
    private void FillBlock(TileType[,] grid, int x, int y, int width, int height, TileType fillType) {
        for (int tileY=0; tileY<height; tileY++) {
            for (int tileX=0; tileX<width; tileX++) {
                grid[tileY + y, tileX + x] = fillType;
            }
        }
    }

    private void FillWhole(TileType[,] grid, int x, int y, int width, int height, TileType fillType)
    {
        for (int tileY = 0; tileY < height; tileY++)
        {
            for (int tileX = 0; tileX < width; tileX++)
            {
                bool shitFuck = (Random.value > 0.5f);
                if (shitFuck) { grid[tileY + y, tileX + x] = fillType; }
            }
        }
    }

    //use array to create tiles
    private void CreateTilesFromArray(TileType[,] grid) {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);
        for (int y=0; y<height; y++) {
            for (int x=0; x<width; x++) {
                 TileType tile = grid[y, x];
                 if (tile != TileType.Empty) {
                     CreateTile(x, y, tile);
                 }
            }
        }
    }

    //create a single tile
    private GameObject CreateTile(int x, int y, TileType type) {
        int tileID = ((int)type) - 1;
        if (tileID >= 0 && tileID < tiles.Length)
        {
            GameObject tilePrefab = tiles[tileID];
            if (tilePrefab != null) {
                GameObject newTile = GameObject.Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                newTile.transform.SetParent(transform);
                return newTile;
            }

        } else {
            Debug.LogError("Invalid tile type selected");
        }

        return null;
    }

}
