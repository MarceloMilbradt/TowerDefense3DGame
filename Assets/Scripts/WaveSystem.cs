using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSystem : Singleton<WaveSystem>
{
    public struct EnemySpawnNumber
    {
        public int boss;
        public int normal;
        public int medium;
        public int hard;
    }

    List<Enemy> createdEnemies = new List<Enemy>();

    [SerializeField] private List<WaveModifierWaveNumber> waveModifiers;
    [SerializeField] private List<EnemyTypeTransform> enemyTransforms;
    private EnemySpawnNumber enemySpawnNumber;
    private EnemySpawnNumber tempEnemyType;
    private int totalEnemyKilled;
    public void AddEnemy(Enemy enemy)
    {
        createdEnemies.Add(enemy);
        enemy.OnDeath += Enemy_OnDeath;
    }
    private void Awake()
    {
        CreateInstance(this);
    }
    private void Start()
    {
        EndNode.OnAnyUnitPass += EndNode_OnAnyUnitPass;
        TurnSystem.Instance.OnTurnBegin += TurnSystem_OnTurnBegin;
    }

    private void TurnSystem_OnTurnBegin(object sender, EventArgs e)
    {
        GenerateEnemisForWave();
    }

    private void Enemy_OnDeath(object sender, EventArgs e)
    {
        totalEnemyKilled++;
        TryEndTurn();
    }
    private void EndNode_OnAnyUnitPass(object sender, EventArgs e)
    {
        totalEnemyKilled++;
        TryEndTurn();
    }

    private void TryEndTurn()
    {
        if (!HasAnyEnemyInTheBoard())
        {
            TurnSystem.Instance.TryEndTurn();
        }
    }

    private bool HasAnyEnemyInTheBoard()
    {
        return totalEnemyKilled < GetEnemyCount();
    }

    public Enemy GetEnemy(EnemyType enemyType)
    {
        return enemyTransforms.First(t => t.type == enemyType).transform;
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
        int waveNumber = TurnSystem.Instance.GetTurn();
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
