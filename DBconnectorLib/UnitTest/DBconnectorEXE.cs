using System;

using System.Collections.Generic;

using DBconnector;

namespace DBexe {

	class MainClass {

		public static void Main (string[] args) {


			DBConnect db = new DBConnect ("localhost", "MarketData", "maksandi", "credence2012");

			db.OpenConnection ();
			Console.WriteLine (db.ConnectionIsOpened());

			int id = 9;
			char period = 'D';
			double volume;
			TimeSpan time = DateTime.Now.TimeOfDay;
			List<string> dbTable = new List<string>{ "ticker", "period", "date","time","open","high","low","close","volume" };


			// Insertion and update, delete
			db.Insert ("Prices", dbTable, "shit", period, "20161011", time, 200, 100, 100, 100, 100);
			db.Update ("Prices", "id > 0 AND id < 15 AND (volume = 100 OR volume = 200)", "period = 'O'", "volume = 300"); // criteria is id = 10 and after - values to set
			// db.Delete ("Prices", "id > 0 AND id < 22");


			// Lists selection and displaying
			List<object>[] mylist = new List<object>[2];
			for (int i = 0; i<2; i++) {

				mylist [i] = new List<object> ();

			}

			mylist =  db.Select ("Prices", "id > 0", "ticker", "time");

			for (int i = 0; i<mylist.Length; i++) {
				mylist [i].ForEach (Console.WriteLine);
			}

			List<TimeSpan> mytimelist = new List<TimeSpan>();

			int j = 0;
			foreach (object k in mylist[1]) {
				mytimelist [j] = TimeSpan.Parse (k.ToString ());
				Console.WriteLine (mytimelist [j]);
				j++;
			}

		}
	}
}
