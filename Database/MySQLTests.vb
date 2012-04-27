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

        Using connection As New ConnectionScope(database)
            If connection.Execute(New SQLTableExists(testTable.Name)).Read Then
                connection.Execute(New SQLDropTable(testTable.Name))
            End If

            connection.ExecuteNonQuery(testTable)

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

End Class
