using System;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        private MenuView menuView;
        private TopPanelView topPanelView;
        private MixPanelView mixPanelView;

        public MixPanelView MixPanelView()
        {
            if (mixPanelView == null)
                mixPanelView = InstantiateView<MixPanelView>("MixPanelView");

            return mixPanelView;
        }

        public TopPanelView TopPanelView()
        {
            if (topPanelView == null)
                topPanelView = InstantiateView<TopPanelView>("TopPanelView");

            return topPanelView;
        }

        public MenuView MenuView()
        {
            if (menuView == null)
                menuView = InstantiateView<MenuView>("MenuView");

            return menuView;
        }

        private T InstantiateView<T>(string resourcePath, bool useExist = true) where T : UnityEngine.Object, IView
        {
            T view = null;

            if (useExist)
                view = GameObject.FindObjectOfType<T>();

            if (view == null)
            {
                GameObject prefab = Resources.Load<GameObject>("UI/View/" + resourcePath);
                if (prefab == null)
                    throw new Exception(string.Format($"Не найден префаб UI/View/{resourcePath} для {typeof(T)}"));

                GameObject go = GameObject.Instantiate(prefab);
                go.transform.SetParent(transform, false);
                view = go.GetComponent<T>();
            }
            view.Hide();
            return view;
        }
    }
}