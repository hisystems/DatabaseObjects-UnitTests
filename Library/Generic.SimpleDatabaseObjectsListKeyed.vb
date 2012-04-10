Imports System
Imports DatabaseObjects.SQL

Namespace Generic

    <Table(SimpleDatabaseObjectsListKeyed.Name)>
    <DistinctField("PrimaryField", True)>
    <OrderByField("Field1")>
    <KeyField("KeyField")>
    Public Class SimpleDatabaseObjectsListKeyed
        Inherits Global.DatabaseObjects.Generic.DatabaseObjectsListKeyed(Of SimpleKeyedDatabaseObject, String)

        Public Const Name As String = "Generic-SimpleDatabaseObjectsListKeyed"

        Friend Sub New(rootContainer As RootContainer)

            MyBase.New(rootContainer)

        End Sub

        Friend Sub New(database As Database)

            MyBase.New(database)

        End Sub

        Public Function First() As SimpleKeyedDatabaseObject

            Return MyBase.ObjectByOrdinalFirst

        End Function

        Public Function Last() As SimpleKeyedDatabaseObject

            Return MyBase.ObjectByOrdinalLast

        End Function

        Public Sub Delete(item As SimpleKeyedDatabaseObject)

            MyBase.ObjectDelete(item)

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
