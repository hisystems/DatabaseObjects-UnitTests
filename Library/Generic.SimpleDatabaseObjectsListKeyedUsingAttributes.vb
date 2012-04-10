Imports System
Imports DatabaseObjects.SQL

Namespace Generic

    <Table(SimpleDatabaseObjectsListKeyedUsingAttributes.Name)>
    <DistinctField("PrimaryField", FieldValueAutoAssignmentType.AutoIncrement)>
    <KeyField("KeyField")>
    <ItemInstance(GetType(SimpleKeyedDatabaseObject))>
    Public Class SimpleDatabaseObjectsListKeyedUsingAttributes
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsListKeyedUsingAttributes(Of SimpleKeyedDatabaseObject, String)

        Public Const Name As String = "Generic-SimpleDatabaseObjectsListKeyedUsingAttributes"

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

End Namespace
