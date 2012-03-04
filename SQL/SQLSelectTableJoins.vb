Imports System.Text
Imports DatabaseObjects.SQL

<TestClass()>
Public Class SQLSelectTableJoins

    Public TestContext As TestContext

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableJoins")>
    Public Sub SingleTableJoin()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.SQLServer

        Dim table1 As New SQLSelectTable("Table1")
        Dim table2 As New SQLSelectTable("Table2")

        Dim selectStatement As New SQLSelect()
        selectStatement.Tables.Add(table1)

        With selectStatement.Tables.Joins.Add(table1, SQLSelectTableJoin.Type.Inner, table2)
            .Where.Add("Table1Key", SQL.ComparisonOperator.EqualTo, "Table2Key")
        End With

        Assert.AreEqual(Of String)("SELECT * FROM ([Table1] INNER JOIN [Table2] ON [Table1].[Table1Key] = [Table2].[Table2Key])", selectStatement.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableJoins")>
    Public Sub SingleTableJoinWithMultipleConditions()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.SQLServer

        Dim table1 As New SQLSelectTable("Table1")
        Dim table2 As New SQLSelectTable("Table2")

        Dim selectStatement As New SQLSelect()
        selectStatement.Tables.Add(table1)

        With selectStatement.Tables.Joins.Add(table1, SQLSelectTableJoin.Type.Inner, table2)
            .Where.Add("Table1Key", SQL.ComparisonOperator.EqualTo, "Table2Key")
            .Where.Add("Table1Value", SQL.ComparisonOperator.EqualTo, "Table2Value")
        End With

        Assert.AreEqual(Of String)("SELECT * FROM ([Table1] INNER JOIN [Table2] ON [Table1].[Table1Key] = [Table2].[Table2Key] AND [Table1].[Table1Value] = [Table2].[Table2Value])", selectStatement.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableJoins")>
    Public Sub CreateMultipleTableJoinsUsingSourceTableAsJoinedTables()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.SQLServer

        Dim table1 As New SQLSelectTable("Table1")
        Dim table2 As New SQLSelectTable("Table2")
        Dim table3 As New SQLSelectTable("Table3")

        Dim selectStatement As New SQLSelect()
        selectStatement.Tables.Add(table1)

        Dim table1Table2Join = selectStatement.Tables.Joins.Add(table1, SQLSelectTableJoin.Type.Inner, table2)
        table1Table2Join.Where.Add("Table1Key", SQL.ComparisonOperator.EqualTo, "Table2Key")

        Dim table2Table3Join = selectStatement.Tables.Joins.Add(table1Table2Join, SQLSelectTableJoin.Type.Inner, table3)
        table2Table3Join.Where.Add(New SQLFieldExpression(table2, "Table2Key"), SQL.ComparisonOperator.EqualTo, New SQLFieldExpression(table3, "Table3Key"))

        Assert.AreEqual(Of String)("SELECT * FROM (([Table1] INNER JOIN [Table2] ON [Table1].[Table1Key] = [Table2].[Table2Key]) INNER JOIN [Table3] ON [Table2].[Table2Key] = [Table3].[Table3Key])", selectStatement.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableJoins")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub CreateMultipleTableJoinsUsingSQLSelectTableAndOldAddFieldNames()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.SQLServer

        Dim table1 As New SQLSelectTable("Table1")
        Dim table2 As New SQLSelectTable("Table2")
        Dim table3 As New SQLSelectTable("Table3")

        Dim selectStatement As New SQLSelect()
        selectStatement.Tables.Add(table1)

        Dim table1Table2Join = selectStatement.Tables.Joins.Add(table1, SQLSelectTableJoin.Type.Inner, table2)
        table1Table2Join.Where.Add("Table1Key", SQL.ComparisonOperator.EqualTo, "Table2Key")

        Dim table2Table3Join = selectStatement.Tables.Joins.Add(table1Table2Join, SQLSelectTableJoin.Type.Inner, table3)
        'This call is incorrect because table1Table2Join is a SQLSelectTableJoin not an SQLSelectTable as expected. This is really just required for backward compatibility.
        table2Table3Join.Where.Add("Table2Key", SQL.ComparisonOperator.EqualTo, "Table3Key")

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableJoins")>
    Public Sub CreateMultipleTableJoinsUsingSourceTableAsJoinedTablesWithMultipleConditions()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.SQLServer

        Dim table1 As New SQLSelectTable("Table1")
        Dim table2 As New SQLSelectTable("Table2")
        Dim table3 As New SQLSelectTable("Table3")

        Dim selectStatement As New SQLSelect()
        selectStatement.Tables.Add(table1)

        Dim table1Table2Join = selectStatement.Tables.Joins.Add(table1, SQLSelectTableJoin.Type.Inner, table2)
        table1Table2Join.Where.Add("Table1Key", SQL.ComparisonOperator.EqualTo, "Table2Key")

        Dim table2Table3Join = selectStatement.Tables.Joins.Add(table1Table2Join, SQLSelectTableJoin.Type.Inner, table3)
        table2Table3Join.Where.Add(New SQLFieldExpression(table2, "Table2Key"), SQL.ComparisonOperator.EqualTo, New SQLFieldExpression(table3, "Table3Key"))
        table2Table3Join.Where.AddLogicalOperator(LogicalOperator.And)
        table2Table3Join.Where.Add(New SQLFieldExpression(table2, "Table2Value"), SQL.ComparisonOperator.EqualTo, New SQLValueExpression(1))

        Assert.AreEqual(Of String)("SELECT * FROM (([Table1] INNER JOIN [Table2] ON [Table1].[Table1Key] = [Table2].[Table2Key]) INNER JOIN [Table3] ON [Table2].[Table2Key] = [Table3].[Table3Key] AND [Table2].[Table2Value] = 1)", selectStatement.SQL)

    End Sub

End Class
