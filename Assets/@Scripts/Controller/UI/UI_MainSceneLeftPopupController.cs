using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace JJORY.Controller.UI
{
    public class UI_MainSceneLeftPopupController : MonoBehaviour
    {
        #region Variable
        [Header("TabContents Variable")]
        [SerializeField] private GameObject tabContentsContainer;
        [SerializeField] private List<GameObject> tabContentsList = new List<GameObject>();

        [Header("TabButtons Variable")]
        [SerializeField] private GameObject tabButtonsContainer;
        [SerializeField] private List<GameObject> tabButtonsList = new List<GameObject>();

        [Header("Tabs Variable")]
        private int tabIndex = 0;
        #endregion

        #region LifeCycle
        private void Start()
        {
            Init();
        }
        #endregion

        #region Method
        private void Init()
        {
            if (tabContentsContainer != null)
            {
                for (int i = 0; i < tabContentsContainer.transform.childCount; i++)
                {
                    tabContentsList.Add(tabContentsContainer.transform.GetChild(i).gameObject);
                }
            }

            if (tabButtonsContainer != null)
            {
                for (int i = 0; i < tabButtonsContainer.transform.childCount; i++)
                {
                    tabContentsList.Add(tabButtonsContainer.transform.GetChild(i).gameObject);
                }
            }
        }

        private void ShowTabContent(int _index)
        {

        }

        private void OnClickedTabButton()
        {

        }
        #endregion
    }
}
