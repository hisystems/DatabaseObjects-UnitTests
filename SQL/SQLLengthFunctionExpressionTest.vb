Imports System.Text
Imports DatabaseObjects.SQL

<TestClass()>
Public Class SQLLengthFunctionExpressionTest

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SQLLengthFunctionExpressionTSQL()

        Dim lengthFunction As New SQLLengthFunctionExpression(New SQLValueExpression("123"))

        Assert.AreEqual(Of String)(lengthFunction.SQL(Database.ConnectionType.SQLServer), "LEN('123')")

    End Sub

    <TestMethod()> _
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SQLLengthFunctionExpressionMySQL()

        Dim lengthFunction As New SQLLengthFunctionExpression(New SQLValueExpression("123"))

        Assert.AreEqual(Of String)(lengthFunction.SQL(Database.ConnectionType.MySQL), "LENGTH('123')")

    End Sub

    <TestMethod()> _
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    <ExpectedException(GetType(ArgumentNullException))>
    Public Sub SQLLengthFunctionExpressionInvalidArgument()

        Dim lengthFunction As New SQLLengthFunctionExpression(Nothing)

    End Sub

End Class
