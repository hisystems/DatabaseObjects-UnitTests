Option Strict On
Option Explicit On

Imports System.Text
Imports System.Configuration
Imports DatabaseObjects.SQL

<TestClass()>
Public Class KeyFieldIsUniqueInCollectionConstraintTests

    Public Property TestContext As TestContext

    Private Shared database As Database
    Private Shared table As SimpleTable

    <ClassInitialize()>
    Public Shared Sub ClassInitialize(context As TestContext)

        database = MicrosoftSQLServerDatabase.Parse(ConfigurationManager.ConnectionStrings("SQLServerTestDatabase").ConnectionString)
        table = New SimpleTable(database)

    End Sub

    <TestInitialize()>
    Public Sub InitializeTable1WithAutoIncrementPrimaryKeyAndField1()

        AddHandler database.Connection.StatementExecuted, _
            Sub(statement As ISQLStatement)
                TestContext.WriteLine(statement.SQL)
            End Sub

		database.RecreateTable(SimpleTable.TableSchema)

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
    <TestCategory("Constraint")>
    Public Sub KeyFieldIsUniqueInCollectionConstraintForNewObject()

        Dim newItem = table.Add
        Dim keyIsUniqueInCollectionConstraint As Constraints.IConstraint(Of String) = New Constraints.KeyFieldIsUniqueInCollectionConstraint(Of String)(newItem)

        Assert.IsTrue(keyIsUniqueInCollectionConstraint.ValueSatisfiesConstraint("Field1-4"))

    End Sub

    <TestMethod()>
    <TestCategory("Constraint")>
    Public Sub KeyFieldIsNotUniqueInCollectionConstraintForNewObject()

        Dim newItem = table.Add
        Dim keyIsUniqueInCollectionConstraint As Constraints.IConstraint(Of String) = New Constraints.KeyFieldIsUniqueInCollectionConstraint(Of String)(newItem)

        Assert.IsFalse(keyIsUniqueInCollectionConstraint.ValueSatisfiesConstraint("Field1-1"))

    End Sub

    <TestMethod()>
    <TestCategory("Constraint")>
    Public Sub KeyFieldIsUniqueInCollectionConstraintForExistingObject()

        Dim existingItem = table("Field1-1")
        Dim keyIsUniqueInCollectionConstraint As Constraints.IConstraint(Of String) = New Constraints.KeyFieldIsUniqueInCollectionConstraint(Of String)(existingItem)

        Assert.IsTrue(keyIsUniqueInCollectionConstraint.ValueSatisfiesConstraint("Field1-4"))

    End Sub

    <TestMethod()>
    <TestCategory("Constraint")>
    Public Sub KeyFieldIsNotUniqueInCollectionConstraintForExistingObject()

        Dim existingItem = table("Field1-1")
        Dim keyIsUniqueInCollectionConstraint As Constraints.IConstraint(Of String) = New Constraints.KeyFieldIsUniqueInCollectionConstraint(Of String)(existingItem)

        Assert.IsFalse(keyIsUniqueInCollectionConstraint.ValueSatisfiesConstraint("Field1-2"))

    End Sub

End Class
