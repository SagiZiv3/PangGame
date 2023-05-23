using UnityEngine;

namespace Pang
{
    internal sealed class WallsPlacer : MonoBehaviour
    {
        [SerializeField] private ScreenBoundsHandler boundsHandler;
        [SerializeField] private Transform leftWall, rightWall;
        [SerializeField] private Transform topWall, bottomWall;

        private void Start()
        {
            Rect screenBounds = boundsHandler.ScreenBounds;
            float wallThickness = leftWall.localScale.x / 2f;
            float centerY = screenBounds.center.y;
            leftWall.position = new Vector3(screenBounds.xMin + wallThickness, centerY);
            rightWall.position = new Vector3(screenBounds.xMax - wallThickness, centerY);

            float multiplier = topWall.GetComponent<SpriteRenderer>().bounds.size.x;
            float horizontalWallsWidth = screenBounds.width / multiplier;
            Vector3 scale = topWall.localScale;
            scale.x = horizontalWallsWidth;
            topWall.localScale = scale;
            scale = bottomWall.localScale;
            scale.x = horizontalWallsWidth;
            bottomWall.localScale = scale;
            Destroy(this);
        }
    }
}