using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
	public class Sequencer : MonoBehaviour
	{
		public event Action OnStart = delegate { };
		public event Action OnFinish = delegate { };

		private SortedDictionary<int, Queue<IEnumerator>> actionsDirectory = new SortedDictionary<int, Queue<IEnumerator>>();

		public void Enqueue(IEnumerator action, int priority = 0)
		{
			bool hasQueue = actionsDirectory.TryGetValue(priority, out Queue<IEnumerator> queue);
			if(!hasQueue) { actionsDirectory[priority] = queue = new Queue<IEnumerator>(); }
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

		private IEnumerator SequencerRoutine()
		{
			OnStart();
			bool shouldRun = true;
			while(shouldRun) {
				bool hasRunAction = false;
				foreach(KeyValuePair<int, Queue<IEnumerator>> pair in actionsDirectory) {
					Queue<IEnumerator> queue = actionsDirectory[pair.Key];
					if(queue.Count > 0) {
						yield return StartCoroutine(queue.Dequeue());
						hasRunAction = true;
						break; 
					}
				}
				shouldRun = hasRunAction;
				yield return null;
			}
			OnFinish();
		}
	}
}
