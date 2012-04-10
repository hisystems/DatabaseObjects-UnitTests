Imports System
Imports DatabaseObjects.SQL

Namespace Generic

    <Table(SimpleDatabaseObjectsMultipleSubclassUsingAttributes.Name)>
    <DistinctField("PrimaryField", FieldValueAutoAssignmentType.AutoIncrement)>
    Public Class SimpleDatabaseObjectsMultipleSubclassUsingAttributes
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsMultipleSubclassUsingAttributes(Of SimpleDatabaseObject)

        Public Const Name As String = "Generic-SimpleDatabaseObjectsMultipleSubclassUsingAttributes"

        Friend Sub New(rootContainer As RootContainer)

            MyBase.New(rootContainer)

        End Sub

        Friend Sub New(database As Database)

            MyBase.New(database)

        End Sub

        Public Function CreateItemInstance() As SimpleDatabaseObject

            Return MyBase.ItemInstance

        End Function

        Public Function CreateItemInstance_() As SimpleDatabaseObject

            Return MyBase.ItemInstance

        End Function

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

        Protected Overrides Function ItemInstanceForSubclass_(objFieldValues As SQL.SQLFieldValues) As SimpleDatabaseObject

            Return New SimpleDatabaseObject(Me)

        End Function

    End Class

End Namespace
