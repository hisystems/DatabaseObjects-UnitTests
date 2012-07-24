Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.SQL.Serializers

<TestClass()>
Public Class SQLGetDateFunctionExpressionTests

    Public TestContext As TestContext

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SQLGetDateFunctionExpressionSQLServer()

        Dim getDateExpression As New SQLGetDateFunctionExpression

		Assert.AreEqual("GETDATE()", getDateExpression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)), ignoreCase:=True)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SQLGetDateFunctionExpressionMySQL()

        Dim getDateExpression As New SQLGetDateFunctionExpression

		Assert.AreEqual("CURDATE()", getDateExpression.SQL(Serializers.Items(Database.ConnectionType.MySQL)), ignoreCase:=True)

    End Sub


End Class
