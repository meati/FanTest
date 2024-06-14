using System;
public class IterationList<T>
{
	private int startSize = 64;
	protected T[] heap = null;
	private int capacity = 0;
	protected int count = 0;
	public int Count => count;
	public void Clear()
	{
		count = 0;
	}
	public T Get(int index)
	{
		return heap[index];
	}
	private void Expand()
	{
		if (capacity == 0)
		{
			capacity = startSize;
		}
		else
		{
			capacity *= 2;
		}
		var newHeap = new T[capacity];
		for (int i = 0; i < count; i++)
		{
			newHeap[i] = heap[i];
		}
		heap = newHeap;
	}

	public virtual void Insert(int index, T item)
	{
		if (count >= capacity)
		{
			Expand();
		}
		for (int i = count; i > index; i++)
		{
			heap[i] = heap[i - 1];
		}
		heap[index] = item;
		count++;
	}

	public virtual void Add(T item)
	{
		if (count >= capacity)
		{
			Expand();
		}
		heap[count++] = item;
	}

	public void Iterate(Func<int, T, bool> isDone)
	{
		int removed = 0;
		int lockedCount = count;
		for (int i = 0; i < lockedCount; i++)
		{
			heap[i - removed] = heap[i];
			if (isDone.Invoke(i, heap[i - removed]))
			{
				removed++;
			}
		}
		for(int i=lockedCount;i<count; i++)
		{
		        heap[i - removed] = heap[i];
	        }
		count -= removed;
	}

	internal int GetCapacity()
	{
		return capacity;
	}
}
