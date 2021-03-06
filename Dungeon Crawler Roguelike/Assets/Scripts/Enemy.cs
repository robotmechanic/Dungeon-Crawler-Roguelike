﻿using UnityEngine;
using System.Collections;
using System;

public class Enemy : MovingObject
{
    private float walkTime = 0.5f;
    private Transform target;

    protected override void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
        health = new EnemyHealth(baseHealth);
    }

    void Update()
    {
        Coord playerLoc = new Coord((int)target.position.x, (int)target.position.y);
        Coord enemyLoc = new Coord((int)transform.position.x, (int)transform.position.y);
        if (!GameManager.instance.isPaused && startTime + walkTime <= Time.time && enemyLoc.Distance(playerLoc) < 15 && !(GameManager.instance.levelUp))
        {
            int horizontal = 0;
            int vertical = 0;

            if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            {
                vertical = target.position.y > transform.position.y ? 1 : -1;
            }
            else
            {
                horizontal = target.position.x > transform.position.x ? 1 : -1;
            }

            if (horizontal != 0 || vertical != 0)
            {
                AttemptMove<Player>(horizontal, vertical);
            }

            startTime = Time.time;
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (health.IsDepleted())
        {
            gameObject.SetActive(false);
            manager.movingObjects[(int)transform.position.x, (int)transform.position.y] = false;
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        MovingObject playerHit = component as MovingObject;
        playerHit.TakeDamage(baseAttack);
    }
}
