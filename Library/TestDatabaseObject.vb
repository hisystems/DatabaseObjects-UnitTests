Public Class TestDatabaseObject
	Implements IDatabaseObject

	Private _distinctValue As Object
	Private _isSaved As Boolean

	Public Property DistinctValue As Object Implements IDatabaseObject.DistinctValue
		Get

			Return _distinctValue

		End Get

		Set(value As Object)

			_distinctValue = value

		End Set
	End Property

	Public Property IsSaved As Boolean Implements IDatabaseObject.IsSaved
		Get

			Return _isSaved

		End Get

		Set(value As Boolean)

			_isSaved = value

		End Set
	End Property

	Public Sub LoadFields(objFields As SQL.SQLFieldValues) Implements IDatabaseObject.LoadFields

		Throw New NotImplementedException

	End Sub

	Public Function SaveFields() As SQL.SQLFieldValues Implements IDatabaseObject.SaveFields

		Throw New NotImplementedException

	End Function

End Class
