using System;
using UnityEngine;

namespace UI
{
    public class MenuView : MonoBehaviour, IView
    {
        public Action<int> onStartGame;

        public void OnStartGame(int countBalls)
        {
            onStartGame?.Invoke(countBalls);
        }

        public void OnExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
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