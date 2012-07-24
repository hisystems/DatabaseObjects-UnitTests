Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.SQL.Serializers

<TestClass()>
Public Class SQLRightFunctionExpressionTest

    Public TestContext As TestContext

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub RightFunctionBasic()

        Dim right As New SQLRightFunctionExpression("Field1", 2)

		Assert.AreEqual("RIGHT([Field1], 2)", right.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub RightFunctionWithInvalidLength()

        Dim right As New SQLRightFunctionExpression("Field1", -1)

    End Sub

End Class
