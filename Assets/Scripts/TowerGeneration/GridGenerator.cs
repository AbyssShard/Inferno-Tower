using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("EssentialComponents")]
    public GameplayManager gameplayManager;
    public PlayerStatus player;
    [Space(3)]

    [Header("GridByLevel")]

    public PossibleGrids[] possibleGrids;

    [Space(3)]

    [Header("TowerPreferences")]
    public Vector2 posOffset;
    public Vector2 gridSize;
    public float tileDistance;

    [HideInInspector] public float nextPos;
    [HideInInspector] public bool updateGrid;

    public List<GameObject> curentTowerChunk = new List<GameObject>();
    public List<GameObject> lastTowerChunk = new List<GameObject>();
    public List<Transform> TowerChunks = new List<Transform>();
    public List<GameObject> nextTowerChunk = new List<GameObject>();
    int heightIndex;

    public Vector3 cameraPos;
    Vector3 vel;
    int currentIndex;

    private void Start()
    {
        //UpdateGrids();
    }

    private void Update()
    {
        
        cameraPos.z = -10f;
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, cameraPos, ref vel, 0.4f);

        if (player.posIndex.y == gridSize.y && player.climbing)
        {
            updateGrid = false;
            gameplayManager.StopTurn();
            gameplayManager.floor += 1;
            heightIndex += 1;

            player.posIndex = new Vector2(player.posIndex.x, 0f);

            GenerateGrids();
        }
    }

    void UpdateGrids()
    {
        currentIndex = TowerChunks.Count - 2;
        currentIndex = (int)Mathf.Clamp(currentIndex, 0, Mathf.Infinity);
        curentTowerChunk.Clear();
        foreach (Transform grid in TowerChunks[currentIndex])
        {
            curentTowerChunk.Add(grid.gameObject);
        }
        if(TowerChunks.Count - 3 != 0)
        {
            for(int i = 0; i < TowerChunks.Count - 3; i++)
            {
                Destroy(TowerChunks[i].gameObject);
                TowerChunks.Remove(TowerChunks[i]);
            }
        }
    }

    public void GenerateGrids()
    {
        // Limpa e atualiza os chunks
        if (lastTowerChunk.Count > 0)
        {
            foreach (GameObject grid in lastTowerChunk)
            {
                Destroy(grid);
            }
            lastTowerChunk.Clear();
        }

        // Gerar duas novas chunks
        if (TowerChunks.Count == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                GenerateChunk();
            }
        }
        else
            GenerateChunk();

        void GenerateChunk()
        {
            Transform newChunk = new GameObject("newChunk").transform;
            TowerChunks.Add(newChunk);
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int horIndex = -1; horIndex <= 1; horIndex++)
                {
                    tileDistance = Mathf.Clamp(tileDistance, 1f, Mathf.Infinity);
                    Vector2 position = new Vector2(horIndex, y);

                    Vector2 newGridPos = position + Vector2.up * nextPos * tileDistance + posOffset;
                    Grid newGrid = Instantiate(possibleGrids[0].gridPrefab, newGridPos, Quaternion.identity, newChunk).GetComponent<Grid>();


                    newGrid.gridIndex = position;
                    newGrid.gridPos = newGridPos;
                }

            }
            UpdateGrids();
            nextPos += gridSize.y * tileDistance;
        }

        // Verifica se há elementos em curentTowerChunk antes de ajustar a câmera
        if (curentTowerChunk.Count > 0)
        {
            cameraPos = new Vector2(0f, Mathf.Lerp(curentTowerChunk[0].transform.position.y, curentTowerChunk[curentTowerChunk.Count - 1].transform.position.y, 0.5f));
        }

        updateGrid = true;
    }

}

[System.Serializable]
public class PossibleGrids
{
    public GameObject gridPrefab;
    public int chance;
}
