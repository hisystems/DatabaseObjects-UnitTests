﻿Imports System.Text
Imports System.Configuration
Imports DatabaseObjects.SQL
Imports DatabaseObjects.Exceptions

<TestClass()>
Public Class DatabaseTests

    Public Property TestContext As TestContext

    Private Shared database As Database
    Private Shared table As SimpleTable

    <ClassInitialize()>
    Public Shared Sub ClassInitialize(context As TestContext)

        database = MicrosoftSQLServerDatabase.Parse(ConfigurationManager.ConnectionStrings("SQLServerTestDatabase").ConnectionString)
        table = New SimpleTable(database)

    End Sub

    <TestInitialize()>
    Public Sub TestInitialize()

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

    <TestMethod()>
    <TestCategory("Database"), TestCategory("Connection")>
    Public Sub ConnectionScopeExecute()

        Using connection = New ConnectionScope(database)
            Assert.IsTrue(connection.Execute(New SQLSelect(SimpleTable.Name)).Read)
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("Connection")>
    Public Sub ConnectionScopeExecuteScalar()

        Using connection = New ConnectionScope(database)
            Assert.AreNotEqual(connection.ExecuteScalar(New SQLSelect(SimpleTable.Name)), DBNull.Value)
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("Connection")>
    Public Sub ConnectionScopeExecuteNonQuery()

        Using connection = New ConnectionScope(database)
            With table.Add
                .Field1 = "abc"
                .Save()
            End With
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("Connection")>
    <ExpectedException(GetType(DatabaseObjectsException))>
    Public Sub ConnectionScopeExecuteAfterDispose()

        Dim connection As New ConnectionScope(database)

        Using connection
            'Will close the connection here...
        End Using

        Assert.IsTrue(connection.Execute(New SQLSelect(SimpleTable.Name)).Read)

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("Connection")>
    Public Sub ConnectionScopeDisposeTwice()

        Using connection As New ConnectionScope(database)
            'Dispose once
            connection.Dispose()
            'Dispose twice
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("Connection")>
    Public Sub ConnectionScopeInsideTransactionScope()

        Using transaction = New System.Transactions.TransactionScope
            Using connection = New ConnectionScope(database)
                Assert.IsTrue(connection.Execute(New SQLSelect(SimpleTable.Name)).Read)
            End Using
            transaction.Complete()
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("Connection")>
    Public Sub ConnectionScopeOutsideTransactionScope()

        Using connection = New ConnectionScope(database)
            Using transaction = New System.Transactions.TransactionScope
                Assert.IsTrue(connection.Execute(New SQLSelect(SimpleTable.Name)).Read)
                transaction.Complete()
            End Using
            Assert.IsTrue(connection.Execute(New SQLSelect(SimpleTable.Name)).Read)
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectIfExists()

        Assert.AreNotSame(database.ObjectIfExists(table, 1), Nothing)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectExistsByDistinctValue()

        Assert.IsTrue(database.ObjectExistsByDistinctValue(table, 1))

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectSaveNew()

        Dim newObject = table.Add
        newObject.Field1 = "Field1-NEW"

        database.ObjectSave(table, newObject)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectSaveExisting()

        Dim existingObject = table("Field1-1")
        existingObject.Field1 = "Field1-1-Amended"

        database.ObjectSave(table, existingObject)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectByKeyIfExists()

        Assert.AreNotSame(database.ObjectByKeyIfExists(table, "Field1-1"), Nothing)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectByOrdinalFirst()

        Dim firstObject As SimpleTableItem = database.ObjectByOrdinalFirst(table)

        Assert.AreEqual(firstObject.Field1, "Field1-1")

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectByOrdinalLast()

        Dim lastObject As SimpleTableItem = database.ObjectByOrdinalLast(table)

        Assert.AreEqual(lastObject.Field1, "Field1-3")

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectsCount()

        Assert.AreEqual(database.ObjectsCount(table), 3)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectDelete()

        database.ObjectDelete(table, table("Field1-1"))

        Assert.AreEqual(database.ObjectsCount(table), 2)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectsDeleteAll()

        database.ObjectsDeleteAll(table)

        Assert.AreEqual(database.ObjectsCount(table), 0)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectsList()

        Dim items = database.ObjectsList(table)

        Assert.AreEqual(items.Count, 3)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectsSearch()

        Dim conditions As New SQLConditions()
        conditions.Add("Field1", ComparisonOperator.EqualTo, "Field1-1")

        Dim items = database.ObjectsSearch(table, conditions)

        Assert.AreEqual(items.Count, 1)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectGetFieldValue()

        Dim item2 = table("Field1-2")

        Assert.AreEqual(item2.Field1, database.ObjectGetFieldValue(item2, "Field1"))

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectSetFieldValue()

        Dim item2 = table("Field1-2")
        database.ObjectSetFieldValue(item2, "Field1", "Field1-2-NEW")

        Assert.AreEqual("Field1-2-NEW", database.ObjectGetFieldValue(item2, "Field1"))

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectsDictionary()

        Dim items = database.ObjectsDictionary(table)

        Assert.AreEqual(items.Count, 3)
        Assert.AreEqual(DirectCast(items("Field1-2"), SimpleTableItem).Field1, "Field1-2")

    End Sub

End Class
