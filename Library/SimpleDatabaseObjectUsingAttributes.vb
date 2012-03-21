Imports System
Imports DatabaseObjects.SQL

Public Class SimpleDatabaseObjectUsingAttributes
    Inherits DatabaseObjectUsingAttributes

    <FieldMapping("Field1")>
    Public Field1 As String

    Friend Sub New(parent As DatabaseObjects)

        MyBase.New(parent)

    End Sub

    Public Shadows Sub Save()

        MyBase.Save()

    End Sub

End Class