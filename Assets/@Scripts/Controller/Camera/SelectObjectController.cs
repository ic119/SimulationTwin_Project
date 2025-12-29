using DG.Tweening;
using JJORY.Util;
using UnityEngine;

public class SelectObjectController : SingletonObject<SelectObjectController>
{
    #region Variable
    [Header("Door Object 이동 값")]
    [SerializeField] private float closeHeight = 1.558f;
    [SerializeField] private float openHeight = 4.11f;

    [Header("Rack Object 이동 값")]
    [SerializeField] private float forwardDistance = 3.5f;
    [SerializeField] private float backDistance = 0.5f;

    private GameObject container_Door;
    private GameObject rack_container;
    private Tween moduleSequence;
    private bool isAnimating = false;

    [Header("Tween 변수")]
    private DG.Tweening.Sequence module_Sep;
    #endregion

    #region Method
    /// <summary>
    /// 장비 오브젝트 탐지
    /// </summary>
    /// <param name="_go"></param>
    /// <param name="_isActive"></param>
    /// <param name="_isEvent"></param>
    public void DetectedDevice(GameObject _go, bool _isActive, bool _isEvent)
    {
        if (_go == null)
        {
            Utils.CreateLogMessage<SelectObjectController>("DetectedDevice: target GameObject is null");
            return;
        }

        
    }

    #endregion
}
