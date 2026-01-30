using UnityEngine;

namespace Code.Utils
{
    public static class SpriteUtils
    {
        public static void SetPicture(Sprite sprite, SpriteRenderer rendererTarget)
        {
            rendererTarget.sprite = sprite;
            rendererTarget.transform.localScale = Vector3.one;
        
            Vector2 spriteSize = sprite.bounds.size;
            float maxSide = Mathf.Max(spriteSize.x, spriteSize.y);
            if (maxSide <= 0f) return;

            float scaleFactor = 1f / maxSide;

            rendererTarget.transform.localScale = Vector3.one * scaleFactor;
            // Debug.Log($"spriteSize={spriteSize}, maxSide={maxSide}, scale={rendererTarget.transform.localScale}");
        }
    }
}