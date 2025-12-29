using DG.Tweening;
using JJORY.Module;
using JJORY.Util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JJORY.Controller
{
    public class DetectController : SingletonObject<DetectController>
    {
        #region Variable
        [SerializeField] private GameObject clicked_ParentObject;
        [SerializeField] private GameObject clicked_ChildObject;
        [SerializeField] private Camera camera;
        public bool isMoveToTarget = false;

        [Header("LayerMask")]
        [SerializeField] private LayerMask object_Layer;
        [SerializeField] private LayerMask childObject_Layer;
        private int target_LayerMask;

        [Header("Camera OffSet")]
        public Vector3 forword_OffSet = new Vector3(16f, 10f, -4f);
        public Vector3 back_OffSet = new Vector3(0.0f, 3.6f, 4.0f);

        [Header("예외처리 변수")]
        [SerializeField] private bool isSelected = false;

        [Header("DOTween 변수")]
        private DG.Tweening.Sequence moveToTarget_Seq;
        private Tween moveToTargetMove_TW;
        private Tween moveToTargetRotate_TW;
        #endregion

        #region LifeCycle
        private void OnEnable()
        {
            //EventController.Instance.OnRequestMoveToTarget += MoveToTarget;
        }

        private void OnDisable()
        {
            //EventController.Instance.OnRequestMoveToTarget -= MoveToTarget;
        }
        #endregion  

        #region Method
        /// <summary>
        /// 카메라 Ray를 통해 Object인식 후 이동 처리
        /// </summary>
        /// <returns></returns>
        public bool IsDetectObject()
        {
            if (isMoveToTarget == true && isSelected == true)
            {
                return false;
            }

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return false;
            }

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            target_LayerMask = object_Layer | childObject_Layer;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, target_LayerMask))
            {
                GameObject hit_GameObject = hit.collider.gameObject;
                CameraController.Instance.zoomPivot = hit_GameObject.transform;

                int hit_Layer = hit_GameObject.layer;

                //switch ((LayerMaskType)hit_Layer)
                //{
                //    case LayerMaskType.Object_LayerMask:
                //        {
                //            if (hit_GameObject == clicked_ParentObject)
                //            {
                //                if (property != null)
                //                {
                //                    SelectObjectController.Instance.DetectedDevice(hit_GameObject, false, false);
                //
                //                    isSelected = false;
                //                    clicked_ParentObject = null;
                //                    clicked_ChildObject = null;
                //                    CameraController.Instance.zoomPivot = null;
                //                }
                //                return true;
                //            }
                //
                //            if (isMoveToTarget)
                //            {
                //                return true;
                //            }
                //
                //            //EventController.Instance.InvokeMoveToTarget(hit_GameObject, true, false);
                //            return true;
                //        }
                //    case LayerMaskType.ChildObject_LayerMask:
                //        {
                //            if (hit_GameObject == clicked_ChildObject)
                //            {
                //                SelectObjectController.Instance.DetectedDevice(hit_GameObject, false, false);
                //
                //                isSelected = false;
                //                clicked_ChildObject = null;
                //                CameraController.Instance.zoomPivot = null;
                //
                //                return true;
                //            }
                //
                //            if (isMoveToTarget)
                //            {
                //                return true;
                //            }
                //
                //            EventController.Instance.InvokeMoveToTarget(hit_GameObject, true, false);
                //            return true;
                //        }
                //}
            }

            return false;
        }

        /// <summary>
        /// 오브젝트 위치로 카메라 시점 변경(부모 오브젝트에만 적용)
        /// </summary>
        /// <param name="_target"></param>
        private void MoveToTarget(GameObject _target, bool _isMove, bool _isEvent = false)
        {
            isSelected = true;

            Transform camera_Tr = camera.transform;
            Transform target_Tr = _target.transform;

            //switch ((LayerMaskType)_target.layer)
            //{
            //    case LayerMaskType.Object_LayerMask:
            //        {
            //            if (_isMove == true)
            //            {
            //                CameraController.Instance.origin_Position = camera_Tr.position;
            //                CameraController.Instance.origin_Rotate = camera_Tr.rotation;
            //
            //                Vector3 target_Pos = target_Tr.TransformPoint(back_OffSet);
            //                Vector3 look_Dir = (target_Tr.position - target_Pos).normalized;
            //
            //                isMoveToTarget = true;
            //                clicked_ParentObject = _target;
            //
            //                if (moveToTarget_Seq != null && moveToTarget_Seq.IsActive())
            //                {
            //                    moveToTarget_Seq.Kill();
            //                    moveToTarget_Seq = null;
            //                }
            //
            //                moveToTarget_Seq = DOTween.Sequence();
            //
            //                moveToTargetMove_TW = camera_Tr.DOMove(target_Pos, 1.5f).SetEase(Ease.InOutSine);
            //                moveToTargetRotate_TW = camera_Tr.DORotateQuaternion(Quaternion.LookRotation(look_Dir), 1.5f).SetEase(Ease.InOutSine);
            //
            //                moveToTarget_Seq.Append(moveToTargetMove_TW)
            //                                .Join(moveToTargetRotate_TW)
            //                                .OnPlay(() =>
            //                                {
            //                                    // 줌 인 진행 중 처리할 내용 기입
            //                                    if (property != null)
            //                                    {
            //
            //                                        SelectObjectController.Instance.DetectedDevice(_target, _isMove, _isEvent);
            //                                    }
            //                                })
            //                                .OnComplete(() =>
            //                                {
            //                                    // 줌 인 완료 후 처리할 내용 기입
            //                                    isMoveToTarget = false;
            //                                    CameraController.Instance.cur_Rotation = camera_Tr.eulerAngles;
            //                                });
            //            }
            //            else
            //            {
            //                if (property != null)
            //                {
            //                    SelectObjectController.Instance.DetectedDevice(_target, _isMove, _isEvent);
            //                }
            //            }
            //            break;
            //        }
            //    case LayerMaskType.ChildObject_LayerMask:
            //        {
            //            if (_isMove == true)
            //            {
            //                clicked_ChildObject = _target;
            //
            //                if (property != null)
            //                {
            //                    property = _target.GetComponent<ObjectProperty>();
            //                    SelectObjectController.Instance.DetectedDevice(_target, _isMove, _isEvent);
            //
            //                    EventController.Instance.InvokeSelectedViewItem(property, _isMove);
            //                }
            //
            //                isMoveToTarget = false;
            //                CameraController.Instance.cur_Rotation = camera_Tr.eulerAngles;
            //            }
            //            break;
            //        }
            //}
        }
        #endregion
    }
}