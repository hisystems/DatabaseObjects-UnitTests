Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.UnitTestExtensions

<DatabaseTestClass(ConnectionStringNames:={"SQLServerTestDatabase", "MySQLTestDatabase", "SQLiteTestDatabase", "MicrosoftAccessTestDatabase", "SQLServerCETestDatabase"})>
Public Class SQLDataTypeSerialisationTests

	Public Property TestContext As TestContext

	<DatabaseTestInitialize()>
	Public Sub DatabaseTestInitialize(database As Database)

		AddHandler database.Connection.StatementExecuted, _
		  Sub(statement As ISQLStatement)
			  TestContext.WriteLine(statement.SQL)
		  End Sub

		database.DropTableIfExists("TableWithDates")

		Dim createTable As New SQLCreateTable
		createTable.Name = "TableWithDates"
		createTable.Fields.Add("DateTimeField", DataType.DateTime)

		Using connection = New ConnectionScope(database)
			connection.Execute(createTable)
		End Using

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("DataTypeSerialisation")>
	Public Sub DateAndTime()

		Dim dateAndTime As New DateTime(2000, 1, 2, 3, 4, 5, 6)

		Dim insert As New SQLInsert
		insert.TableName = "Table"
		insert.ConnectionType = Database.ConnectionType.SQLServer
		insert.Fields.Add("DateTimeField", dateAndTime)

		Assert.AreEqual(Of String)("INSERT INTO [Table] ([DateTimeField]) VALUES ('2000-01-02 03:04:05.006')", insert.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("DataTypeSerialisation")>
	Public Sub DateAndTimeWithOnlyMilliseconds()

		Dim dateAndTime As New DateTime(2000, 1, 2, Hour:=0, Minute:=0, Second:=0, millisecond:=6)

		Dim insert As New SQLInsert
		insert.TableName = "Table"
		insert.ConnectionType = Database.ConnectionType.SQLServer
		insert.Fields.Add("DateTimeField", dateAndTime)

		Assert.AreEqual(Of String)("INSERT INTO [Table] ([DateTimeField]) VALUES ('2000-01-02 00:00:00.006')", insert.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("DataTypeSerialisation")>
	Public Sub DateTimeWithFractionalSeconds(database As Database)

		Dim dateAndTime As New DateTime(2000, 1, 1)

		Select Case database.Connection.Type
			Case Global.DatabaseObjects.Database.ConnectionType.MicrosoftAccess
				'Does not support milliseconds
			Case Global.DatabaseObjects.Database.ConnectionType.MySQL
				'Does not support milliseconds (unless running 5.6.4+) 
			Case Global.DatabaseObjects.Database.ConnectionType.SQLite
				'Supports nano-seconds
				dateAndTime = dateAndTime.AddTicks(1234567)
			Case Else
				dateAndTime = dateAndTime.AddMilliseconds(123)
		End Select

		Dim insert As New SQLInsert
		insert.TableName = "TableWithDates"
		insert.ConnectionType = database.Connection.Type
		insert.Fields.Add("DateTimeField", dateAndTime)

		Using connection = New ConnectionScope(database)
			connection.Execute(insert)

			Assert.AreEqual(dateAndTime, connection.ExecuteScalar(New SQLSelect("TableWithDates")))
		End Using

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("DataTypeSerialisation")>
	<ExpectedException(GetType(InvalidOperationException))>
	Public Sub MicrosoftAccessDateTimeWithInvalidFractionalSeconds()

		Dim insert As New SQLInsert
		insert.TableName = "TableWithDates"
		insert.ConnectionType = Database.ConnectionType.MicrosoftAccess
		insert.Fields.Add("DateTimeField", New DateTime(1 * TimeSpan.TicksPerMillisecond))

		'Force serialisation
		Dim sql = insert.SQL

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("DataTypeSerialisation")>
	Public Sub DateAndNoTime()

		Dim dateAndTime As New DateTime(2000, 1, 2)

		Dim insert As New SQLInsert
		insert.TableName = "Table"
		insert.ConnectionType = Database.ConnectionType.SQLServer
		insert.Fields.Add("DateTimeField", dateAndTime)

		Assert.AreEqual(Of String)("INSERT INTO [Table] ([DateTimeField]) VALUES ('2000-01-02')", insert.SQL)

	End Sub

	Private Enum TestEnum
		Value1 = 1
		Value2 = 2
	End Enum

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("DataTypeSerialisation")>
	Public Sub SerializeEnumValueAsInteger()

		Dim insert As New SQLInsert
		insert.TableName = "Table"
		insert.ConnectionType = Database.ConnectionType.SQLServer
		insert.Fields.Add("EnumValue", TestEnum.Value1)

		Assert.AreEqual(Of String)("INSERT INTO [Table] ([EnumValue]) VALUES (1)", insert.SQL)

	End Sub

End Class
