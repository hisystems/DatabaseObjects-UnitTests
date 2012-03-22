Imports System.Text
Imports System.Configuration
Imports DatabaseObjects.SQL
Imports DatabaseObjects.Exceptions

<TestClass()>
Public Class DatabaseObjectLockControllerTests

    Public Property TestContext As TestContext

    Private Shared database As Database
    Private Shared table As SimpleTable

    Private user1LockController As DatabaseObjectLockController
    Private user2LockController As DatabaseObjectLockController

    Private Const LockTableName As String = "LockTable"

    <ClassInitialize()>
    Public Shared Sub ClassInitialize(context As TestContext)

        database = MicrosoftSQLServerDatabase.Parse(ConfigurationManager.ConnectionStrings("SQLServerTestDatabase").ConnectionString)
        table = New SimpleTable(database)

    End Sub

    <TestInitialize()>
    Public Sub InitializeTable1WithAutoIncrementPrimaryKeyAndField1()

        AddHandler database.Connection.StatementExecuted, _
            Sub(statement As ISQLStatement)
                TestContext.WriteLine(statement.SQL)
            End Sub

        Using connection = New ConnectionScope(database)

            If connection.Execute(New SQLTableExists(LockTableName)).Read Then
                connection.Execute(New SQLDropTable(LockTableName))
            End If

            If connection.Execute(New SQLTableExists(SimpleTable.Name)).Read Then
                connection.Execute(New SQLDropTable(SimpleTable.Name))
            End If

            connection.Execute(SimpleTable.TableSchema)

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

        End Using

        user1LockController = New DatabaseObjectLockController(database, LockTableName, "UserID")
        user2LockController = New DatabaseObjectLockController(database, "LockTable", "OtherUserID")

    End Sub

    <TestMethod()>
    <TestCategory("LockController")>
    Public Sub LockControllerLock()

        Dim firstItem = table("Field1-1")

        user1LockController.Lock(table, firstItem)

        Assert.IsTrue(user1LockController.IsLocked(table, firstItem))
        Assert.IsTrue(user1LockController.IsLockedByCurrentUser(table, firstItem))
        Assert.IsTrue(user1LockController.LockedByUserID(table, firstItem) = "UserID")

    End Sub

    <TestMethod()>
    <TestCategory("LockController")>
    Public Sub LockControllerLockByAnotherUser()

        Dim firstItem = table("Field1-1")
        user2LockController.Lock(table, firstItem)

        Assert.IsTrue(user1LockController.IsLocked(table, firstItem))
        Assert.IsFalse(user1LockController.IsLockedByCurrentUser(table, firstItem))
        Assert.IsTrue(user1LockController.LockedByUserID(table, firstItem) = "OtherUserID")

    End Sub

    <TestMethod()>
    <TestCategory("LockController")>
    <ExpectedException(GetType(Exceptions.ObjectAlreadyLockedException))>
    Public Sub LockControllerLockWhenAlreadyLockedByAnotherUser()

        Dim firstItem = table("Field1-1")

        user1LockController.Lock(table, firstItem)
        user2LockController.Lock(table, firstItem)  ' Should fail

    End Sub

    <TestMethod()>
    <TestCategory("LockController")>
    <ExpectedException(GetType(Exceptions.DatabaseObjectsException))>
    Public Sub LockControllerLockUnsavedObject()

        Dim firstItem = table.Add

        user1LockController.Lock(table, firstItem)

    End Sub

    <TestMethod()>
    <TestCategory("LockController")>
    Public Sub LockControllerUnlock()

        Dim firstItem = table("Field1-1")

        user1LockController.Lock(table, firstItem)
        user1LockController.UnLock(table, firstItem)

        Assert.IsFalse(user1LockController.IsLocked(table, firstItem))
        Assert.IsFalse(user1LockController.IsLockedByCurrentUser(table, firstItem))

    End Sub

    <TestMethod()>
    <TestCategory("LockController")>
    Public Sub LockControllerUnlockAll()

        Dim firstItem = table("Field1-1")
        Dim secondItem = table("Field1-2")

        user1LockController.Lock(table, firstItem)
        user1LockController.Lock(table, secondItem)

        Assert.IsTrue(user1LockController.IsLocked(table, firstItem))
        Assert.IsTrue(user1LockController.IsLocked(table, secondItem))

        user1LockController.UnlockAll()

        Assert.IsFalse(user1LockController.IsLocked(table, firstItem))
        Assert.IsFalse(user1LockController.IsLocked(table, secondItem))

    End Sub

End Class
