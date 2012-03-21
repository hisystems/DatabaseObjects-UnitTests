Imports System
Imports DatabaseObjects.SQL

Public Class SimpleKeyedDatabaseObjectUsingAttributes
    Inherits SimpleDatabaseObjectUsingAttributes

    <FieldMapping("KeyField")>
    Public KeyField As String

    Friend Sub New(parent As DatabaseObjects)

        MyBase.New(parent)

    End Sub

End Class