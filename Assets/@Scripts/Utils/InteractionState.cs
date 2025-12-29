using UnityEngine;

namespace JJORY.Util
{
    /// <summary>
    /// 마우스 입력방식에 따라 중첩되는 방식에 플래그를 둠으로써 중복되지않게 처리
    /// </summary>
    public class InteractionState : MonoBehaviour
    {
        public static InteractionType cur_InteractionType { get; private set; } = InteractionType.None;
        public static bool isInteracting => cur_InteractionType != InteractionType.None;

        /// <summary>
        /// 상호작용 시작 시 해당 Type으로 변경
        /// </summary>
        /// <param name="_type"></param>
        public static void Begin(InteractionType _type)
        {
            cur_InteractionType = _type;
        }

        /// <summary>
        /// 상호작용 종료 시 초기화
        /// </summary>
        /// <param name="_type"></param>
        public static void End(InteractionType _type)
        {
            if (cur_InteractionType == _type)
            {
                cur_InteractionType = InteractionType.None;
            }
        }
    }
}