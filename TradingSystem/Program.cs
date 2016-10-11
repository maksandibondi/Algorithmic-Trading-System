using System;

using Algorithm;

using Sender;

namespace TradingSystem {
	

	public class Publisher {

		// delegate declaration
		public delegate void ReceiveNewTransactionDel(string trs);
		// event decaration with event handler name
		public event ReceiveNewTransactionDel newTransaction; 

		public IfileAlgorithm algorithm = null;

		public string transaction = null;

		// constructor
		public Publisher(IfileAlgorithm algorithm) {

			this.algorithm = algorithm;

		}

		// watcher will be executed on a separate thread and when unnecessary, the thread will be stopped
		public void transactionWatcher() {

			this.transaction = null;

			this.transaction = this.algorithm.run();

			// handler invokation (with transaction argument)
			if (this.transaction != null) {

				newTransaction (transaction); 

				this.transactionWatcher ();

			}  

			else {

				this.transactionWatcher ();

			}

		}

	}

	public class Subscriber {

		public Isender sender = null;

		public Subscriber(Isender sender) {

			this.sender = sender;
		}

		// newTransaction event handler
		public void OnreceiveNewTransactionNotif (string transaction) {
			// Active MQ sender DLL
			this.sender.send (transaction);

		}

	}

}