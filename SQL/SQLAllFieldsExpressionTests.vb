Imports System.Text
Imports DatabaseObjects.SQL

<TestClass()>
Public Class SQLAllFieldsExpressionTests

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SelectAllFieldsSimpleFromTable()

        Dim selectStatement As New SQLSelect("Table")
        selectStatement.ConnectionType = Database.ConnectionType.SQLServer
        selectStatement.Fields.Add(New SQLAllFieldsExpression())

        Assert.AreEqual("SELECT * FROM [Table]", selectStatement.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SelectAllFieldsFromTable1WithTable2Join()

        Dim table1 As New SQLSelectTable("Table1")
        Dim table2 As New SQLSelectTable("Table2")

        Dim selectStatement As New SQLSelect()
        selectStatement.ConnectionType = Database.ConnectionType.SQLServer
        selectStatement.Tables.Add(table1)
        With selectStatement.Tables.Joins.Add(table1, SQLSelectTableJoin.Type.Inner, table2)
            .Where.Add("Table1Field", ComparisonOperator.EqualTo, "Table2Field")
        End With

        selectStatement.Fields.Add(New SQLAllFieldsExpression(table1))
        selectStatement.Fields.Add("Field2", table2)

        Assert.AreEqual("SELECT [Table1].*, [Table2].[Field2] FROM ([Table1] INNER JOIN [Table2] ON [Table1].[Table1Field] = [Table2].[Table2Field])", selectStatement.SQL)

    End Sub

End Class
