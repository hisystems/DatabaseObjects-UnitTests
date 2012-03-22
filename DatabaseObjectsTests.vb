Imports System.Text
Imports System.Configuration
Imports DatabaseObjects.SQL
Imports DatabaseObjects.Exceptions

<TestClass()>
Public Class DatabaseObjectsTests

    Public Property TestContext As TestContext

    Private Shared _database As Database
    Private Shared _rootContainer As TestRootContainer

    <ClassInitialize()>
    Public Shared Sub ClassInitialize(context As TestContext)

        _database = MicrosoftSQLServerDatabase.Parse(ConfigurationManager.ConnectionStrings("SQLServerTestDatabase").ConnectionString)
        _rootContainer = New TestRootContainer(_database)

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

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsInitializeWithRootContainer()

        Dim collection As New SimpleDatabaseObjects(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsInitializeWithDatabase()

        Dim collection As New SimpleDatabaseObjects(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsEnumerableInitializeWithRootContainer()

        Dim collection As New SimpleDatabaseObjectsEnumerable(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsEnumerableInitializeWithDatabase()

        Dim collection As New SimpleDatabaseObjectsEnumerable(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsUsingAttributesInitializeWithRootContainer()

        Dim collection As New SimpleDatabaseObjectsUsingAttributes(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsUsingAttributesInitializeWithDatabase()

        Dim collection As New SimpleDatabaseObjectsUsingAttributes(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsVolatileInitializeWithRootContainer()

        EnsureTableExists(SimpleDatabaseObjectsVolatile.TableSchema)

        Dim collection As New SimpleDatabaseObjectsVolatile(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsVolatileInitializeWithDatabase()

        EnsureTableExists(SimpleDatabaseObjectsVolatile.TableSchema)

        Dim collection As New SimpleDatabaseObjectsVolatile(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsVolatileUsingAttributesInitializeWithRootContainer()

        EnsureTableExists(SimpleDatabaseObjectsVolatileUsingAttributes.TableSchema)

        Dim collection As New SimpleDatabaseObjectsVolatileUsingAttributes(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub SimpleDatabaseObjectsVolatileUsingAttributesInitializeWithDatabase()

        EnsureTableExists(SimpleDatabaseObjectsVolatileUsingAttributes.TableSchema)

        Dim collection As New SimpleDatabaseObjectsVolatileUsingAttributes(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsInitializeWithRootContainer()

        Dim collection As New Generic.SimpleDatabaseObjects(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsInitializeWithDatabase()

        Dim collection As New Generic.SimpleDatabaseObjects(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsEnumerableInitializeWithRootContainer()

        Dim collection As New Generic.SimpleDatabaseObjectsEnumerable(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsEnumerableInitializeWithDatabase()

        Dim collection As New Generic.SimpleDatabaseObjectsEnumerable(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsUsingAttributesInitializeWithRootContainer()

        Dim collection As New Generic.SimpleDatabaseObjectsUsingAttributes(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsUsingAttributesInitializeWithDatabase()

        Dim collection As New Generic.SimpleDatabaseObjectsUsingAttributes(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsVolatileInitializeWithRootContainer()

        EnsureTableExists(Generic.SimpleDatabaseObjectsVolatile.TableSchema)

        Dim collection As New Generic.SimpleDatabaseObjectsVolatile(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsVolatileInitializeWithDatabase()

        EnsureTableExists(Generic.SimpleDatabaseObjectsVolatile.TableSchema)

        Dim collection As New Generic.SimpleDatabaseObjectsVolatile(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsVolatileUsingAttributesInitializeWithRootContainer()

        EnsureTableExists(Generic.SimpleDatabaseObjectsVolatileUsingAttributes.TableSchema)

        Dim collection As New Generic.SimpleDatabaseObjectsVolatileUsingAttributes(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsVolatileUsingAttributesInitializeWithDatabase()

        EnsureTableExists(Generic.SimpleDatabaseObjectsVolatileUsingAttributes.TableSchema)

        Dim collection As New Generic.SimpleDatabaseObjectsVolatileUsingAttributes(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsEnumerableUsingAttributesInitializeWithRootContainer()

        Dim collection As New Generic.SimpleDatabaseObjectsEnumerableUsingAttributes(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsEnumerableUsingAttributesInitializeWithDatabase()

        Dim collection As New Generic.SimpleDatabaseObjectsEnumerableUsingAttributes(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsListInitializeWithRootContainer()

        Dim collection As New Generic.SimpleDatabaseObjectsList(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsListInitializeWithDatabase()

        Dim collection As New Generic.SimpleDatabaseObjectsList(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsListKeyedInitializeWithRootContainer()

        Dim collection As New Generic.SimpleDatabaseObjectsListKeyed(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsListKeyedInitializeWithDatabase()

        Dim collection As New Generic.SimpleDatabaseObjectsListKeyed(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsListKeyedUsingAttributesInitializeWithRootContainer()

        Dim collection As New Generic.SimpleDatabaseObjectsListKeyedUsingAttributes(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsListKeyedUsingAttributesInitializeWithDatabase()

        Dim collection As New Generic.SimpleDatabaseObjectsListKeyedUsingAttributes(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsListUsingAttributesInitializeWithRootContainer()

        Dim collection As New Generic.SimpleDatabaseObjectsListUsingAttributes(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsListUsingAttributesInitializeWithDatabase()

        Dim collection As New Generic.SimpleDatabaseObjectsListUsingAttributes(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsMultipleSubclassInitializeWithRootContainer()

        Dim collection As New Generic.SimpleDatabaseObjectsMultipleSubclass(_rootContainer)
        'The contents of the fields are irrelevant for testing, so pass no values
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjectsMultipleSubclass).ItemInstanceForSubclass(New SQLFieldValues())

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsMultipleSubclassInitializeWithDatabase()

        Dim collection As New Generic.SimpleDatabaseObjectsMultipleSubclass(_database)
        'The contents of the fields are irrelevant for testing, so pass no values
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjectsMultipleSubclass).ItemInstanceForSubclass(New SQLFieldValues())

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsMultipleSubclassUsingAttributesInitializeWithRootContainer()

        Dim collection As New Generic.SimpleDatabaseObjectsMultipleSubclassUsingAttributes(_rootContainer)
        'The contents of the fields are irrelevant for testing, so pass no values
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjectsMultipleSubclass).ItemInstanceForSubclass(New SQLFieldValues())

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsMultipleSubclassUsingAttributesInitializeWithDatabase()

        Dim collection As New Generic.SimpleDatabaseObjectsMultipleSubclassUsingAttributes(_database)
        'The contents of the fields are irrelevant for testing, so pass no values
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjectsMultipleSubclass).ItemInstanceForSubclass(New SQLFieldValues())

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsVolatileListInitializeWithRootContainer()

        EnsureTableExists(Generic.SimpleDatabaseObjectsVolatileList.TableSchema)

        Dim collection As New Generic.SimpleDatabaseObjectsVolatileList(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsVolatileListInitializeWithDatabase()

        EnsureTableExists(Generic.SimpleDatabaseObjectsVolatileList.TableSchema)

        Dim collection As New Generic.SimpleDatabaseObjectsVolatileList(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsVolatileListUsingAttributesInitializeWithRootContainer()

        EnsureTableExists(Generic.SimpleDatabaseObjectsVolatileListUsingAttributes.TableSchema)

        Dim collection As New Generic.SimpleDatabaseObjectsVolatileListUsingAttributes(_rootContainer)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    End Sub

    <TestMethod()>
    <TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    Public Sub Generic_SimpleDatabaseObjectsVolatileListUsingAttributesInitializeWithDatabase()

        EnsureTableExists(Generic.SimpleDatabaseObjectsVolatileListUsingAttributes.TableSchema)

        Dim collection As New Generic.SimpleDatabaseObjectsVolatileListUsingAttributes(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

        Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
        Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    End Sub

    '<TestMethod()>
    '<TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    'Public Sub Generic_SimpleDatabaseObjectsXXXXInitializeWithRootContainer()

    '    Dim collection As New Generic.SimpleDatabaseObjectsXXXX(_rootContainer)
    '    Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

    '    Assert.AreSame(_rootContainer, objNewItem.RootContainer(Of TestRootContainer)())
    '    Assert.AreSame(_rootContainer, collection.RootContainer(Of TestRootContainer)())

    'End Sub

    '<TestMethod()>
    '<TestCategory("RootContainer"), TestCategory("DatabaseObjectsCollections")>
    'Public Sub Generic_SimpleDatabaseObjectsXXXXInitializeWithDatabase()

    '    Dim collection As New Generic.SimpleDatabaseObjectsXXXX(_database)
    '    Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

    '    Assert.AreSame(Nothing, objNewItem.RootContainer(Of TestRootContainer)())
    '    Assert.AreSame(Nothing, collection.RootContainer(Of TestRootContainer)())

    'End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    <ExpectedException(GetType(NotSupportedException))>
    Public Sub SimpleDatabaseObjectsMultipleSubClassUsingAttributesItemInstanceFailure()

        Dim collection As New Generic.SimpleDatabaseObjectsMultipleSubclassUsingAttributes(_database)
        Dim objNewItem As DatabaseObject = DirectCast(collection, IDatabaseObjects).ItemInstance

    End Sub

End Class
