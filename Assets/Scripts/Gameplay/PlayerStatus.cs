using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatus : MonoBehaviour
{
    [Header("EssentialComponents")]

    public GameplayManager gameplayManager;
    public GridGenerator gridManager;
    public Animator anim;

    [Header("PlayerValues")]
    public int maxLife, maxShild, maxEnergy;
    
    [HideInInspector] public bool climbing;
    [HideInInspector] public int life, shild, energy;

    [HideInInspector] public Vector2 posIndex;
    [HideInInspector] public Vector2 currentGridPos;
    private Vector2 vel;

    private void Start()
    {
        anim = GetComponent<Animator>();

        life = maxLife;
        shild = maxShild;
        energy = maxEnergy;
    }
    private void Update()
    {
        life = Mathf.Clamp(life, 0, maxLife);
        shild = Mathf.Clamp(life, 0, maxShild);
        energy = Mathf.Clamp(energy, 0, maxEnergy);

        posIndex.y = Mathf.Clamp(posIndex.y, 2, gameplayManager.grid.gridSize.y);
        posIndex.x = Mathf.Clamp(posIndex.x, 0, gameplayManager.grid.gridSize.x);

        List<GameObject> grids = gridManager.curentTowerChunk;
        foreach (GameObject grid in grids)
        {
            if (posIndex == grid.GetComponent<Grid>().gridIndex)
            {
                currentGridPos = grid.transform.position;
            }
        }

        //transform.position = Vector2.SmoothDamp(transform.position, gameplayManager.grids[posIndex].gridPos,ref vel, 0.4f);
        transform.position = Vector2.MoveTowards(transform.position, currentGridPos, 5 * Time.deltaTime);
        climbing = Vector2.Distance(transform.position, currentGridPos) < 0.1f;
        anim.SetBool("Move", !climbing);
    }
}
