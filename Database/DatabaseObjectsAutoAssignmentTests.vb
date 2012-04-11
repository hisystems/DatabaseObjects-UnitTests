Imports System.Text
Imports System.Configuration
Imports DatabaseObjects.SQL
Imports DatabaseObjects.Exceptions

<TestClass()>
Public Class DatabaseObjectsAutoAssignmentTests

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

    <Table(CollectionWithAutoIncrementDistinctField.Name)>
    <DistinctField("PrimaryField", FieldValueAutoAssignmentType.AutoIncrement)>
    Public Class CollectionWithAutoIncrementDistinctField
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsList(Of Item)

        Public Const Name As String = "CollectionWithAutoIncrementDistinctField"

        Friend Sub New(database As Database)

            MyBase.New(database)

        End Sub

        Friend Shared Function TableSchema() As SQLCreateTable

            Dim createTable As New SQLCreateTable

            createTable.Name = Name

            With createTable.Fields.Add("PrimaryField", DataType.Integer)
                .KeyType = KeyType.Primary
                .AutoIncrements = True
            End With

            createTable.Fields.Add("Field1", DataType.VariableCharacter, 100)

            Return createTable

        End Function

    End Class

    <Table(CollectionWithAutoIncrementDistinctFieldUsingOverrides.Name)>
    Public Class CollectionWithAutoIncrementDistinctFieldUsingOverrides
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsList(Of Item)

        Public Const Name As String = "CollectionWithAutoIncrementDistinctFieldUsingOverrides"

        Friend Sub New(database As Database)

            MyBase.New(database)

        End Sub

        Protected Overrides Function DistinctFieldName() As String

            Return "PrimaryField"

        End Function

        Protected Overrides Function DistinctFieldAutoIncrements() As Boolean

            Return True

        End Function

        Friend Shared Function TableSchema() As SQLCreateTable

            Dim createTable As New SQLCreateTable

            createTable.Name = Name

            With createTable.Fields.Add("PrimaryField", DataType.Integer)
                .KeyType = KeyType.Primary
                .AutoIncrements = True
            End With

            createTable.Fields.Add("Field1", DataType.VariableCharacter, 100)

            Return createTable

        End Function

    End Class

    <Table(CollectionWithNoAutoAssignmentField.Name)>
    <DistinctField("PrimaryField", FieldValueAutoAssignmentType.None)>
    Public Class CollectionWithNoAutoAssignmentField
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsList(Of Item)

        Public Const Name As String = "CollectionWithNoAutoAssignmentField"

        Friend Sub New(database As Database)

            MyBase.New(database)

        End Sub

        Friend Shared Function TableSchema() As SQLCreateTable

            Dim createTable As New SQLCreateTable

            createTable.Name = Name

            createTable.Fields.Add("PrimaryField", DataType.Integer)
            createTable.Fields.Add("Field1", DataType.VariableCharacter, 100)

            Return createTable

        End Function

    End Class

    <Table(CollectionWithUniqueIdentifierAutoAssignmentField.Name)>
    <DistinctField("PrimaryField", FieldValueAutoAssignmentType.NewUniqueIdentifier)>
    Public Class CollectionWithUniqueIdentifierAutoAssignmentField
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsList(Of Item)

        Public Const Name As String = "CollectionWithUniqueIdentifierAutoAssignmentField"

        Friend Sub New(database As Database)

            MyBase.New(database)

        End Sub

        Friend Shared Function TableSchema() As SQLCreateTable

            Dim createTable As New SQLCreateTable

            createTable.Name = Name

            createTable.Fields.Add("PrimaryField", DataType.UniqueIdentifier)
            createTable.Fields.Add("Field1", DataType.VariableCharacter, 100)

            Return createTable

        End Function

    End Class

    Public Class Item
        Inherits DatabaseObject

        <FieldMapping("Field1")>
        Public Field1 As String

        Friend Sub New(parent As DatabaseObjects)

            MyBase.New(parent)

        End Sub

        Public Shadows Sub Save()

            MyBase.Save()

        End Sub

    End Class

    <TestInitialize()>
    Public Sub TestInitialize()

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub AutoIncrementField()

        EnsureTableExists(CollectionWithAutoIncrementDistinctField.TableSchema)

        Dim collection As New CollectionWithAutoIncrementDistinctField(_database)
        Dim item = collection.Add
        item.Save()

        Assert.AreEqual(1D, DirectCast(item, IDatabaseObject).DistinctValue)

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub AutoIncrementFieldUsingOverride()

        EnsureTableExists(CollectionWithAutoIncrementDistinctFieldUsingOverrides.TableSchema)

        Dim collection As New CollectionWithAutoIncrementDistinctFieldUsingOverrides(_database)
        Dim item = collection.Add
        item.Save()

        Assert.AreEqual(1D, DirectCast(item, IDatabaseObject).DistinctValue)

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub NoAutoAssignmentField()

        EnsureTableExists(CollectionWithNoAutoAssignmentField.TableSchema)

        Dim collection As New CollectionWithNoAutoAssignmentField(_database)
        Dim item = collection.Add
        item.Save()

        Assert.AreEqual(Nothing, DirectCast(item, IDatabaseObject).DistinctValue)

    End Sub

    <TestMethod()>
    <TestCategory("DatabaseObjectsCollections")>
    Public Sub UniqueIdentifierAutoAssignmentField()

        EnsureTableExists(CollectionWithUniqueIdentifierAutoAssignmentField.TableSchema)

        Dim collection As New CollectionWithUniqueIdentifierAutoAssignmentField(_database)
        Dim item = collection.Add
        item.Save()

        Assert.AreNotEqual(Guid.Empty, DirectCast(item, IDatabaseObject).DistinctValue)

    End Sub

End Class
