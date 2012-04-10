Imports System
Imports DatabaseObjects.SQL

Namespace Generic

    <Table(SimpleDatabaseObjectsList.Name)>
    <DistinctField("PrimaryField", True)>
    Public Class SimpleDatabaseObjectsList
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsList(Of SimpleDatabaseObject)

        Public Const Name As String = "Generic-SimpleDatabaseObjectsList"

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

    End Class

End Namespace
