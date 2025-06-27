using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public List<GameObject> moveDirections;
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
        int directionID = 1;
        int previousDirectionID = 0;
        int nextDirectionID = 2;

        for (int i = 0; i < moveDirections.Count; i++)
        {
            if (moveDirections[i].name == directionName)
            {
                directionID = i;
                break;
            }
        }

        if (directionID == 0)
        {
            previousDirectionID = moveDirections.Count - 1;
            nextDirectionID = directionID + 1;
        }
        else if (directionID == moveDirections.Count - 1)
        {
            previousDirectionID = directionID - 1;
            nextDirectionID = 0;
        }
        else
        {
            previousDirectionID = directionID - 1;
            nextDirectionID = directionID + 1;
        }

        CheckAndSpawnChunk(moveDirections[directionID].name);
        CheckAndSpawnChunk(moveDirections[previousDirectionID].name);
        CheckAndSpawnChunk(moveDirections[nextDirectionID].name);
    }

    void CheckAndSpawnChunk(string direction)
    {
        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(direction).position, checkerRadius, terrainMask))
        {
            SpawnChunk(currentChunk.transform.Find(direction).position);
        }
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
