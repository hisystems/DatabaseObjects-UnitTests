using System;
using System.Linq;
using NUnit.Framework;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using System.Diagnostics;

namespace UnitTests
{
    [TestFixture]
    public class SQLiteConnectionTests
    {
        private DatabaseObjects.Database database;

        [SetUp]
        public void SetUp()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "TestDatabase.db3");

            if (File.Exists(databasePath))
                File.Delete(databasePath);

            SqliteConnection.CreateFile(databasePath);

            this.database = new DatabaseObjects.Database(new SqliteConnection("Data Source=" + databasePath), DatabaseObjects.Database.ConnectionType.SQLite);

            var createTable = new DatabaseObjects.SQL.SQLCreateTable();
            createTable.Name = "T1";
            createTable.Fields.Add("Name", DatabaseObjects.SQL.DataType.UnicodeVariableCharacter, 100);

            using (var connection = new DatabaseObjects.ConnectionScope(this.database))
                connection.ExecuteNonQuery(createTable);
        }
        
        [Test]
        public void RepeatedlyInsertDelete()
        {
            var insert = new DatabaseObjects.SQL.SQLInsert();
            insert.TableName = "T1";
            insert.Fields.Add("Name", "ABC123");

            var delete = new DatabaseObjects.SQL.SQLDelete("T1");

            for (int i = 0; i < 10000; i++)
            {
                using (var connectionScope = new DatabaseObjects.ConnectionScope(database))
                {
                    connectionScope.ExecuteNonQuery(insert);
                    connectionScope.ExecuteNonQuery(delete);
                    Debug.WriteLine("Executed {0} times", (i + 1));
                }
            }
        }
    }
}
