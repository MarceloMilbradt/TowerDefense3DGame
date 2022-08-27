using System;
using UnityEngine;

public enum EnemyType
{
    Normal,
    Medium,
    Hard,
    Boss,
}
[Serializable]
public class WaveModifierWaveNumber
{
    public int everyWaveNumber;
    public int enemyCount;
    public EnemyType enemyType;
    public WaveNumberOperation operation;
    public WaveSpawnType spawnType;
}
[Serializable]
public class EnemyTypeTransform
{
    public EnemyType type;
    public Enemy transform;
}
public enum WaveNumberOperation
{
    Add, Subtract
}
public enum WaveSpawnType
{
    EveryWave, ThisWave
}