Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.SQL.Serializers

<TestClass()>
Public Class SQLCaseExpressionTest

    Public TestContext As TestContext

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub CaseExpressionBasic()

        Dim caseExpression As New SQLCaseExpression
        caseExpression.Cases.Add(New SQLConditionExpression(New SQLValueExpression(1), ComparisonOperator.EqualTo, New SQLValueExpression(1)), New SQLValueExpression(2))

		Assert.AreEqual("CASE WHEN 1 = 1 THEN 2 END", caseExpression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub CaseExpressionWithInputExpression()

        Dim caseExpression As New SQLCaseExpression(New SQLFieldExpression("Field1"))
        caseExpression.Cases.Add(New SQLValueExpression("Value1"), New SQLValueExpression("1"))

		Assert.AreEqual("CASE [Field1] WHEN 'Value1' THEN '1' END", caseExpression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub CaseExpressionWithInputExpressionMultipleCasesAndElse()

        Dim caseExpression As New SQLCaseExpression(New SQLFieldExpression("Field1"))
        caseExpression.Cases.Add(New SQLValueExpression("Value1"), New SQLValueExpression("1"))
        caseExpression.Cases.Add(New SQLValueExpression("Value2"), New SQLValueExpression("2"))
        caseExpression.ElseResult = New SQLValueExpression("99")

		Assert.AreEqual("CASE [Field1] WHEN 'Value1' THEN '1' WHEN 'Value2' THEN '2' ELSE '99' END", caseExpression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    <ExpectedException(GetType(InvalidOperationException))>
    Public Sub CaseExpressionnNoCases()

        Dim caseExpression As New SQLCaseExpression(New SQLFieldExpression("Field1"))

		caseExpression.SQL(Serializers.Items(Database.ConnectionType.SQLServer))

    End Sub

End Class
