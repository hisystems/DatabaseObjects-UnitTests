Imports System.Text
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

        Assert.AreEqual(Of String)("INSERT INTO [Table] ([DateTimeField]) VALUES ('2000-01-02 03:04:05.0060000')", insert.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("DataTypeSerialisation")>
    Public Sub DateAndTimeWithOnlyMilliseconds()

        Dim dateAndTime As New DateTime(2000, 1, 2, Hour:=0, Minute:=0, Second:=0, millisecond:=6)

        Dim insert As New SQLInsert
        insert.TableName = "Table"
        insert.ConnectionType = Database.ConnectionType.SQLServer
        insert.Fields.Add("DateTimeField", dateAndTime)

        Assert.AreEqual(Of String)("INSERT INTO [Table] ([DateTimeField]) VALUES ('2000-01-02 00:00:00.0060000')", insert.SQL)

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("DataTypeSerialisation")>
    Public Sub NanoSecondPrecision()

        Dim dateAndTime As New DateTime(2000, 1, 1)
        dateAndTime = dateAndTime.AddTicks(1234567)

        Dim insert As New SQLInsert
        insert.TableName = "Table"
        insert.ConnectionType = Database.ConnectionType.SQLServer
        insert.Fields.Add("DateTimeField", dateAndTime)

        Assert.AreEqual(Of String)("INSERT INTO [Table] ([DateTimeField]) VALUES ('2000-01-01 00:00:00.1234567')", insert.SQL)

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
