Imports System.Text
Imports System.Configuration
Imports DatabaseObjects.SQL
Imports DatabaseObjects.Exceptions

<TestClass()>
Public Class MySQLTests

    Public Property TestContext As TestContext

    Private Shared database As Database

    <ClassInitialize()>
    Public Shared Sub ClassInitialize(context As TestContext)

        database = New Database(ConfigurationManager.ConnectionStrings("MySQLTestDatabase").ConnectionString, Database.ConnectionType.MySQL)

    End Sub

    <TestInitialize()>
    Public Sub TestInitialize()

        AddHandler database.Connection.StatementExecuted, _
            Sub(statement As ISQLStatement)
                TestContext.WriteLine(statement.SQL)
            End Sub

        Dim testTable = New SQLCreateTable
        testTable.Name = "MySQLTestTable"
        testTable.Fields.Add("BooleanField", DataType.Boolean)

		database.RecreateTable(testTable)

        Using connection As New ConnectionScope(database)
			Dim insert As New SQLInsert
            insert.TableName = testTable.Name
            insert.Fields.Add("BooleanField", True)
            connection.ExecuteNonQuery(insert)
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("MySQL")>
    Public Sub GetBooleanField()

        Dim selectStatement As New SQLSelect("MySQLTestTable")
        selectStatement.Fields.Add("BooleanField")

        Using connection As New ConnectionScope(database)
            Dim booleanValue As Object = connection.ExecuteScalar(selectStatement)
            Assert.AreEqual(GetType(Boolean), booleanValue.GetType())
            Assert.IsTrue(booleanValue)
        End Using

    End Sub

    ''' <summary>
    ''' Tests the FLAG_FOUND_ROWS option.
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()>
    <TestCategory("SQL"), TestCategory("MySQL")>
    Public Sub MySQLDatabaseConnectionReturnsRowsAffected()

        Dim database As New MySQLDatabase("localhost", "TestDatabase", "root", "root")

        Using connection As New ConnectionScope(database)
            Dim update As New SQLUpdate("MySQLTestTable")
            update.Fields.Add("BooleanField", False)
            Dim rowsAffected = connection.ExecuteNonQuery(update)

            Assert.IsTrue(rowsAffected > 0)
        End Using

    End Sub

    ''' <summary>
    ''' Tests FLAG_MULTI_STATEMENTS option.
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()>
    <TestCategory("SQL"), TestCategory("MySQL")>
    Public Sub MySQLDatabaseConnectionExecuteBatchCommands()

        Dim database As New MySQLDatabase("localhost", "TestDatabase", "root", "root")

        Using connection As New ConnectionScope(database)
            Dim updateToFalse As New SQLUpdate("MySQLTestTable")
            updateToFalse.Fields.Add("BooleanField", False)

            Dim updateToTrue As New SQLUpdate("MySQLTestTable")
            updateToTrue.Fields.Add("BooleanField", True)

            connection.ExecuteNonQuery(New ISQLStatement() {updateToFalse, updateToTrue})

            Dim selectStatement As New SQLSelect("MySQLTestTable")
            selectStatement.Fields.Add("BooleanField")

            Assert.IsTrue(connection.ExecuteScalar(selectStatement))
        End Using

    End Sub

End Class
