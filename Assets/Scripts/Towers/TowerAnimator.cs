using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimator : MonoBehaviour
{
    //[SerializeField] Animator animator;
    [SerializeField] Transform bulletPorjectile;
    [SerializeField] Transform shootPoint;
    private void Start()
    {
        if (TryGetComponent(out TowerAttack attack))
        {
            attack.OnShoot += Attack_OnShoot;
        }
    }

    private void Attack_OnShoot(object sender, TowerAttack.OnShootEventAgrs e)
    {
        //animator.SetTrigger(Animations.SHOOT);
        var bulletTransform = Instantiate(bulletPorjectile, shootPoint.position, Quaternion.identity);
        var bullet = bulletTransform.GetComponent<Projectile>();
        bullet.Setup(e.target);
        ((Enemy) e.target).AddProjectile(bullet);
    }

}
