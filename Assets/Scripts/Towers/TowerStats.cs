using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStats : MonoBehaviour
{
    [SerializeField] private int damage = 25;
    [SerializeField] private float attackSpeed = 0.5f;
    [SerializeField] private int range = 2;
    [SerializeField] private int projectileNumber = 1;
    public int GetDamage() { return damage; }
    public float GetAttackSpeed() { return attackSpeed; }
    public int GetRange() { return range; }
    public int GetProjectileNumber() { return projectileNumber; }
    public void SetDamage(int damage) { this.damage = damage; }
    public void SetAttackSpeed(float attackSpeed) { this.attackSpeed = attackSpeed; }
    public void SetRange(int range) { this.range = range; }
    public void SetProjectileNumber(int projectileNumber) { this.projectileNumber = projectileNumber; }

}
