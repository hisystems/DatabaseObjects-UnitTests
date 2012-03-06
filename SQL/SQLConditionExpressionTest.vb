Imports System.Text
Imports DatabaseObjects.SQL

<TestClass()>
Public Class SQLConditionExpressionTest

    Public TestContext As TestContext

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SQLCondition()

        Dim condition As New SQLConditionExpression(New SQLValueExpression(1), ComparisonOperator.LessThan, New SQLValueExpression(2))

        Assert.AreEqual("1 < 2", condition.SQL(Database.ConnectionType.SQLServer))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SQLConditionUsingProperties()

        Dim condition As New SQLConditionExpression()
        condition.LeftExpression = New SQLValueExpression(1)
        condition.Compare = ComparisonOperator.LessThan
        condition.RightExpression = New SQLValueExpression(2)

        Assert.AreEqual("1 < 2", condition.SQL(Database.ConnectionType.SQLServer))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SQLConditionWithSelectStatement()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.SQLServer

        Dim selectStatement As New SQLSelect("Table1")
        selectStatement.Where.Add(New SQLConditionExpression(New SQLValueExpression(1), ComparisonOperator.LessThan, New SQLValueExpression(2)))

        Assert.AreEqual("SELECT * FROM [Table1] WHERE 1 < 2", selectStatement.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    <ExpectedException(GetType(InvalidOperationException))>
    Public Sub SQLConditionRightExpressionNotSet()

        Dim condition As New SQLConditionExpression()
        condition.LeftExpression = New SQLValueExpression(1)

        condition.SQL(Database.ConnectionType.SQLServer)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    <ExpectedException(GetType(InvalidOperationException))>
    Public Sub SQLConditionLeftExpressionNotSet()

        Dim condition As New SQLConditionExpression()
        condition.RightExpression = New SQLValueExpression(2)

        condition.SQL(Database.ConnectionType.SQLServer)

    End Sub

End Class
