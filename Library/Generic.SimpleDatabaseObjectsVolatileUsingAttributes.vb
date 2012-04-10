Imports System
Imports DatabaseObjects.SQL

Namespace Generic

    <Table(SimpleDatabaseObjectsVolatileUsingAttributes.Name)>
    <DistinctField("ParentPrimaryField")>
    <Subset("ParentPrimaryField")>
    Public Class SimpleDatabaseObjectsVolatileUsingAttributes
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsVolatileUsingAttributes(Of SimpleDatabaseObject)

        Public Const Name As String = "Generic-SimpleDatabaseObjectsVolatileUsingAttributes"

        Friend Sub New(parent As DatabaseObject)

            MyBase.New(parent)

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
