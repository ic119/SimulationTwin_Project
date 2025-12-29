using DG.Tweening;
using JJORY.Module;
using JJORY.Util;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JJORY.Controller
{
    [Serializable]
    public class CameraMoveOption
    {
        [SerializeField] private float moveSpeed = 0.1f;
        public float MoveSpeed => moveSpeed;
    }

    [Serializable]
    public class CameraRotationOption
    {
        [SerializeField] private float rotateSpeed = 10.0f;
        [SerializeField] private float minAngle = -40.0f;
        [SerializeField] private float maxAngle = 70.0f;
        public float RotateSpeed => rotateSpeed;
        public float MinAngle => minAngle;
        public float MaxAngle => maxAngle;
    }

    [Serializable]
    public class CameraZoomOption
    {
        [SerializeField] private float minStep = 10.0f;
        [SerializeField] private float maxStep = 15.0f;
        [SerializeField] private float minDistance = 10.0f;
        [SerializeField] private float maxDistance = 80.0f;
        [SerializeField] private float zoomSpeed = 30.0f;
        [SerializeField] private float zoomTweenTime = 0.25f;
        public float MinStep => minStep;
        public float MaxStep => maxStep;
        public float MinDistance => minDistance;
        public float MaxDistance => maxDistance;
        public float ZoomSpeed => zoomSpeed;
        public float ZoomTweenTime => zoomTweenTime;
    }
    public class CameraController : SingletonObject<CameraController>
    {
        #region Variable
        [Header("Camera Move")]
        [SerializeField] private CameraMoveOption moveOption = new();
        private Vector3 last_MousePos;

        [Header("Camera Rotation")]
        [SerializeField] private CameraRotationOption rotationOption = new();
        public Vector3 cur_Rotation;

        [Header("Camera Zoom")]
        [SerializeField] public CameraZoomOption zoomOption = new();
        public Transform zoomPivot;

        [Header("Camera Init")]
        [SerializeField] private Vector3 init_Pos;
        [SerializeField] private Quaternion init_Rot;
        [SerializeField] private Camera camera;
        private Transform camera_Tr;
        public CameraViewType viewType = CameraViewType.IsoView;

        [Header("MoveToTarget Varialbe")]
        public Vector3 origin_Position;
        public Quaternion origin_Rotate;

        [Header("Interacte Object Variable")]
        [SerializeField] private DetectController detecteController;

        [Header("View 전환 관련 변수")]
        public Tween camaraView_TW;
        private Vector3 isoView_Positon;
        private Quaternion isoView_Rotation;

        [Header("Sequence 변수")]
        private DG.Tweening.Sequence cameraReset_Seq;
        private DG.Tweening.Sequence cameraView_Seq;

        [Header("Tween 변수")]
        private Tween cameraMove_TW;
        private Tween cameraRotate_TW;
        private Tween cameraZoom_TW;
        private Tween cameraIsoViewMove_TW;
        private Tween cameraIsoViewRotate_TW;
        private Tween cameraTopViewMove_TW;
        private Tween cameraTopViewRotate_TW;
        #endregion

        #region LifeCycle
        private void Start()
        {
            if (camera == null)
            {
                camera = Camera.main;
            }

            camera_Tr = camera.transform;
            init_Pos = transform.position;
            init_Rot = transform.rotation;
            cur_Rotation = transform.eulerAngles;
        }

        private void OnEnable()
        {
            if (EventController.Instance != null)
            {
                //EventController.Instance.OnRequestCameraReset += ResetCamera;
                //EventController.Instance.OnRequestChangeCameraView += ChangeCameraView;
            }
        }

        private void Update()
        {
            CheckLeftMouseInput();
        }

        private void LateUpdate()
        {
            if (detecteController != null && detecteController.isMoveToTarget)
            {
                return;
            }
            if (viewType == CameraViewType.IsoView)
            {
                CameraRotate();
            }

            CameraMove();
            CameraZoom();
        }

        private void OnDisable()
        {
            if (EventController.Instance != null)
            {
                //EventController.Instance.OnRequestCameraReset -= ResetCamera;
                //EventController.Instance.OnRequestChangeCameraView -= ChangeCameraView;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 마우스 좌클릭에 대한 예외 처리
        /// </summary>
        private void CheckLeftMouseInput()
        {
            // 좌클릭이 아닐 경우
            if (Input.GetMouseButtonDown(0) == false)
            {
                return;
            }

            // 클릭 위치가 UI위에 있는지에 대한 경우
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // 오브젝트 감지 시도
            if (detecteController.IsDetectObject() == true && InteractionState.cur_InteractionType != InteractionType.CameraMove)
            {
                return;
            }

            // 감지 실패 시 카메라 이동 시작
            InteractionState.Begin(InteractionType.CameraMove);
            last_MousePos = Input.mousePosition;
        }

        /// <summary>
        /// 오브젝트 상호작용 전 위치기준 카메라 시점 변경
        /// </summary>
        public void ResetCamera(bool _isInit)
        {
            detecteController.isMoveToTarget = true;

            if (cameraReset_Seq != null && cameraReset_Seq.IsActive())
            {
                cameraReset_Seq.Kill();
                cameraReset_Seq = null;
            }

            cameraReset_Seq = DOTween.Sequence();
            if (_isInit == false)
            {
                cameraMove_TW = camera_Tr.DOMove(origin_Position, 0.8f).SetEase(Ease.InOutSine);
                cameraRotate_TW = camera_Tr.DORotateQuaternion(origin_Rotate, 0.8f).SetEase(Ease.InOutSine);

                cameraReset_Seq.Append(cameraMove_TW)
                               .Join(cameraRotate_TW)
                               .OnComplete(() =>
                               {
                                   detecteController.isMoveToTarget = false;
                               })
                               .OnKill(() =>
                               {
                                   detecteController.isMoveToTarget = false;
                               });
            }
            else
            {
                cameraMove_TW = camera_Tr.DOMove(init_Pos, 0.8f).SetEase(Ease.InOutSine);
                cameraRotate_TW = camera_Tr.DORotateQuaternion(init_Rot, 0.8f).SetEase(Ease.InOutSine);

                cameraReset_Seq.Append(cameraMove_TW)
                               .Join(cameraRotate_TW)
                               .OnComplete(() =>
                               {
                                   detecteController.isMoveToTarget = false;
                               })
                               .OnKill(() =>
                               {
                                   detecteController.isMoveToTarget = false;
                               });
            }
        }

        /// <summary>
        /// 마우스 좌 드래그앤 드랍으로 카메라 움직임 처리
        /// </summary>  
        private void CameraMove()
        {
            if (Input.GetMouseButton(0) && InteractionState.cur_InteractionType == InteractionType.CameraMove)
            {
                Vector3 delta = Input.mousePosition - last_MousePos;
                Vector3 move_Pos = new Vector3(-delta.x * moveOption.MoveSpeed, -delta.y * moveOption.MoveSpeed, 0);

                camera.transform.Translate(move_Pos, Space.Self);

                Vector3 limit_Pos = camera.transform.position;

                if (limit_Pos.y < 2.0f)
                {
                    limit_Pos.y = 2.0f;
                    camera.transform.position = limit_Pos;
                }
                last_MousePos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0) && InteractionState.cur_InteractionType == InteractionType.CameraMove)
            {
                InteractionState.End(InteractionType.CameraMove);
            }
        }

        /// <summary>
        /// 마우스 우클릭으로 카메라 회전 처리
        /// </summary>  
        private void CameraRotate()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                last_MousePos = Input.mousePosition;
                cur_Rotation = camera.transform.eulerAngles;
            }

            if (Input.GetMouseButton(1))
            {
                Vector3 delta = Input.mousePosition - last_MousePos;

                float rot_X = delta.y * rotationOption.RotateSpeed * Time.deltaTime;
                float rot_Y = delta.x * rotationOption.RotateSpeed * Time.deltaTime;

                cur_Rotation.x -= rot_X;
                cur_Rotation.y += rot_Y;

                // 상하 회전 시 제한 각도 설정
                cur_Rotation.x = Mathf.Clamp(cur_Rotation.x, rotationOption.MinAngle, rotationOption.MaxAngle);

                camera.transform.rotation = Quaternion.Euler(cur_Rotation);

                last_MousePos = Input.mousePosition;
            }
        }

        /// <summary>
        /// 마우스 휠 Up&Down으로 줌인 아웃 처리
        /// </summary>
        private void CameraZoom()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            float delta = scroll * zoomOption.ZoomSpeed * 3.0f;

            ApplyZoom(delta);
        }

        #region UI Button 처리
        /// <summary>
        /// 커스텀 기능 활성 & 비활성에 따라 카메라 ViewType 변경
        /// </summary>
        public void ChangeCameraView()
        {
            if (cameraView_Seq != null && cameraView_Seq.IsActive())
            {
                cameraView_Seq.Kill();
                cameraView_Seq = null;
            }
            cameraView_Seq = DOTween.Sequence();

            if (viewType == CameraViewType.TopView)
            {
                init_Pos = isoView_Positon;
                init_Rot = isoView_Rotation;
                cameraIsoViewMove_TW = camera_Tr.DOMove(isoView_Positon, 1.0f).SetEase(Ease.InOutSine);
                cameraIsoViewRotate_TW = camera_Tr.DORotateQuaternion(isoView_Rotation, 1.0f).SetEase(Ease.InOutSine);

                cameraView_Seq.Append(cameraIsoViewMove_TW)
                              .Join(cameraIsoViewRotate_TW)
                              .OnComplete(() =>
                              {
                                  viewType = CameraViewType.IsoView;
                              });
            }
            else
            {
                isoView_Positon = camera_Tr.position;
                isoView_Rotation = camera_Tr.rotation;
                Vector3 topView_Position = new Vector3(0.0f, zoomOption.MaxDistance, 0.0f);
                Vector3 topView_Rotation = new Vector3(90.0f, 0.0f, 0.0f);
                Quaternion topView_Rot = Quaternion.Euler(topView_Rotation);
                init_Pos = topView_Position;
                init_Rot = topView_Rot;

                cameraTopViewMove_TW = camera_Tr.DOMove(topView_Position, 1.0f).SetEase(Ease.InOutSine);
                cameraTopViewRotate_TW = camera_Tr.DORotate(topView_Rotation, 1.0f, RotateMode.Fast).SetEase(Ease.InOutSine);

                cameraView_Seq.Append(cameraTopViewMove_TW)
                              .Join(cameraTopViewRotate_TW)
                              .OnComplete(() =>
                              {
                                  viewType = CameraViewType.TopView;
                              });
            }

        }

        /// <summary>
        /// 버튼 & 마우스 휠 공통 적용 Zoom 처리
        /// </summary>
        /// <param name="_delta"></param>
        public void ApplyZoom(float _delta)
        {
            if (Mathf.Approximately(_delta, 0f))
            {
                return;
            }

            Transform camera_Tr = camera.transform;
            Vector3 pivot = zoomPivot ? zoomPivot.position : Vector3.zero;

            float cur_Distance = Vector3.Distance(camera_Tr.position, pivot);

            float target_Distance = Mathf.Clamp(cur_Distance - _delta, zoomOption.MinDistance, zoomOption.MaxDistance);

            if (Mathf.Approximately(cur_Distance, target_Distance))
            {
                return;
            }

            Vector3 direction = (camera_Tr.position - pivot).normalized;
            Vector3 target_Pos = pivot + direction * target_Distance;

            if (cameraZoom_TW != null && cameraZoom_TW.IsActive())
            {
                cameraZoom_TW.Kill();
            }

            cameraZoom_TW = camera_Tr.DOMove(target_Pos, zoomOption.ZoomTweenTime)
                                     .SetEase(Ease.InOutSine);
        }
        #endregion

        #endregion
    }
}