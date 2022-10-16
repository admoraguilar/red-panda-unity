using System;
using System.Collections.Generic;

using SRandom = System.Random;
using URandom = UnityEngine.Random;

namespace WaterToolkit
{
	/*
		Source: https://gist.github.com/omgwtfgames/f917ca28581761b8100f

		MIT License

		Copyright (c) 2021 Andrew Perry

		Permission is hereby granted, free of charge, to any person obtaining a copy
		of this software and associated documentation files (the "Software"), to deal
		in the Software without restriction, including without limitation the rights
		to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
		copies of the Software, and to permit persons to whom the Software is
		furnished to do so, subject to the following conditions:

		The above copyright notice and this permission notice shall be included in all
		copies or substantial portions of the Software.

		THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
		IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
		FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
		AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
		LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
		OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
		SOFTWARE. 
	*/
	public static class ListExtensions
	{
		public static void Shuffle<T>(this IList<T> list)
		{
			SRandom rng = new SRandom();
			int n = list.Count;
			while(n > 1) {
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static T Random<T>(this IList<T> list)
		{
			if(list.Count == 0) throw new IndexOutOfRangeException("Cannot select a random item from an empty list");
			return list[URandom.Range(0, list.Count)];
		}

		public static T Remove<T>(this IList<T> list)
		{
			if(list.Count == 0) throw new IndexOutOfRangeException("Cannot remove a random item from an empty list");
			int index = URandom.Range(0, list.Count);
			T item = list[index];
			list.RemoveAt(index);
			return item;
		}
	}

}
