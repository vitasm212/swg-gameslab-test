using System;
using Control.Core;
using UI;
using UnityEngine;

namespace Control
{
    public class GameControl : MonoBehaviour
    {
        [SerializeField] private UIController uiController = null;
        [SerializeField] private GameBoard gameBoard = null;
        [SerializeField] private GameObject selector = null;

        private int selectedX = -1;
        private int selectedY = -1;

        private int points = 0;

        void Start()
        {
            uiController.MenuView().Show();
            uiController.MenuView().onStartGame += OnStartGame;
            selector.SetActive(false);
        }

        private void Update()
        {
            if (gameBoard.IsProcessing)
                return;
            if (gameBoard.TestMix())
            {
                uiController.MixPanelView().Show();
                uiController.MixPanelView().onMixClick = gameBoard.MixBalls;
            }
        }

        private void OnStartGame(int countBalls)
        {
            uiController.MenuView().Hide();
            uiController.TopPanelView().Show();
            uiController.TopPanelView().SetPoint(points);

            gameBoard.Init(countBalls, OnSelectCell);
            gameBoard.onFindCombination = OnFindCombination;
        }

        private void OnFindCombination(int addPoints)
        {
            points += addPoints;
            uiController.TopPanelView().SetPoint(points);
        }

        private void OnSelectCell(int x, int y)
        {
            if (gameBoard.IsProcessing)
                return;
            selector.transform.localPosition = new Vector3(x * 2.56f, y * 2.56f, 0);

            selector.SetActive(selectedX == -1);

            if (selectedX == -1)
            {
                selectedX = x;
                selectedY = y;
            }
            else
            {
                if ((x == selectedX && Mathf.Abs(y - selectedY) == 1) || (y == selectedY && Mathf.Abs(x - selectedX) == 1))
                {
                    StartCoroutine(gameBoard.Swap(x, y, selectedX, selectedY));
                }
                selectedX = -1;
                selectedY = -1;
                if (Mathf.Abs(y - selectedY) > 1 || Mathf.Abs(x - selectedX) > 1)
                    OnSelectCell(x, y);
            }
        }
    }
}