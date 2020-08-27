using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control.Core
{
    public class GameBoard : MonoBehaviour
    {
        private const float speedMove = 10f;
        public Action<int> onFindCombination;
        public bool IsProcessing { get; private set; } = true;

        private const int sizeBoard = 6;
        private int countTypeBalls = 4;
        private Ball[,] balls = new Ball[sizeBoard, sizeBoard];

        public void Init(int countBalls, Action<int, int> OnSelectCell)
        {
            countTypeBalls = countBalls;
            for (var x = 0; x < sizeBoard; x++)
                for (var y = 0; y < sizeBoard; y++)
                {
                    balls[x, y].cellView = GameObject.Instantiate(Resources.Load<CellView>("View/Cell"), transform);
                    balls[x, y].cellView.Setup(x, y);
                    balls[x, y].cellView.onClickCell += OnSelectCell;

                    var ignoreType1 = 0;
                    var ignoreType2 = 0;
                    if (x > 1 && balls[x - 1, y].typeBall == balls[x - 2, y].typeBall)
                        ignoreType1 = balls[x - 1, y].typeBall;
                    if (y > 1 && balls[x, y - 1].typeBall == balls[x, y - 2].typeBall)
                        ignoreType2 = balls[x, y - 1].typeBall;

                    balls[x, y].deleteFlag = DeleteFlag.None;
                    balls[x, y].typeBall = GenerateType(ignoreType1, ignoreType2);
                    balls[x, y].ballView = GameObject.Instantiate(Resources.Load<BallView>("View/Ball"), balls[x, y].cellView.transform);

                    balls[x, y].ballView.SetColor(Combinations.GetColor(balls[x, y].typeBall));
                    balls[x, y].ballView.transform.localPosition = new Vector3(0, 0, 0);
                }

            var countDelete = 3;
            while (countDelete > 0)
            {
                var x = UnityEngine.Random.Range(0, sizeBoard);
                var y = UnityEngine.Random.Range(0, sizeBoard);
                if (balls[x, y].cellView.gameObject != null)
                {
                    GameObject.Destroy(balls[x, y].cellView.gameObject);
                    countDelete--;
                }
            }
            IsProcessing = false;
        }

        private void SwapBalls(int x1, int y1, int x2, int y2)
        {
            balls[x1, y1].ballView.transform.SetParent(balls[x2, y2].cellView.transform);
            balls[x2, y2].ballView.transform.SetParent(balls[x1, y1].cellView.transform);

            var temp = balls[x1, y1].ballView;
            var tepmType = balls[x1, y1].typeBall;
            balls[x1, y1].ballView = balls[x2, y2].ballView;
            balls[x1, y1].typeBall = balls[x2, y2].typeBall;
            balls[x2, y2].ballView = temp;
            balls[x2, y2].typeBall = tepmType;
        }

        private IEnumerator MoveBall()
        {
            while (TestMove())
            {
                for (var x = 0; x < sizeBoard; x++)
                    for (var y = 0; y < sizeBoard; y++)
                    {
                        if (balls[x, y].cellView == null)
                            continue;
                        balls[x, y].group = 0;
                        if (balls[x, y].ballView.transform.localPosition.magnitude > 0.01f)
                        {
                            var pos = balls[x, y].ballView.transform.localPosition;
                            var stepMove = Time.deltaTime * speedMove > balls[x, y].ballView.transform.localPosition.magnitude
                                ? balls[x, y].ballView.transform.localPosition.magnitude : Time.deltaTime * speedMove;

                            if (pos.x != 0)
                                pos.x += pos.x < 0 ? stepMove : -stepMove;
                            if (pos.y != 0)
                                pos.y += pos.y < 0 ? stepMove : -stepMove;
                            balls[x, y].ballView.transform.localPosition = pos;
                        }
                        else
                        {
                            balls[x, y].ballView.transform.localPosition = new Vector3(0, 0, 0);
                        }
                    }
                yield return null;
            }
        }

        private IEnumerator DeleteBall()
        {
            var waitDelete = true;
            while (waitDelete)
            {
                waitDelete = false;
                for (var x = 0; x < sizeBoard; x++)
                    for (var y = 0; y < sizeBoard; y++)
                    {
                        if (balls[x, y].cellView != null)
                            if (balls[x, y].deleteFlag == DeleteFlag.Preparation)
                            {
                                waitDelete = true;
                                var color = balls[x, y].ballView.GetColor();
                                color.a -= Time.deltaTime * speedMove;
                                balls[x, y].ballView.SetColor(color);
                                if (color.a < 0.1f)
                                {
                                    balls[x, y].deleteFlag = DeleteFlag.Delete;
                                    GameObject.Destroy(balls[x, y].ballView.gameObject);
                                    balls[x, y].ballView = null;
                                }
                            }
                    }
                yield return null;
            }

            for (var x = 0; x < sizeBoard; x++)
                for (var y = sizeBoard - 1; y > 0; y--)
                {
                    if (balls[x, y].deleteFlag == DeleteFlag.Delete)
                    {
                        for (var y2 = y - 1; y2 >= 0; y2--)
                        {
                            if (balls[x, y2].cellView != null)
                                if (balls[x, y2].deleteFlag != DeleteFlag.Delete)
                                {
                                    balls[x, y2].ballView.transform.SetParent(balls[x, y].cellView.transform);
                                    balls[x, y].ballView = balls[x, y2].ballView;
                                    balls[x, y].typeBall = balls[x, y2].typeBall;
                                    balls[x, y].deleteFlag = 0;

                                    balls[x, y2].deleteFlag = DeleteFlag.Delete;
                                    balls[x, y2].ballView = null;
                                    break;
                                }
                        }
                    }
                }
        }

        private void AddBall()
        {
            for (var x = 0; x < sizeBoard; x++)
                for (var y = 0; y < sizeBoard; y++)
                {
                    if (balls[x, y].deleteFlag == DeleteFlag.Delete)
                    {
                        balls[x, y].typeBall = GenerateType(0, 0);
                        if (balls[x, y].ballView == null)
                        {
                            balls[x, y].ballView = GameObject.Instantiate(Resources.Load<BallView>("View/Ball"), balls[x, y].cellView.transform);
                        }
                        balls[x, y].deleteFlag = DeleteFlag.None;
                        balls[x, y].ballView.SetColor(Combinations.GetColor(balls[x, y].typeBall));
                        balls[x, y].ballView.transform.localPosition = new Vector3(0, -15, 0);
                    }
                }
        }

        public IEnumerator Swap(int x1, int y1, int x2, int y2)
        {
            IsProcessing = true;
            SwapBalls(x1, y1, x2, y2);

            yield return StartCoroutine(MoveBall());
            bool undo = true;

            while (TestCombination())
            {
                undo = false;
                yield return StartCoroutine(DeleteBall());
                AddBall();
                yield return StartCoroutine(MoveBall());
            }

            if (undo)
            {
                SwapBalls(x1, y1, x2, y2);
                yield return StartCoroutine(MoveBall());
            }
            IsProcessing = false;
        }

        public bool TestMix()
        {
            for (var x = 0; x < sizeBoard - 1; x++)
                for (var y = 0; y < sizeBoard - 1; y++)
                {
                    if (balls[x, y].cellView != null)
                    {
                        var points = 0;
                        if (balls[x + 1, y].cellView != null)
                        {
                            SwapBalls(x, y, x + 1, y);
                            points = FindCombination(Combinations.pointsCombinations10, false);
                            SwapBalls(x, y, x + 1, y);
                            if (points > 0)
                                return false;
                        }
                        if (balls[x, y + 1].cellView != null)
                        {
                            SwapBalls(x, y, x, y + 1);
                            points = FindCombination(Combinations.pointsCombinations10, false);
                            SwapBalls(x, y, x, y + 1);
                            if (points > 0)
                                return false;
                        }
                    }
                }
            IsProcessing = true;
            return true;
        }

        public void MixBalls()
        {
            var count = sizeBoard * sizeBoard;
            while (count > 0)
            {
                var x1 = UnityEngine.Random.Range(0, sizeBoard);
                var y1 = UnityEngine.Random.Range(0, sizeBoard);
                var x2 = UnityEngine.Random.Range(0, sizeBoard);
                var y2 = UnityEngine.Random.Range(0, sizeBoard);

                if (balls[x1, y1].cellView != null && balls[x2, y2].cellView != null)
                {
                    SwapBalls(x1, y1, x2, y2);
                    count--;
                }
            }
            StartCoroutine(StartMix());
        }

        public IEnumerator StartMix()
        {
            yield return StartCoroutine(MoveBall());
            IsProcessing = false;
        }

        private bool TestCombination()
        {
            var points = 0;

            points += 30 * FindCombination(Combinations.pointsCombinations30, true);
            points += 20 * FindCombination(Combinations.pointsCombinations20, true);
            points += 15 * FindCombination(Combinations.pointsCombinations15, true);
            points += 10 * FindCombination(Combinations.pointsCombinations10, true);

            if (points > 0)
                onFindCombination?.Invoke(points);

            return points > 0;
        }

        private int FindCombination(List<int[,]> combinations, bool markDelete)
        {
            var countCombination = 0;
            foreach (var combination in combinations)
            {
                for (var x = 0; x < sizeBoard; x++)
                    for (var y = 0; y < sizeBoard; y++)
                    {
                        if (balls[x, y].deleteFlag == DeleteFlag.None)
                        {

                            if (combination.GetLength(0) + x <= sizeBoard && combination.GetLength(1) + y <= sizeBoard)
                            {
                                var curentTypeBall = -1;
                                var ready = true;
                                for (var x1 = 0; x1 < combination.GetLength(0); x1++)
                                    for (var y1 = 0; y1 < combination.GetLength(1); y1++)
                                    {
                                        if (combination[x1, y1] == 1)
                                        {
                                            if (curentTypeBall == -1)
                                                curentTypeBall = balls[x + x1, y + y1].typeBall;
                                            if (balls[x + x1, y + y1].cellView == null
                                                || balls[x + x1, y + y1].typeBall != curentTypeBall
                                                || balls[x + x1, y + y1].deleteFlag != DeleteFlag.None)
                                            {
                                                ready = false;
                                                break;
                                            }
                                        }
                                    }
                                if (ready)
                                {
                                    countCombination++;
                                    if (markDelete)
                                    {
                                        for (var x1 = 0; x1 < combination.GetLength(0); x1++)
                                            for (var y1 = 0; y1 < combination.GetLength(1); y1++)
                                            {
                                                if (combination[x1, y1] == 1)
                                                    balls[x + x1, y + y1].deleteFlag = DeleteFlag.Preparation;
                                            }
                                    }
                                }
                            }
                        }
                    }
            }
            return countCombination;
        }

        private bool TestMove()
        {
            for (var x = 0; x < sizeBoard; x++)
                for (var y = 0; y < sizeBoard; y++)
                {
                    if (balls[x, y].cellView != null)
                        if (balls[x, y].ballView.transform.localPosition.magnitude > 0)
                            return true;
                }
            return false;
        }

        private int GenerateType(int ignoreType1, int ignoreType2)
        {
            var typeBall = ignoreType1;

            while (typeBall == ignoreType1 || typeBall == ignoreType2)
                typeBall = UnityEngine.Random.Range(1, countTypeBalls + 1);

            return typeBall;
        }
    }
}