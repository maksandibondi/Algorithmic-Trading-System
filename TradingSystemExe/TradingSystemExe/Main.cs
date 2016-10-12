using System;

using TradingSystem;

using Algorithm;

using Sender;

using System.IO;

using finamLib;

using System.Text.RegularExpressions;


class MainClass {

	static void Main (string[] args) {


		// thread 1
		finamCrawler crawler = new finamCrawler("/home/maksandi/Project_Sprints/Trading system/System/TradingSystem/Databases/marketdata");

		crawler.Crawl ("ETF", "Canada Index", "01/11/2011", "09/10/2015", "тики", "csv", "file");

		// thread 2

		Isender sender = new AMQSender ("localhost");

		// determining the algo. path should be a path to crawlers download folder
		IfileAlgorithm algo = new FileAlgorithm ();

		Publisher pub = new Publisher (algo);

		Subscriber sub1 = new Subscriber (sender);

		pub.newTransaction += sub1.OnreceiveNewTransactionNotif;


		// thread 3

		pub.transactionWatcher ();

	}

}
