Imports System.Text
Imports DatabaseObjects.SQL

<TestClass()>
Public Class SQLSpecialCharactersTests

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("SpecialCharacters")>
	Public Sub SingleQuoteInString()

		Dim selectStatement As New SQLSelect("Table")
		selectStatement.ConnectionType = Database.ConnectionType.SQLServer
		selectStatement.Where.Add("Field1", ComparisonOperator.EqualTo, "A'B")

		Assert.AreEqual("SELECT * FROM [Table] WHERE [Field1] = 'A''B'", selectStatement.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("SpecialCharacters")>
	Public Sub SingleQuoteCharacter()

		Dim selectStatement As New SQLSelect("Table")
		selectStatement.ConnectionType = Database.ConnectionType.SQLServer
		selectStatement.Where.Add("Field1", ComparisonOperator.EqualTo, "'"c)

		Assert.AreEqual("SELECT * FROM [Table] WHERE [Field1] = ''''", selectStatement.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("SpecialCharacters")>
	Public Sub SingleQuoteTestMySQL()

		Dim selectStatement As New SQLSelect("Table")
		selectStatement.ConnectionType = Database.ConnectionType.MySQL
		selectStatement.Where.Add("Field1", ComparisonOperator.EqualTo, "A'B")

		Assert.AreEqual("SELECT * FROM `Table` WHERE `Field1` = 'A\'B'", selectStatement.SQL)

	End Sub

	<TestMethod()>
	<TestCategory("SQL"), TestCategory("SpecialCharacters")>
	Public Sub SingleQuoteCharacterMySQL()

		Dim selectStatement As New SQLSelect("Table")
		selectStatement.ConnectionType = Database.ConnectionType.MySQL
		selectStatement.Where.Add("Field1", ComparisonOperator.EqualTo, "'"c)

		Assert.AreEqual("SELECT * FROM `Table` WHERE `Field1` = '\''", selectStatement.SQL)

	End Sub

End Class
