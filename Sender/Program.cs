using System;

namespace Sender {

	public interface Isender {

		void send(string obj);

	}

	public class AMQSender : Isender{

		private string queue;

		public AMQSender (string queue) {

			this.queue = queue;

		}

		public void send (string obj) {

			//simple implementation of sending an object to activemq queue

		}
	}


}
