using UnityEngine;
using UnityEngine.UI;

namespace JJORY.View.UI
{
    public class TabButtonView : MonoBehaviour
    {
        #region Variable
        [Header("Button UI")]
        [SerializeField] private Button tabButton;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Sprite unselectedSprite;

        private bool isSelected = false;
        #endregion

        #region LifeCycle
        private void Start()
        {
            if (tabButton == null)
            {
                tabButton = GetComponent<Button>();
            }
            else
            {
                if (buttonImage == null)
                {
                    buttonImage = tabButton.GetComponent<Image>();
                }
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// Tab버튼 클릭 여부 이벤트 
        /// </summary>
        /// <param name="_isSelected"></param>
        public void OnClickedButton(bool _isSelected)
        {
            if (_isSelected == true)
            {
                buttonImage.sprite = selectedSprite;
                isSelected = true;
            }
            else
            {
                buttonImage.sprite = unselectedSprite;
                isSelected = false;
            }
        }
        #endregion
    }
}