Imports System
Imports DatabaseObjects.SQL

Public Class SimpleDatabaseObjectVolatile
    Inherits DatabaseObject

    <FieldMapping("Field1")>
    Public Field1 As String

    Friend Sub New(parent As DatabaseObjects)

        MyBase.New(parent)

    End Sub

    <FieldMapping("ParentPrimaryField")>
    Private ReadOnly Property ParentPrimaryField As Integer
        Get

            Return MyBase.GrandParentDistinctValue

        End Get
    End Property

End Class