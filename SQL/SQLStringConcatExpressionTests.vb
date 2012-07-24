Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.SQL.Serializers

<TestClass()>
Public Class SQLStringConcatExpressionTests

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub StringConcatenate()

        Dim concat As New SQLStringConcatExpression("LeftString", "RightString")

		Assert.AreEqual("'LeftString' + 'RightString'", concat.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub StringConcatenateUsingExpressions()

        Dim leftExpression As New SQLValueExpression("LeftString")
        Dim rightExpression As New SQLFieldExpression("RightField")
        Dim concat As New SQLStringConcatExpression(leftExpression, rightExpression)

		Assert.AreEqual("'LeftString' + [RightField]", concat.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub StringConcatenateAll()

        Dim concatAll = SQLStringConcatExpression.ConcatenateAll(
            New SQLValueExpression("Expression1Value"), _
            New SQLValueExpression("Expression2Value"), _
            New SQLValueExpression("Expression3Value"))

		Assert.AreEqual("'Expression1Value' + 'Expression2Value' + 'Expression3Value'", concatAll.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

End Class
