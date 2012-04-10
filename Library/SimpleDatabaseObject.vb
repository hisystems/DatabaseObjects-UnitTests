Imports System
Imports DatabaseObjects.SQL

Public Class SimpleDatabaseObject
    Inherits DatabaseObject

    <FieldMapping("Field1")>
    Public Field1 As String

    Private _childObjects As Generic.SimpleDatabaseObjectsVolatile

    Friend Sub New(parent As DatabaseObjects)

        MyBase.New(parent)

    End Sub

    Public ReadOnly Property ChildObjects As Generic.SimpleDatabaseObjectsVolatile
        Get

            If _childObjects Is Nothing Then
                _childObjects = New Generic.SimpleDatabaseObjectsVolatile(Me)
            End If

            Return _childObjects

        End Get
    End Property

    Public Shadows Sub Save()

        MyBase.Save()

        If _childObjects IsNot Nothing Then
            _childObjects.SaveAll()
        End If

    End Sub

End Class