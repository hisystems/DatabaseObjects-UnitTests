Option Infer On

Imports System.Text
Imports System.Configuration
Imports System.Linq
Imports DatabaseObjects.SQL
Imports DatabaseObjects.Exceptions
Imports DatabaseObjects.UnitTestExtensions

<DatabaseTestClass(ConnectionStringNames:={"SQLServerTestDatabase", "MySQLTestDatabase", "SQLiteTestDatabase", "MicrosoftAccessTestDatabase", "SQLServerCETestDatabase"})>
Public Class DatabaseTests

    Public Property TestContext As TestContext

    Private table As SimpleTable

    <DatabaseTestInitialize()>
    Public Sub DatabaseTestInitialize(database As Database)

        AddHandler database.Connection.StatementExecuted, _
            Sub(statement As ISQLStatement)
                TestContext.WriteLine(statement.SQL)
            End Sub

        database.RecreateTable(SimpleTable.TableSchema)

        table = New SimpleTable(database)

        With table.Add
            .Field1 = "Field1-1"
            .Save()
        End With

        With table.Add
            .Field1 = "Field1-2"
            .Save()
        End With

        With table.Add
            .Field1 = "Field1-3"
            .Save()
        End With

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectByDistinctValueExists(database As Database)

        Assert.IsTrue(database.ObjectExistsByDistinctValue(table, 2))

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectByDistinctValueDoesNotExist(database As Database)

        Assert.IsFalse(database.ObjectExistsByDistinctValue(table, 4))

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectExists(database As Database)

        Assert.IsTrue(database.ObjectExists(table, "Field1-1"))

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectExistsWhenItDoesNotExist(database As Database)

        Assert.IsFalse(database.ObjectExists(table, "Field1-9999999999999"))

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectIfExists(database As Database)

        Assert.AreNotSame(database.ObjectIfExists(table, 1), Nothing)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectExistsByDistinctValue(database As Database)

        Assert.IsTrue(database.ObjectExistsByDistinctValue(table, 1))

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectSaveNew(database As Database)

        Dim newObject = table.Add
        newObject.Field1 = "Field1-NEW"

        database.ObjectSave(table, newObject)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectSaveExisting(database As Database)

        Dim existingObject = table("Field1-1")
        existingObject.Field1 = "Field1-1-Amended"

        database.ObjectSave(table, existingObject)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectByKeyIfExists(database As Database)

        Assert.AreNotSame(database.ObjectByKeyIfExists(table, "Field1-1"), Nothing)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectByOrdinalFirst(database As Database)

        Dim firstObject As SimpleTableItem = database.ObjectByOrdinalFirst(table)

        Assert.AreEqual(firstObject.Field1, "Field1-1")

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectByOrdinalLast(database As Database)

        Dim lastObject As SimpleTableItem = database.ObjectByOrdinalLast(table)

        Assert.AreEqual(lastObject.Field1, "Field1-3")

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectsCount(database As Database)

        Assert.AreEqual(database.ObjectsCount(table), 3)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectDelete(database As Database)

        database.ObjectDelete(table, table("Field1-1"))

        Assert.AreEqual(database.ObjectsCount(table), 2)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectsDeleteAll(database As Database)

        database.ObjectsDeleteAll(table)

        Assert.AreEqual(database.ObjectsCount(table), 0)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectsList(database As Database)

        Dim items = database.ObjectsList(table)

        Assert.AreEqual(items.Count, 3)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectsSearch(database As Database)

        Dim conditions As New SQLConditions()
        conditions.Add("Field1", ComparisonOperator.EqualTo, "Field1-1")

        Dim items = database.ObjectsSearch(table, conditions)

        Assert.AreEqual(items.Count, 1)

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectGetFieldValue(database As Database)

        Dim item2 = table("Field1-2")

        Assert.AreEqual(item2.Field1, database.ObjectGetFieldValue(item2, "Field1"))

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectSetFieldValue(database As Database)

        Dim item2 = table("Field1-2")
        database.ObjectSetFieldValue(item2, "Field1", "Field1-2-NEW")

        Assert.AreEqual("Field1-2-NEW", database.ObjectGetFieldValue(item2, "Field1"))

    End Sub

    <TestMethod()>
    <TestCategory("Database")>
    Public Sub ObjectsDictionary(database As Database)

        Dim items = database.ObjectsDictionary(table)

        Assert.AreEqual(items.Count, 3)
        Assert.AreEqual(DirectCast(items("Field1-2"), SimpleTableItem).Field1, "Field1-2")

    End Sub

End Class
