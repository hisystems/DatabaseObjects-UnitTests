Imports System
Imports DatabaseObjects.SQL

Namespace Generic

    <Table(SimpleDatabaseObjectsVolatile.Name)>
    <DistinctField("ParentPrimaryField")>
    <Subset("ParentPrimaryField")>
    Public Class SimpleDatabaseObjectsVolatile
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsVolatileList(Of SimpleDatabaseObjectVolatile)

        Public Const Name As String = "Generic-SimpleDatabaseObjectsVolatile"

        Friend Sub New(parent As DatabaseObject)

            MyBase.New(parent)

        End Sub

        Friend Sub SaveAll()

            MyBase.VolatileObjectsSave()

        End Sub

        Friend Shared Function TableSchema() As SQLCreateTable

            Dim createTable As New SQLCreateTable

            createTable.Name = Name

            createTable.Fields.Add("ParentPrimaryField", DataType.Integer)
            createTable.Fields.Add("Field1", DataType.VariableCharacter, 100)

            Return createTable

        End Function

    End Class

End Namespace
