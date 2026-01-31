using UnityEngine;
using UnityEngine.Events;

namespace Code.Utils
{
    public class ColliderEventTarget : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Collider2D> onTriggerEnter2D;

        private void OnTriggerEnter2D(Collider2D other)
        {
            onTriggerEnter2D.Invoke(other);
        }
    }
}