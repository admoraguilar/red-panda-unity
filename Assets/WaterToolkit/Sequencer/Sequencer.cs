using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterToolkit.Sequences
{
	public class Sequencer
	{
		private static MonoDelegate monoDelegate => MonoDelegate.GetOrCreate(nameof(Sequencer), true);



		public event Action OnStart = delegate { };
		public event Action OnFinish = delegate { };

		private SortedDictionary<int, Queue<IEnumerator>> _actionsDirectory = new SortedDictionary<int, Queue<IEnumerator>>();
		private List<Coroutine> _routines = new List<Coroutine>();

		public void Enqueue(IEnumerator action, int priority = 0)
		{
			bool hasQueue = _actionsDirectory.TryGetValue(priority, out Queue<IEnumerator> queue);
			if(!hasQueue) { _actionsDirectory[priority] = queue = new Queue<IEnumerator>(); }
			queue.Enqueue(action);
		}

		public void Run() {
			RunNextFrame(() => {
				StopAllCoroutines();
				StartCoroutine(SequencerRoutine());
			}); 
		}

		private void RunNextFrame(Action action)
		{
			StartCoroutine(RunNextFrameRoutine());

			IEnumerator RunNextFrameRoutine()
			{
				yield return new WaitForEndOfFrame();
				action();
			}
		}

		private Coroutine StartCoroutine(IEnumerator routine)
		{
			Coroutine result = monoDelegate.StartCoroutine(routine);
			_routines.Add(result);
			return result;
		} 

		private void StopAllCoroutines()
		{
			foreach(Coroutine routine in _routines) { monoDelegate.StopCoroutine(routine); }
			_routines.Clear();
		}

		private IEnumerator SequencerRoutine()
		{
			OnStart();
			bool shouldRun = true;
			while(shouldRun) {
				bool hasRunAction = false;
				foreach(KeyValuePair<int, Queue<IEnumerator>> pair in _actionsDirectory) {
					Queue<IEnumerator> queue = _actionsDirectory[pair.Key];
					if(queue.Count > 0) {
						yield return StartCoroutine(queue.Dequeue());
						hasRunAction = true;
						break; 
					}
				}
				shouldRun = hasRunAction;
				yield return null;
			}
			StopAllCoroutines();
			OnFinish();
		}
	}
}
