Imports System
Imports DatabaseObjects.SQL

Public Class SimpleKeyedDatabaseObject
    Inherits SimpleDatabaseObject

    <FieldMapping("KeyField")>
    Public KeyField As String

    Friend Sub New(parent As DatabaseObjects)

        MyBase.New(parent)

    End Sub

End Class