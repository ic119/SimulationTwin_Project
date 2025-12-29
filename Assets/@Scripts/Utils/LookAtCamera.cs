using DG.Tweening;
using UnityEngine;

namespace JJORY.Util
{
    public class LookAtCamera : MonoBehaviour
    {
        #region Variable
        [Header("Rotate Variable")]
        public float rotation_Duration = 0.4f;
        private Camera camera;
        #endregion

        #region LifeCycle
        private void Awake()
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
        }

        private void LateUpdate()
        {
            Vector3 directionToCamera = camera.transform.position - transform.position;

            directionToCamera.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x,
                                              targetRotation.eulerAngles.y + 180f,
                                              targetRotation.eulerAngles.z);

            transform.DORotate(targetRotation.eulerAngles, rotation_Duration, RotateMode.FastBeyond360);
        }
        #endregion
    }
}