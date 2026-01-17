using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ZombieSpawner : MonoBehaviour
{
    public Transform target;

    public enum SpawnState
    {
        None,
        Spawn
    }
    public SpawnState spawnState = SpawnState.None;

    public List<Transform> spawnPos = new List<Transform>(); //소환 장소
    public GameObject zombiePrefab; //좀비 프리팹
    private float spawnTime;
    public float spawnInterval; //소환 간격
    private int SpawnCount;

    void Start()
    {
        SpawnIntervvalOn();
    }

    void Update()
    {
        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
        switch (spawnState)
        {
            case SpawnState.Spawn:
                {
                    spawnTime += Time.deltaTime;
                    if (spawnTime >= spawnInterval)
                    {
                        for (int i = 0; i < SpawnCount; i++)
                            CreateZombie();
                        SpawnIntervvalOn();
                        spawnTime = 0f;
                    }

                    break;
                }
        }
    }

    void SpawnIntervvalOn()
    {
        SpawnCount = Random.Range(10, 30);
        spawnInterval = Random.Range(0.5f, 3.0f);
    }

    void CreateZombie()
    {
        int n = Random.Range(0, spawnPos.Count); //0번부터 spawnPos의 마지막 배열번호 중 무작위 수 하나 출력
        GameObject zombie = Instantiate(zombiePrefab, spawnPos[n].position, spawnPos[n].rotation);
        Destroy(zombie, 60.0f);
    }
}
