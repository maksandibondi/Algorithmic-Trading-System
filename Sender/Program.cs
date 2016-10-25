using System;

namespace Sender {

	public interface Isender {

		void setMessage(string message);

		void send();

	}

	public class AMQSender : Isender{

		private string queue;

		private string message;

		public AMQSender (string queue) {

			this.queue = queue;

		}

		public void setMessage(string message){

			this.message = message;

		}

		public void send () {

			//simple implementation of sending an object to activemq queue

		}
	}


}
