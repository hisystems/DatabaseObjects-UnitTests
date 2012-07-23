Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.UnitTestExtensions

<TestClass()>
Public Class SQLInsertUpdateTests

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("TableSchema")>
	Public Sub FieldsIndexerByName()

		Dim fieldValues As New SQLFieldValues
		fieldValues.Add("Field1", 123)

		Dim field1 = fieldValues("Field1")

		Assert.AreEqual(123, field1.Value)

	End Sub

End Class
