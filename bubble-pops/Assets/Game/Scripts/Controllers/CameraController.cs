using Game.Scripts.CameraEntity;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera gameCamera;
        [SerializeField] private CameraShaker cameraShaker;

        public void Initialize()
        {
            cameraShaker.Initialize(gameCamera.transform);
        }

        public void ShakeCamera()
        {
            cameraShaker.ShakeCamera();
        }
    }
}
