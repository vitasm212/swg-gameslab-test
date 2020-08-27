using System;
using UnityEngine;

namespace UI
{
    public class MixPanelView : MonoBehaviour, IView
    {
        public Action onMixClick;

        public void OnMixClick()
        {
            onMixClick?.Invoke();
            Close();
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