Option Strict On
Option Explicit On
Option Infer On

Imports System.Text
Imports System.Configuration
Imports System.Linq
Imports DatabaseObjects.SQL
Imports DatabaseObjects.Exceptions

<TestClass()>
Public Class ObjectReferenceBindingTests

    Private Class ObjectReferenceRootContainer
        Inherits RootContainer

        Public ReadOnly ForeignObjects As ForeignCollection
        Public ReadOnly Foreign2Objects As Foreign2Collection

        Public Sub New(database As Database)

            MyBase.New(database)

            ForeignObjects = New ForeignCollection(Me)
            Foreign2Objects = New Foreign2Collection(Me)

        End Sub

    End Class

    <Table(MainCollection(Of IDatabaseObject).Name)>
    <DistinctField("PrimaryField", FieldValueAutoAssignmentType.AutoIncrement)>
    Private MustInherit Class MainCollection(Of T As IDatabaseObject)
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsListUsingAttributes(Of T)

        Public Const Name As String = "Main"

        Protected Sub New(container As RootContainer)

            MyBase.New(container)

        End Sub

        Friend Shared Function TableSchema() As SQLCreateTable

            Dim createTable As New SQLCreateTable
            createTable.Name = Name

            With createTable.Fields.Add("PrimaryField", DataType.Integer)
                .KeyType = KeyType.Primary
                .AutoIncrements = True
            End With

            createTable.Fields.Add("ForeignID", DataType.Integer)
            createTable.Fields.Add("Foreign2ID", DataType.Integer)

            Return createTable

        End Function

    End Class

    Private MustInherit Class MainItem
        Inherits DatabaseObjectUsingAttributes

        Friend Sub New(parent As DatabaseObjects)

            MyBase.New(parent)

        End Sub

        Public Shadows Sub Save()

            MyBase.Save()

        End Sub

    End Class

    Private Class MainCollectionUsingLateBinding
        Inherits MainCollection(Of MainItemUsingLateBinding)

        Friend Sub New(container As RootContainer)

            MyBase.New(container)

        End Sub

    End Class

    Private Class MainItemUsingLateBinding
        Inherits MainItem

        <FieldMapping("ForeignID")>
        Public ForeignObject As Global.DatabaseObjects.Generic.ObjectReference(Of ForeignItem)

        Friend Sub New(parent As MainCollectionUsingLateBinding)

            MyBase.New(parent)

            'Initialize the ObjectReference with the associated foreign objects collection from which the object will be sourced
            Me.ForeignObject = New Global.DatabaseObjects.Generic.ObjectReference(Of ForeignItem)(MyBase.RootContainer(Of ObjectReferenceRootContainer).ForeignObjects)

        End Sub

    End Class

    Private Class MainCollectionUsingEarlyBindingAndTwoForeignReferences
        Inherits MainCollection(Of MainItemUsingEarlyBindingAndTwoForeignReferences)

        Friend Sub New(container As RootContainer)

            MyBase.New(container)

        End Sub

    End Class

    Private Class MainItemUsingEarlyBindingAndTwoForeignReferences
        Inherits MainItem

        <FieldMapping("ForeignID")>
        <ObjectReferenceEarlyBinding()>
        Public ForeignObject As Global.DatabaseObjects.Generic.ObjectReference(Of ForeignItem)

        <FieldMapping("Foreign2ID")>
        <ObjectReferenceEarlyBinding()>
        Public Foreign2Object As Global.DatabaseObjects.Generic.ObjectReference(Of Foreign2Item)

        Friend Sub New(parent As MainCollectionUsingEarlyBindingAndTwoForeignReferences)

            MyBase.New(parent)

            'Initialize the ObjectReference with the associated foreign objects collection from which the object will be sourced
            Me.ForeignObject = New Global.DatabaseObjects.Generic.ObjectReference(Of ForeignItem)(MyBase.RootContainer(Of ObjectReferenceRootContainer).ForeignObjects)
            Me.Foreign2Object = New Global.DatabaseObjects.Generic.ObjectReference(Of Foreign2Item)(MyBase.RootContainer(Of ObjectReferenceRootContainer).Foreign2Objects)

        End Sub

    End Class

    Private Class MainCollectionUsingEarlyBindingTwoForeignReferencesWithInhertance
        Inherits MainCollection(Of MainItemUsingEarlyBindingTwoForeignReferencesWithInhertanceSuperClass)

        Friend Sub New(container As RootContainer)

            MyBase.New(container)

        End Sub

    End Class

    Private MustInherit Class MainItemUsingEarlyBindingTwoForeignReferencesWithInhertanceBaseClass
        Inherits MainItem

        <FieldMapping("ForeignID")>
        <ObjectReferenceEarlyBinding()>
        Public ForeignObject As Global.DatabaseObjects.Generic.ObjectReference(Of ForeignItem)

        Protected Sub New(parent As MainCollectionUsingEarlyBindingTwoForeignReferencesWithInhertance)

            MyBase.New(parent)

            'Initialize the ObjectReference with the associated foreign objects collection from which the object will be sourced
            Me.ForeignObject = New Global.DatabaseObjects.Generic.ObjectReference(Of ForeignItem)(MyBase.RootContainer(Of ObjectReferenceRootContainer).ForeignObjects)

        End Sub

    End Class

    Private Class MainItemUsingEarlyBindingTwoForeignReferencesWithInhertanceSuperClass
        Inherits MainItemUsingEarlyBindingTwoForeignReferencesWithInhertanceBaseClass

        <FieldMapping("Foreign2ID")>
        <ObjectReferenceEarlyBinding()>
        Public Foreign2Object As Global.DatabaseObjects.Generic.ObjectReference(Of Foreign2Item)

        Friend Sub New(parent As MainCollectionUsingEarlyBindingTwoForeignReferencesWithInhertance)

            MyBase.New(parent)

            Me.Foreign2Object = New Global.DatabaseObjects.Generic.ObjectReference(Of Foreign2Item)(MyBase.RootContainer(Of ObjectReferenceRootContainer).Foreign2Objects)

        End Sub

    End Class

    Private Class MainCollectionUsingEarlyBinding
        Inherits MainCollection(Of MainItemUsingEarlyBinding)

        Friend Sub New(container As RootContainer)

            MyBase.New(container)

        End Sub

    End Class

    Private Class MainItemUsingEarlyBinding
        Inherits MainItem

        <FieldMapping("ForeignID")>
        <ObjectReferenceEarlyBinding()>
        Public ForeignObject As Global.DatabaseObjects.Generic.ObjectReference(Of ForeignItem)

        Friend Sub New(parent As MainCollectionUsingEarlyBinding)

            MyBase.New(parent)

            'Initialize the ObjectReference with the associated foreign objects collection from which the object will be sourced
            Me.ForeignObject = New Global.DatabaseObjects.Generic.ObjectReference(Of ForeignItem)(MyBase.RootContainer(Of ObjectReferenceRootContainer).ForeignObjects)

        End Sub

    End Class

    ''' <summary>
    ''' The MainItemUsingEarlyBindingWithNonGenericObjectReference is invalid because ObjectReferenceEarlyBinding
    ''' is not being used with class Global.DatabaseObjects.Generic.ObjectReference.
    ''' </summary>
    Private Class MainCollectionUsingEarlyBindingWithNonGenericObjectReference
        Inherits MainCollection(Of MainItemUsingEarlyBindingWithNonGenericObjectReference)

        Friend Sub New(container As RootContainer)

            MyBase.New(container)

        End Sub

    End Class

    Private Class MainItemUsingEarlyBindingWithNonGenericObjectReference
        Inherits MainItem

        <FieldMapping("ForeignID")>
        <ObjectReferenceEarlyBinding()>
        Public ForeignObject As Global.DatabaseObjects.ObjectReference

        Friend Sub New(parent As MainCollectionUsingEarlyBinding)

            MyBase.New(parent)

            'Initialize the ObjectReference with the associated foreign objects collection from which the object will be sourced
            Me.ForeignObject = New Global.DatabaseObjects.Generic.ObjectReference(Of ForeignItem)(MyBase.RootContainer(Of ObjectReferenceRootContainer).ForeignObjects)

        End Sub

    End Class

    ''' <summary>
    ''' The MainItemUsingUsingEarlyBindingWithNoFieldMapping is invalid because there is no FieldMapping on
    ''' on the field.
    ''' </summary>
    Private Class MainCollectionUsingEarlyBindingWithNoFieldMapping
        Inherits MainCollection(Of MainItemUsingUsingEarlyBindingWithNoFieldMapping)

        Friend Sub New(container As RootContainer)

            MyBase.New(container)

        End Sub

    End Class

    Private Class MainItemUsingUsingEarlyBindingWithNoFieldMapping
        Inherits MainItem

        <ObjectReferenceEarlyBinding()>
        Public ForeignObject As Global.DatabaseObjects.ObjectReference

        Friend Sub New(parent As MainCollectionUsingEarlyBinding)

            MyBase.New(parent)

            'Initialize the ObjectReference with the associated foreign objects collection from which the object will be sourced
            Me.ForeignObject = New Global.DatabaseObjects.Generic.ObjectReference(Of ForeignItem)(MyBase.RootContainer(Of ObjectReferenceRootContainer).ForeignObjects)

        End Sub

    End Class

    Private Class MainItemUsingEarlyBindingAndAttribtues
        Inherits MainItem

        <FieldMapping("ForeignID")>
        <ObjectReferenceEarlyBinding()>
        Public ForeignObject As Global.DatabaseObjects.Generic.ObjectReference(Of ForeignItem)

        Friend Sub New(parent As MainCollectionUsingEarlyBinding)

            MyBase.New(parent)

            'Initialize the ObjectReference with the associated foreign objects collection from which the object will be sourced
            Me.ForeignObject = New Global.DatabaseObjects.Generic.ObjectReference(Of ForeignItem)(MyBase.RootContainer(Of ObjectReferenceRootContainer).ForeignObjects)

        End Sub

    End Class

    <Table(ForeignCollection.Name)>
    <DistinctField("ForeignID", FieldValueAutoAssignmentType.AutoIncrement)>
    Private Class ForeignCollection
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsListUsingAttributes(Of ForeignItem)

        Friend Const Name As String = "ForeignTable"

        Friend Sub New(container As RootContainer)

            MyBase.New(container)

        End Sub

        Friend Shared Function TableSchema() As SQLCreateTable

            Dim createTable As New SQLCreateTable

            createTable.Name = Name

            With createTable.Fields.Add("ForeignID", DataType.Integer)
                .KeyType = KeyType.Primary
                .AutoIncrements = True
            End With

            createTable.Fields.Add("Name", DataType.VariableCharacter, 50)

            Return createTable

        End Function

    End Class

    Private Class ForeignItem
        Inherits DatabaseObjectUsingAttributes

        <FieldMapping("Name")>
        Public Name As String

        Friend Sub New(parent As ForeignCollection)

            MyBase.New(parent)

        End Sub

        Public Shadows Sub Save()

            MyBase.Save()

        End Sub

    End Class

    <Table(Foreign2Collection.Name)>
    <DistinctField("Foreign2ID", FieldValueAutoAssignmentType.AutoIncrement)>
    Private Class Foreign2Collection
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsListUsingAttributes(Of Foreign2Item)

        Friend Const Name As String = "Foreign2Table"

        Friend Sub New(container As RootContainer)

            MyBase.New(container)

        End Sub

        Friend Shared Function TableSchema() As SQLCreateTable

            Dim createTable As New SQLCreateTable

            createTable.Name = Name

            With createTable.Fields.Add("Foreign2ID", DataType.Integer)
                .KeyType = KeyType.Primary
                .AutoIncrements = True
            End With

            createTable.Fields.Add("Description", DataType.VariableCharacter, 50)

            Return createTable

        End Function

    End Class

    ''' <summary>
    ''' Represents another "foreign" referenced object that is referenced from the main item.
    ''' </summary>
    Private Class Foreign2Item
        Inherits DatabaseObjectUsingAttributes

        <FieldMapping("Description")>
        Public Description As String

        Friend Sub New(parent As Foreign2Collection)

            MyBase.New(parent)

        End Sub

        Public Shadows Sub Save()

            MyBase.Save()

        End Sub

    End Class

    Private Class ForeignCollectionWithNoTableAndDistinctFieldAttributes
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsListUsingAttributes(Of ForeignItemWithNoTableAndDistinctFieldAttributes)

        Friend Sub New(container As RootContainer)

            MyBase.New(container)

        End Sub

    End Class

    Private Class ForeignItemWithNoTableAndDistinctFieldAttributes
        Inherits DatabaseObjectUsingAttributes

        Friend Sub New(parent As Foreign2Collection)

            MyBase.New(parent)

        End Sub

    End Class

    ''' <summary>
    ''' The MainItemUsingUsingEarlyBindingWithNoFieldMapping is invalid because there is no FieldMapping on
    ''' on the field.
    ''' </summary>
    Private Class MainCollectionUsingForeignCollectionWithNoTableAndDistinctFieldAttributes
        Inherits MainCollection(Of MainItemUsingForeignCollectionWithNoTableAndDistinctFieldAttributes)

        Friend Sub New(container As RootContainer)

            MyBase.New(container)

        End Sub

    End Class

    Private Class MainItemUsingForeignCollectionWithNoTableAndDistinctFieldAttributes
        Inherits MainItem

        <FieldMapping("ForeignID")>
        <ObjectReferenceEarlyBinding()>
        Public ForeignObject As Global.DatabaseObjects.Generic.ObjectReference(Of ForeignItemWithNoTableAndDistinctFieldAttributes)

        Friend Sub New(parent As MainCollectionUsingEarlyBinding)

            MyBase.New(parent)

        End Sub

    End Class

    Public Property TestContext As TestContext

    Private Shared database As Database

    <ClassInitialize()>
    Public Shared Sub ClassInitialize(context As TestContext)

        database = MicrosoftSQLServerDatabase.Parse(ConfigurationManager.ConnectionStrings("SQLServerTestDatabase").ConnectionString)

    End Sub

    <TestInitialize()>
    Public Sub TestInitialize()

        AddHandler database.Connection.StatementExecuted, _
            Sub(statement As ISQLStatement)
                TestContext.WriteLine(statement.SQL)
            End Sub

        'Clear the main collection database table
        Using connection As New ConnectionScope(database)
            DropIfExistsAndCreateTable(MainCollection(Of IDatabaseObject).TableSchema)
            DropIfExistsAndCreateTable(ForeignCollection.TableSchema)
            DropIfExistsAndCreateTable(Foreign2Collection.TableSchema)
        End Using

    End Sub

    Private Shared Sub DropIfExistsAndCreateTable(table As SQLCreateTable)

        Using connection As New ConnectionScope(database)
            If connection.Execute(New SQLTableExists(table.Name)).Read Then
                connection.Execute(New SQLDropTable(table.Name))
            End If
            connection.Execute(table)
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("ObjectReference")>
    Public Sub LateBinding()

        Dim rootContainer As New ObjectReferenceRootContainer(database)

        Dim foreignItem As ForeignItem = rootContainer.ForeignObjects.Add
        foreignItem.Name = "ForeignName"
        foreignItem.Save()

        Dim mainCollectionUsingLateBinding As New MainCollectionUsingLateBinding(rootContainer)
        Dim mainItem = mainCollectionUsingLateBinding.Add()
        mainItem.ForeignObject.Object = foreignItem
        mainItem.Save()

        Dim sqlStatementCount As Integer
        AddHandler database.Connection.StatementExecuted, Sub() sqlStatementCount += 1

        Dim mainItemReloaded = mainCollectionUsingLateBinding.First()

        'One statement will be executed to retrieve the first object
        Assert.AreEqual(1, sqlStatementCount)
        Assert.AreEqual(foreignItem, mainItemReloaded.ForeignObject.Object)
        Assert.AreEqual(foreignItem.Name, mainItemReloaded.ForeignObject.Object.Name)
        'One statement will be executed to retrieve the first object, and the second statement will be executed to fetch the foreign object in the above Assert
        Assert.AreEqual(2, sqlStatementCount)

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("ObjectReference")>
    Public Sub EarlyBinding()

        Dim rootContainer As New ObjectReferenceRootContainer(database)

        Dim foreignItem As ForeignItem = rootContainer.ForeignObjects.Add
        foreignItem.Name = "ForeignName"
        foreignItem.Save()

        Dim mainCollectionUsingEarlyBinding As New MainCollectionUsingEarlyBinding(rootContainer)
        Dim mainItem = mainCollectionUsingEarlyBinding.Add()
        mainItem.ForeignObject.Object = foreignItem
        mainItem.Save()

        Dim sqlStatementCount As Integer
        AddHandler database.Connection.StatementExecuted, Sub() sqlStatementCount += 1

        Dim mainItemReloaded = mainCollectionUsingEarlyBinding.First()

        'One statement will be executed to retrieve the first object and the foreign object via a table join in the above Assert
        Assert.AreEqual(1, sqlStatementCount)
        Assert.AreEqual(foreignItem, mainItemReloaded.ForeignObject.Object)
        Assert.AreEqual(foreignItem.Name, mainItemReloaded.ForeignObject.Object.Name)
        'Ensure that accessing the foreign object was not using lazy loading
        Assert.AreEqual(1, sqlStatementCount)

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("ObjectReference")>
    <ExpectedException(GetType(InvalidOperationException))>
    Public Sub EarlyBindingWithNonGenericObjectReference()

        Dim rootContainer As New ObjectReferenceRootContainer(database)
        Dim mainCollectionUsingEarlyBindingWithNonGenericObjectReference As New MainCollectionUsingEarlyBindingWithNonGenericObjectReference(rootContainer)

        mainCollectionUsingEarlyBindingWithNonGenericObjectReference.First()

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("ObjectReference")>
    <ExpectedException(GetType(InvalidOperationException))>
    Public Sub EarlyBindingWithNoFieldMapping()

        Dim rootContainer As New ObjectReferenceRootContainer(database)
        Dim mainCollectionUsingEarlyBindingWithNoFieldMapping As New MainCollectionUsingEarlyBindingWithNoFieldMapping(rootContainer)

        mainCollectionUsingEarlyBindingWithNoFieldMapping.First()

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("ObjectReference")>
    <ExpectedException(GetType(InvalidOperationException))>
    Public Sub EarlyBindingWithForeignCollectionTableAndDistinctFieldAttributes()

        Dim rootContainer As New ObjectReferenceRootContainer(database)
        Dim mainCollectionUsingEarlyBindingWithNoTableAndDistinctFieldAttributes As New MainCollectionUsingForeignCollectionWithNoTableAndDistinctFieldAttributes(rootContainer)

        mainCollectionUsingEarlyBindingWithNoTableAndDistinctFieldAttributes.First()

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("ObjectReference")>
    Public Sub EarlyBindingTwoForeignReferences()

        Dim rootContainer As New ObjectReferenceRootContainer(database)

        Dim foreignItem As ForeignItem = rootContainer.ForeignObjects.Add
        foreignItem.Name = "ForeignName"
        foreignItem.Save()

        Dim foreign2Item As Foreign2Item = rootContainer.Foreign2Objects.Add
        foreign2Item.Description = "Foreign2Description"
        foreign2Item.Save()

        Dim mainCollectionUsingEarlyBindingAndTwoForeignReferences As New MainCollectionUsingEarlyBindingAndTwoForeignReferences(rootContainer)
        Dim mainItem = MainCollectionUsingEarlyBindingAndTwoForeignReferences.Add()
        mainItem.ForeignObject.Object = foreignItem
        mainItem.Foreign2Object.Object = foreign2Item
        mainItem.Save()

        Dim sqlStatementCount As Integer
        AddHandler database.Connection.StatementExecuted, Sub() sqlStatementCount += 1

        Dim mainItemReloaded = mainCollectionUsingEarlyBindingAndTwoForeignReferences.First()

        'One statement will be executed to retrieve the two foreign objects via two table joins
        Assert.AreEqual(1, sqlStatementCount)
        Assert.AreEqual(foreignItem, mainItemReloaded.ForeignObject.Object)
        Assert.AreEqual(foreignItem.Name, mainItemReloaded.ForeignObject.Object.Name)
        Assert.AreEqual(foreign2Item, mainItemReloaded.Foreign2Object.Object)
        Assert.AreEqual(foreign2Item.Description, mainItemReloaded.Foreign2Object.Object.Description)
        'Ensure that accessing the foreign objects was not using lazy loading
        Assert.AreEqual(1, sqlStatementCount)

    End Sub

    <TestMethod()>
    <TestCategory("Database"), TestCategory("ObjectReference")>
    Public Sub EarlyBindingTwoForeignReferencesWithInhertance()

        Dim rootContainer As New ObjectReferenceRootContainer(database)

        Dim foreignItem As ForeignItem = rootContainer.ForeignObjects.Add
        foreignItem.Name = "ForeignName"
        foreignItem.Save()

        Dim foreign2Item As Foreign2Item = rootContainer.Foreign2Objects.Add
        foreign2Item.Description = "Foreign2Description"
        foreign2Item.Save()

        Dim mainCollectionUsingEarlyBindingTwoForeignReferencesWithInhertance As New MainCollectionUsingEarlyBindingTwoForeignReferencesWithInhertance(rootContainer)
        Dim mainItem = mainCollectionUsingEarlyBindingTwoForeignReferencesWithInhertance.Add()
        mainItem.ForeignObject.Object = foreignItem
        mainItem.Foreign2Object.Object = foreign2Item
        mainItem.Save()

        Dim sqlStatementCount As Integer
        AddHandler database.Connection.StatementExecuted, Sub() sqlStatementCount += 1

        Dim mainItemReloaded = mainCollectionUsingEarlyBindingTwoForeignReferencesWithInhertance.First()

        'One statement will be executed to retrieve the two foreign objects via two table joins
        Assert.AreEqual(1, sqlStatementCount)
        Assert.AreEqual(foreignItem, mainItemReloaded.ForeignObject.Object)
        Assert.AreEqual(foreignItem.Name, mainItemReloaded.ForeignObject.Object.Name)
        Assert.AreEqual(foreign2Item, mainItemReloaded.Foreign2Object.Object)
        Assert.AreEqual(foreign2Item.Description, mainItemReloaded.Foreign2Object.Object.Description)
        'Ensure that accessing the foreign objects was not using lazy loading
        Assert.AreEqual(1, sqlStatementCount)

    End Sub

End Class
