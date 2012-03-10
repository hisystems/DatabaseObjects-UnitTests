﻿Imports System.Text
Imports System.Configuration
Imports DatabaseObjects.SQL

<TestClass()>
Public Class DatabaseTests

    Public Property TestContext As TestContext

    Private Shared database As Database
    Private Shared table As SimpleTable

    <ClassInitialize()>
    Public Shared Sub ClassInitialize(context As TestContext)

        database = New Database(ConfigurationManager.ConnectionStrings("SQLServerTestDatabase").ConnectionString, database.ConnectionType.SQLServer)
        table = New SimpleTable(database)

    End Sub

    <TestInitialize()>
    Public Sub InitializeTable1WithAutoIncrementPrimaryKeyAndField1()

        AddHandler database.Connection.StatementExecuted, _
            Sub(statement As ISQLStatement)
                TestContext.WriteLine(statement.SQL)
            End Sub

        database.Connection.Start()

        If database.Connection.Execute(New SQLTableExists(SimpleTable.Name)).Read Then
            database.Connection.Execute(New SQLDropTable(SimpleTable.Name))
        End If

        database.Connection.Execute(SimpleTable.TableSchema)

        With table.Add
            .Field1 = "Field1-1"
            .Save()
        End With

        With table.Add
            .Field1 = "Field1-2"
            .Save()
        End With

        With table.Add
            .Field1 = "Field1-3"
            .Save()
        End With

        database.Connection.Finished()

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectByDistinctValueExists()

        Assert.IsTrue(database.ObjectExistsByDistinctValue(table, 2))

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectByDistinctValueDoesNotExist()

        Assert.IsFalse(database.ObjectExistsByDistinctValue(table, 4))

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectExists()

        Assert.IsTrue(database.ObjectExists(table, "Field1-1"))

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectExistsWhenItDoesNotExist()

        Assert.IsFalse(database.ObjectExists(table, "Field1-9999999999999"))

    End Sub

End Class
