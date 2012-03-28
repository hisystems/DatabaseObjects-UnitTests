Imports System.Configuration
Imports DatabaseObjects.SQL

<TestClass()>
Public Class ItemInstanceTests

    Private Class TableItem
        Inherits DatabaseObjectUsingAttributes

        Friend Sub New(parent As DatabaseObjects)

            MyBase.New(parent)

        End Sub

    End Class

    Private Class TableItemSuperClass
        Inherits TableItem

        Friend Sub New(parent As DatabaseObjects)

            MyBase.New(parent)

        End Sub

    End Class

    Public Property TestContext As TestContext

    Private Shared database As Database

    <ClassInitialize()>
    Public Shared Sub ClassInitialize(context As TestContext)

        database = MicrosoftSQLServerDatabase.Parse(ConfigurationManager.ConnectionStrings("SQLServerTestDatabase").ConnectionString)

    End Sub

    ''' <summary>
    ''' This should utilise the base class's T argument to determine the ItemInstance type (TableItem)
    ''' </summary>
    ''' <remarks></remarks>
    <Table("Table")>
    <DistinctField("Field", FieldValueAutoAssignmentType.AutoIncrement)>
    Private Class GenericTableUsingTArgument
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsUsingAttributes(Of TableItem)

        Friend Sub New(database As Database)

            MyBase.New(database)

        End Sub

    End Class

    <TestMethod()>
    <TestCategory("ItemInstance")>
    Public Sub GenericTableUsingTArgumentTest()

        Dim collection As New GenericTableUsingTArgument(database)

        Assert.AreEqual(GetType(TableItem), DirectCast(collection, IDatabaseObjects).ItemInstance.GetType)

    End Sub

    ''' <summary>
    ''' The ItemInstance will be determined the ItemInstanceAttribute (TableItemSuperClass)
    ''' NOT by the generic type argument (TableItem).
    ''' </summary>
    ''' <remarks></remarks>
    <Table("Table")>
    <DistinctField("Field", FieldValueAutoAssignmentType.AutoIncrement)>
    <ItemInstance(GetType(TableItemSuperClass))>
    Private Class GenericTableUsingItemInstance
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsUsingAttributes(Of TableItem)

        Friend Sub New(database As Database)

            MyBase.New(database)

        End Sub

    End Class

    <TestMethod()>
    <TestCategory("ItemInstance")>
    Public Sub GenericTableUsingItemInstanceTest()

        Dim collection As New GenericTableUsingItemInstance(database)

        Assert.AreEqual(GetType(TableItemSuperClass), DirectCast(collection, IDatabaseObjects).ItemInstance.GetType)

    End Sub

    ''' <summary>
    ''' The ItemInstance will be determined the ItemInstanceAttribute (TableItemSuperClass)
    ''' NOT by the generic type argument (TableItem) specified in the base class.
    ''' </summary>
    ''' <remarks></remarks>
    <ItemInstance(GetType(TableItemSuperClass))>
    Private Class GenericTableUsingItemInstanceNotBaseClassTArgument
        Inherits GenericTableUsingTArgument

        Friend Sub New(database As Database)

            MyBase.New(database)

        End Sub

    End Class

    <TestMethod()>
    <TestCategory("ItemInstance")>
    Public Sub GenericTableUsingItemInstanceNotBaseClassTArgumentTest()

        Dim collection As New GenericTableUsingItemInstanceNotBaseClassTArgument(database)

        Assert.AreEqual(GetType(TableItemSuperClass), DirectCast(collection, IDatabaseObjects).ItemInstance.GetType)

    End Sub

    ''' <summary>
    ''' The ItemInstance will be determined the ItemInstanceAttribute (TableItem).
    ''' </summary>
    ''' <remarks></remarks>
    Private Class GenericTableUsingTArgumentNoAttributes
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsList(Of TableItem)

        Friend Sub New(database As Database)

            MyBase.New(database)

        End Sub

        Protected Overrides Function DistinctFieldAutoIncrements() As Boolean

            Return False

        End Function

        Protected Overrides Function DistinctFieldName() As String

            Return "Field"

        End Function

        Protected Overrides Function KeyFieldName() As String

            Return String.Empty

        End Function

        Protected Overrides Function OrderBy() As SQL.SQLSelectOrderByFields

            Return Nothing

        End Function

        Protected Overrides Function Subset() As SQL.SQLConditions

            Return Nothing

        End Function

        Protected Overrides Function TableJoins(objPrimaryTable As SQL.SQLSelectTable, objTables As SQL.SQLSelectTables) As SQL.SQLSelectTableJoins

            Return Nothing

        End Function

        Protected Overrides Function TableName() As String

            Return "Table"

        End Function

    End Class

    <TestMethod()>
    <TestCategory("ItemInstance")>
    Public Sub GenericTableUsingTArgumentNoAttributesTest()

        Dim collection As New GenericTableUsingTArgumentNoAttributes(database)

        Assert.AreEqual(GetType(TableItem), DirectCast(collection, IDatabaseObjects).ItemInstance.GetType)

    End Sub

    ''' <summary>
    ''' This should fail because the ItemInstance attribute has not been specified
    ''' and a non DatabaseObjects.Generic collection object is being used so the type 
    ''' cannot be determined from the T argumetn.
    ''' </summary>
    ''' <remarks></remarks>
    <Table("Table")>
    <DistinctField("Field", FieldValueAutoAssignmentType.AutoIncrement)>
    Private Class TableWithNoItemInstanceAttribute
        Inherits Global.DatabaseObjects.DatabaseObjectsUsingAttributes

        Friend Sub New(database As Database)

            MyBase.New(database)

        End Sub

    End Class

    <TestMethod()>
    <TestCategory("ItemInstance")>
    <ExpectedException(GetType(Exceptions.DatabaseObjectsException))>
    Public Sub TableWithNoItemInstanceAttributeTest()

        Dim collection As New TableWithNoItemInstanceAttribute(database)

        DirectCast(collection, IDatabaseObjects).ItemInstance()

    End Sub

    ''' <summary>
    ''' The function override should supercede the attribute defined or the 
    ''' argument type defined.
    ''' </summary>
    <Table("Table")>
    <DistinctField("Field", FieldValueAutoAssignmentType.AutoIncrement)>
    <ItemInstance(GetType(TableItem))>
    Private Class GenericTableOverridingItemInstance
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsUsingAttributes(Of TableItem)

        Friend Sub New(database As Database)

            MyBase.New(database)

        End Sub

        Protected Overrides Function ItemInstance_() As TableItem

            Return New TableItemSuperClass(Me)

        End Function

    End Class

    <TestMethod()>
    <TestCategory("ItemInstance")>
    Public Sub GenericTableOverridingItemInstanceTest()

        Dim collection As New GenericTableOverridingItemInstance(database)

        Assert.AreEqual(GetType(TableItemSuperClass), DirectCast(collection, IDatabaseObjects).ItemInstance.GetType)

    End Sub

    ''' <summary>
    ''' The ItemInstance_ override will supercede the attribute defined and the argument T (TableItem) type defined.
    ''' </summary>
    <Table("Table")>
    <DistinctField("Field", FieldValueAutoAssignmentType.AutoIncrement)>
    Private Class GenericTableOverridingItemInstanceWithNoItemInstanceAttribute
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsUsingAttributes(Of TableItem)

        Friend Sub New(database As Database)

            MyBase.New(database)

        End Sub

        Protected Overrides Function ItemInstance_() As TableItem

            Return New TableItemSuperClass(Me)

        End Function

    End Class

    <TestMethod()>
    <TestCategory("ItemInstance")>
    Public Sub GenericTableOverridingItemInstanceWithNoItemInstanceAttributeTest()

        Dim collection As New GenericTableOverridingItemInstanceWithNoItemInstanceAttribute(database)

        Assert.AreEqual(GetType(TableItemSuperClass), DirectCast(collection, IDatabaseObjects).ItemInstance.GetType)

    End Sub

    ''' <summary>
    ''' The ItemInstanceForSubclass_ override will supercede the argument T type (TableItem) defined.
    ''' </summary>
    <Table("Table")>
    <DistinctField("Field", FieldValueAutoAssignmentType.AutoIncrement)>
    Private Class GenericTableMultipleSubClass
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsMultipleSubclassUsingAttributes(Of TableItem)

        Friend Sub New(database As Database)

            MyBase.New(database)

        End Sub

        Protected Overrides Function ItemInstanceForSubclass_(objFieldValues As SQL.SQLFieldValues) As TableItem

            Return New TableItemSuperClass(Me)

        End Function

    End Class

    <TestMethod()>
    <TestCategory("ItemInstance")>
    Public Sub GenericTableMultipleSubClassTest()

        Dim collection As New GenericTableMultipleSubClass(database)

        Assert.AreEqual(GetType(TableItemSuperClass), DirectCast(collection, IDatabaseObjectsMultipleSubclass).ItemInstanceForSubclass(New SQLFieldValues()).GetType)

    End Sub

    <TestMethod()>
    <TestCategory("ItemInstance")>
    <ExpectedException(GetType(NotSupportedException))>
    Public Sub GenericTableMultipleSubClassTestItemInstanceFailure()

        Dim collection As New GenericTableMultipleSubClass(database)

        DirectCast(collection, IDatabaseObjectsMultipleSubclass).ItemInstance().GetType()

    End Sub

End Class
