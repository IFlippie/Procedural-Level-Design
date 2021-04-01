using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame {
    public class Enemy : Movable
    {
        const float DetectRadius = 6.0f;

        enum EnemyState {
            Patrol,
            Attack
        }

        private int direction = 1;
        private EnemyState state = EnemyState.Patrol;
        private Player target = null;

        protected override void Start() {
            base.Start();
        }

        protected override void HandleControls() {
            switch (state) {
                case EnemyState.Patrol:
                    HandlePatrolling();
                    break;
                case EnemyState.Attack:
                    HandleAttack();
                    break;
            }
        }

        private void HandlePatrolling() {
            Move(direction, 0);
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, DetectRadius)) {
                GameObject other = collider.gameObject;
                Player player = other.GetComponent<Player>();
                if (player != null) {
                    target = player;
                    state = EnemyState.Attack;
                }
            }
        }

        private void HandleAttack() {
            if (target != null) {
                int moveX = 0;
                int moveY = 0;
                if (target.transform.position.x <= transform.position.x) {
                    moveX = -1;
                }
                if (target.transform.position.x >= transform.position.x) {
                    moveX = 1;
                }
                if (target.transform.position.y <= transform.position.y) {
                    moveY = -1;
                }
                if (target.transform.position.y >= transform.position.y) {
                    moveY = 1;
                }
                Move(moveX, 0);
                Move(0, moveY);//hack to remove obstruction when going diagonal
            } else {
                state = EnemyState.Patrol;
            }
        }

        protected override bool HandleCollision(GameObject other) {
            switch (state) {
                case EnemyState.Patrol:
                    return HandlePatrolCollision(other);
                case EnemyState.Attack:
                    return HandleAttackCollision(other);
            }
            return true;
        }

        private bool HandlePatrolCollision(GameObject other) {
            if (!HandleObstruction(other)) {
                direction = -direction;
                return false;
            }
            return true;
        }

        private bool HandleAttackCollision(GameObject other) {
            if (!HandleObstruction(other)) {
                return false;
            }
            return true;
        }

    }
}