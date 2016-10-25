using System;

using System.Data;

using MySql.Data.MySqlClient;

using System.Windows.Forms;

using System.Collections.Generic;

using System.Diagnostics;

using System.IO;

namespace DBconnector {

	public interface IDBConnect {

		void OpenConnection();

		void CloseConnection();

		bool ConnectionIsOpened();

	}

	public interface IDBSQLClassic {

		void Insert (string tablename, List<string>columnNames, params object[] values);

		void InsertMultiple (string tablename, List<string>columnNames, params List<object>[] listsOfValues);

		void Update (string tablename, string criteria, params object[] valuesToSet);

		void Delete (string tablename, string criteria);

		List<object>[] Select(string tablename, string criteria, params string[] columnNames);

		List<object>[] ExecuteMySQLReader (string query, MySqlConnection connection, List<object>[] listToStoreData, params string[] columnNames);

	}

	public interface IDBFileWorker {

		void Backup();

		void Restore();

		/// void FileLoader (string fileadress, string delimiter, string tablename, string[] columnnames);

	}

	public class DBConnect : IDBConnect, IDBSQLClassic, IDBFileWorker {

		private MySqlConnection connection;
		private string server;
		private string database;
		private string uid;
		private string password;

		//Constructor
		public DBConnect(string servername, string databasename, string uidname, string passwordname) {
			this.server = servername;
			this.database = databasename;
			this.uid = uidname;
			this.password = passwordname;
			string connectionString;
			connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
			connection = new MySqlConnection(connectionString);
		}

		//open connection to database
		public void OpenConnection() {

			if (ConnectionIsOpened () == false) {

				try {
					connection.Open ();
				} catch (MySqlException ex) {
					//When handling errors, you can your application's response based 
					//on the error number.
					//The two most common error numbers when connecting are as follows:
					//0: Cannot connect to server.
					//1045: Invalid user name and/or password.
					switch (ex.Number) {
						case 0:
						MessageBox.Show ("Cannot connect to server.  Contact administrator");
						break;

						case 1045:
						MessageBox.Show ("Invalid username/password, please try again");
						break;
					}

				}
			} 

		}

		//Close connection
		public void CloseConnection() {
			try
			{
				connection.Close();
			}
			catch (MySqlException ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public bool ConnectionIsOpened(){

			ConnectionState state = this.connection.State;

			if (state == ConnectionState.Open) {
				return true;
			} else {
				return false;
			}


		}




		public bool IsNumericOrNULL(object obj) {

			if (obj is decimal || obj is double || (obj is string && obj == "NULL")) {

				return true;

			} else {

				return false;

			}

		}

		public void ExecuteMySQLCommand (string query, MySqlConnection connection) {

			if (this.ConnectionIsOpened() == true) {
				//create command and assign the query and connection from the constructor
				MySqlCommand cmd = new MySqlCommand(query, connection);

				//Execute command
				cmd.ExecuteNonQuery();

				//close connection
				// this.CloseConnection();
			}

		}

		public List<object>[] ExecuteMySQLReader (string query, MySqlConnection connection, List<object>[] listToStoreData, params string [] columnNames) {

			if (this.ConnectionIsOpened() == true) {
				//Create Command
				MySqlCommand cmd = new MySqlCommand(query, connection);
				//Create a data reader and Execute the command
				MySqlDataReader dataReader = cmd.ExecuteReader();

				while (dataReader.Read()) {

					for (int i = 0; i<columnNames.Length; i++) {
						listToStoreData [i].Add (dataReader [columnNames[i]] + "");
					}

				}

				//close Data Reader
				dataReader.Close();

				//close Connection
				// this.CloseConnection();

				//return list to be displayed
				return listToStoreData;

			}

			else {

				return listToStoreData;

			}

		}

		public void Insert(string tablename, List<string>columnNames, params object[] values) {

			string query = "INSERT INTO " + tablename + " (";

			foreach (string i in columnNames) {

				query = query + i + ",";

			}

			query = query.Remove (query.Length - 1); //removal of ","

			query = query + ")" + " VALUES (";


			foreach (object i in values) {

				if (IsNumericOrNULL(i)) {

					query = query + i.ToString () + ",";

				} else {

					query = query + "'" + i.ToString () + "'" + ",";

				}

			}

			query = query.Remove (query.Length - 1); //removal of "," 

			query = query + ");";

			ExecuteMySQLCommand (query, connection);

		}

		public void InsertMultiple(string tablename, List<string>columnNames, params List<object>[] listsOfValues) {

			string query = "INSERT INTO " + tablename + " (";

			foreach (string i in columnNames) {

				query = query + i + ",";

			}

			query = query.Remove (query.Length - 1); //removal of ","

			query = query + ")" + " VALUES ";

			foreach (List<object> i in listsOfValues) {

				query = query + "(";

				foreach (object k in i) {

					if (IsNumericOrNULL(k)) {

						query = query + k.ToString () + ",";

					} else {

						query = query + "'" + k.ToString () + "'" + ",";

					}

				}

				query = query + "),";

			}

			query = query.Remove (query.Length - 1); //removal of "," 

			query = query + ";";

			ExecuteMySQLCommand (query, connection);

		}




		public void Update(string tablename, string criteria, params object[] valuesToSet) {

			string query = "UPDATE " + tablename + " SET ";

			foreach (object i in valuesToSet) {

				query = query + i.ToString () + ",";

			}

			query = query.Remove (query.Length - 1); // removal of "," 

			query = query + " WHERE " + criteria + ";";

			ExecuteMySQLCommand (query, connection);

		}

		public void Delete(string tablename, string criteria) { //"DELETE FROM tableinfo WHERE name='John Smith'"; 

			string query = "DELETE FROM " + tablename + " WHERE " + criteria;

			query = query + ";";

			ExecuteMySQLCommand (query, connection);

		}

		public List<object>[] Select(string tablename, string criteria, params string[] columnNames) {	 // "SELECT * FROM tableinfo"{

			string query = "SELECT ";

			foreach (string i in columnNames) {

				query = query + i + ",";

			}

			query = query.Remove (query.Length - 1); 
			query = query + " FROM " + tablename + " WHERE " + criteria;


			int k = columnNames.Length;
			//Create a list of lists (matrix) to store the result
			List<object>[] listToStoreData = new List<object>[k]; // horizontal list of lists
			for (int i = 0; i<k; i++) {
				listToStoreData [i] = new List<object>(); // vertical lists for data
			}

			listToStoreData = ExecuteMySQLReader (query, connection, listToStoreData, columnNames);

			return listToStoreData;

		}



		/* public int Count()
		{
			string query = "SELECT Count(*) FROM tableinfo";
			int Count = -1;

			//Open Connection
			if (this.OpenConnection () == true) {
				//Create Mysql Command
				MySqlCommand cmd = new MySqlCommand (query, connection);

				//ExecuteScalar will return one value
				Count = int.Parse (cmd.ExecuteScalar () + "");

				//close Connection
				this.CloseConnection ();

				return Count;
			} else {
				return Count;
			}
		} */

		//Backup
		public void Backup()
		{
			try
			{
				DateTime Time = DateTime.Now;
				int year = Time.Year;
				int month = Time.Month;
				int day = Time.Day;
				int hour = Time.Hour;
				int minute = Time.Minute;
				int second = Time.Second;
				int millisecond = Time.Millisecond;

				//Save file to C:\ with the current date as a filename
				string path;
				path = "C:\\MySqlBackup" + year + "-" + month + "-" + day + 
					"-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
				StreamWriter file = new StreamWriter(path);


				ProcessStartInfo psi = new ProcessStartInfo();
				psi.FileName = "mysqldump";
				psi.RedirectStandardInput = false;
				psi.RedirectStandardOutput = true;
				psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", 
				                              uid, password, server, database);
				psi.UseShellExecute = false;

				Process process = Process.Start(psi);

				string output;
				output = process.StandardOutput.ReadToEnd();
				file.WriteLine(output);
				process.WaitForExit();
				file.Close();
				process.Close();
			}
			catch (IOException ex)
			{
				MessageBox.Show("Error , unable to backup!");
			}
		}

		//Restore
		public void Restore()
		{
			try
			{
				//Read file from C:\
				string path;
				path = "C:\\MySqlBackup.sql";
				StreamReader file = new StreamReader(path);
				string input = file.ReadToEnd();
				file.Close();

				ProcessStartInfo psi = new ProcessStartInfo();
				psi.FileName = "mysql";
				psi.RedirectStandardInput = true;
				psi.RedirectStandardOutput = false;
				psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", 
				                              uid, password, server, database);
				psi.UseShellExecute = false;


				Process process = Process.Start(psi);
				process.StandardInput.WriteLine(input);
				process.StandardInput.Close();
				process.WaitForExit();
				process.Close();
			}
			catch (IOException ex)
			{
				MessageBox.Show("Error , unable to Restore!");
			}
		}

	}


}

