using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using System.Diagnostics;
using UnityEngine;

namespace CustomMSLibrary {

	namespace Standalone {

		/// <summary>
		/// Custom version of Tuples. 
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		public struct Duple<T1, T2> {
			public T1 V1;
			public T2 V2;

			public Duple(T1 v1, T2 v2) {
				V1 = v1;
				V2 = v2;
			}

		}

		/// <summary>
		/// Priority Queue
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public class PriorityQueue<T> {
			private List<Duple<T, float>> queue;
			private readonly bool greaterFirst;

			public int Count {
				get
				{
					return queue.Count;
				}
			}

			public PriorityQueue(bool priorityIsGreater = true) {
				queue = new List<Duple<T, float>>();
				greaterFirst = priorityIsGreater;
			}

			public void Enqueue(T item, float cost) {
				queue.Add(new Duple<T, float>(item, cost));
			}

			public void Enqueue(Duple<T, float> item) {
				queue.Add(item);
			}

			public T Dequeue() {
				if (greaterFirst)
					return SearchGreater().V1;
				else
					return SearchLesser().V1;
			}

			private Duple<T, float> SearchLesser() {
				float testValue = 0;
				int testIndex = -1;
				int queueCount = queue.Count;
				for (int i = 0; i < queueCount; i++)
				{
					if (queue[i].V2 <= testValue)
					{
						testIndex = i;
						testValue = queue[i].V2;
					}
				}
				if (testIndex == -1)
					return default(Duple<T, float>);
				Duple<T, float> returned = queue[testIndex];
				queue.RemoveAt(testIndex);
				return returned;

			}

			private Duple<T, float> SearchGreater() {
				float testValue = 0;
				int testIndex = -1;
				int queueCount = queue.Count;
				for (int i = 0; i < queueCount; i++)
				{
					if (queue[i].V2 >= testValue)
					{
						testIndex = i;
						testValue = queue[i].V2;
					}
				}
				if (testIndex == -1)
					return default(Duple<T, float>);
				Duple<T, float> returned = queue[testIndex];
				queue.RemoveAt(testIndex);
				return returned;

			}

		}

		/// <summary>
		/// Miscelaneous extension methods
		/// </summary>
		public static class CustomExtensions {

			/// <summary>
			/// Get individual bit from a byte.
			/// </summary>
			/// <param name="input">Byte to extract boolean bit from.</param>
			/// <param name="index">Index from 0 to 7 of the desired bit within the byte.</param>
			/// <returns></returns>
			public static bool GetBit(this byte input, int index) {
				if (index < 0 || index > 7) throw new IndexOutOfRangeException();
				return (input & (1 << index)) != 0;
			}

			/// <summary>
			/// Set individual bit to a byte. This does not modifies the referenced byte.
			/// </summary>
			/// <param name="thisByte">Byte to insert bit into.</param>
			/// <param name="index">Index from 0 to 7 of the desired bit within the byte.</param>
			/// <param name="value">Boolean value to set bit as (0 or 1).</param>
			/// <returns></returns>
			public static byte SetBit(this byte thisByte, int index, bool value)
			{
				if (index < 0 || index > 7) throw new IndexOutOfRangeException();
				if (value)
					thisByte = (byte)(thisByte | 1 << index);
				else
					thisByte = (byte)(thisByte & ~(1 << index));
				return thisByte;
			}

			/// <summary>
			/// Same as SetBit, but instead actually modifies the referenced byte.
			/// Set individual bit to a byte.
			/// </summary>
			/// <param name="thisByte">Byte to insert bit into.</param>
			/// <param name="index">Index from 0 to 7 of the desired bit within the byte.</param>
			/// <param name="value">Boolean value to set bit as (0 or 1).</param>
			public static void RefSetBit(this ref byte thisByte, int index, bool value) =>
				thisByte = thisByte.SetBit(index, value);

			/// <summary>
			/// Get individual bit from an int.
			/// </summary>
			/// <param name="input">Int to extract boolean bit from.</param>
			/// <param name="index">Index from 0 to 31 of the desired bit within the int.</param>
			/// <returns></returns>
			public static bool GetBit(this int input, int index) {
				if (index < 0 || index > 31) throw new IndexOutOfRangeException();
				return (input & (1 << index)) != 0;
			}

			/// <summary>
			/// Set individual bit to an in. This does not modifies the referenced int.
			/// </summary>
			/// <param name="input">Int to insert bit into.</param>
			/// <param name="value">Boolean value to set bit as (0 or 1).</param>
			/// <param name="index">Index from 0 to 31 of the desired bit within the byte.</param>
			/// <returns></returns>
			public static int SetBit(this int input, int index, bool value) {
				if (index < 0 || index > 31) throw new IndexOutOfRangeException();
				if (value)
					input = (byte)(input | 1 << index);
				else
					input = (byte)(input & ~(1 << index));
				return input;
			}

			/// <summary>
			/// Get a slice of an array as a new array.
			/// </summary>
			/// <param name="source">Source array from which the slice will be made.</param>
			/// <param name="start">Index from the original array from which to begin the slice.</param>
			/// <param name="count">Count of elements to copy from the array.</param>
			/// <returns></returns>
			public static T[] Slice<T>(this T[] source, int start, int count) {
				var array = new T[count];
				float limit = count + start;
				int c = 0;
				for (int i = start; i < limit; i++)
				{
					array[c] = source[i];
					c++;
				}
				return array;
			}
		}
		/// <summary>
		/// Structure containing 8 boolean values stored inside a byte.
		/// Intended to optimize the use of boolean values in memory at the cost of processing.
		/// </summary>
		public struct BoolByte {
			private byte data;

			public byte RawData { get => data; }

			/// <summary>
			/// Create new BoolByte package from boolean values.
			/// </summary>
			/// <param name="data">Boolean values.</param>
			public BoolByte(params bool[] data) {
				int length;
				if (data.Length > 8)
				{
					length = 8;
					Debug.Log("BoolByte created was provided with more than 8 values. Values with index 8 and above ignored.");
				} else
					length = data.Length;
				this.data = 0;
				for (int i = 0; i < length; i++)
				{
					this.data = this.data.SetBit(i, data[i]);
				}
			}

			/// <summary>
			/// Create new BoolByte package from a preexisting byte.
			/// </summary>
			/// <param name="data">Byte value.</param>
			public BoolByte(byte data) {
				this.data = data;
			}

			public bool this[int i] {
				get
				{
					if (i < 0 || i > 7) throw new System.IndexOutOfRangeException();
					return data.GetBit(i);
				}
				set
				{
					if (i < 0 || i > 7) throw new System.IndexOutOfRangeException();
					data = data.SetBit(i, value);
				}
			}

			#region Overrides
			public override bool Equals(object obj)
			{
				return (data.Equals(obj) && (obj.GetType() == typeof(BoolByte)));
			}

			public override int GetHashCode()
			{
				return (((data.GetHashCode() + 14388) * 3)-2155)/4;
			}
			#endregion

			#region Operators
			public static bool operator ==(BoolByte l, BoolByte r) => l.data == r.data;
			public static bool operator !=(BoolByte l, BoolByte r) => l.data != r.data;
			#endregion
		}

		/// <summary>
		/// A list with it's own index with a Next method and a looping behaviour, preventing Out Of Index errors unless the count is 0.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public class LoopingList<T> {
			private int index = 0;
			private List<T> list;
			public List<T> List {
				get
				{
					return list;
				}
			}

			/// <summary>
			/// Points to LAST USED value.
			/// Setting this index to point a value and using 'Next' will result in returning the value indexed after the set one.
			/// </summary>
			public int Index {
				get
				{
					return index;
				}
				set
				{
					value = (value % list.Count);
					index = value;
				}
			}

			/// <summary>
			/// Count of items in the list.
			/// </summary>
			public int Count {
				get
				{
					return List.Count;
				}
			}

			public LoopingList() {
				list = new List<T>();
			}

			public LoopingList(List<T> l) {
				list = new List<T>(l);
			}

			/// <summary>
			/// Returns next value from the loop. This updates its internal index, then returns value.
			/// </summary>
			public T Next {
				get
				{
					Index += 1;
					return list[Index];
				}
			}

			public void Add(T value) {
				list.Add(value);
			}

			public void Remove(T value) {
				list.Remove(value);
			}

			/// <summary>
			/// CAREFUL: Removing at a defined index still uses the looping behaviour.
			/// Be sure to know which element you're trying to delete after the looping behaviour!
			/// </summary>
			/// <param name="index"></param>
			public void RemoveAt(int index) {
				index = (index % Count);
				list.RemoveAt(index);
				if (Index >= index && Index > 0)
					Index--;
			}

			public static explicit operator LoopingList<T>(List<T> l) {
				return new LoopingList<T>(l);
			}

			public static implicit operator List<T>(LoopingList<T> l) {
				return l.List;
			}
		}

	}

}
