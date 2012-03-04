Imports System.Text
Imports DatabaseObjects.SQL

<TestClass()>
Public Class SQLFunctionExpressionTest

    Public TestContext As TestContext

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SQLFunctionNoArguments()

        Dim function1 As New SQLFunctionExpression("SQLFunction")

        Assert.AreEqual("SQLFunction()", function1.SQL(Database.ConnectionType.SQLServer))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SQLFunctionOneArgument()

        Dim function1 As New SQLFunctionExpression("SQLFunction", New SQLValueExpression(1))

        Assert.AreEqual("SQLFunction(1)", function1.SQL(Database.ConnectionType.SQLServer))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SQLFunctionMultipleComplexArguments()

        Dim function1 As New SQLFunctionExpression("SQLFunction", New SQLValueExpression(1), New SQLArithmeticExpression("Field1", ArithmeticOperator.Add, 2))

        Assert.AreEqual("SQLFunction(1, ([Field1] + 2))", function1.SQL(Database.ConnectionType.SQLServer))

    End Sub

End Class
