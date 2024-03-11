using System;
using System.Collections;
using System.Collections.Generic;
using GameGrid;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TurnData.FragmentedTurn
{
    public class FragmentedTurnContext : ITurnContext
    {
        private readonly Func<IEnumerator, Coroutine> _startCoroutine;
        private readonly Queue<TurnFragment> _fragments;
        private TurnFragment _activeFragment;

        private bool _mainFragmentFinished;
        private bool _finished;

        public FragmentedTurnContext(Func<IEnumerator, Coroutine> startCoroutine)
        {
            _startCoroutine = startCoroutine;

            _fragments = new Queue<TurnFragment>();

            var mainFragment = new TurnFragment(_startCoroutine, TurnFragmentType.Main);
            _fragments.Enqueue(mainFragment);
            _activeFragment = mainFragment;
        }

        public Direction TurnDirection { get; set; }

        public event Action TurnFinished;

        public event Action MainFragmentCompleted;

        public void StartTurn()
        {
            StartNext();
        }

        public void ContinueTurn()
        {
            if (_finished)
            {
                return;
            }

            if (_activeFragment is null)
            {
                StartNext();
            }
        }

        public ITurnContext Next(TurnAction action)
        {
            _activeFragment!.Next(action);
            return this;
        }

        public void Destroy(Object obj, int secondsDelay = 3)
        {
            throw new NotImplementedException();
        }

        public void NextFragment(TurnAction action)
        {
            var fragment = new TurnFragment(_startCoroutine, TurnFragmentType.EndOfTurn);
            fragment.Next(action);

            _fragments.Enqueue(fragment);
        }

        private void StartNext()
        {
            _activeFragment = null;

            if (_fragments.Count == 0)
            {
                if (_mainFragmentFinished)
                {
                    _finished = true;
                    TurnFinished?.Invoke();
                }
                else
                {
                    _mainFragmentFinished = true;
                    MainFragmentCompleted?.Invoke();
                }

                return;
            }

            _activeFragment = _fragments.Dequeue();
            _activeFragment.Completed += StartNext;
            _activeFragment.Start();
        }
    }
}