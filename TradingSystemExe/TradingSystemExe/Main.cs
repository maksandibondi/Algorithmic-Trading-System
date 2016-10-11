using System;

using TradingSystem;

using Algorithm;

using Sender;

using System.IO;

using finamLib;

using System.Text.RegularExpressions;


class MainClass {

	static void Main (string[] args) {

		finamCrawler crawler = new finamCrawler();

		Isender sender = new AMQSender ("localhost");

		// determining the algo. path should be a path to crawlers download folder
		IfileAlgorithm algo = new FileAlgorithm ("file:///home/maksandi/Project_Sprints/finamCrawler/finamCrawler/finamCrawler/finamCrawler/bin/Debug/Downloads");

		Publisher pub = new Publisher (algo);

		Subscriber sub1 = new Subscriber (sender);

		pub.newTransaction += sub1.OnreceiveNewTransactionNotif;


		// here the multithreading system should refresh the datafile and watch it for transactions on different threads

		crawler.Crawl ("ETF", "Canada Index", "01/11/2011", "09/10/2015", "тики", "csv");

		pub.transactionWatcher ();

	}

}
