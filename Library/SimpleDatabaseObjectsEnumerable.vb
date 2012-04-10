Imports System
Imports DatabaseObjects
Imports DatabaseObjects.SQL

<Table(SimpleDatabaseObjectsEnumerable.Name)>
<DistinctField("PrimaryField", True)>
<KeyField("KeyField")>
<ItemInstance(GetType(SimpleDatabaseObject))>
Public Class SimpleDatabaseObjectsEnumerable
    Inherits DatabaseObjectsEnumerable

    Public Const Name As String = "SimpleDatabaseObjectsEnumerable"

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

