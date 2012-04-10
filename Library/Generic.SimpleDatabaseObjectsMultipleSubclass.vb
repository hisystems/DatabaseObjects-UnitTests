Imports System
Imports DatabaseObjects.SQL

Namespace Generic

    <Table(SimpleDatabaseObjectsMultipleSubclass.Name)>
    <DistinctField("PrimaryField", True)>
    Public Class SimpleDatabaseObjectsMultipleSubclass
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsMultipleSubclass(Of SimpleDatabaseObject)

        Public Const Name As String = "Generic-SimpleDatabaseObjectsMultipleSubclass"

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

            createTable.Fields.Add("Field1", DataType.VariableCharacter, 100)

            Return createTable

        End Function

        Protected Overrides Function ItemInstanceForSubclass_(objFieldValues As SQL.SQLFieldValues) As SimpleDatabaseObject

            Return New SimpleDatabaseObject(Me)

        End Function

    End Class

End Namespace
