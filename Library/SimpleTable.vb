Imports System
Imports DatabaseObjects.SQL

<Table(SimpleTable.Name)>
<DistinctField("PrimaryField", FieldValueAutoAssignmentType.AutoIncrement)>
<KeyField("Field1")>
<OrderByField("Field1")>
<ItemInstance(GetType(SimpleTableItem))>
Public Class SimpleTable
    Inherits Generic.DatabaseObjectsListUsingAttributes(Of SimpleTableItem)

    Public Const Name As String = "SimpleTable"

    Friend Sub New(database As Database)

        MyBase.New(database)

    End Sub

    Default Public ReadOnly Property Item(ByVal field1KeyField As String) As SimpleTableItem
        Get

            Return MyBase.ObjectByKey(field1KeyField)

        End Get
    End Property

    Friend Shared Function TableSchema() As SQLCreateTable

        Dim createTable As New SQLCreateTable

        createTable.Name = Name

        With createTable.Fields.Add("PrimaryField", DataType.Integer)
            .KeyType = KeyType.Primary
            .AutoIncrements = True
        End With

        createTable.Fields.Add("Field1", DataType.VariableCharacter, 100)

        Return createTable

    End Function

End Class

Public Class SimpleTableItem
    Inherits DatabaseObjectUsingAttributes

    <FieldMapping("Field1")>
    Public Field1 As String

    Friend Sub New(parent As SimpleTable)

        MyBase.New(parent)

    End Sub

    Public Shadows Sub Save()

        MyBase.Save()

    End Sub

End Class