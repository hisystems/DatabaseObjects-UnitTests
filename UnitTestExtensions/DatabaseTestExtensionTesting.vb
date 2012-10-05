Option Infer On

Imports System.Text
Imports System.Configuration
Imports System.Linq
Imports DatabaseObjects.SQL
Imports DatabaseObjects.Exceptions
Imports DatabaseObjects.UnitTestExtensions

<DatabaseTestClass(ConnectionStringNames:={"SQLServerTestDatabase", "MySQLTestDatabase", "SQLiteTestDatabase"})>
Public Class DatabaseTestExtensionTesting

    Public Property TestContext As TestContext
    Private TestMethodExecutionCount As Integer = 0

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
        TestMethodExecutionCount += 1

    End Sub

    <DatabaseTestCleanup()>
    Public Sub DatabaseTestCleanup(database As Database)

        TestContext.WriteLine("DatabaseTestCleanup")

    End Sub

    <TestCleanup()>
    Public Sub TestCleanup()

        TestContext.WriteLine("TestCleanup")

        'Should be executed 3 times - once for each database
        Assert.AreEqual(3, TestMethodExecutionCount)

    End Sub

End Class


<DatabaseTestClass(ConnectionStringNames:={"SQLServerTestDatabase"})>
Public Class DatabaseTestExtensionTestingOnlySQLServer

    Public Property TestContext As TestContext
    Private TestMethodExecutionCount As Integer = 0

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
        TestMethodExecutionCount += 1

    End Sub

    <DatabaseTestCleanup()>
    Public Sub DatabaseTestCleanup(database As Database)

        TestContext.WriteLine("DatabaseTestCleanup")

    End Sub

    <TestCleanup()>
    Public Sub TestCleanup()

        TestContext.WriteLine("TestCleanup")

        'Should be executed once for the SQL server database
        Assert.AreEqual(1, TestMethodExecutionCount)

    End Sub

End Class
