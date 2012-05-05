Option Infer On

Imports System.Text
Imports System.Configuration
Imports System.Linq
Imports DatabaseObjects.SQL
Imports DatabaseObjects.Exceptions
Imports DatabaseObjects.UnitTestExtensions

<DatabaseTestClass(ConnectionStringNames:={"SQLServerTestDatabase", "MySQLTestDatabase"})>
Public Class DatabaseTestExtensionTesting

    Public Property TestContext As TestContext

    <TestInitialize()>
    Public Sub TestInitialize()

        TestContext.WriteLine("TestInitialize")

    End Sub

    <DatabaseTestInitialize()>
    Public Sub DatabaseTestInitialize(database As Database)

        TestContext.WriteLine("DatabaseTestInitialize")

    End Sub

    <TestMethod()>
    Public Sub TestMethod(database As Database)

        TestContext.WriteLine("TestMethod")

    End Sub

    <DatabaseTestCleanup()>
    Public Sub DatabaseTestCleanup(database As Database)

        TestContext.WriteLine("DatabaseTestCleanup")

    End Sub

    <TestCleanup()>
    Public Sub TestCleanup()

        TestContext.WriteLine("TestCleanup")

    End Sub

End Class
