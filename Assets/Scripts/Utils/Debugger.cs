using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {
    public class Debugger : MonoBehaviour
    {
        public static Debugger instance = null;

        public GameObject textPrefab;

        public List<GameObject> labels;

        void Awake() {
            instance = this;
            labels = new List<GameObject>();
        }

        public TextMesh AddLabel(int x, int y, string text) {
            GameObject newText = GameObject.Instantiate(textPrefab);
            newText.transform.position = new Vector3(x, y, 0);
            newText.transform.SetParent(transform);
            TextMesh textMesh = newText.GetComponent<TextMesh>();
            textMesh.text = text;
            labels.Add(newText);
            return textMesh;
        }

        public void Reset() {
            foreach (GameObject label in labels) 
            {
                Destroy(label);
            }
        }
    }
}