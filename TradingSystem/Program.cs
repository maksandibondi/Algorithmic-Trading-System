using System;

using System.Collections;

using Algorithm;

using Sender;


namespace TradingSystem {

	public interface IObserver {

		void OnreceiveNewTransactionNotif(string transaction);

	}

	public interface ISubject {

		void addObserver(IObserver observer);

		void removeObserver(IObserver observer);

		void notifyObservers();

	}
	

	public class Publisher : ISubject {


		// we could use this .NET events functionality instead of Observer pattern
		// delegate declaration
		//public delegate void ReceiveNewTransactionDel(string trs);
		// event decaration with event handler name
		//public event ReceiveNewTransactionDel newTransaction; 

		private ArrayList observers;

		public IfileAlgorithm algorithm = null;

		public string transaction = null;

		// constructor
		public Publisher(IfileAlgorithm algorithm) {

			observers = new ArrayList();

			this.algorithm = algorithm;

		}


		// ISubject interface implementation
		public void addObserver(IObserver observer){

			observers.Add (observer);

		}

		public void removeObserver(IObserver observer){

			observers.Remove (observer);

		}

		public void notifyObservers() {

			foreach (IObserver i in observers) {

				IObserver tempobserver = i;

				tempobserver.OnreceiveNewTransactionNotif(transaction);

			}

		}


		// watcher will be executed on a separate thread and when unnecessary, the thread will be stopped
		public void transactionWatcher() {

			this.transaction = null;

			this.transaction = this.algorithm.run();

			// handler invokation (with transaction argument)
			if (this.transaction != null) {

				notifyObservers ();
				//newTransaction (transaction); // we could use this .NET events functionality instead of Observer pattern

				this.transactionWatcher ();

			}  

			else {

				this.transactionWatcher ();

			}

		}

	}

	public class Subscriber : IObserver {

		public Isender sender = null;

		public Subscriber(Isender sender) {

			this.sender = sender;
		}

		// newTransaction event handler
		public void OnreceiveNewTransactionNotif (string transaction) {
			// Active MQ sender DLL
			this.sender.setMessage(transaction);

			this.sender.send ();

		}

	}

}