using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int tilesCount;
    public float tilesDistance;
    public GameObject tilePrefab;
    public GameObject shipPrefab;

    private Tile[,] tiles;

    void Start()
    {
        tiles = new Tile[tilesCount, tilesCount];

        for (int x = 0; x < tilesCount; x++)
        {
            for (int z = 0; z < tilesCount; z++)
            {
                GameObject tileObject = Instantiate(tilePrefab, transform);
                tileObject.transform.localRotation = Quaternion.identity;
                tileObject.transform.localPosition = new Vector3(x * tilesDistance, 0, z * tilesDistance);

                tiles[x, z] = tileObject.AddComponent<Tile>();

                if (Random.value < 0.2)
				{
                    SpawnShip(shipPrefab, x, z);
                }
            }
        }

        float boardSize = tilesCount * tilesDistance;
        float boardOffset = boardSize * 0.5f;
        transform.localPosition = new Vector3(-boardOffset, 0, -boardOffset);
    }

	void Update()
	{
        if (Input.touchCount == 0)
            return;

        var touch = Input.GetTouch(0);
 
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Destroy(hit.collider.gameObject);
            Destroy(hit.transform.gameObject);
        }
    }

	void SpawnShip(GameObject shipPrefab, int x, int z)
	{
        GameObject ship = Instantiate(shipPrefab, transform);
        ship.transform.localRotation = Quaternion.identity;
        ship.transform.localPosition = new Vector3(x * tilesDistance, 0.01f, z * tilesDistance);
    }
}
