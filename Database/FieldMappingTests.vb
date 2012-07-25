﻿Option Infer On

Imports System.Text
Imports System.Configuration
Imports System.Linq
Imports DatabaseObjects.SQL
Imports DatabaseObjects.Exceptions
Imports DatabaseObjects.UnitTestExtensions

<DatabaseTestClass(ConnectionStringNames:={"SQLServerTestDatabase", "MySQLTestDatabase", "SQLiteTestDatabase"})>
Public Class FieldMappingTests

	<Table(FieldMappingTestCollection.Name)>
	<DistinctField("PrimaryField", FieldValueAutoAssignmentType.AutoIncrement)>
	Private Class FieldMappingTestCollection
		Inherits Global.DatabaseObjects.Generic.DatabaseObjectsList(Of FieldMappingTestItem)

		Public Const Name As String = "FieldMappingTestCollection"

		Friend Sub New(database As Database)

			MyBase.New(database)

		End Sub

		Default Public ReadOnly Property Item(ByVal primaryID As Integer) As FieldMappingTestItem
			Get

				Return MyBase.Object(primaryID)

			End Get
		End Property

		Friend Shared Function TableSchema() As SQLCreateTable

			Dim createTable As New SQLCreateTable
			createTable.Name = Name
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
			createTable.Fields.Add("Image", DataType.Image)

			Return createTable

		End Function

	End Class

	Private Class FieldMappingTestItem
		Inherits DatabaseObject

		<FieldMapping("Boolean")>
		Public BooleanField As Boolean

		<FieldMapping("Character")>
		Public CharacterField As String

		<FieldMapping("DateTime")>
		Public DateTimeField As DateTime

		<FieldMapping("Decimal")>
		Public DecimalField As Decimal

		<FieldMapping("Float")>
		Public FloatField As Double

		<FieldMapping("Money")>
		Public MoneyField As Decimal

		<FieldMapping("Text")>
		Public TextField As String

		<FieldMapping("Image")>
		Public ImageField As Byte()

		Friend Sub New(parent As FieldMappingTestCollection)

			MyBase.New(parent)

		End Sub

		Public Shadows Sub Save()

			MyBase.Save()

		End Sub

	End Class

	Public Property TestContext As TestContext

	<DatabaseTestInitialize()>
	Public Sub DatabaseTestInitialize(database As Database)

		AddHandler database.Connection.StatementExecuted, _
		 Sub(statement As ISQLStatement)
			 TestContext.WriteLine(statement.SQL)
		 End Sub

		database.RecreateTable(FieldMappingTestCollection.TableSchema)

	End Sub

	<TestMethod()>
	<TestCategory("Database")>
	Public Sub ObjectByDistinctValueExists(database As Database)

		Dim table = New FieldMappingTestCollection(database)
		Dim newItem As FieldMappingTestItem = table.Add

		newItem.BooleanField = True
		newItem.CharacterField = "c"c
		newItem.DateTimeField = New DateTime(2000, 1, 2, 3, 4, 5, 6)
		newItem.DecimalField = 123456789
		newItem.FloatField = 123.123
		newItem.ImageField = New Byte() {1, 2, 3, 4}
		newItem.MoneyField = 123.456
		newItem.TextField = "Text"
		newItem.Save()

		Dim reloaded = table(DirectCast(newItem, IDatabaseObject).DistinctValue)

		Assert.AreEqual(newItem.BooleanField, reloaded.BooleanField)
		Assert.AreEqual(newItem.CharacterField, reloaded.CharacterField)
		Assert.AreEqual(newItem.DateTimeField, reloaded.DateTimeField)
		Assert.AreEqual(newItem.DecimalField, reloaded.DecimalField)
		Assert.AreEqual(newItem.FloatField, reloaded.FloatField)
		Assert.AreEqual(newItem.ImageField.Length, reloaded.ImageField.Length)
		Assert.AreEqual(newItem.MoneyField, reloaded.MoneyField)
		Assert.AreEqual(newItem.TextField, reloaded.TextField)

	End Sub

End Class