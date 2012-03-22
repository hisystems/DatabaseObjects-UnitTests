Imports System.Text
Imports DatabaseObjects.SQL

<TestClass()>
Public Class SQLFunctionExpressionTest

    Public TestContext As TestContext

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SQLFunctionNoArguments()

        Dim function1 As New SQLFunctionExpression("SQLFunction")

        Assert.AreEqual("SQLFunction", function1.SQL(Database.ConnectionType.SQLServer))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SQLFunctionNoArgumentsWithParentheses()

        Dim function1 As New SQLFunctionExpression("SQLFunction")
        function1.IncludeParenthesesWhenArgumentsEmpty = True

        Assert.AreEqual("SQLFunction()", function1.SQL(Database.ConnectionType.SQLServer))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    <ExpectedException(GetType(ArgumentNullException))>
    Public Sub SQLFunctionWithEmptyArguments()

        Dim function1 As New SQLFunctionExpression("SQLFunction", Nothing)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SQLFunctionWithNullArgument()

        Dim function1 As New SQLFunctionExpression("SQLFunction", New SQLValueExpression())

        Assert.AreEqual("SQLFunction(NULL)", function1.SQL(Database.ConnectionType.SQLServer))

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
