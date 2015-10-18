﻿using UnityEngine;
using System.Collections;
using System;

public class Enemy : MovingObject
{
    private float walkTime = 0.5f;
    private float startTime = Time.time;
    private Transform target;

    protected override void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    void Update()
    {
        Coord playerLoc = new Coord((int)target.position.x, (int)target.position.y);
        Coord enemyLoc = new Coord((int)transform.position.x, (int)transform.position.y);
        if (startTime + walkTime <= Time.time && enemyLoc.Distance(playerLoc) < 15)
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

    protected override void OnCantMove<T>(T component)
    {
        Player playerHit = component as Player;
        playerHit.TakeDamage(baseAttack);
    }
}
