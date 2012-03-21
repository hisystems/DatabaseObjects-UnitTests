Imports System
Imports DatabaseObjects.SQL

<Table(SimpleDatabaseObjectsVolatileUsingAttributes.Name)>
<DistinctField("PrimaryField", FieldValueAutoAssignmentType.AutoIncrement)>
<KeyField("KeyField")>
<ItemInstance(GetType(SimpleDatabaseObjectUsingAttributes))>
Public Class SimpleDatabaseObjectsVolatileUsingAttributes
    Inherits DatabaseObjectsUsingAttributes

    Public Const Name As String = "SimpleDatabaseObjectsVolatileUsingAttributes"

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

