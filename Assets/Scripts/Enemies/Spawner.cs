using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Spawner : Singleton<Spawner>
{
    EffectController controller;
    float canSpawnIn;
    bool canSpawn;
    private void Awake()
    {
        CreateInstance(this);

    }
    private void Start()
    {
        controller = GetComponent<EffectController>();
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        TurnSystem.Instance.OnTurnEnd += TurnSystem_OnTurnEnd;
    }

    private void TurnSystem_OnTurnEnd(object sender, System.EventArgs e)
    {
        controller.Stop();
        EndNode.Instance.StopEffect();
    }

    private void TurnSystem_OnTurnChange(object sender, int e)
    {
        controller.Fire();
        EndNode.Instance.StartEffect();
        canSpawnIn = 1.5f;
        canSpawn = true;
    }

    public void Spawn()
    {
        canSpawn = false;
        StartCoroutine(SpawnUnits());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TurnSystem.Instance.NextTurn();
        }
        if (canSpawn)
        {
            canSpawnIn -= Time.deltaTime;
        }
        if (canSpawnIn <= 0 && canSpawn)
        {
            Spawn();
        }
    }
    IEnumerator SpawnUnits()
    {
        var path = PathSystem.Instance.GetPath();
        var enemies = TurnSystem.Instance.GetEnemiesToSpawn();
        var totalCount = TurnSystem.Instance.GetEnemyCount();
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
            TurnSystem.Instance.AddEnemy(instanceEnemy);
            yield return new WaitForSeconds(0.75f);
        }
    }

}
