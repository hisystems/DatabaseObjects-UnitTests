Option Infer On

Imports System.Text
Imports System.Configuration
Imports System.Linq
Imports DatabaseObjects.SQL
Imports DatabaseObjects.Exceptions

<TestClass()>
Public Class TransactionsAndConnectionsTests

    Public Property TestContext As TestContext

    Private Shared database As Database
    Private Shared table As SimpleTable

    <ClassInitialize()>
    Public Shared Sub ClassInitialize(TestContext As TestContext)

        database = MicrosoftSQLServerDatabase.Parse(ConfigurationManager.ConnectionStrings("SQLServerTestDatabase").ConnectionString)
        table = New SimpleTable(database)

    End Sub

    <TestInitialize()>
    Public Sub TestInitialize()

        AddHandler database.Connection.StatementExecuted, _
            Sub(statement As ISQLStatement)
                TestContext.WriteLine(statement.SQL)
            End Sub

        Using connection As New ConnectionScope(database)
            If connection.Execute(New SQLTableExists(SimpleTable.Name)).Read Then
                connection.Execute(New SQLDropTable(SimpleTable.Name))
            End If
            connection.Execute(SimpleTable.TableSchema)
        End Using

        With table.Add
            .Field1 = "Field1-1"
            .Save()
        End With

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
    <TestCategory("Database"), TestCategory("Connection"), TestCategory("TransactionScope")>
    Public Sub TransactionScopeAndConnectionScope()

        Using transaction = New System.Transactions.TransactionScope
            Using connection = New ConnectionScope(database)
                Dim insert As New SQLInsert With {.TableName = SimpleTable.Name}
                insert.Fields.Add("Field1", "Field1-2")
                connection.Execute(insert)
            End Using

            With table.Add
                .Field1 = "Field1-3"
                .Save()
            End With
            transaction.Complete()
        End Using

        Using connection = New ConnectionScope(database)
            Assert.AreEqual(3, connection.ExecuteScalar(GetRowCount(SimpleTable.Name)))
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("Connection"), TestCategory("TransactionScope")>
    Public Sub TransactionScopeAndConnectionStartFinished()

        Using transaction = New System.Transactions.TransactionScope
            database.Connection.Start()
            Dim insert As New SQLInsert With {.TableName = SimpleTable.Name}
            insert.Fields.Add("Field1", "Field1-2")
            database.Connection.Execute(insert)
            database.Connection.Finished()

            With table.Add
                .Field1 = "Field1-3"
                .Save()
            End With
            transaction.Complete()
        End Using

        Using connection = New ConnectionScope(database)
            Assert.AreEqual(3, connection.ExecuteScalar(GetRowCount(SimpleTable.Name)))
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("Connection"), TestCategory("TransactionScope")>
    Public Sub TransactionScopeAndConnectionScopeWithRollback()

        Using transaction = New System.Transactions.TransactionScope
            Using connection = New ConnectionScope(database)
                Dim insert As New SQLInsert With {.TableName = SimpleTable.Name}
                insert.Fields.Add("Field1", "Field1-2")
                connection.Execute(insert)
            End Using

            With table.Add
                .Field1 = "Field1-3"
                .Save()
            End With
        End Using

        Using connection = New ConnectionScope(database)
            Assert.AreEqual(1, connection.ExecuteScalar(GetRowCount(SimpleTable.Name)))
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("Connection"), TestCategory("TransactionScope")>
    Public Sub BeingCommitTransaction()

        database.Connection.BeginTransaction(IsolationLevel.Serializable)

        Using connection = New ConnectionScope(database)
            Dim insert As New SQLInsert With {.TableName = SimpleTable.Name}
            insert.Fields.Add("Field1", "Field1-2")
            connection.Execute(insert)
        End Using

        With table.Add
            .Field1 = "Field1-3"
            .Save()
        End With

        database.Connection.CommitTransaction()

        Using connection = New ConnectionScope(database)
            Assert.AreEqual(3, connection.ExecuteScalar(GetRowCount(SimpleTable.Name)))
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("Connection"), TestCategory("TransactionScope")>
    Public Sub BeingRollbackTransaction()

        database.Connection.BeginTransaction(IsolationLevel.Serializable)

        Using connection = New ConnectionScope(database)
            Dim insert As New SQLInsert With {.TableName = SimpleTable.Name}
            insert.Fields.Add("Field1", "Field1-2")
            connection.Execute(insert)
        End Using

        With table.Add
            .Field1 = "Field1-3"
            .Save()
        End With

        database.Connection.RollbackTransaction()

        Using connection = New ConnectionScope(database)
            Assert.AreEqual(1, connection.ExecuteScalar(GetRowCount(SimpleTable.Name)))
        End Using

    End Sub

    Private Shared Function GetRowCount(tableName As String) As SQLSelect

        Dim rowsCount = New SQLSelect(tableName)
        rowsCount.Fields.Add("", AggregateFunction.Count)
        Return rowsCount

    End Function

End Class
