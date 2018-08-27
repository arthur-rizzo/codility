using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Codility
{
    class Program
    {
		static void Main(string[] args)
		{
			Solution s = new Solution();
			var result = s.solution(new int[] {3, 1});
		}
    }

	class Solution
	{
		public int solution(int[] A)
		{
			if (A.Length == 0)
				return 0;
			if (A.Length == 1)
				return Math.Abs(A[0]);

			//Transoform A in abs(A), gets sum and max value of transformed array
			int sum = 0;
			int max = int.MinValue;
			for (int i = 0; i < A.Length; i++)
			{
				A[i] = Math.Abs(A[i]);
				sum += A[i];
				max = Math.Max(max, A[i]);
			}

			bool[] achievableSumMap = new bool[sum + 1];
			achievableSumMap[0] = true;

			for (int i = 0; i < A.Length; i++)
			{
				for(int j = sum; j >= 0; j--)
				{
					var position = j - A[i];
					if (position >= 0)
					{
						if (achievableSumMap[position])
							achievableSumMap[j] = true;
					}
					else
						break;
				}
			}

			int target = sum / 2;
			int lessOrEqualToTargetSum = 0;

			for(int i = target; i >= 0; i--)
				if(achievableSumMap[i])
				{
					lessOrEqualToTargetSum = i;
					break;
				}

			return sum - (2*lessOrEqualToTargetSum);
		}

		public int Dice(int[] A)
		{
			int?[] partialResults = new int?[A.Length];

			int position = A.Length - 1;
			return dice(A, position, partialResults);
		}
		private int dice(int[] a, int position, int?[] partialResults)
		{
			if (partialResults[position] != null)
				return partialResults[position].Value;
			else
			{
				int? maxResult = null;
				for(int i = 1; i <= 6; i++)
					if ((position - i) >= 0)
					{
						var positionMinusIResult = dice(a, position - i, partialResults);
						if (!maxResult.HasValue || positionMinusIResult > maxResult.Value)
							maxResult = positionMinusIResult;
					}

				int result = a[position] + maxResult.GetValueOrDefault();
				partialResults[position] = result;
				return result;
			}
		}

		public int Ropes(int K, int[] A)
		{
			int result = 0;
			int currentSum = 0;

			for (int i = 0; i < A.Length; i++)
			{
				currentSum += A[i];
				if (currentSum >= K)
				{ 
					result++;
					currentSum = 0;
				}
			}

			return result;
		}

		public int OverlapingIntervals(int[] A, int[] B)
		{
			if (A.Length == 0)
				return 0;

			int lastStart = A[0];
			int lastEnd = B[0];
			int result = 1; //Adds first

			for(int i = 1; i < A.Length; i++)
			{
				var currentStart = A[i];
				var currentEnd = B[i];

				//does overlap
				if(currentStart <= lastEnd)
					continue;
				else //current can be added to set and be lastResult
				{
					result++;
					lastStart = currentStart;
					lastEnd = currentEnd;
				}
			}

			return result;
		}

		public int MixAbsSumOfTwo(int[] A)
		{
			Array.Sort(A);

			int result = int.MaxValue;

			//finds element closer to zero
			int closerToZeroValue = int.MaxValue;
			int closerToZeroIndex = 0;
			for(int i = 0; i < A.Length; i++)
			{
				if(Math.Abs(A[i]) < closerToZeroValue)
				{
					closerToZeroValue = Math.Abs(A[i]);
					closerToZeroIndex = i;
				}
			}

			//start at index closer to zero
			int left = closerToZeroIndex;
			int right = closerToZeroIndex;
			while(true)
			{
				var sum = A[left] + A[right];
				var value = Math.Abs(sum);

				if (value < result)
				{ 
					result = value;
					if (result == 0)
						return result;
				}

				//try to move right++
				if(sum < 0)
				{
					if (right + 1 > A.Length - 1)
						break;
					else
						right++;
				}
				else //greater than zero
				{
					//try to move left--
					if (left - 1 < 0)
						break;
					else
						left--;
				}
			}

			return result;
		}

		public int Triangles(int[] A)
		{
			if (A.Length < 3)
				return 0;

			int result = 0;
			Array.Sort(A);

			for(int x = 0; x < A.Length; x++)
			{
				int z = x + 2;
				for(int y = x+1; y < A.Length; y++)
				{
					while (z < A.Length && A[x] + A[y] > A[z])
						z++;

					result += z - y - 1; //since Z is already at a position where triangle is not possible, 
				}
			}
			return result;
		}

		public int solution(int M, int[] A)
		{
			int result = 0;
			HashSet<int> s = new HashSet<int>();

			if (A.Length == 1)
				return 1;

			int bot = 0;
			int top = 0;

			while(bot < A.Length)
			{
				if(top < A.Length && !s.Contains(A[top]))
				{
					s.Add(A[top]);
					top++;

					//counts for current closed block
					int numberofElements = top - bot;
					result += numberofElements;
					if (result > 1000000000)
						return 1000000000;
				}
				else
				{
					while(bot < A.Length && top < A.Length && A[bot] != A[top])
					{
						s.Remove(A[bot]);
						bot++;
					}

					s.Remove(A[bot]);
					bot++;
				}
			}

			return result;
		}

		public int DistinctAbs(int[] A)
		{
			return A.Where(x => x != int.MinValue).Select(Math.Abs).Distinct().Count();
		}

		public int MinMax(int K, int M, int[] A)
		{
			var max = A.Sum();
			var min = A.Max();

			int result = 0;

			while(min <= max)
			{
				var mid = (min + max) / 2;

				var blocksCount = blockNeededForLargeSum(A, mid);
				if(blocksCount <= K)
				{
					max = mid - 1;
					result = mid;
				}
				else
				{
					min = mid + 1;
				}
			}

			return result;
		}

		private int blockNeededForLargeSum(int[] a, int mid)
		{
			int result = 1;
			int sum = a[0];
			
			for(int i = 1; i < a.Length; i++)
			{
				if (sum + a[i] <= mid)
					sum += a[i];
				else
				{
					sum = a[i];
					result++;
				}
			}

			return result;
		}

		public int Nails(int[] A, int[] B, int[] C)
		{
			//nails have to be used sequentially

			int result = -1;
			int minNailsIndex = 0; //using only zeroth nail
			int maxNailsIndex = C.Length - 1; //using all nails;

			while(minNailsIndex <= maxNailsIndex)
			{
				int midIndex = (minNailsIndex + maxNailsIndex) / 2;

				if (allPlanksNailed(A, B, C, midIndex))
				{
					result = midIndex + 1;
					maxNailsIndex = midIndex - 1;
				}
				else
					minNailsIndex = midIndex + 1;
			}

			return result;
		}
		private bool allPlanksNailed(int[] a, int[] b, int[] c, int midIndex)
		{
			int[] preffixSumNails = new int[c.Length * 2 + 1];

			for (int i = 0; i <= midIndex; i++)
				preffixSumNails[c[i]] = 1;

			int nailCount = 0;
			for (int i = 0; i < preffixSumNails.Length; i++)
			{ 
				if (preffixSumNails[i] == 1)
					nailCount++;

				preffixSumNails[i] = nailCount;
			}

			for (int i = 0; i < a.Length; i++)
				if (preffixSumNails[b[i]] - preffixSumNails[a[i] - 1] == 0) //no nail for this plank
					return false;

			return true;
		}

		public List<int> fibUpTo(int n)
		{
			List<int> result = new List<int>();
			result.Add(1);
			result.Add(2);

			var lastresult = 2;
			int i = 2;
			while(lastresult < n)
			{
				result.Add(lastresult = result[i - 1] + result[i - 2]);
				i++;
			}

			return result;
		}
		public int FrogLeap(int[] A)
		{
			var minPossibility = int.MaxValue;

			var fib = fibUpTo(A.Length + 1); //Max jump size to go all the way
			var destination = A.Length;

			Dictionary<int, int> leaps = new Dictionary<int, int>(); //foreachPosition, min amount of leaps

			//leap 1
			Dictionary<int,int> minTracker = new Dictionary<int, int>() { { -1, 0 } }; //starts at -1, needs 0 leaps to get there.
			Queue<int> positions = new Queue<int>();
			positions.Enqueue(-1);

			while(positions.Count > 0 )
			{
				var position = positions.Dequeue();
				var minToPosition = minTracker[position];

				foreach(var f in fib)
				{
					var newPosition = f + position;

					if (newPosition > destination)
						break; //since fib is in ascending order, following would also be greater than destination
					if (newPosition == destination) //got to the other side safely
						minPossibility = Math.Min(minPossibility, minToPosition + 1);
					else 
					{
						if (A[newPosition] == 1)
						{
							int currentMinForNewPosition = minToPosition + 1;
							int oldMinForNewPosition;
							if (minTracker.TryGetValue(newPosition, out oldMinForNewPosition))
								minTracker[newPosition] = Math.Min(currentMinForNewPosition, oldMinForNewPosition);
							else
							{ 
								minTracker.Add(newPosition, currentMinForNewPosition);
								positions.Enqueue(newPosition);
							}
						}
					}
				}
			}

			return minPossibility == int.MaxValue ? -1 : minPossibility;
		}

		public int[] Ladders(int[] A, int[] B)
		{
			ulong[] waysUpToL = new ulong[A.Length + 1];

			waysUpToL[0] = 1;
			waysUpToL[1] = 1;

			for (int i = 2; i < waysUpToL.Length; i++)
			{
				//to get to ith step, either was at i-1 and jumped one step or was at i-2 and jumped 2 steps
				waysUpToL[i] = waysUpToL[i - 1] + waysUpToL[i - 2];
			}

			int[] result = new int[A.Length];
			for (int i = 0; i < result.Length; i++)
				result[i] = (int)(waysUpToL[A[i]] % (ulong)(1 << B[i]));

			return result;
		}

		public int PermCheck(int[] A)
		{
			BitArray array = new BitArray(A.Length);
			foreach(int value in A)
			{
				if (value < 1 || value > A.Length)
					return 0;
				else if (array.Get(value))
					return 0;
				else
					array.Set(value, true);
			}
			return 1;
		}

		public int[] PermCheck(int N, int[] A)
		{
			int[] counters = new int[N];
			int currentMaxValue = 0;
			int lastSetMaxValue = 0;

			foreach (int x in A)
			{
				if (x >= 1 && x <= N)
				{
					counters[x - 1] = Math.Max(counters[x - 1], lastSetMaxValue);
					counters[x - 1]++;

					currentMaxValue = Math.Max(currentMaxValue, counters[x - 1]);
				}
				else
					lastSetMaxValue = currentMaxValue;
			}

			for (int i = 0; i < counters.Length; i++)
				counters[i] = Math.Max(lastSetMaxValue, counters[i]);

			return counters;
		}

		public int PassingCars(int[] A)
		{
			int zeroes = 0;
			int count = 0;

			foreach(var i in A)
			{
				if (i == 0)
					zeroes++;
				else if(i == 1)
				{
					count += zeroes;
					if (count > 1000000000)
						return -1;
				}
			}

			return count;
		}

		public int NumberOfDivs(int A, int B, int K)
		{
			//if nonDivisable
			if (A % K != 0)
				A = A + (K - (A % K)); //shifts A to nearest divisable

			if (A > B)
				return 0;

			return ((B - A) / K) + 1;
		}

		public int[] GenomeSequence(string S, int[] P, int[] Q)
		{
			int[,] preffixSumArray = new int[4, S.Length];
			for(int i = 0; i < S.Length; i++)
			{
				char c = S[i];
				if (c == 'A')
					preffixSumArray[0, i] = 1;
				else if (c == 'C')
					preffixSumArray[1, i] = 1;
				else if (c == 'G')
					preffixSumArray[2, i] = 1;
				else if (c == 'T')
					preffixSumArray[3, i] = 1;

				preffixSumArray[0, i] = preffixSumArray[0, i] + ((i == 0) ? 0 : preffixSumArray[0, i - 1]);
				preffixSumArray[1, i] = preffixSumArray[1, i] + ((i == 0) ? 0 : preffixSumArray[1, i - 1]);
				preffixSumArray[2, i] = preffixSumArray[2, i] + ((i == 0) ? 0 : preffixSumArray[2, i - 1]);
				preffixSumArray[3, i] = preffixSumArray[3, i] + ((i == 0) ? 0 : preffixSumArray[3, i - 1]);

			}

			int[] results = new int[P.Length];
			for(int i = 0; i < P.Length; i++)
			{
				int bottomIndex = P[i] - 1;
				int topIndex = Q[i];

				if ((preffixSumArray[0, topIndex] - (bottomIndex < 0 ? 0 : preffixSumArray[0, bottomIndex])) > 0)
					results[i] = 1;
				else if ((preffixSumArray[1, topIndex] - (bottomIndex < 0 ? 0 : preffixSumArray[1, bottomIndex])) > 0)
					results[i] = 2;
				else if ((preffixSumArray[2, topIndex] - (bottomIndex < 0 ? 0 : preffixSumArray[2, bottomIndex])) > 0)
					results[i] = 3;
				else
					results[i] = 4;
			}

			return results;
		
		}

		public int AvgMinSlice(int[] A)
		{
			int minSliceIndex = -1;
			decimal minAvg = decimal.MinValue;

			for(int i = 0; i < A.Length - 1; i++)
			{
				decimal avgFor2 = (A[i] + A[i + 1]) / 2m;
				if(avgFor2 < minAvg)
				{
					minAvg = avgFor2;
					minSliceIndex = i;
				}

				if (i + 2 < A.Length)
				{
					decimal avgFor3 = (A[i] + A[i + 1] + A[i + 2]) / 3m;
					if (avgFor3 < minAvg)
					{
						minAvg = avgFor3;
						minSliceIndex = i;
					}
				}
			}

			return minSliceIndex;
		}

		public int MaxProductOf3(int[] A)
		{
			int[] b = A.ToArray();
			Array.Sort(b);
			return Math.Max(b[0] * b[1] * b[b.Length - 1], b[A.Length - 3] * b[A.Length - 2] * b[A.Length - 1]);
		}

		public int Triangle(int[] A)
		{
			Array.Sort(A);
			for(int i = 0; i < A.Length -2; i++)
			{
				if ((ulong)A[i] + (ulong)A[i + 1] > (ulong)A[i + 2])
					return 1;
			}
			return 0;
		}

		public int Distinct(int[] A)
		{
			int result = 0;
			HashSet<int> hash = new HashSet<int>();
			
			foreach(var a in A)
			{
				if(!hash.Contains(a))
				{
					hash.Add(a);
					result++;
				}
			}
			return result;
		}

		public int CircleIntersection(int[] A)
		{
			long[] orderedStart = new long[A.Length];
			long[] orderedEnd = new long[A.Length];
			for (int i = 0; i < A.Length; i++)
			{
				orderedStart[i] = i - A[i];
				orderedEnd[i] = i + A[i];
			}

			Array.Sort(orderedStart);
			Array.Sort(orderedEnd);

			int startIndex = 0;
			int endIndex = 0;
			int openCount = 0;
			long intersections = 0;

			while(startIndex < A.Length || endIndex < A.Length)
			{
				if(startIndex < A.Length && orderedStart[startIndex] <= orderedEnd[endIndex])
				{
					openCount++;
					startIndex++;
				}
				else if(endIndex < A.Length)
				{
					openCount--;
					intersections += openCount;
					endIndex = Math.Min(A.Length, endIndex + 1);
				}

				if (intersections > 10000000)
					return -1;
			}

			return Convert.ToInt32(intersections);
		}

		public int Parenthesis(string S)
		{
			int count = 0;
			foreach(var c in S)
			{
				if (c == '(')
					count++;
				if(c == ')')
				{
					count--;
					if (count < 0)
						return 0;
				}
			}

			return count == 0 ? 1 : 0;

		}

		public int Brackets(string S)
		{
			int countP = 0;
			int countS = 0;
			int countC = 0;

			var last = ' ';
			foreach (var c in S)
			{
				if (c == '(')
					countP++;
				else if (c == '[')
					countS++;
				else if (c == '{')
					countC++;

				else if(c == ')')
				{
					if (last == '[' || last == '{')
						return 0; 
					else
					{
						countP--;
						if (countP < 0)
							return 0;
					}
				}
				else if (c == ']')
				{
					if (last == '(' || last == '{')
						return 0;
					else
					{
						countS--;
						if (countS < 0)
							return 0;
					}
				}
				else if (c == '}')
				{
					if (last == '[' || last == '(')
						return 0;
					else
					{
						countC--;
						if (countC < 0)
							return 0;
					}
				}


				last = c;
			}

			return (countS == 0 && countP == 0 && countC == 0) ? 1 : 0;
		}

		public int Fish(int[] A, int[] B)
		{
			Stack<int> s = new Stack<int>();
			int result = A.Length;

			for (int i = 0; i < A.Length; i++)
			{
				//moving downstream
				if (B[i] == 1)
				{
					s.Push(A[i]);
				}
				else //moving upstream
				{
					if (s.Count == 0)
						continue; //fish moves upstream without trouble
					else
					{
						while(s.Count > 0 && s.Peek() < A[i])
						{
							s.Pop();
							result--; //fish flowing downstream gets eaten
						}

						if (s.Count > 0)
							result--; //means there was a fish gowing downstream bigger than our current fish (poor guy)..
					}
				}
			}

			return result;
		}

		public int ManhatanWall(int[] H)
		{
			Stack<int> s = new Stack<int>();
			int result = 0;
			foreach(var h in H)
			{
				if(s.Count == 0 || h > s.Peek())
				{
					s.Push(h);
					result++;
				}
				else if(h == s.Peek()) //user
				{
					continue;
				}
				else
				{
					while(s.Count > 0 && s.Peek() > h) //While stack top is greater or equal to current height
						s.Pop(); //top block not reusable, pops it

					if (s.Count == 0 || s.Peek() != h) //uses top block for current height
					{
						s.Push(h);
						result++; //adds new block
					}
				}
			}

			return result;
		}

		public int Leader(int[] A)
		{
			Dictionary<int, int> d = new Dictionary<int, int>(A.Length);
			for (int i = 0; i < A.Length; i++)
			{
				if (d.ContainsKey(A[i]))
					d[A[i]]++;
				else
					d[A[i]] = 1;

				if (d[A[i]] > A.Length / 2M)
					return i;
			}

			return -1;
		}

		public int leaderAux(int[] A)
		{
			Dictionary<int, int> d = new Dictionary<int, int>(A.Length);
			for (int i = 0; i < A.Length; i++)
			{
				if (d.ContainsKey(A[i]))
					d[A[i]]++;
				else
					d[A[i]] = 1;

				if (d[A[i]] > A.Length / 2M)
					return A[i];
			}

			return -1;
		}
		public int EquiLeader(int[] A)
		{
			int leader = leaderAux(A); //O(N)

			if (leader == -1)
				return 0;

			int result = 0;
			int leaderCount = A.Where(x => x == leader).Count(); //O(N)

			int leftSizeLeaderCount = 0;
			int rightSizeLeaderCount = leaderCount;

			for (int i = 0; i < A.Length - 1; i++) //O(N)
			{
				int leftSize = i + 1;
				int rightSize = A.Length - (i + 1);

				if(A[i] == leader)
				{
					leftSizeLeaderCount++;
					rightSizeLeaderCount--;
				}

				if (leftSizeLeaderCount > (leftSize / 2M) && rightSizeLeaderCount > (rightSize / 2M))
					result++;
			}

			return result;
		}

		public int Stock(int[] A)
		{
			int minBuyPrice = 0;
			int maxProfit = 0;

			for (int i = 0; i < A.Length; i++)
			{
				if (i == 0)
					minBuyPrice = A[i];
				else
				{
					int profit = A[i] - minBuyPrice;
					if (profit > maxProfit)
						maxProfit = profit;

					if (A[i] < minBuyPrice)
						minBuyPrice = A[i];
				}
			}

			return maxProfit;
		}

		public int MaxSlice(int[] A)
		{
			int maxResult = int.MinValue;
			int currentMaxSlice = 0;

			foreach(var i in A)
			{
				currentMaxSlice = Math.Max(0, currentMaxSlice) + i;
				maxResult = Math.Max(maxResult, currentMaxSlice);
			}

			return maxResult;
		}

		public int MaxSumDoubleSlice(int[] A)
		{
			int[] A1 = new int[A.Length]; //max sum that ends in index I
			int[] A2 = new int[A.Length]; //max sum thar start at index I

			for (int i = 1; i < A.Length - 1; i++) //max sum up to index I
				A1[i] = Math.Max(A1[i - 1] + A[i], 0);

			for (int i = A.Length - 2; i > 0; i--)
				A2[i] = Math.Max(A2[i + 1] + A[i], 0);

			int max = 0;
			for (int i = 1; i < A.Length - 1; i++)
			{
				max = Math.Max(A1[i - 1] + A2[i + 1], max);
			}

			return max;
		}

		public int NumberOfDivisors(int N)
		{
			int i = 1;
			int result = 0;

			while (i < Math.Sqrt(N))
			{
				if (N % i == 0)
					result += 2; //adds i and counterpart

				i++;
			}
			if (i == Math.Sqrt(N))
				result++;

			return result;
		}

		public int MinPerimeter(int N)
		{
			int i = 1;
			int minResult = int.MaxValue;

			while (i < Math.Sqrt(N))
			{
				if (N % i == 0)
					minResult = Math.Min(minResult, (i * 2) + ((N / i) * 2));
				i++;
			}
			if (i == Math.Sqrt(N))
				minResult = Math.Min(minResult, i * 4);

			return minResult;
		}

		public int Peaks(int[] A)
		{
			var divisors = AllDivisorsOrdered(A.Length);

			for (int i = 0; i < divisors.Count; i++)
			{
				int numberOfBlocks = divisors[i];
				int blockSize = A.Length / numberOfBlocks;

				if(blockSize == 1) //blocos de tamanho 1 nao funcionam pois elemento 0 e N-1 nunca terao picos..
					return i == 0 ? 0 : divisors[i - 1];

				for (int blockNumber = 0; blockNumber < numberOfBlocks; blockNumber++)
				{
					if(!BlockHasPeak(A, blockNumber, numberOfBlocks, blockSize))
						return i == 0 ? 0 : divisors[i - 1];
				}
			}

			return -1; //shouldn't get to this point
		}
		public List<int> AllDivisorsOrdered(int N)
		{
			int i = 1;
			List<int> result = new List<int>();
			Stack<int> resultStack = new Stack<int>();

			while (i < Math.Sqrt(N))
			{
				if (N % i == 0)
				{
					result.Add(i);
					resultStack.Push(N / i);
				}

				i++;
			}
			if (i == Math.Sqrt(N))
				result.Add(i);

			result.AddRange(resultStack); //adds from sorted stack

			return result;
		}
		private bool BlockHasPeak(int[] a, int blockNumber, int numberOfBlocks, int blockSize)
		{
			int startIndex = blockNumber * blockSize;
			int endIndex = startIndex + blockSize - 1;

			for(int i = startIndex; i <= endIndex; i++)
			{
				if (i == 0 || i == a.Length - 1)
					continue;

				if (a[i - 1] < a[i] && a[i] > a[i + 1])
					return true;
			}

			return false;
		}

		public int Flags(int[] A)
		{
			if (A.Length <= 2)
				return 0;

			//list all peaks indexes
			List<int> peaks = new List<int>();
			for (int i = 1; i < A.Length - 1; i++)
				if (A[i - 1] < A[i] && A[i] > A[i + 1])
					peaks.Add(i);

			if (peaks.Count <= 1)
				return peaks.Count; // 0 or 1 can always be set

			var maxResult = 0;
			for(int i = 2; i <= peaks.Count; i++) //starts at 2 flags
			{
				var remainingFlags = i - 1; // sets first flag at peaks[0]
				var lastUsedPeak = peaks[0];
				var currentPeakIndex = 1;
				while(remainingFlags > 0 && currentPeakIndex < peaks.Count)
				{
					if(peaks[currentPeakIndex] - lastUsedPeak >= i)
					{
						remainingFlags--;
						lastUsedPeak = peaks[currentPeakIndex];
					}

					currentPeakIndex++;
				}

				if (remainingFlags == 0)
					maxResult = i;
			}

			return maxResult;
		}

		public int Flags2(int[] A)
		{
			if (A.Length <= 2)
				return 0;

			//list all peaks indexes
			List<int> peaks = new List<int>();
			for (int i = 1; i < A.Length - 1; i++)
				if (A[i - 1] < A[i] && A[i] > A[i + 1])
					peaks.Add(i);

			if (peaks.Count <= 1)
				return peaks.Count; // 0 or 1 can always be set
			
			int[] nextPeakArray = new int[A.Length];
			int peaksIndex = 0;

			//fill next peaks array
			for(int i = 0; i < nextPeakArray.Length; i++)
			{
				if (peaksIndex >= peaks.Count)
					nextPeakArray[i] = -1;
				else if (i <= peaks[peaksIndex])
					nextPeakArray[i] = peaks[peaksIndex];
				else
				{
					peaksIndex++;
					nextPeakArray[i] = peaksIndex >= peaks.Count ? -1 : peaks[peaksIndex];
				}
			}

			int maxResult = 1;
			for(int i = 2; i <= peaks.Count; i++)
			{
				int position = 0;
				int remainingFlags = i; //sets flag at first

				while (position < A.Length && remainingFlags > 0)
				{
					int nextPeak = nextPeakArray[position]; //sets flag in next peak following position
					if(nextPeak == -1 )
						break;
					else
					{
						remainingFlags--;
						position = nextPeak + i; //next position can only start at +1 due to flags distance constraint
					}
				}

				if (remainingFlags == 0)
					maxResult = i;
				else
					break;
			}

			return maxResult;
		}

		public int[] prepateForFactorization(int n)
		{
			//for each number n, a[n] contains the smallest prime divisor.
			int[] a = new int[n + 1];

			int i = 2;
			while(i * i <= n)
			{
				if(a[i] == 0)
				{
					int k = i * i;
					while (k <= n)
					{ 
						if(a[k] == 0)
							a[k] = i;
						k += i;
					}
				}

				i++;
			}


			return a;
		}
		public List<int>Factorization(int n)
		{
			List<int> result = new List<int>();
			var a = prepateForFactorization(n);

			int i = n;
			while(a[i] != 0)
			{
				result.Add(a[i]);
				i = i / a[i];
			}
			result.Add(i);

			return result;
		}

		public bool[] SieveOfEratosthenes(int n)
		{
			bool[] sieve = new bool[n+1];
			for (int i = 0; i < sieve.Length; i++)
				sieve[i] = true;

			sieve[0] = false;
			sieve[1] = false;

			int currentNumber = 2;
			while(currentNumber * currentNumber <= n)
			{
				if (sieve[currentNumber])
				{
					int k = currentNumber * currentNumber;
					while (k <= n)
					{
						sieve[k] = false;
						k += currentNumber;
					}
				}
				currentNumber++;
			}

			return sieve;
		}

		int[] CountNonDivisableFromWeb(int[] A) // time complexity O(N*log(N)), space complexity O(N)
		{
			int nIntCount = A.Length; // = N in problem statement
			int nMaxInt = nIntCount + nIntCount; // max possible input int; problem statement specifies this
			int[] anIntCounts = new int[nMaxInt + 1]; // array of counters for all possible input ints
													  // (plus a never-used counter for 0)
			int[] anDivisorCounts = new int[nMaxInt + 1]; // array of counters for counts of divisors
			int[] anNotDivCounts = new int[nIntCount]; // to be returned, length and order same as input array A
			foreach (int a in A) // transform input array A into counts of its ints
				anIntCounts[a]++;
			for (int i = 0; i <= nMaxInt; i++) // cycling thru counter array instead of input array A speeds things up!
				if (anIntCounts[i] > 0) // skip this iteration if input array A didn't have this int
					for (int im = i; im <= nMaxInt; im += i) // mark multiples (they're divisable by this int, of course)
															 // since we're in the Sieve of Eratosthenes lesson
						anDivisorCounts[im] += anIntCounts[i]; // mark by storing input int counts
															   // (some counts will never be read)
			for (int i = 0; i < nIntCount; i++)
				anNotDivCounts[i] = nIntCount - anDivisorCounts[A[i]]; // compute counts of non-divisors
																	   // (in original order of input array A)
			return anNotDivCounts;
		}

		int[] CountNonDivisable(int[] A)
		{
			//from question we know that every element in N is > 0 <= N*2
			int[] result = new int[A.Length];
			int[] countArray = new int[A.Length * 2 + 1];
			foreach (var i in A)
				countArray[i]++;

			int[] divisorsCount = new int[A.Length * 2 + 1];

			//foreach present number in A, count in divisorCount how many are there
			for (int i = 0; i < countArray.Length; i++)
			{
				if (countArray[i] == 0) //no need to account for numbers that are not present
					continue;

				for(int j = i; j < countArray.Length; j += i)
					divisorsCount[j] += countArray[i];
			}

			for (int i = 0; i < A.Length; i++)
				result[i] = A.Length - divisorsCount[A[i]];

			return result;
		}

		public int[] solution(int N, int[] P, int[] Q)
		{
			var factorizationArray = prepateForFactorization(N);
			int[] sumArray = new int[factorizationArray.Length];
			
			for(int i = 2; i < sumArray.Length; i++)
			{
				bool subPrime = isSubPrime(i, factorizationArray);
				sumArray[i] = (subPrime ? 1 : 0) + sumArray[i - 1];
			}

			int[] result = new int[P.Length];

			for(int i = 0; i < result.Length; i++)
				result[i] = sumArray[Q[i]] - sumArray[P[i] - 1];

			return result;
		}
		private bool isSubPrime(int i, int[] factorizationArray)
		{
			//Has more than 2 prime factors
			int factorCount = 0;
			while(factorizationArray[i] != 0)
			{
				factorCount++;
				i = i / factorizationArray[i];

				if (factorCount > 1) //in reality this means that theres more than two, since we would add one more after the while
					return false;
			}
			factorCount++;

			return factorCount == 2;
		}

		public int solution(int N, int M)
		{
			//adjusting m to be between 1 and n/2
			if (M > N)
				M = M % N;

			if (M == 0) //full circle always
				return 1;

			int position = 0;
			int nextPosition = (position + M) % N;
			int chocolatesEaten = 0; //chocolate zero
			do
			{
				chocolatesEaten += (N / M) + 1;
				var rest = N % M;
				position = ((position + N - rest) % N);
				nextPosition = (position + M) % N;

				if (nextPosition < position)
					chocolatesEaten--;

			} while (position != 0 && nextPosition != 0);

			if (position == 0)
				chocolatesEaten--;

			return chocolatesEaten;
		}

		public int solution2(int N, int M)
		{
			//adjusting m to be between 1 and n/2
			if (M > N)
				M = M % N;

			if (M == 0) //full circle always
				return 1;

			int result = 0;
			int gdc = calculateGDC(N, M);
			var rest = N % M;
			var stepSize = gdc;

			int column = 0;
			do
			{
				if ((column+1) <= rest)
					result += (N / M) + 1;
				else
					result += (N / M);

				column = (column + stepSize) % M;
			} while (column != 0);

			return result;
		}

		public int calculateGDC(int a, int b)
		{
			if (a % b == 0)
				return b;
			else
				return calculateGDC(b, a % b);
		}

		public int HaveCommonDivisors(int[] A, int[] B)
		{

			int result = 0;
			for (int i = 0; i < A.Length; i++)
			{
				if (haveCommonDivisors(A[i], B[i]))
					result++;
			}

				return result;
		}

		private bool haveCommonDivisors(int v1, int v2)
		{
			if (v1 == v2)
				return true;

			int gdc = calculateGDC(v1, v2);
			
			//reducing v1 using gdc of v1 and v2
			while(true) 
			{
				var gdcV1 = calculateGDC(v1, gdc);
				if (gdcV1 == 1)
					break;

				v1 = v1 / gdcV1;
			}

			//reducing v1 using gdc of v1 and v2
			while (true)
			{
				var gdcV2 = calculateGDC(v2, gdc);
				if (gdcV2 == 1)
					break;

				v2 = v2 / gdcV2;
			}

			return v1 == 1 && v2 == 1;
		}
	}
}