Imports System.Configuration
Imports DatabaseObjects.SQL

<TestClass()>
Public Class AttributesInvalidTests

    Public Property TestContext As TestContext

    Private Shared database As Database

    <ClassInitialize()>
    Public Shared Sub ClassInitialize(context As TestContext)

        database = MicrosoftSQLServerDatabase.Parse(ConfigurationManager.ConnectionStrings("SQLServerTestDatabase").ConnectionString)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    <ExpectedException(GetType(InvalidOperationException))>
    Public Sub KeyFieldAttributeMissingObjectByKey()

        'This collection does not have KeyFieldAttribute specified
        Dim collection As New Generic.SimpleDatabaseObjects(database)

        database.ObjectByKey(collection, "ABC")

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    <ExpectedException(GetType(InvalidOperationException))>
    Public Sub KeyFieldAttributeMissingObjectExists()

        'This collection does not have KeyFieldAttribute specified
        Dim collection As New Generic.SimpleDatabaseObjects(database)

        database.ObjectExists(collection, "ABC")

    End Sub

    <Table("Table")>
    <DistinctField("DistinctField")>
    <ItemInstance(GetType(InvalidItemInstanceType))>
    Private Class InvalidItemInstanceTypeCollection
        Inherits DatabaseObjects

        Public Sub New(database As Database)

            MyBase.New(database)

        End Sub

    End Class

    Private Class InvalidItemInstanceType

    End Class

    <TestMethod()>
    <TestCategory("Database"), TestCategory("Attribtues")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub InvalidItemInstanceAttributeParameter()

        'This collection does not have KeyFieldAttribute specified
        Dim collection As New InvalidItemInstanceTypeCollection(database)
        collection.GetType()    'Loading the type information will cause the attributes to be loaded and the exception to occur

    End Sub

End Class
