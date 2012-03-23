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
    <TestCategory("SQL"), TestCategory("TableJoins"), TestCategory("TableAlias")>
    Public Sub SingleTableJoinWithTableAlias()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.SQLServer

        Dim table1 As New SQLSelectTable("Table1", "T1")
        Dim table2 As New SQLSelectTable("Table2", "T2")

        Dim selectStatement As New SQLSelect()
        selectStatement.Tables.Add(table1)

        With selectStatement.Tables.Joins.Add(table1, SQLSelectTableJoin.Type.Inner, table2)
            .Where.Add("Table1Key", SQL.ComparisonOperator.EqualTo, "Table2Key")
        End With

        Assert.AreEqual(Of String)("SELECT * FROM ([Table1] [T1] INNER JOIN [Table2] [T2] ON [T1].[Table1Key] = [T2].[Table2Key])", selectStatement.SQL)

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

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableJoins")>
    Public Sub TableJoinsWithOrderOfOperators()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.SQLServer

        Dim table1 As New SQLSelectTable("Table1")
        Dim table2 As New SQLSelectTable("Table2")

        Dim selectStatement As New SQLSelect()
        selectStatement.Tables.Add(table1)

        Dim innerCondition As New SQL.SQLSelectTableJoinConditions
        innerCondition.Add(New SQLFieldExpression("Field1"), ComparisonOperator.EqualTo, New SQLValueExpression(1))
        innerCondition.AddLogicalOperator(LogicalOperator.Or)
        innerCondition.Add(New SQLFieldExpression("Field2"), ComparisonOperator.EqualTo, New SQLValueExpression(2))

        Dim table1Table2Join = selectStatement.Tables.Joins.Add(table1, SQLSelectTableJoin.Type.Inner, table2)
        table1Table2Join.Where.Add("Table1Key", SQL.ComparisonOperator.EqualTo, "Table2Key")
        table1Table2Join.Where.Add(innerCondition)

        Assert.AreEqual(Of String)("SELECT * FROM ([Table1] INNER JOIN [Table2] ON [Table1].[Table1Key] = [Table2].[Table2Key] AND ([Field1] = 1 OR [Field2] = 2))", selectStatement.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableJoins")>
    <ExpectedException(GetType(InvalidOperationException))>
    Public Sub SQLSelectTableJoinConditionsInstanceWithFieldsInsteadOfExpressions()

        Dim innerCondition As New SQL.SQLSelectTableJoinConditions
        'This is only support for backward compatibility - should use SQLExpression subclasses instead
        innerCondition.Add("Field1", ComparisonOperator.EqualTo, "Field2")

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableJoins")>
    Public Sub CreateSingleTableJoinAndSQLFunction()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.SQLServer

        Dim table1 As New SQLSelectTable("Table1")
        Dim table2 As New SQLSelectTable("Table2")

        Dim selectStatement As New SQLSelect()
        selectStatement.Tables.Add(table1)

        Dim table1Table2Join = selectStatement.Tables.Joins.Add(table1, SQLSelectTableJoin.Type.Inner, table2)
        table1Table2Join.Where.Add("Table1Key", SQL.ComparisonOperator.EqualTo, "Table2Key")
        table1Table2Join.Where.Add(New SQLFunctionExpression("IS_MEMBER", New SQLValueExpression("domain\username")), ComparisonOperator.GreaterThan, New SQLValueExpression(0))

        Assert.AreEqual(Of String)("SELECT * FROM ([Table1] INNER JOIN [Table2] ON [Table1].[Table1Key] = [Table2].[Table2Key] AND IS_MEMBER('domain\username') > 0)", selectStatement.SQL)

    End Sub

End Class
