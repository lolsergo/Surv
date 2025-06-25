using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    public LayerMask terrainMask;
    public GameObject currentChunk;
    Vector3 playerLastPosition;


    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOptimizedDistance;
    float optimizedDistance;
    float optimizerCooldown;
    public float optimizerCooldownDuration;

    void Start()
    {
        playerLastPosition = player.transform.position;
    }

    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    void ChunkChecker()
    {
        if (!currentChunk)
        {
            return;
        }

        Vector3 moveDirection = player.transform.position - playerLastPosition;
        playerLastPosition = player.transform.position;

        string directionName = GetDirectionName(moveDirection);

        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(directionName).position, checkerRadius, terrainMask))
        {
            SpawnChunk(currentChunk.transform.Find(directionName).position);
        }

        //if (playerMovement.moveDirection.x > 0 && playerMovement.moveDirection.y == 0) //right
        //{
        //    if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
        //    {
        //        noTerrainPosition = currentChunk.transform.Find("Right").position;
        //        SpawnChunk();
        //    }
        //}
        //else if (playerMovement.moveDirection.x < 0 && playerMovement.moveDirection.y == 0) //left
        //{
        //    if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
        //    {
        //        noTerrainPosition = currentChunk.transform.Find("Left").position;
        //        SpawnChunk();
        //    }
        //}
        //else if (playerMovement.moveDirection.x == 0 && playerMovement.moveDirection.y > 0) //up
        //{
        //    if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
        //    {
        //        noTerrainPosition = currentChunk.transform.Find("Up").position;
        //        SpawnChunk();
        //    }
        //}
        //else if (playerMovement.moveDirection.x == 0 && playerMovement.moveDirection.y < 0) //down
        //{
        //    if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
        //    {
        //        noTerrainPosition = currentChunk.transform.Find("Down").position;
        //        SpawnChunk();
        //    }
        //}
        //else if (playerMovement.moveDirection.x > 0 && playerMovement.moveDirection.y > 0) //up+right
        //{
        //    if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up+Right").position, checkerRadius, terrainMask))
        //    {
        //        noTerrainPosition = currentChunk.transform.Find("Up+Right").position;
        //        SpawnChunk();
        //    }
        //}
        //else if (playerMovement.moveDirection.x < 0 && playerMovement.moveDirection.y > 0) //up+left
        //{
        //    if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up+Left").position, checkerRadius, terrainMask))
        //    {
        //        noTerrainPosition = currentChunk.transform.Find("Up+Left").position;
        //        SpawnChunk();
        //    }
        //}
        //else if (playerMovement.moveDirection.x > 0 && playerMovement.moveDirection.y < 0) //down+right
        //{
        //    if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down+Right").position, checkerRadius, terrainMask))
        //    {
        //        noTerrainPosition = currentChunk.transform.Find("Down+Right").position;
        //        SpawnChunk();
        //    }
        //}
        //else if (playerMovement.moveDirection.x < 0 && playerMovement.moveDirection.y < 0)
        //{
        //    if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down+Left").position, checkerRadius, terrainMask))
        //    {
        //        noTerrainPosition = currentChunk.transform.Find("Down+Left").position;
        //        SpawnChunk();
        //    }
        //}
    }

    string GetDirectionName(Vector3 direction)
    {
        direction = direction.normalized;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // move horizontal more than vertical
            if (direction.y > 0.5f)
            {
                return direction.x > 0 ? "Up+Right" : "Up+Left";
            }
            else if (direction.y < -0.5f)
            {
                return direction.x > 0 ? "Down+Right" : "Down+Left";
            }
            else
            {
                return direction.x > 0 ? "Right" : "Left";
            }
        }
        else
        {
            // move vertical more than horizontal
            if (direction.x > 0.5f)
            {
                return direction.y > 0 ? "Up+Right" : "Down+Right";
            }
            else if (direction.x < -0.5f)
            {
                return direction.y > 0 ? "Up+Left" : "Down+Left";
            }
            else
            {
                return direction.y > 0 ? "Up" : "Down";
            }
        }
    }

    void SpawnChunk(Vector3 spawnPosition)
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], spawnPosition, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }

    void ChunkOptimizer()
    {
        optimizerCooldown -= Time.deltaTime;

        if (optimizerCooldown < 0f)
        {
            optimizerCooldown = optimizerCooldownDuration;
        }
        else
        {
            return;
        }

        foreach (GameObject chunk in spawnedChunks)
        {
            optimizedDistance = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (optimizedDistance > maxOptimizedDistance)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
