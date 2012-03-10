Imports System.Text
Imports System.Configuration
Imports DatabaseObjects.SQL

<TestClass()>
Public Class DatabaseTests

    <Table("Table1")>
    <DistinctField("PrimaryField", FieldValueAutoAssignmentType.AutoIncrement)>
    <ItemInstance(GetType(Table1Item))>
    Private Class Table1Collection
        Inherits Generic.DatabaseObjectsListUsingAttributes(Of Table1Item)

        Friend Sub New(database As Database)

            MyBase.New(database)

        End Sub

        Friend Shared Function TableSchema() As SQLCreateTable

            Dim createTable As New SQLCreateTable

            createTable.Name = "Table1"

            With createTable.Fields.Add("PrimaryField", DataType.Integer)
                .KeyType = KeyType.Primary
                .AutoIncrements = True
            End With

            createTable.Fields.Add("Field1", DataType.VariableCharacter, 100)

            Return createTable

        End Function

    End Class

    Private Class Table1Item
        Inherits DatabaseObjectUsingAttributes

        <FieldMapping("Field1")>
        Public Field1 As String

        Friend Sub New(parent As Table1Collection)

            MyBase.New(parent)

        End Sub

        Public Shadows Sub Save()

            MyBase.Save()

        End Sub

    End Class

    Private Shared database As Database
    Private Shared table1 As Table1Collection

    <ClassInitialize()>
    Public Shared Sub ClassInitialize(context As TestContext)

        database = New Database(ConfigurationManager.ConnectionStrings("SQLServerTestDatabase").ConnectionString, database.ConnectionType.SQLServer)
        table1 = New Table1Collection(database)

    End Sub

    <TestInitialize()>
    Public Sub InitializeTable1WithAutoIncrementPrimaryKeyAndField1()

        database.Connection.Start()

        If database.Connection.Execute(New SQLTableExists("Table1")).Read Then
            database.Connection.Execute(New SQLDropTable("Table1"))
        End If

        database.Connection.Execute(Table1Collection.TableSchema)

        With table1.Add
            .Save()
        End With

        With table1.Add
            .Save()
        End With

        With table1.Add
            .Save()
        End With

        database.Connection.Finished()

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectByDistinctValueExists()

        Assert.IsTrue(database.ObjectExistsByDistinctValue(table1, 2))

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectByDistinctValueDoesNotExist()

        Assert.IsFalse(database.ObjectExistsByDistinctValue(table1, 4))

    End Sub

End Class
