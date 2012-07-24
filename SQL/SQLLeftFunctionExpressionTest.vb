Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.SQL.Serializers

<TestClass()>
Public Class SQLLeftFunctionExpressionTest

    Public TestContext As TestContext

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub LeftFunctionBasic()

        Dim left As New SQLLeftFunctionExpression("Field1", 2)

		Assert.AreEqual("LEFT([Field1], 2)", left.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub LeftFunctionWithInvalidLength()

        Dim left As New SQLLeftFunctionExpression("Field1", -1)

    End Sub

End Class
