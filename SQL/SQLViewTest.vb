﻿Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.UnitTestExtensions

<DatabaseTestClass(ConnectionStringNames:={"SQLServerTestDatabase", "MySQLTestDatabase"})>
Public Class SQLViewTest

    Public TestContext As TestContext

    <DatabaseTestInitialize()>
    Public Sub DatabaseTestInitialize(database As Database)

        Using connection = New ConnectionScope(database)
            If Not connection.Execute(New SQLTableExists("Table1")).Read Then
                Dim createTable As New SQLCreateTable
                createTable.Name = "Table1"
                createTable.Fields.Add("Field1", DataType.Integer)
                connection.Execute(New SQLDropTable(createTable.Name))
            End If

            If connection.Execute(New SQLViewExists("View1")).Read Then
                connection.Execute(New SQLDropView("View1"))
            End If
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableSchema")>
    Public Sub CreateView(database As Database)

        Dim selectStatement As New SQLSelect
        selectStatement.Tables.Add("Table1")

        Dim createView As New SQLCreateView
        createView.Name = "View1"
        createView.Select = selectStatement

        Using connection = New ConnectionScope(database)
            connection.Execute(createView)
        End Using

    End Sub

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