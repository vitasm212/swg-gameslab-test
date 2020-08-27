using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TopPanelView : MonoBehaviour, IView
    {
        [SerializeField] private Text textPoint = null;

        public void SetPoint(int value)
        {
            textPoint.text = $"Points: {value}";
        }

        public void Close()
        {
            Hide();
            GameObject.Destroy(gameObject);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}