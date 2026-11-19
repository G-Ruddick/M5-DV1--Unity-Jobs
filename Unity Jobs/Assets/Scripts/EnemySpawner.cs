using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using System.Collections;
using System.Collections.Generic;

[BurstCompile]
public class EnemySpawner : MonoBehaviour {
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int numberOfEnemiesToSpawn;
    [SerializeField] private float spawnRadius = 15f;
    [SerializeField] private Transform playerTransform;

    [Header("Enemy Stuff")]
    [SerializeField] private float distanceThreshhold = 15f;
    [SerializeField] private float speed = 2.5f;

    private bool isDone;
    private List<GameObject> enemiesList = new List<GameObject>();

    private TransformAccessArray accessArray;
    private Transform[] enemyTransform;

    private void Awake() {
        enemyTransform = new Transform[numberOfEnemiesToSpawn];

        for (int i = 0; i < numberOfEnemiesToSpawn; i++) {
            Vector2 spawnPosition = UnityEngine.Random.insideUnitCircle * spawnRadius;
            GameObject go = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemiesList.Add(go);
            enemyTransform[i] = go.transform;
        }

        accessArray = new TransformAccessArray(enemyTransform);

        isDone = true;
    }

    private void Update() {
        if (isDone) {
            ControlEnemyJob job = new ControlEnemyJob() {
                playerPosition = playerTransform.position,
                deltaTime = Time.deltaTime,
                distThreshhold = distanceThreshhold,
                moveSpeed = speed
            };

            JobHandle jobHandle = job.Schedule(accessArray);
            jobHandle.Complete();

            // for (int i = 0; i < enemiesList.Count; i++) {
            //     Vector2 direction = (playerTransform.position - enemiesList[i].transform.position).normalized;
            //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            //     enemiesList[i].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //     float distance = Vector2.Distance(playerTransform.position, enemiesList[i].transform.position);

            //     if (distance <= distanceThreshhold) {
            //         enemiesList[i].transform.position += -(Vector3)direction * Time.deltaTime * speed;
            //     }
            // }
        }
    }
}

[BurstCompile]
public struct ControlEnemyJob : IJobParallelForTransform {
    [ReadOnly] public float3 playerPosition;
    [ReadOnly] public float deltaTime;
    [ReadOnly] public float distThreshhold;
    [ReadOnly] public float moveSpeed;
    
    public void Execute(int index, TransformAccess transform) {
        float3 direction = math.normalize(playerPosition - (float3)transform.position);
        float angle = math.atan2(direction.y, direction.x);
        angle -= math.radians(90f);

        transform.rotation = quaternion.AxisAngle(new float3(0, 0, 1), angle);

        float distance = math.distance(playerPosition, transform.position);
        float3 newPosition = (float3)transform.position - direction * deltaTime * moveSpeed;
        float3 maskedPosition = math.select(transform.position, newPosition, distance <= distThreshhold);

        transform.position = maskedPosition;
    }
}
