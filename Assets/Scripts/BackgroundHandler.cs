using UnityEngine;

namespace Pang
{
    internal sealed class BackgroundHandler : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Background initialBackground;

        private void Start()
        {
            InitializeBackground(initialBackground);
        }

        public void InitializeBackground(Background backgroundAsset)
        {
            spriteRenderer.sprite = FindClosestSprite(backgroundAsset.Versions);
            ScaleToCameraView();
        }

        private Sprite FindClosestSprite(Sprite[] backgroundsVersions)
        {
            Sprite closestSprite = null;
            float closestAspectDifference = float.MaxValue;

            float cameraAspect = camera.aspect;

            foreach (Sprite sprite in backgroundsVersions)
            {
                float spriteAspect = sprite.rect.width / sprite.rect.height;
                float aspectDifference = Mathf.Abs(cameraAspect - spriteAspect);

                if (aspectDifference < closestAspectDifference)
                {
                    closestAspectDifference = aspectDifference;
                    closestSprite = sprite;
                }
            }

            return closestSprite;
        }

        private void ScaleToCameraView()
        {
            if (spriteRenderer.sprite == null)
                return;

            float cameraHeight = camera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * camera.aspect;
            Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

            // Calculate the scale required to fit the sprite to the camera view
            Vector2 newScale = transform.localScale;
            newScale.x = cameraWidth / spriteSize.x;
            newScale.y = cameraHeight / spriteSize.y;

            // Apply the new scale to the GameObject containing the SpriteRenderer
            transform.localScale = newScale;
        }
    }
}