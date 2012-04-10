Imports System
Imports DatabaseObjects.SQL

<Table(SimpleDatabaseObjectsUsingAttributes.Name)>
<DistinctField("PrimaryField", FieldValueAutoAssignmentType.AutoIncrement)>
<KeyField("KeyField")>
<ItemInstance(GetType(SimpleDatabaseObject))>
Public Class SimpleDatabaseObjectsUsingAttributes
    Inherits DatabaseObjectsUsingAttributes

    Public Const Name As String = "SimpleDatabaseObjectsUsingAttributes"

    Friend Sub New(rootContainer As RootContainer)

        MyBase.New(rootContainer)

    End Sub

    Friend Sub New(database As Database)

        MyBase.New(database)

    End Sub

    Friend Shared Function TableSchema() As SQLCreateTable

        Dim createTable As New SQLCreateTable

        createTable.Name = Name

        With createTable.Fields.Add("PrimaryField", DataType.Integer)
            .KeyType = KeyType.Primary
            .AutoIncrements = True
        End With

        createTable.Fields.Add("KeyField", DataType.VariableCharacter, 100)
        createTable.Fields.Add("Field1", DataType.VariableCharacter, 100)

        Return createTable

    End Function

End Class

