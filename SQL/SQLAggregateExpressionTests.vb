Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.SQL.Serializers

<TestClass()>
Public Class SQLAggregateExpressionTests

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("SQLExpression")>
	Public Sub AggregateFunctionAverage()

		Dim aggregateExpression As New SQLAggregateExpression(AggregateFunction.Average, New SQLFieldExpression("Field1"))

		Assert.AreEqual("AVG([Field1])", aggregateExpression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("SQLExpression")>
	Public Sub AggregateFunctionCount()

		Dim aggregateExpression As New SQLAggregateExpression(AggregateFunction.Count, New SQLAllFieldsExpression)

		Assert.AreEqual("COUNT(*)", aggregateExpression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

	End Sub

End Class
