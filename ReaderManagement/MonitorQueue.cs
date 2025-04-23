using System;
using System.Collections;
using System.Threading;


namespace ReaderManagement
{
	/// <summary>
	/// Summary description for System.Threading.MonitorQueue.
	/// </summary>
	public class MonitorQueue
	{
		//Define the queue to safe thread access.
		private Queue inputQueue;

		public MonitorQueue()
		{
			inputQueue = new Queue();
		}
		

		//Add an element to the queue and obtain the System.Threading.Monitor lock for the queue object.
		public void AddElement(object qValue)
		{
			//Lock the queue.
			System.Threading.Monitor.Enter(inputQueue);
			//Add element
			inputQueue.Enqueue(qValue);
			//Unlock the queue.
			System.Threading.Monitor.Exit(inputQueue);
		}

		//Try to add an element to the queue.
		//Add the element to the queue only if the queue object is unlocked.
		public bool AddElementWithoutWait(object qValue)
		{
			//Determine whether the queue is locked 
			if(!System.Threading.Monitor.TryEnter(inputQueue))
				return false;
			inputQueue.Enqueue(qValue);

			System.Threading.Monitor.Exit(inputQueue);
			return true;
		}

		//Try to add an element to the queue. 
		//Add the element to the queue only if during the specified time the queue object will be unlocked.
		public bool WaitToAddElement(object qValue, int waitTime)
		{
			//Wait while the queue is locked.
			if(!System.Threading.Monitor.TryEnter(inputQueue,waitTime))
				return false;
			inputQueue.Enqueue(qValue);
			System.Threading.Monitor.Exit(inputQueue);

			return true;
		}
        
		//Delete all elements that equal the given object and obtain the System.Threading.Monitor lock for the queue object.
		public void DeleteElement(object qValue)
		{
			//Lock the queue.
			System.Threading.Monitor.Enter(inputQueue);
			int counter = inputQueue.Count;
			while(counter > 0)
			{    
				//Check each element.
				object elm = inputQueue.Dequeue();
				if(!elm.Equals(qValue))
				{
					inputQueue.Enqueue(elm);
				}
				--counter;
			}
			//Unlock the queue.
			System.Threading.Monitor.Exit(inputQueue);
		}
        
		//Print all queue elements.
		public void PrintAllElements()
		{
			//Lock the queue.
			System.Threading.Monitor.Enter(inputQueue);            
			IEnumerator elmEnum = inputQueue.GetEnumerator();
			while(elmEnum.MoveNext())
			{
				//Print the next element.
				Console.WriteLine(elmEnum.Current.ToString());
			}
			//Unlock the queue.
			System.Threading.Monitor.Exit(inputQueue);    
		}

		//Print elements that equal the given object and obtain the System.Threading.Monitor lock for the queue object.
		public void PrintElement(object qValue)
		{
			//Lock the queue.
			System.Threading.Monitor.Enter(inputQueue);            
			IEnumerator elmEnum = inputQueue.GetEnumerator();
			while(elmEnum.MoveNext())
			{
				//Print the next element.
				if ( elmEnum.Current.Equals(qValue) )
					Console.WriteLine(elmEnum.Current.ToString());
			}
			//Unlock the queue.
			System.Threading.Monitor.Exit(inputQueue);    
		}

	}
}
