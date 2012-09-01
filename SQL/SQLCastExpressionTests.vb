Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.SQL.Serializers

<TestClass()>
Public Class SQLCastExpressionTests

    Public TestContext As TestContext

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub CastExpression()

        Dim castExpression As New SQLCastExpression("Field1", DataType.UnicodeVariableCharacter, 10)

        Assert.AreEqual("CAST([Field1] AS NVARCHAR(10))", castExpression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub CastExpressionNonString()

        Dim castExpression As New SQLCastExpression("Field1", DataType.DateTime)

        Assert.AreEqual("CAST([Field1] AS DATETIME)", castExpression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

End Class
