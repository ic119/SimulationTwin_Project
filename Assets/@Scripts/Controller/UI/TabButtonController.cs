using JJORY.Util;
using UnityEngine;
using UnityEngine.UI;

namespace JJORY.Controller.UI
{
    public class TabButtonController : MonoBehaviour
    {
        #region Variable
        [Header("Tab Button & Tab Content")]
        [SerializeField] private Button tabButton;
        [SerializeField] private GameObject tabContent;
        #endregion

        #region LifeCycle
        private void Start()
        {
            if (tabButton == null)
            {
                tabButton = GetComponent<Button>();
            }

            if (tabContent == null)
            {
                Utils.CreateLogMessage<TabButtonController>("tabContent ¾øÀ½");
            }
        }
        #endregion

        #region Method
        #endregion
    }
}