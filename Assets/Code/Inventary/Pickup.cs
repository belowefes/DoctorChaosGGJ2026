using Code.Utils;
using UnityEngine;

namespace Code.Inventary
{
    [CreateAssetMenu(menuName = "Items/PickupItem")]
    public class PickupItem : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public int respChance;
        public PickupType pickupType;
    }

}