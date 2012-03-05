Imports System.Text
Imports DatabaseObjects.SQL

<TestClass()>
Public Class SQLDropViewTest

    Public TestContext As TestContext

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("Views")>
    Public Sub DropView()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.SQLServer

        Dim dropView As New SQLDropView("ViewName")

        Assert.AreEqual(Of String)("DROP VIEW [ViewName]", dropView.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("Views")>
    <ExpectedException(GetType(ArgumentNullException))>
    Public Sub DropViewWithInvalidName()

        Dim view As New SQLDropView("")

    End Sub

End Class
