Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.UnitTestExtensions

<DatabaseTestClass(ConnectionStringNames:={"SQLServerTestDatabase", "MySQLTestDatabase"})>
Public Class SQLIndexTests

    <DatabaseTestInitialize()>
    Public Sub DatabaseTestInitialize(database As Database)

        Using connection = New ConnectionScope(database)
            If connection.Execute(New SQLTableExists("Table1")).Read Then
                connection.Execute(New SQLDropTable("Table1"))
            End If

            Dim createTable As New SQLCreateTable
            createTable.Name = "Table1"
            createTable.Fields.Add("Field1", DataType.Integer)
            createTable.Fields.Add("Field2", DataType.Integer)
            connection.Execute(createTable)
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableSchema")>
    Public Sub CreateIndex(database As Database)

        Dim createIndex As New SQLCreateIndex
        createIndex.Name = "Index1"
        createIndex.TableName = "Table1"
        createIndex.Fields.Add("Field1")
        createIndex.Fields.Add("Field2")

        Using connection = New ConnectionScope(database)
            connection.Execute(createIndex)
        End Using

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableSchema")>
    Public Sub DropIndex(database As Database)

        CreateIndex(database)

        Using connection = New ConnectionScope(database)
            connection.Execute(New SQLDropIndex("Index1", "Table1"))
        End Using

    End Sub

End Class
