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
    <TestCategory("SQL"), TestCategory("SQLExpression")>
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

End Class
