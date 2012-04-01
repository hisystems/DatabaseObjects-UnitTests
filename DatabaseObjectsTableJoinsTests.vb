Imports System.Text
Imports System.Configuration
Imports DatabaseObjects.SQL
Imports DatabaseObjects.Exceptions

<TestClass()>
Public Class DatabaseObjectsTableJoinsTests

    Private Class TestRootContainer
        Inherits RootContainer

        Public TestCollection As TestCollection
        Public TestCollectionWithTwoTableJoins As TestCollectionWithTwoTableJoins
        Public TestReferenceCollection As TestReferenceCollection
        Public TestReferenceCollection2 As TestReferenceCollection2

        Public Sub New(database As Database)

            MyBase.New(database)

            TestCollection = New TestCollection(Me)
            TestCollectionWithTwoTableJoins = New TestCollectionWithTwoTableJoins(Me)
            TestReferenceCollection = New TestReferenceCollection(Me)
            TestReferenceCollection2 = New TestReferenceCollection2(Me)

        End Sub

    End Class

    <Table(TestCollection.Name)>
    <DistinctField("PrimaryField", FieldValueAutoAssignmentType.AutoIncrement)>
    <OrderByField("PrimaryField")>
    <TableJoin("ReferenceID", TestReferenceCollection.Name, "ReferenceID")>
    Private Class TestCollection
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsListUsingAttributes(Of TestItem)

        Public Const Name As String = "TestCollection"

        Friend Sub New(rootContainer As RootContainer)

            MyBase.New(rootContainer)

        End Sub

        Friend Shared Function TableSchema() As SQLCreateTable

            Dim createTable As New SQLCreateTable

            createTable.Name = Name

            With createTable.Fields.Add("PrimaryField", DataType.Integer)
                .KeyType = KeyType.Primary
                .AutoIncrements = True
            End With

            createTable.Fields.Add("ReferenceID", DataType.Integer)
            createTable.Fields.Add("ReferenceID2", DataType.Integer)    ' Only used by TestCollectionWithTwoTableJoins

            Return createTable

        End Function

    End Class

    ''' <summary>
    ''' Inherits the base table joins and also adds another table join
    ''' </summary>
    ''' <remarks></remarks>
    <TableJoin("ReferenceID2", TestReferenceCollection2.Name, "ReferenceID")>
    Private Class TestCollectionWithTwoTableJoins
        Inherits TestCollection

        Friend Sub New(rootContainer As RootContainer)

            MyBase.New(rootContainer)

        End Sub

    End Class

    ''' <summary>
    ''' Works with the TestCollection and TestCollectionWithTwoTableJoins classes.
    ''' If there is a second table join then this is used to load the ReferenceObject2.
    ''' </summary>
    Private Class TestItem
        Inherits DatabaseObject

        Public ReferenceObject As TestReferenceItem

        ''' <summary>
        ''' Object is optional (and is only set when the table join exists - i.e. used with the TestCollectionWithTwoTableJoins).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReferenceObject2 As TestReferenceItem2

        Friend Sub New(parent As TestCollection)

            MyBase.New(parent)

        End Sub

        Public Shadows Sub Save()

            MyBase.Save()

        End Sub

        Protected Overrides Sub LoadFields(fieldValues As SQL.SQLFieldValues)

            ReferenceObject = New TestReferenceItem(MyBase.RootContainer(Of TestRootContainer).TestReferenceCollection)
            Database.ObjectLoad(ReferenceObject, fieldValues)

            If fieldValues.Exists("ReferenceName2") Then
                ReferenceObject2 = New TestReferenceItem2(MyBase.RootContainer(Of TestRootContainer).TestReferenceCollection2)
                Database.ObjectLoad(ReferenceObject2, fieldValues)
            End If

        End Sub

        Protected Overrides Function SaveFields() As SQL.SQLFieldValues

            Dim values As New SQL.SQLFieldValues

            values.Add("ReferenceID", DirectCast(ReferenceObject, IDatabaseObject).DistinctValue)

            If ReferenceObject2 Is Nothing Then
                values.Add("ReferenceID2", Nothing)
            Else
                values.Add("ReferenceID2", DirectCast(ReferenceObject, IDatabaseObject).DistinctValue)
            End If

            Return values

        End Function

    End Class

    <Table(TestReferenceCollection.Name)>
    <DistinctField("ReferenceID", FieldValueAutoAssignmentType.AutoIncrement)>
    Private Class TestReferenceCollection
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsListUsingAttributes(Of TestReferenceItem)

        Public Const Name As String = "TestReferenceCollection"

        Friend Sub New(rootContainer As RootContainer)

            MyBase.New(rootContainer)

        End Sub

        Friend Shared Function TableSchema() As SQLCreateTable

            Dim createTable As New SQLCreateTable

            createTable.Name = Name

            With createTable.Fields.Add("ReferenceID", DataType.Integer)
                .KeyType = KeyType.Primary
                .AutoIncrements = True
            End With

            createTable.Fields.Add("ReferenceName", DataType.VariableCharacter, 100)

            Return createTable

        End Function

    End Class

    Private Class TestReferenceItem
        Inherits DatabaseObjectUsingAttributes

        <FieldMapping("ReferenceName")>
        Public Name As String

        Friend Sub New(parent As TestReferenceCollection)

            MyBase.New(parent)

        End Sub

        Public Shadows Sub Save()

            MyBase.Save()

        End Sub

    End Class

    <Table(TestReferenceCollection2.Name)>
    <DistinctField("ReferenceID", FieldValueAutoAssignmentType.AutoIncrement)>
    Private Class TestReferenceCollection2
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsListUsingAttributes(Of TestReferenceItem2)

        Public Const Name As String = "TestReferenceCollection2"

        Friend Sub New(rootContainer As RootContainer)

            MyBase.New(rootContainer)

        End Sub

        Friend Shared Function TableSchema() As SQLCreateTable

            Dim createTable As New SQLCreateTable

            createTable.Name = Name

            With createTable.Fields.Add("ReferenceID", DataType.Integer)
                .KeyType = KeyType.Primary
                .AutoIncrements = True
            End With

            createTable.Fields.Add("ReferenceName2", DataType.VariableCharacter, 100)

            Return createTable

        End Function

    End Class

    Private Class TestReferenceItem2
        Inherits DatabaseObjectUsingAttributes

        <FieldMapping("ReferenceName2")>
        Public Name As String

        Friend Sub New(parent As TestReferenceCollection2)

            MyBase.New(parent)

        End Sub

        Public Shadows Sub Save()

            MyBase.Save()

        End Sub

    End Class

    Public Property TestContext As TestContext

    Private Shared _database As Database
    Private Shared _rootContainer As TestRootContainer

    <ClassInitialize()>
    Public Shared Sub ClassInitialize(context As TestContext)

        _database = MicrosoftSQLServerDatabase.Parse(ConfigurationManager.ConnectionStrings("SQLServerTestDatabase").ConnectionString)
        _rootContainer = New TestRootContainer(_database)

    End Sub

    <TestInitialize()>
    Public Sub TestInitialize()

        AddHandler _database.Connection.StatementExecuted, _
            Sub(statement As ISQLStatement)
                TestContext.WriteLine(statement.SQL)
            End Sub

        EnsureTableExists(TestCollection.TableSchema)
        EnsureTableExists(TestReferenceCollection.TableSchema)
        EnsureTableExists(TestReferenceCollection2.TableSchema)

    End Sub

    Private Sub EnsureTableExists(createTable As SQLCreateTable)

        Using connection = New ConnectionScope(_database)
            If connection.Execute(New SQLTableExists(createTable.Name)).Read Then
                connection.Execute(New SQLDropTable(createTable.Name))
            End If

            connection.Execute(createTable)
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("TableJoins")>
    Public Sub SingleTableJoin()

        Dim testReferenceCollectionItem1 = _rootContainer.TestReferenceCollection.Add
        testReferenceCollectionItem1.Name = "ReferenceCollection_Item1"
        testReferenceCollectionItem1.Save()

        'Contains a reference to the first reference collection item (for testing single table joins)
        Dim newItem1 = _rootContainer.TestCollection.Add
        newItem1.ReferenceObject = testReferenceCollectionItem1
        newItem1.Save()

        Dim first = _rootContainer.TestCollection.First

        Assert.AreEqual("ReferenceCollection_Item1", first.ReferenceObject.Name)
        Assert.AreEqual(Nothing, first.ReferenceObject2)

    End Sub

    <TestMethod()>
    <TestCategory("TableJoins")>
    Public Sub MultipleTableJoins()

        Dim testReferenceCollectionItem1 = _rootContainer.TestReferenceCollection.Add
        testReferenceCollectionItem1.Name = "ReferenceCollection_Item1"
        testReferenceCollectionItem1.Save()

        Dim testReferenceCollection2Item1 = _rootContainer.TestReferenceCollection2.Add
        testReferenceCollection2Item1.Name = "ReferenceCollection2_Item1"
        testReferenceCollection2Item1.Save()

        'Contains a reference to the first reference collection item and the second reference collection item (for testing multiple table joins)
        Dim newItem1 = _rootContainer.TestCollection.Add
        newItem1.ReferenceObject = testReferenceCollectionItem1
        newItem1.ReferenceObject2 = testReferenceCollection2Item1
        newItem1.Save()

        Dim first = _rootContainer.TestCollectionWithTwoTableJoins.First

        Assert.AreEqual("ReferenceCollection_Item1", first.ReferenceObject.Name)
        Assert.AreEqual("ReferenceCollection2_Item1", first.ReferenceObject2.Name)

    End Sub

End Class
