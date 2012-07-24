Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.SQL.Serializers

<TestClass()>
Public Class SQLBitwiseExpressionTest

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub BitwiseAndOperator()

        Dim objBitwiseOperator As New SQLBitwiseExpression()
        objBitwiseOperator.LeftExpression = New SQLValueExpression(1)
        objBitwiseOperator.Operator = BitwiseOperator.And
        objBitwiseOperator.RightExpression = New SQLValueExpression(2)

		Assert.AreEqual(Of String)(objBitwiseOperator.SQL(Serializers.Items(Database.ConnectionType.SQLServer)), "(1 & 2)")

    End Sub

    <TestMethod()> _
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub BitwiseOrOperator()

        Dim objBitwiseOperator As New SQLBitwiseExpression()
        objBitwiseOperator.LeftExpression = New SQLValueExpression(1)
        objBitwiseOperator.Operator = BitwiseOperator.Or
        objBitwiseOperator.RightExpression = New SQLValueExpression(2)

		Assert.AreEqual(Of String)(objBitwiseOperator.SQL(Serializers.Items(Database.ConnectionType.SQLServer)), "(1 | 2)")

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    <ExpectedException(GetType(ArgumentNullException))>
    Public Sub BitwiseOperatorRightExpressionNotSet()

        Dim objBitwiseOperator As New SQLBitwiseExpression()
        objBitwiseOperator.LeftExpression = New SQLValueExpression(1)

		objBitwiseOperator.SQL(Serializers.Items(Database.ConnectionType.SQLServer))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    <ExpectedException(GetType(ArgumentNullException))>
    Public Sub BitwiseOperatorLeftExpressionNotSet()

        Dim objBitwiseOperator As New SQLBitwiseExpression()
        objBitwiseOperator.RightExpression = New SQLValueExpression(1)

		objBitwiseOperator.SQL(Serializers.Items(Database.ConnectionType.SQLServer))

    End Sub

End Class
