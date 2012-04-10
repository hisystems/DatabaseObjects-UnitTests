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

End Class
