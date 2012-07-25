Imports System.Text
Imports System.Configuration
Imports DatabaseObjects.SQL
Imports DatabaseObjects.Exceptions

''' <summary>
''' Tests create, read, update, delete and sorting.
''' Uses the SimpleDatabaseObjectsListKeyed because this incorporates all of the 
''' core functionality that is inherited / used by the other collection classes.
''' </summary>
''' <remarks></remarks>
<TestClass()>
Public Class SimpleDatabaseObjectsListKeyedTests

    Public Property TestContext As TestContext

    Private Shared _database As Database
    Private Shared _rootContainer As TestRootContainer
    Private Shared _collection As Generic.SimpleDatabaseObjectsListKeyed

    <ClassInitialize()>
    Public Shared Sub ClassInitialize(context As TestContext)

        _database = MicrosoftSQLServerDatabase.Parse(ConfigurationManager.ConnectionStrings("SQLServerTestDatabase").ConnectionString)
        _rootContainer = New TestRootContainer(_database)
        _collection = New Generic.SimpleDatabaseObjectsListKeyed(_database)

    End Sub

    <TestInitialize()>
    Public Sub TestInitialize()

        AddHandler _database.Connection.StatementExecuted, _
            Sub(statement As ISQLStatement)
                TestContext.WriteLine(statement.SQL)
            End Sub

		_database.RecreateTable(Generic.SimpleDatabaseObjectsListKeyed.TableSchema())

        Dim newItem = _collection.Add
        newItem.Field1 = "Field1-CCC"
        newItem.KeyField = "Key1"
        newItem.Save()

        Dim newItem2 = _collection.Add
        newItem2.Field1 = "Field1-BBB"
        newItem2.KeyField = "Key2"
        newItem2.Save()

        Dim newItem3 = _collection.Add
        newItem3.Field1 = "Field1-AAA"
        newItem3.KeyField = "Key3"
        newItem3.Save()

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsListKeyedRead()

        Dim items = _collection.ToArray

        Assert.AreEqual("Field1-AAA", items(0).Field1)
        Assert.AreEqual("Key3", items(0).KeyField)

        Assert.AreEqual("Field1-BBB", items(1).Field1)
        Assert.AreEqual("Key2", items(1).KeyField)

        Assert.AreEqual("Field1-CCC", items(2).Field1)
        Assert.AreEqual("Key1", items(2).KeyField)

        Assert.AreEqual(3, items.Length)

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsListKeyedCreate()

        Dim newItem = _collection.Add
        newItem.Field1 = "Field1-DDD"
        newItem.KeyField = "Key4"
        newItem.Save()

        Assert.AreEqual(4, _collection.Count)

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsListKeyedUpdate()

        Dim existingItem = _collection("Key2")
        existingItem.Field1 = "Field1-BBB-Updated"
        existingItem.Save()

        Dim existingItemReloaded = _collection("Key2")
        Assert.AreEqual(3, _collection.Count)
        Assert.AreEqual("Field1-BBB-Updated", existingItemReloaded.Field1)
        Assert.AreEqual("Key2", existingItemReloaded.KeyField)

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsListKeyedExists()

        Assert.IsTrue(_collection.Exists("Key2"))
        Assert.IsFalse(_collection.Exists("KeyThatDoesNotExist"))

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsListKeyedDelete()

        Dim existingItem = _collection("Key2")
        _collection.Delete(existingItem)

        Dim itemsReloaded = _collection.ToArray

        Assert.AreEqual(2, itemsReloaded.Length)
        Assert.AreEqual("Field1-AAA", itemsReloaded(0).Field1)
        Assert.AreEqual("Field1-CCC", itemsReloaded(1).Field1)

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsListKeyedCount()

        Assert.AreEqual(3, _collection.Count)

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsListKeyedFirst()

        Assert.AreEqual("Key3", _collection.First.KeyField)

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsListKeyedLast()

        Assert.AreEqual("Key1", _collection.Last.KeyField)

    End Sub

End Class
