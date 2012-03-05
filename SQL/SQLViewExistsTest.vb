Imports System.Text
Imports DatabaseObjects.SQL

<TestClass()>
Public Class SQLViewExistsTest

    Public TestContext As TestContext

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("Views")>
    Public Sub ViewExistsTSQL()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.SQLServer

        Dim viewExists As New SQLViewExists("ViewName")

        Assert.AreEqual(Of String)("SELECT * FROM [sysobjects] WHERE [Name] = 'ViewName' AND [XType] = 'V'", viewExists.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("Views")>
    Public Sub ViewExistsMySQL()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.MySQL

        Dim viewExists As New SQLViewExists("ViewName")

        Assert.AreEqual(Of String)("SHOW VIEWS LIKE 'ViewName'", viewExists.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("Views")>
    Public Sub ViewExistsHyperSQL()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.HyperSQL

        Dim viewExists As New SQLViewExists("ViewName")

        Assert.AreEqual(Of String)("SELECT * FROM ""INFORMATION_SCHEMA"".""VIEWS"" WHERE ""TABLE_SCHEMA"" = 'PUBLIC' AND ""TABLE_NAME"" = 'ViewName'", viewExists.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("Views")>
    <ExpectedException(GetType(ArgumentNullException))>
    Public Sub ViewExistsWithInvalidName()

        Dim view As New SQLViewExists("")

    End Sub

End Class
