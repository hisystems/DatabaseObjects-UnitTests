Imports System.Text
Imports DatabaseObjects.SQL

<TestClass()>
Public Class SQLTableTest

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableSchema")>
    Public Sub CreateTableSQLServer()

        Dim createTable As New SQLCreateTable
        createTable.ConnectionType = Database.ConnectionType.SQLServer
        createTable.Name = "Table1"
        With createTable.Fields.Add("Field1", DataType.Integer)
            .AutoIncrements = True
            .KeyType = KeyType.Primary
        End With
        createTable.Fields.Add("Field2", DataType.VariableCharacter, 10)

        Assert.AreEqual("CREATE TABLE [Table1] ([Field1] INTEGER IDENTITY NOT NULL PRIMARY KEY, [Field2] VARCHAR(10) NULL)", createTable.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableSchema")>
    Public Sub CreateTableMySQL()

        Dim createTable As New SQLCreateTable
        createTable.ConnectionType = Database.ConnectionType.MySQL
        createTable.Name = "Table1"
        With createTable.Fields.Add("Field1", DataType.Integer)
            .AutoIncrements = True
            .KeyType = KeyType.Primary
        End With
        createTable.Fields.Add("Field2", DataType.VariableCharacter, 10)

        Assert.AreEqual("CREATE TABLE `Table1` (`Field1` INT AUTO_INCREMENT NOT NULL PRIMARY KEY, `Field2` VARCHAR(10) NULL)", createTable.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableSchema")>
    Public Sub AlterTableAlterFieldsSQLServer()

        Dim alterTable As New SQLAlterTable
        alterTable.ConnectionType = Database.ConnectionType.SQLServer
        alterTable.Name = "Table1"
        alterTable.Fields("Field2").Size = 20
        alterTable.Fields("Field3").Size = 30

        Assert.AreEqual("ALTER TABLE [Table1] ALTER COLUMN [Field2] VARCHAR(20) NULL, [Field3] VARCHAR(30) NULL", alterTable.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableSchema")>
    Public Sub AlterTableAlterFieldsMySQL()

        Dim alterTable As New SQLAlterTable
        alterTable.ConnectionType = Database.ConnectionType.MySQL
        alterTable.Name = "Table1"
        alterTable.Fields("Field2").Size = 20
        alterTable.Fields("Field3").Size = 30

        Assert.AreEqual("ALTER TABLE `Table1` MODIFY COLUMN `Field2` VARCHAR(20) NULL, MODIFY COLUMN `Field3` VARCHAR(30) NULL", alterTable.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableSchema")>
    Public Sub AlterTableAddFieldsSQLServer()

        Dim alterTable As New SQLAlterTable
        alterTable.ConnectionType = Database.ConnectionType.SQLServer
        alterTable.Name = "Table1"
        alterTable.Fields.Add("Field2", DataType.VariableCharacter, 20)
        alterTable.Fields.Add("Field3", DataType.VariableCharacter, 30)

        Assert.AreEqual("ALTER TABLE [Table1] ADD [Field2] VARCHAR(20) NULL, [Field3] VARCHAR(30) NULL", alterTable.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableSchema")>
    Public Sub AlterTableAddFieldsMySQL()

        Dim alterTable As New SQLAlterTable
        alterTable.ConnectionType = Database.ConnectionType.MySQL
        alterTable.Name = "Table1"
        alterTable.Fields.Add("Field2", DataType.VariableCharacter, 20)
        alterTable.Fields.Add("Field3", DataType.VariableCharacter, 30)

        Assert.AreEqual("ALTER TABLE `Table1` ADD `Field2` VARCHAR(20) NULL, ADD `Field3` VARCHAR(30) NULL", alterTable.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableSchema")>
    Public Sub AlterTableDropFieldsMySQL()

        Dim alterTable As New SQLAlterTable
        alterTable.ConnectionType = Database.ConnectionType.MySQL
        alterTable.Name = "Table1"
        alterTable.Fields.Drop("Field1")
        alterTable.Fields.Drop("Field2")

        Assert.AreEqual("ALTER TABLE `Table1` DROP COLUMN `Field1`, DROP COLUMN `Field2`", alterTable.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableSchema")>
    Public Sub AlterTableDropFieldsSQLServer()

        Dim alterTable As New SQLAlterTable
        alterTable.ConnectionType = Database.ConnectionType.SQLServer
        alterTable.Name = "Table1"
        alterTable.Fields.Drop("Field1")
        alterTable.Fields.Drop("Field2")

        Assert.AreEqual("ALTER TABLE [Table1] DROP COLUMN [Field1], [Field2]", alterTable.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableSchema")>
    Public Sub CreateTableWithComputedColumn()

        Dim caseExpression As New SQLCaseExpression(New SQLFieldExpression("Field1"))
        caseExpression.Cases.Add(New SQLValueExpression("Value1"), New SQLValueExpression("1"))

        Dim createTable As New SQLCreateTable
        createTable.ConnectionType = Database.ConnectionType.SQLServer
        createTable.Name = "Table1"
        createTable.Fields.Add("Field1", DataType.VariableCharacter, 20)
        createTable.Fields.AddComputed("Computed1", caseExpression)

        Assert.AreEqual("CREATE TABLE [Table1] ([Field1] VARCHAR(20) NULL, [Computed1] AS (CASE [Field1] WHEN 'Value1' THEN '1' END))", createTable.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("TableSchema")>
    Public Sub AlterTableAddComputedColumn()

        Dim caseExpression As New SQLCaseExpression(New SQLFieldExpression("Field1"))
        caseExpression.Cases.Add(New SQLValueExpression("Value1"), New SQLValueExpression("1"))

        Dim alterTable As New SQLAlterTable
        alterTable.ConnectionType = Database.ConnectionType.SQLServer
        alterTable.Name = "Table1"
        alterTable.Fields.AddComputed("Computed2", caseExpression)

        Assert.AreEqual("ALTER TABLE [Table1] ADD [Computed2] AS (CASE [Field1] WHEN 'Value1' THEN '1' END)", alterTable.SQL)

    End Sub

End Class
