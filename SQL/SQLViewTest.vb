Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.UnitTestExtensions

<DatabaseTestClass(ConnectionStringNames:={"SQLServerTestDatabase", "MySQLTestDatabase"})>
Public Class SQLViewTest

    Public TestContext As TestContext

    <DatabaseTestInitialize()>
    Public Sub DatabaseTestInitialize(database As Database)

        Dim createTable As New SQLCreateTable
        createTable.Name = "Table1"
        createTable.Fields.Add("Field1", DataType.Integer)

        database.RecreateTable(createTable)
        database.DropViewIfExists("View1")

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableSchema")>
    Public Sub CreateView(database As Database)

        Dim selectStatement As New SQLSelect
        selectStatement.Tables.Add("Table1")

        Dim createView As New SQLCreateView
        createView.Name = "View1"
        createView.Select = selectStatement

        Using connection = New ConnectionScope(database)
            connection.Execute(createView)
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("Views")>
    Public Sub ViewExistsHyperSQL()

        SQLStatement.DefaultConnectionType = Database.ConnectionType.HyperSQL

        Dim viewExists As New SQLViewExists("ViewName")

        Assert.AreEqual(Of String)("SELECT * FROM ""INFORMATION_SCHEMA"".""VIEWS"" WHERE ""TABLE_SCHEMA"" = 'PUBLIC' AND ""TABLE_NAME"" = 'ViewName'", viewExists.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("Views")>
    <ExpectedException(GetType(ArgumentNullException))>
    Public Sub ViewExistsWithInvalidName()

        Dim view As New SQLViewExists("")

    End Sub

End Class
