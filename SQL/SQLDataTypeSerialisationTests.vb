﻿Imports System.Text
Imports DatabaseObjects.SQL

<TestClass()>
Public Class SQLDataTypeSerialisationTests

    Public TestContext As TestContext

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("DataTypeSerialisation")>
    Public Sub DateAndTime()

        Dim dateAndTime As New DateTime(2000, 1, 2, 3, 4, 5, 6)

        Dim insert As New SQLInsert
        insert.TableName = "Table"
        insert.ConnectionType = Database.ConnectionType.SQLServer
        insert.Fields.Add("DateTimeField", dateAndTime)

        Assert.AreEqual(Of String)("INSERT INTO [Table] ([DateTimeField]) VALUES ('2000-01-02 3:4:5.6')", insert.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("DataTypeSerialisation")>
    Public Sub DateAndNoTime()

        Dim dateAndTime As New DateTime(2000, 1, 2)

        Dim insert As New SQLInsert
        insert.TableName = "Table"
        insert.ConnectionType = Database.ConnectionType.SQLServer
        insert.Fields.Add("DateTimeField", dateAndTime)

        Assert.AreEqual(Of String)("INSERT INTO [Table] ([DateTimeField]) VALUES ('2000-01-02')", insert.SQL)

    End Sub

End Class
