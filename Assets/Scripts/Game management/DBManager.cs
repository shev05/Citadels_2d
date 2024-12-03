using System.Collections.Generic;
using System.Data;
using Mono.Data.SqliteClient;
using SQLiter;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    public static DBManager Instance = null;
		public bool DebugMode = false;

		// Location of database - this will be set during Awake as to stop Unity 5.4 error regarding initialization before scene is set
		// file should show up in the Unity inspector after a few seconds of running it the first time
		private static string _sqlDBLocation = "";
		
		private const string SQL_DB_NAME = "CitadelsDB";

		private const string SQL_TABLE_NAME = "PlayersScore";

		private const string COL_GAMEID = "id";
		private const string COL_NICKNAME = "nick";  // using name as example of primary, unique, key
		private const string COL_POINTS = "points";
		
		private IDbConnection _connection = null;
		private IDbCommand _command = null;
		private IDataReader _reader = null;
		private string _sqlString;

		private bool _createNewTable = false;
		
		void Awake()
		{
			if (DebugMode)
				Debug.Log("--- Awake ---");

			// here is where we set the file location
			// ------------ IMPORTANT ---------
			// - during builds, this is located in the project root - same level as Assets/Library/obj/ProjectSettings
			// - during runtime (Windows at least), this is located in the SAME directory as the executable
			// you can play around with the path if you like, but build-vs-run locations need to be taken into account
			_sqlDBLocation = "URI=file:" + SQL_DB_NAME + ".db";

			Debug.Log(_sqlDBLocation);
			Instance = this;
			SQLiteInit();
		}

		void Start()
		{
			if (DebugMode)
				Debug.Log("--- Start ---");
		}

		/// <summary>
		/// Clean up SQLite Connections, anything else
		/// </summary>
		void OnDestroy()
		{
			SQLiteClose();
		}

		/// <summary>
		/// Example using the Loom to run an asynchronous method on another thread so SQLite lookups
		/// do not block the main Unity thread
		/// </summary>
		public void RunAsyncInit()
		{
			LoomManager.Loom.QueueOnMainThread(() =>
			{
				SQLiteInit();
			});
		}

		/// <summary>
		/// Basic initialization of SQLite
		/// </summary>
		private void SQLiteInit()
		{
			Debug.Log("SQLiter - Opening SQLite Connection");
			_connection = new SqliteConnection(_sqlDBLocation);
			_command = _connection.CreateCommand();
			_connection.Open();

			// WAL = write ahead logging, very huge speed increase
			_command.CommandText = "PRAGMA journal_mode = WAL;";
			_command.ExecuteNonQuery();

			// journal mode = look it up on google, I don't remember
			_command.CommandText = "PRAGMA journal_mode";
			_reader = _command.ExecuteReader();
			if (DebugMode && _reader.Read())
				Debug.Log("SQLiter - WAL value is: " + _reader.GetString(0));
			_reader.Close();

			// more speed increases
			_command.CommandText = "PRAGMA synchronous = OFF";
			_command.ExecuteNonQuery();

			// and some more
			_command.CommandText = "PRAGMA synchronous";
			_reader = _command.ExecuteReader();
			if (DebugMode && _reader.Read())
				Debug.Log("SQLiter - synchronous value is: " + _reader.GetInt32(0));
			_reader.Close();

			// here we check if the table you want to use exists or not.  If it doesn't exist we create it.
			_command.CommandText = "SELECT name FROM sqlite_master WHERE name='" + SQL_TABLE_NAME + "'";
			_reader = _command.ExecuteReader();
			if (!_reader.Read())
			{
				Debug.Log("SQLiter - Could not find SQLite table " + SQL_TABLE_NAME);
				_createNewTable = true;
			}
			_reader.Close();

			// create new table if it wasn't found
			if (_createNewTable)
			{
				Debug.Log("SQLiter - Creating new SQLite table " + SQL_TABLE_NAME);

				// insurance policy, drop table
				_command.CommandText = "DROP TABLE IF EXISTS " + SQL_TABLE_NAME;
				_command.ExecuteNonQuery();

				// create new - SQLite recommendation is to drop table, not clear it
				_sqlString = "CREATE TABLE IF NOT EXISTS " + SQL_TABLE_NAME + " (" +
				             COL_GAMEID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
				             COL_NICKNAME + " TEXT NOT NULL, " +
				             COL_POINTS + " INTEGER NOT NULL)";
				_command.CommandText = _sqlString;
				_command.ExecuteNonQuery();
			}
			else
			{
				if (DebugMode)
					Debug.Log("SQLiter - SQLite table " + SQL_TABLE_NAME + " was found");
			}

			// close connection
			_connection.Close();
		}

		#region Insert
		/// <summary>
		/// Inserts a player into the database
		/// http://www.sqlite.org/lang_insert.html
		/// name must be unique, it's our primary key
		/// </summary>
		/// <param name="id"></param>
		/// <param name="nick"></param>
		/// <param name="points"></param>
		public void InsertPlayerStat(string nick, int points)
		{
			nick = nick.ToLower();

			// note - this will replace any item that already exists, overwriting them.  
			// normal INSERT without the REPLACE will throw an error if an item already exists
			_sqlString = "INSERT INTO " + SQL_TABLE_NAME
			                            + " ("
			                            + COL_NICKNAME + ","
			                            + COL_POINTS
			                            + ") VALUES ('"
			                            + nick + "',"
			                            + points
			                            + ");";

			if (DebugMode)
				Debug.Log(_sqlString);
			ExecuteNonQuery(_sqlString);
		}

		#endregion
		
		/// <summary>
		/// Quick method to show how you can query everything.  Expland on the query parameters to limit what you're looking for, etc.
		/// </summary>
		public (List<string>,List<int>) GetAllStat()
		{
			List<string> nicks = new List<string>();
			List<int> scores = new List<int>();

			_connection.Open();

			// if you have a bunch of stuff, this is going to be inefficient and a pain.  it's just for testing/show
			_command.CommandText = "SELECT * FROM " + SQL_TABLE_NAME;
			_reader = _command.ExecuteReader();
			while (_reader.Read())
			{
				nicks.Add(_reader.GetString(1));
				scores.Add(_reader.GetInt32(2));
			}
			_reader.Close();
			_connection.Close();
			return (nicks, scores);
		}

		/// <summary>
		/// Basic execute command - open, create command, execute, close
		/// </summary>
		/// <param name="commandText"></param>
		public void ExecuteNonQuery(string commandText)
		{
			_connection.Open();
			_command.CommandText = commandText;
			_command.ExecuteNonQuery();
			_connection.Close();
		}

		/// <summary>
		/// Clean up everything for SQLite
		/// </summary>
		private void SQLiteClose()
		{
			if (_reader != null && !_reader.IsClosed)
				_reader.Close();
			_reader = null;

			if (_command != null)
				_command.Dispose();
			_command = null;

			if (_connection != null && _connection.State != ConnectionState.Closed)
				_connection.Close();
			_connection = null;
		}
}
