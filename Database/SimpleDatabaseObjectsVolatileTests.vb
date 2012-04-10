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
Public Class SimpleDatabaseObjectsVolatileTests

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

    Private Sub EnsureTableExists(createTable As SQLCreateTable)

        Using connection = New ConnectionScope(_database)
            If connection.Execute(New SQLTableExists(createTable.Name)).Read Then
                connection.Execute(New SQLDropTable(createTable.Name))
            End If

            connection.Execute(createTable)
        End Using

    End Sub

    <TestInitialize()>
    Public Sub TestInitialize()

        AddHandler _database.Connection.StatementExecuted, _
            Sub(statement As ISQLStatement)
                TestContext.WriteLine(statement.SQL)
            End Sub

        EnsureTableExists(Generic.SimpleDatabaseObjectsListKeyed.TableSchema())
        EnsureTableExists(Generic.SimpleDatabaseObjectsVolatile.TableSchema())

        Dim newItem1 = _collection.Add
        newItem1.Field1 = "Field1-BBB"
        newItem1.KeyField = "Key1"
        Dim newItem1Child1 = newItem1.ChildObjects.Add
        newItem1Child1.Field1 = "BBB-Child1"
        Dim newItem1Child2 = newItem1.ChildObjects.Add
        newItem1Child2.Field1 = "BBB-Child2"
        newItem1.Save()

        Dim newItem2 = _collection.Add
        newItem2.Field1 = "Field1-AAA"
        newItem2.KeyField = "Key2"
        Dim newItem2Child1 = newItem2.ChildObjects.Add
        newItem2Child1.Field1 = "AAA-Child1"
        newItem2.Save()

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsVolatileRead()

        Dim items = _collection.ToArray

        Assert.AreEqual("Field1-AAA", items(0).Field1)
        Assert.AreEqual("Key2", items(0).KeyField)
        Assert.AreEqual(1, items(0).ChildObjects.Count)
        Assert.AreEqual("AAA-Child1", items(0).ChildObjects(0).Field1)

        Assert.AreEqual("Field1-BBB", items(1).Field1)
        Assert.AreEqual("Key1", items(1).KeyField)
        Assert.AreEqual(2, items(1).ChildObjects.Count)
        Assert.AreEqual("BBB-Child1", items(1).ChildObjects(0).Field1)
        Assert.AreEqual("BBB-Child2", items(1).ChildObjects(1).Field1)

        Assert.AreEqual(2, items.Length)

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsVolatileCreate()

        Dim newItem = _collection.Add
        newItem.Field1 = "Field1-CCC"
        newItem.KeyField = "Key4"
        Dim newItemChild1 = newItem.ChildObjects.Add
        newItemChild1.Field1 = "CCC-Field1"
        newItem.Save()

        Assert.AreEqual(3, _collection.Count)
        Assert.AreEqual(1, _collection("Key4").ChildObjects.Count)

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsVolatileUpdate()

        Dim existingItem = _collection("Key2")
        Dim newItemChild = existingItem.ChildObjects.Add
        newItemChild.Field1 = "AAA-Child2"
        existingItem.Save()

        Dim existingItemReloaded = _collection("Key2")
        Assert.AreEqual(2, _collection("Key2").ChildObjects.Count)
        Assert.AreEqual("AAA-Child1", _collection("Key2").ChildObjects(0).Field1)
        Assert.AreEqual("AAA-Child2", _collection("Key2").ChildObjects(1).Field1)

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsVolatileDelete()

        Dim existingItem = _collection("Key2")

        Assert.AreEqual(1, existingItem.ChildObjects.Count)

        Dim itemToDelete = _collection("Key2").ChildObjects(0)
        existingItem.ChildObjects.Delete(itemToDelete)

        Assert.AreEqual(0, existingItem.ChildObjects.Count)
        'From the database - there is still one record because the parent object has not been saved
        Assert.AreEqual(1, _collection("Key2").ChildObjects.Count)

        existingItem.Save()

        Assert.AreEqual(0, _collection("Key2").ChildObjects.Count)

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsVolatileCount()

        Assert.AreEqual(2, _collection("Key1").ChildObjects.Count)

    End Sub

End Class
