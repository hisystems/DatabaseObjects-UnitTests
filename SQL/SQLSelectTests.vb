Imports System.Text
Imports DatabaseObjects.SQL

<TestClass()>
Public Class SQLSelectTests

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLSelect")>
    Public Sub SQLSelectAllFieldsFromTable()

        Dim selectStatement As New SQLSelect("Table")
        selectStatement.ConnectionType = Database.ConnectionType.SQLServer

        Assert.AreEqual("SELECT * FROM [Table]", selectStatement.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLSelect")>
    Public Sub SelectWithTableJoin()

        Dim table1 As New SQLSelectTable("Table1")
        Dim table2 As New SQLSelectTable("Table2")

        Dim selectStatement As New SQLSelect()
        selectStatement.ConnectionType = Database.ConnectionType.SQLServer
        selectStatement.Tables.Add(table1)
        With selectStatement.Tables.Joins.Add(table1, SQLSelectTableJoin.Type.Inner, table2)
            .Where.Add("Table1Field", ComparisonOperator.EqualTo, "Table2Field")
        End With

        Assert.AreEqual("SELECT * FROM ([Table1] INNER JOIN [Table2] ON [Table1].[Table1Field] = [Table2].[Table2Field])", selectStatement.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLSelect")>
    Public Sub SelectTableParentProperty()

        Dim selectStatement As New SQLSelect

        Assert.AreSame(selectStatement, selectStatement.Tables.Parent)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLSelect")>
    Public Sub SelectTableParentPropertyForNewTablesObject()

        Dim newTablesCollection = New SQLSelectTables
        Dim selectStatement As New SQLSelect
        selectStatement.Tables = newTablesCollection

        Assert.AreSame(selectStatement, selectStatement.Tables.Parent)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLSelect")>
    Public Sub SelectTableWithSchemaName()

        Dim table1 As New SQLSelectTable("Table1")
        table1.SchemaName = "dbo"

        Dim selectStatement As New SQLSelect
        selectStatement.ConnectionType = Database.ConnectionType.SQLServer
        selectStatement.Tables.Add(table1)

        Assert.AreEqual("SELECT * FROM [dbo].[Table1]", selectStatement.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLSelect")>
    Public Sub SelectTableWithDatabaseAndSchemaName()

        Dim table1 As New SQLSelectTable("Table1")
        table1.SchemaName = "dbo"
        table1.DatabaseName = "database"

        Dim selectStatement As New SQLSelect
        selectStatement.ConnectionType = Database.ConnectionType.SQLServer
        selectStatement.Tables.Add(table1)

        Assert.AreEqual("SELECT * FROM [database].[dbo].[Table1]", selectStatement.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLSelect"), TestCategory("TableAlias")>
    Public Sub SelectTableWithDatabaseSchemaNameAndAlias()

        Dim table1 As New SQLSelectTable("Table1", "T1")
        table1.SchemaName = "dbo"
        table1.DatabaseName = "database"

        Dim selectStatement As New SQLSelect
        selectStatement.ConnectionType = Database.ConnectionType.SQLServer
        selectStatement.Tables.Add(table1)

        Assert.AreEqual("SELECT * FROM [database].[dbo].[Table1] [T1]", selectStatement.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLSelect")>
    Public Sub SelectFromSelect()

        Dim table1 As New SQLSelectTable("Table1")
        Dim table2 As New SQLSelectTableFromSelect(New SQLSelect("Table2"), "T2")

        Dim selectStatement As New SQLSelect
        selectStatement.ConnectionType = Database.ConnectionType.SQLServer
        selectStatement.Tables.Add(table1)
        selectStatement.Tables.Add(table2)

        Assert.AreEqual("SELECT * FROM [Table1], (SELECT * FROM [Table2]) [T2]", selectStatement.SQL)

    End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("SQLSelect")>
	Public Sub SelectOrderByFieldsIndexerByName()

		Dim selectStatement As New SQLSelect("Table")
		selectStatement.ConnectionType = Database.ConnectionType.SQLServer
		selectStatement.OrderBy.Add("Field1")

		Dim field1 = selectStatement.OrderBy("Field1")
		Assert.AreEqual("Field1", field1.Name)
		Assert.AreEqual(OrderBy.Ascending, field1.Order)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("SQLSelect")>
	Public Sub SelectWithTableNameAndCondition()

		Dim condition As New SQLCondition("Field", ComparisonOperator.EqualTo, 1)
		Dim selectStatement As New SQLSelect("Table", condition)
		selectStatement.ConnectionType = Database.ConnectionType.SQLServer

		Assert.AreEqual("SELECT * FROM [Table] WHERE [Field] = 1", selectStatement.SQL)

	End Sub

End Class
