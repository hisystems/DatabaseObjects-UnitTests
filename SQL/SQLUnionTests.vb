Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.UnitTestExtensions

<TestClass()>
Public Class SQLUnionTests

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("Union")>
	Public Sub UnionDistinct()

		Dim selectStatement1 As New SQLSelect("Table1")
		selectStatement1.Fields.Add("Field1")

		Dim selectStatement2 As New SQLSelect("Table2")
		selectStatement2.Fields.Add("Field2")

		Dim union As New SQLUnion(selectStatement1, selectStatement2)
		union.ConnectionType = Global.DatabaseObjects.Database.ConnectionType.SQLServer
		union.OrderBy.Add("Field1")

		Assert.AreEqual("(SELECT [Field1] FROM [Table1]) UNION (SELECT [Field2] FROM [Table2]) ORDER BY [Field1]", union.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("Union")>
	Public Sub UnionAll()

		Dim selectStatement1 As New SQLSelect("Table1")
		selectStatement1.Fields.Add("Field1")

		Dim selectStatement2 As New SQLSelect("Table2")
		selectStatement2.Fields.Add("Field2")

		Dim union As New SQLUnion(selectStatement1, SQLUnion.Type.All, selectStatement2)
		union.ConnectionType = Global.DatabaseObjects.Database.ConnectionType.SQLServer

		Assert.AreEqual("(SELECT [Field1] FROM [Table1]) UNION ALL (SELECT [Field2] FROM [Table2])", union.SQL)

	End Sub

End Class
