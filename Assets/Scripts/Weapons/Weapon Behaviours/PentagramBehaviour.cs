using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagramBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> markedEnemies;

    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && !markedEnemies.Contains(collider.gameObject))
        {
            EnemyStats enemy = collider.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);

            markedEnemies.Add(collider.gameObject);
        }
        else if (collider.CompareTag("Prop"))
        {
            if (collider.TryGetComponent(out BreakableProps breakableProps) && !markedEnemies.Contains(collider.gameObject))
            {
                breakableProps.TakeDamage(currentDamage);

                markedEnemies.Add(collider.gameObject);
            }
        }
    }
}
