using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Spawner : Singleton<Spawner>
{
    EffectController controller;
    private void Awake()
    {
        CreateInstance(this);

    }
    private void Start()
    {
        controller = GetComponent<EffectController>();
        TurnSystem.Instance.OnTurnBegin += TurnSystem_OnTurnBegin;
        TurnSystem.Instance.OnTurnEnd += TurnSystem_OnTurnEnd;
        TurnSystem.Instance.OnTurnRun += TurnSystem_OnTurnRun;
    }

    private void TurnSystem_OnTurnRun(object sender, System.EventArgs e)
    {
        Spawn();
    }

    private void TurnSystem_OnTurnEnd(object sender, System.EventArgs e)
    {
        controller.Stop();
        EndNode.Instance.StopEffect();
    }

    private void TurnSystem_OnTurnBegin(object sender, System.EventArgs e)
    {
        controller.Fire();
        EndNode.Instance.StartEffect();
    }

    public void Spawn()
    {
        StartCoroutine(SpawnUnits());
    }

    IEnumerator SpawnUnits()
    {
        var path = PathSystem.Instance.GetPath();
        var enemies = WaveSystem.Instance.GetEnemiesToSpawn();
        var totalCount = WaveSystem.Instance.GetEnemyCount();
        int enemySpawned = 0;

        while (totalCount > enemySpawned)
        {
            var enemy = enemies.ElementAt(enemySpawned);
            var start = path[0];
            var enemyTransform = Instantiate(enemy.transform, LevelGrid.Instance.GetWorldPosition(start), Quaternion.identity);
            var enemyMovement = enemyTransform.GetComponent<EnemyMovement>();
            var instanceEnemy = enemyTransform.GetComponent<Enemy>();
            enemyMovement.Setup(path);
            enemySpawned++;
            WaveSystem.Instance.AddEnemy(instanceEnemy);
            yield return new WaitForSeconds(0.75f);
        }
    }

}
