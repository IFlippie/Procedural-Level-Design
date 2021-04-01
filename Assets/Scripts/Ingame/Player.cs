using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InGame {
    public class Player : Movable
    {
        public int keys = 0;
        public int daggers = 0;

        public Action OnGameDone = null;
        public Action OnGameReset = null;

        protected override void Start() {
            base.Start();
        }

        protected override void HandleControls() {
            int moveX = 0;
            int moveY = 0;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                moveY = 5;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                moveY = -5;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                moveX = -5;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                moveX = 5;
            }
            if (Input.GetKey(KeyCode.LeftShift)) {
                moveX += moveX;
                moveY += moveY;
            }
            Move(moveX, 0);//always moves: lazy implementation to make player<->enemy collisions work in both directions
            Move(0, moveY);//using SAT to make wall-sliding possible
        }

        protected override bool HandleCollision(GameObject other) {
            if (!HandleObstruction(other)) return false;
            if (!HandlePickup(other)) return false;
            if (!HandleEnemy(other)) return false;
            if (!HandleEnd(other)) return false;
            return true;
        }

        //when colliding with endflag->level done
        private bool HandleEnd(GameObject other) {
            End end = other.GetComponent<End>();
            if (end != null) {
                if (OnGameDone != null) {
                    OnGameDone();
                }
                Destroy(gameObject);
            }
            return true;
        }

        //when colliding with enemy->reset level
        private bool HandleEnemy(GameObject other) {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null) {
                if (daggers > 0) {
                    daggers --;
                    Destroy(other);
                } else {
                    Destroy(gameObject);
                    if (OnGameReset != null) {
                        OnGameReset();
                    }
                }
            }
            return true;
        }

        //when picking up dagger or key
        private bool HandlePickup(GameObject other) {
            Pickup pickup = other.GetComponent<Pickup>();
            if (pickup != null) {
                if (pickup.type == PickupType.Key) {
                    keys ++;
                }
                if (pickup.type == PickupType.Dagger) {
                    daggers ++;
                }
                Destroy(other);
            }
            return true;
        }

        //when colliding with wall or door
        protected override bool HandleObstruction(GameObject other) {
            Obstruction obstruction = other.GetComponent<Obstruction>();
            if (obstruction != null) {
                if (obstruction.canBeOpened) {
                    if (keys > 0) {
                        keys --;
                        Destroy(other);
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

    }
}