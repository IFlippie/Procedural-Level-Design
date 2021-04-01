using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InGame;

namespace Utils {
    public class GameManager : MonoBehaviour
    {
        const float ResetTimeout = 2f;

        public GameObject doneMessage; //prefab. for endscreen
        public GameObject levelPrefab; //prefab. contains LevelGenerator
        public GameObject mainCamera; //reference to camera, for chasecam
        public bool autoReset = false; //refreshes generated level every 2 seconds

        private GameObject level = null; //holds level

        private int randomSeed = 1; //current seed
        private bool isDone = false;//for enabling space
        private Transform playerTransform = null;//for chasecam
        private float resetTime = 0.0f;//for auto reset

        //on start, generate new level
        protected void Start() {
            OnReset();
        }

        //ends game, places done message
        private void OnDone() {
            playerTransform = null;
            randomSeed ++; //sets new randomseed
            GameObject newDoneMessage = GameObject.Instantiate(doneMessage);
            newDoneMessage.transform.parent = level.transform;
            if (mainCamera != null) {
                Vector3 pos = mainCamera.transform.position;
                pos.z = 0;
                newDoneMessage.transform.position = pos; //align message with camera
            }
            isDone = true;
        }

        //regenerates level
        private void OnReset() {
            playerTransform = null;
            if (level != null) {
                Destroy(level);
            }
            Debugger.instance.Reset();
            StartLevel();
        }

        private void StartLevel() {
            Random.InitState(randomSeed);
            level = GameObject.Instantiate(levelPrefab);
            isDone = false;
        }

        protected void UpdateCamera() {
            if (mainCamera != null) {
                if (playerTransform != null) {
                    Vector3 pos = playerTransform.position;
                    pos.z = mainCamera.transform.position.z;
                    mainCamera.transform.position = pos;
                } else {
                    FindPlayerTransform();
                }
            }
        }

        private void FindPlayerTransform() {
            foreach (Player player in FindObjectsOfType<Player>()) {
                player.OnGameDone = OnDone;
                player.OnGameReset = OnReset;
                playerTransform = player.transform;
            }
        }

        protected void Update() {
            if (autoReset) { //regenerates level every ResetTimeout seconds
                resetTime += Time.deltaTime;
                if (resetTime > ResetTimeout) {
                    resetTime = 0;
                    OnDone();
                    OnReset();
                }
            }
            if (isDone) {
                if (Input.GetKeyDown(KeyCode.Space)) { //space = reset (when done)
                    OnReset();
                }
            }

            if (Input.GetKeyDown(KeyCode.R)) { //R = reset (same randomseed)
                if (Input.GetKey(KeyCode.LeftShift)) { //Shift+R = new randomseed, then reset
                    OnDone();
                    OnReset();
                } else {
                    OnReset();
                }
            }
        }

        protected void LateUpdate() {
            UpdateCamera();
        }

    }
}