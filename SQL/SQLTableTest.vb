﻿Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.UnitTestExtensions

<DatabaseTestClass(ConnectionStringNames:={"SQLServerTestDatabase", "MySQLTestDatabase", "SQLiteTestDatabase", "MicrosoftAccessTestDatabase", "SQLServerCETestDatabase"})>
Public Class SQLTableTest

	<DatabaseTestInitialize()>
	Public Sub DatabaseTestInitialize(database As Database)

		database.DropTableIfExists("Table1")

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	Public Sub CreateTable(database As Database)

		Dim createTable As New SQLCreateTable
		createTable.Name = "Table1"
		With createTable.Fields.Add("PrimaryField", DataType.Integer)
			.AutoIncrements = True
			.KeyType = KeyType.Primary
		End With
		createTable.Fields.Add("Boolean", DataType.Boolean)
		createTable.Fields.Add("Character", DataType.Character, 10)
		createTable.Fields.Add("DateTime", DataType.DateTime)
		createTable.Fields.Add("Decimal", DataType.Decimal)
		createTable.Fields.Add("Float", DataType.Float)
		createTable.Fields.Add("Money", DataType.Money)
		createTable.Fields.Add("Text", DataType.Text)
		createTable.Fields.Add("UnicodeVariableCharacter", DataType.UnicodeVariableCharacter, 10)
		createTable.Fields.Add("Image", DataType.Image)

		Using connection = New ConnectionScope(database)
			connection.Execute(createTable)
		End Using

		Dim insert As New SQLInsert
		insert.TableName = "Table1"
		insert.Fields.Add("Boolean", True)
		insert.Fields.Add("Character", "A"c)
		'Some databases do not support milliseconds.
		insert.Fields.Add("DateTime", New DateTime(2000, 1, 2, 3, 4, 5))
		insert.Fields.Add("Decimal", 1234567890.123)
		insert.Fields.Add("Float", 1234567890.123)
		insert.Fields.Add("Money", 123456789012345)
		insert.Fields.Add("Text", "ABC")
		insert.Fields.Add("UnicodeVariableCharacter", "0123456789")
		insert.Fields.Add("Image", New Byte() {1, 2, 3, 4})

		Using connection = New ConnectionScope(database)
			connection.Execute(insert)
		End Using

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	Public Sub AlterTableAddField(database As Database)

		Dim createTable As New SQLCreateTable
		createTable.Name = "Table1"
		createTable.Fields.Add("Field1", DataType.Integer)

		Using connection = New ConnectionScope(database)
			connection.Execute(createTable)
		End Using

		Dim alterTable As New SQLAlterTable
		alterTable.Name = "Table1"
		alterTable.Fields.Add("Field2", DataType.Integer)

		Using connection = New ConnectionScope(database)
			connection.Execute(alterTable)
		End Using

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	Public Sub AlterTableDropField(database As Database)

		If database.Connection.Type = Global.DatabaseObjects.Database.ConnectionType.SQLite Then
			'SQLite does not support dropping columns
			Return
		End If

		Dim createTable As New SQLCreateTable
		createTable.Name = "Table1"
		createTable.Fields.Add("Field1", DataType.Integer)
		createTable.Fields.Add("Field2", DataType.Integer)

		Using connection = New ConnectionScope(database)
			connection.Execute(createTable)
		End Using

		Dim alterTable As New SQLAlterTable
		alterTable.Name = "Table1"
		alterTable.Fields.Drop("Field2")

		Using connection = New ConnectionScope(database)
			connection.Execute(alterTable)
		End Using

	End Sub

	''' <summary>
	''' This is not support in SQLite databases.
	''' </summary>
	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	<ExpectedException(GetType(NotSupportedException))>
	Public Sub AlterTableDropFieldSQLiteFailure()

		Dim alterTable As New SQLAlterTable
		alterTable.ConnectionType = Database.ConnectionType.SQLite
		alterTable.Name = "Table1"
		alterTable.Fields.Drop("Field2")

		Dim sql = alterTable.SQL

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	Public Sub CreateTableSQLServer()

		Dim createTable As New SQLCreateTable
		createTable.ConnectionType = Database.ConnectionType.SQLServer
		createTable.Name = "Table1"
		With createTable.Fields.Add("Field1", DataType.Integer)
			.AutoIncrements = True
			.KeyType = KeyType.Primary
		End With
		createTable.Fields.Add("Field2", DataType.VariableCharacter, 10)

		Assert.AreEqual("CREATE TABLE [Table1] ([Field1] INTEGER IDENTITY NOT NULL PRIMARY KEY, [Field2] VARCHAR(10) NULL)", createTable.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	Public Sub CreateTableMySQL()

		Dim createTable As New SQLCreateTable
		createTable.ConnectionType = Database.ConnectionType.MySQL
		createTable.Name = "Table1"
		With createTable.Fields.Add("Field1", DataType.Integer)
			.AutoIncrements = True
			.KeyType = KeyType.Primary
		End With
		createTable.Fields.Add("Field2", DataType.VariableCharacter, 10)

		Assert.AreEqual("CREATE TABLE `Table1` (`Field1` INT AUTO_INCREMENT NOT NULL PRIMARY KEY, `Field2` VARCHAR(10) NULL)", createTable.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	Public Sub AlterTableAlterFieldsSQLServer()

		Dim alterTable As New SQLAlterTable
		alterTable.ConnectionType = Database.ConnectionType.SQLServer
		alterTable.Name = "Table1"
		alterTable.Fields("Field2").Size = 20
		alterTable.Fields("Field3").Size = 30

		Assert.AreEqual("ALTER TABLE [Table1] ALTER COLUMN [Field2] VARCHAR(20) NULL, [Field3] VARCHAR(30) NULL", alterTable.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	Public Sub AlterTableAlterFieldsMySQL()

		Dim alterTable As New SQLAlterTable
		alterTable.ConnectionType = Database.ConnectionType.MySQL
		alterTable.Name = "Table1"
		alterTable.Fields("Field2").Size = 20
		alterTable.Fields("Field3").Size = 30

		Assert.AreEqual("ALTER TABLE `Table1` MODIFY COLUMN `Field2` VARCHAR(20) NULL, MODIFY COLUMN `Field3` VARCHAR(30) NULL", alterTable.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	Public Sub AlterTableAddFieldsSQLServer()

		Dim alterTable As New SQLAlterTable
		alterTable.ConnectionType = Database.ConnectionType.SQLServer
		alterTable.Name = "Table1"
		alterTable.Fields.Add("Field2", DataType.VariableCharacter, 20)
		alterTable.Fields.Add("Field3", DataType.VariableCharacter, 30)

		Assert.AreEqual("ALTER TABLE [Table1] ADD [Field2] VARCHAR(20) NULL, [Field3] VARCHAR(30) NULL", alterTable.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	Public Sub AlterTableAddFieldsMySQL()

		Dim alterTable As New SQLAlterTable
		alterTable.ConnectionType = Database.ConnectionType.MySQL
		alterTable.Name = "Table1"
		alterTable.Fields.Add("Field2", DataType.VariableCharacter, 20)
		alterTable.Fields.Add("Field3", DataType.VariableCharacter, 30)

		Assert.AreEqual("ALTER TABLE `Table1` ADD `Field2` VARCHAR(20) NULL, ADD `Field3` VARCHAR(30) NULL", alterTable.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	Public Sub AlterTableDropFieldsMySQL()

		Dim alterTable As New SQLAlterTable
		alterTable.ConnectionType = Database.ConnectionType.MySQL
		alterTable.Name = "Table1"
		alterTable.Fields.Drop("Field1")
		alterTable.Fields.Drop("Field2")

		Assert.AreEqual("ALTER TABLE `Table1` DROP COLUMN `Field1`, DROP COLUMN `Field2`", alterTable.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	Public Sub AlterTableDropFieldsSQLServer()

		Dim alterTable As New SQLAlterTable
		alterTable.ConnectionType = Database.ConnectionType.SQLServer
		alterTable.Name = "Table1"
		alterTable.Fields.Drop("Field1")
		alterTable.Fields.Drop("Field2")

		Assert.AreEqual("ALTER TABLE [Table1] DROP COLUMN [Field1], [Field2]", alterTable.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	Public Sub CreateTableWithComputedColumn()

		Dim caseExpression As New SQLCaseExpression(New SQLFieldExpression("Field1"))
		caseExpression.Cases.Add(New SQLValueExpression("Value1"), New SQLValueExpression("1"))

		Dim createTable As New SQLCreateTable
		createTable.ConnectionType = Database.ConnectionType.SQLServer
		createTable.Name = "Table1"
		createTable.Fields.Add("Field1", DataType.VariableCharacter, 20)
		createTable.Fields.AddComputed("Computed1", caseExpression)

		Assert.AreEqual("CREATE TABLE [Table1] ([Field1] VARCHAR(20) NULL, [Computed1] AS (CASE [Field1] WHEN 'Value1' THEN '1' END))", createTable.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	Public Sub AlterTableAddComputedColumn()

		Dim caseExpression As New SQLCaseExpression(New SQLFieldExpression("Field1"))
		caseExpression.Cases.Add(New SQLValueExpression("Value1"), New SQLValueExpression("1"))

		Dim alterTable As New SQLAlterTable
		alterTable.ConnectionType = Database.ConnectionType.SQLServer
		alterTable.Name = "Table1"
		alterTable.Fields.AddComputed("Computed2", caseExpression)

		Assert.AreEqual("ALTER TABLE [Table1] ADD [Computed2] AS (CASE [Field1] WHEN 'Value1' THEN '1' END)", alterTable.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	Public Sub AlterTableFieldsIndexderByName()

		Dim alterTable As New SQLAlterTable
		alterTable.Name = "Table1"
		Dim field1 = alterTable.Fields("Field1")
		Dim field2 = alterTable.Fields("Field2")
		field1.KeyType = KeyType.Primary

		Dim field1Reloaded = alterTable.Fields("Field1")
		Assert.AreEqual(KeyType.Primary, field1Reloaded.KeyType)

	End Sub

End Class
