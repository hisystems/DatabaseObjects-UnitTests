Imports System
Imports DatabaseObjects.SQL

<Table(SimpleDatabaseObjectsVolatileUsingAttributes.Name)>
<DistinctField("ParentPrimaryField")>
<Subset("ParentPrimaryField")>
<ItemInstance(GetType(SimpleDatabaseObjectVolatile))>
Public Class SimpleDatabaseObjectsVolatileUsingAttributes
    Inherits DatabaseObjectsUsingAttributes

    Public Const Name As String = "SimpleDatabaseObjectsVolatileUsingAttributes"

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

