using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame {
    public enum PickupType {
        Key,
        Dagger
    }

    public class Pickup : MonoBehaviour
    {
        public PickupType type = PickupType.Key;
    }
}