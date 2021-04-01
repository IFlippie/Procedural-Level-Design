using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InGame {
    public class UpdateHUD : MonoBehaviour
    {
        private Player player;

        public Text daggerText;
        public Text keyText;
        public Text infoText;

        void Update()
        {
            if (Input.anyKeyDown) infoText.enabled = false;
            if (player == null) {
                infoText.enabled = true;
                player = FindObjectOfType<Player>();
            } else {
                daggerText.text = "x" + player.daggers;
                keyText.text = "x" + player.keys;
            }
        }
    }
}