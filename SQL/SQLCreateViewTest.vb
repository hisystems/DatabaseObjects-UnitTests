Imports System.Text
Imports DatabaseObjects.SQL

<TestClass()>
Public Class SQLCreateViewTest

    Public TestContext As TestContext

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("Views")>
    Public Sub CreateView()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.SQLServer

        Dim view As New SQLCreateView
        view.Name = "ViewName"
        view.Select = New SQLSelect("Table1")

        Assert.AreEqual(Of String)("CREATE VIEW [ViewName] AS SELECT * FROM [Table1]", view.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("Views")>
    Public Sub CreateViewUsingConstructor()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.SQLServer

        Dim view As New SQLCreateView("ViewName", New SQLSelect("Table1"))

        Assert.AreEqual(Of String)("CREATE VIEW [ViewName] AS SELECT * FROM [Table1]", view.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("Views")>
    <ExpectedException(GetType(Exceptions.DatabaseObjectsException))>
    Public Sub CreateViewWithInvalidName()

        Dim view As New SQLCreateView
        Dim sql As String = view.SQL()

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("Views")>
    <ExpectedException(GetType(Exceptions.DatabaseObjectsException))>
    Public Sub CreateViewWithInvalidSelectStatement()

        Dim view As New SQLCreateView
        view.Name = "ViewName"

        Dim sql As String = view.SQL()

    End Sub

End Class
