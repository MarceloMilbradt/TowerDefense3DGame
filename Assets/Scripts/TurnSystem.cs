using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public static class ListExtensions
{

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class TurnSystem : Singleton<TurnSystem>
{


    public struct EnemySpawnNumber
    {
        public int boss;
        public int normal;
        public int medium;
        public int hard;
    }

    private int turnNumber;
    public event EventHandler<int> OnTurnChange;
    public event EventHandler OnTurnEnd;

    [SerializeField] private List<WaveModifierWaveNumber> waveModifiers;
    [SerializeField] private List<EnemyTypeTransform> enemyTransforms;

    [SerializeField] private float turnCooldown;

    List<Enemy> createdEnemies = new List<Enemy>();

    private bool preparing;

    private EnemySpawnNumber enemySpawnNumber;
    private EnemySpawnNumber tempEnemyType;

    private float timeToNextTurn;
    public Enemy GetEnemy(EnemyType enemyType)
    {
        return enemyTransforms.First(t => t.type == enemyType).transform;
    }
    public float GetCountDown()
    {
        if (!preparing) return 0f;
        return turnCooldown - timeToNextTurn;
    }
    private void LateUpdate()
    {
        if (GetEnemyCount() > 0 && GetEnemyCount() == totalEnemyKilled)
        {
            OnTurnEnd?.Invoke(this, EventArgs.Empty);
            NextTurn();
        }
        if (preparing)
        {
            timeToNextTurn += Time.deltaTime;
        }
        if (timeToNextTurn >= turnCooldown && preparing)
        {
            StartTurn();
        }
    }
    private int totalEnemyKilled;
    public void AddEnemy(Enemy enemy)
    {
        createdEnemies.Add(enemy);
        enemy.OnDeath += Enemy_OnDeath;
    }

    private void Enemy_OnDeath(object sender, EventArgs e)
    {
        totalEnemyKilled++;
    }

    private void Awake()
    {
        CreateInstance(this);
    }
    public int GetTurn()
    {
        return turnNumber;
    }
    public void NextTurn()
    {
        if (preparing) return;
        Prepare();
    }

    private void StartTurn()
    {
        preparing = false;
        timeToNextTurn = 0;
        turnNumber++;
        OnTurnChange?.Invoke(this, turnNumber);
    }

    private void Prepare()
    {
        preparing = true;
        timeToNextTurn = 0;
        totalEnemyKilled = 0;
        CleanTemporaryEnemies();
        Cleanup();
        GenerateEnemisForWave();
    }

    public List<Enemy> GetEnemiesToSpawn()
    {
        var enemiesToSpawn = new List<Enemy>(GetEnemyCount());
        enemiesToSpawn.AddRange(Enumerable.Repeat(GetEnemy(EnemyType.Normal), enemySpawnNumber.normal));
        enemiesToSpawn.AddRange(Enumerable.Repeat(GetEnemy(EnemyType.Medium), enemySpawnNumber.medium));
        enemiesToSpawn.AddRange(Enumerable.Repeat(GetEnemy(EnemyType.Hard), enemySpawnNumber.hard));
        enemiesToSpawn.AddRange(Enumerable.Repeat(GetEnemy(EnemyType.Boss), enemySpawnNumber.boss));
        enemiesToSpawn.Shuffle();
        return enemiesToSpawn;
    }
    public int GetEnemyCount()
    {
        return enemySpawnNumber.boss +
        enemySpawnNumber.normal +
        enemySpawnNumber.medium +
        enemySpawnNumber.hard;
    }
    private void GenerateEnemisForWave()
    {
        int waveNumber = GetTurn() + 1;
        foreach (WaveModifierWaveNumber waveModifierWaveNumber in waveModifiers)
        {
            if (waveNumber % waveModifierWaveNumber.everyWaveNumber == 0)
            {
                var sign = waveModifierWaveNumber.operation == WaveNumberOperation.Add ? 1 : -1;
                var enemyNumber = waveModifierWaveNumber.enemyCount * sign;

                switch (waveModifierWaveNumber.enemyType)
                {
                    case EnemyType.Normal:
                        enemySpawnNumber.normal += enemyNumber;
                        if (waveModifierWaveNumber.spawnType == WaveSpawnType.ThisWave)
                        {
                            tempEnemyType.normal += enemyNumber;
                        }
                        break;
                    case EnemyType.Medium:
                        enemySpawnNumber.medium += enemyNumber;
                        if (waveModifierWaveNumber.spawnType == WaveSpawnType.ThisWave)
                        {
                            tempEnemyType.medium += enemyNumber;
                        }
                        break;
                    case EnemyType.Boss:
                        enemySpawnNumber.boss += enemyNumber;
                        if (waveModifierWaveNumber.spawnType == WaveSpawnType.ThisWave)
                        {
                            tempEnemyType.boss += enemyNumber;
                        }
                        break;
                    case EnemyType.Hard:
                        enemySpawnNumber.hard += enemyNumber;
                        if (waveModifierWaveNumber.spawnType == WaveSpawnType.ThisWave)
                        {
                            tempEnemyType.hard += enemyNumber;
                        }
                        break;
                }

            }
        }
        createdEnemies = new List<Enemy>(GetEnemyCount());
    }
    private void Cleanup()
    {
        foreach (var enemy in new List<Enemy>(createdEnemies))
        {
            enemy.OnDeath -= Enemy_OnDeath;
        }
        createdEnemies.Clear();
    }
    private void CleanTemporaryEnemies()
    {
        enemySpawnNumber.boss -= tempEnemyType.boss;
        enemySpawnNumber.normal -= tempEnemyType.normal;
        enemySpawnNumber.medium -= tempEnemyType.medium;
        enemySpawnNumber.hard -= tempEnemyType.hard;
    }
}
