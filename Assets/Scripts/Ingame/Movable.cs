using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame {
    public abstract class Movable : MonoBehaviour
    {
        const float Step = 64.0f; //pixels per unit
    
        protected virtual void Start()
        {
        }

        protected abstract void HandleControls();

        protected virtual void Update()
        {
            HandleControls();
        }

        protected void Move(int x, int y) {
            Vector3 newPosition = transform.position + new Vector3(x / Step, y / Step, 0);
            if (HandlePosition(newPosition, x, y)) {
                transform.position = newPosition;
            }
        }

        private bool HandlePosition(Vector3 position, int moveX, int moveY) {
            bool result = true;
            foreach (Collider2D collider in Physics2D.OverlapBoxAll(position, new Vector2(0.6f, 0.8f), 0.0f)) {
                GameObject other = collider.gameObject;
                if (!HandleCollision(other)) {
                    //ResolveCollision(other.transform.position, moveX, moveY);
                    result = false;
                }
            }
            return result;
        }

        private void ResolveCollision(Vector3 position, int x, int y) {
            if (x < 0) transform.position = new Vector3(position.x + 1, transform.position.y);
            if (x > 0) transform.position = new Vector3(position.x - 1, transform.position.y);
            if (y < 0) transform.position = new Vector3(transform.position.x, position.y + 1);
            if (y > 0) transform.position = new Vector3(transform.position.x, position.y - 1);
        }

        protected virtual bool HandleCollision(GameObject other) {
            if (!HandleObstruction(other)) return false;
            return true;
        }

        protected virtual bool HandleObstruction(GameObject other) {
            Obstruction obstruction = other.GetComponent<Obstruction>();
            if (obstruction != null) {
                return false;
            }
            return true;
        }

    }
}