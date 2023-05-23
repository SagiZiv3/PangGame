using UnityEngine;

namespace Pang
{
    internal sealed class ScreenBoundsHandler : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        public Rect ScreenBounds { get; private set; }

        private void Awake()
        {
            float cameraHeight = camera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * camera.aspect;

            ScreenBounds = new Rect(-cameraWidth / 2f, -cameraHeight / 2f, cameraWidth, cameraHeight);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color c = Gizmos.color;
            Gizmos.color = Color.red;
            Vector2 pos = new Vector2(ScreenBounds.xMin, ScreenBounds.yMin);
            Gizmos.DrawCube(pos, Vector3.one * 0.5f);
            pos.y = ScreenBounds.yMax;
            Gizmos.DrawCube(pos, Vector3.one * 0.5f);
            pos.x = ScreenBounds.xMax;
            Gizmos.DrawCube(pos, Vector3.one * 0.5f);
            pos.y = ScreenBounds.yMin;
            Gizmos.DrawCube(pos, Vector3.one * 0.5f);
            Gizmos.color = c;
        }
#endif
    }
}