﻿Imports System
Imports DatabaseObjects.SQL

Public Class SimpleDatabaseObjectsVolatile
    Inherits DatabaseObjectsVolatile

    Public Const Name As String = "SimpleDatabaseObjectsVolatile"

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

    Protected Overrides Function DistinctFieldAutoIncrements() As Boolean

        Return True

    End Function

    Protected Overrides Function DistinctFieldName() As String

        Return "PrimaryField"

    End Function

    Protected Overrides Function ItemInstance() As IDatabaseObject

        Return New SimpleDatabaseObjectUsingAttributes(Me)

    End Function

    Protected Overrides Function KeyFieldName() As String

        Return "KeyField"

    End Function

    Protected Overrides Function OrderBy() As SQL.SQLSelectOrderByFields

        Return Nothing

    End Function

    Protected Overrides Function Subset() As SQL.SQLConditions

        Return Nothing

    End Function

    Protected Overrides Function TableJoins(objPrimaryTable As SQL.SQLSelectTable, objTables As SQL.SQLSelectTables) As SQL.SQLSelectTableJoins

        Return Nothing

    End Function

    Protected Overrides Function TableName() As String

        Return Name

    End Function

End Class

