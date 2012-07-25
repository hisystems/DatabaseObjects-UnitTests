Imports DatabaseObjects.SQL
Imports System.Runtime.CompilerServices

Public Module DatabaseExtensions

	<Extension()>
	Public Sub DropTableIfExists(database As Database, tableName As String)

		Using connection As New ConnectionScope(database)
			Dim tableExists As Boolean

			Using reader = connection.Execute(New SQLTableExists(tableName))
				tableExists = reader.Read
			End Using

			If tableExists Then
				connection.Execute(New SQLDropTable(tableName))
			End If
		End Using

	End Sub

	<Extension()>
	Public Sub DropViewIfExists(database As Database, viewName As String)

		Using connection As New ConnectionScope(database)
			Dim viewExists As Boolean

			Using reader = connection.Execute(New SQLViewExists(viewName))
				viewExists = reader.Read
			End Using

			If viewExists Then
				connection.Execute(New SQLDropView(viewName))
			End If
		End Using

	End Sub

	''' <summary>
	''' Drops the existing table if it exists and then creates the table.
	''' </summary>
	<Extension()>
	Public Sub RecreateTable(database As Database, createTable As SQLCreateTable)

		Using connection = New ConnectionScope(database)
			database.DropTableIfExists(createTable.Name)
			connection.Execute(createTable)
		End Using

	End Sub

End Module
