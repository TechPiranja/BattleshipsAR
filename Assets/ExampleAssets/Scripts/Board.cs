using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int tilesCount;
    public float tilesDistance;
    public GameObject tilePrefab;

    private GameObject[,] tiles;

    void Start()
    {
        tiles = new GameObject[tilesCount, tilesCount];

        for (int x = 0; x < tilesCount; x++)
        {
            for (int z = 0; z < tilesCount; z++)
            {
                GameObject tile = Instantiate(tilePrefab, transform);
                tile.transform.localRotation = Quaternion.identity;
                tile.transform.localPosition = new Vector3(x * tilesDistance, 0, z * tilesDistance);

                tiles[x, z] = tile;
            }
        }

        float boardSize = tilesCount * tilesDistance;
        float boardOffset = boardSize * 0.5f;
        transform.localPosition = new Vector3(-boardOffset, 0, -boardOffset);
    }
}
