Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.SQL.Serializers

<TestClass()>
Public Class SQLLogicalExpressionTests

    Public TestContext As TestContext

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub LogicalAndExpression()

        Dim logicalExpression As New SQLLogicalExpression(New SQLFieldExpression("F1"), LogicalOperator.And, New SQLFieldExpression("F2"))

        Assert.AreEqual("([F1] AND [F2])", logicalExpression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub
    
    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub LogicalOrExpression()

        Dim logicalExpression As New SQLLogicalExpression(New SQLFieldExpression("F1"), LogicalOperator.Or, New SQLFieldExpression("F2"))

        Assert.AreEqual("([F1] OR [F2])", logicalExpression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

End Class
