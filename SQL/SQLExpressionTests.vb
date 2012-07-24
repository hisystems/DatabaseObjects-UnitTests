Imports System.Text
Imports DatabaseObjects.SQL
Imports DatabaseObjects.SQL.Serializers

<TestClass()>
Public Class SQLExpressionTests

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub AddFieldAndOne()

        Dim expression = New SQLFieldExpression("Field") + 1

		Assert.AreEqual("([Field] + 1)", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub AddOneAndField()

        Dim expression = 1 + New SQLFieldExpression("Field")

		Assert.AreEqual("(1 + [Field])", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub AddFieldAndDate()

        Dim expression = New SQLFieldExpression("Field") + New DateTime(2000, 1, 1)

		Assert.AreEqual("([Field] + '2000-1-1')", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub AddDateAndField()

        Dim expression = New DateTime(2000, 1, 1) + New SQLFieldExpression("Field")

		Assert.AreEqual("('2000-1-1' + [Field])", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SubtractFieldFromOne()

        Dim expression = New SQLFieldExpression("Field") - 1

		Assert.AreEqual("([Field] - 1)", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SubtractOneFromField()

        Dim expression = 1 - New SQLFieldExpression("Field")

		Assert.AreEqual("(1 - [Field])", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SubtractFieldFromDate()

        Dim expression = New SQLFieldExpression("Field") - New DateTime(2000, 1, 1)

		Assert.AreEqual("([Field] - '2000-1-1')", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub SubtractDateFromField()

        Dim expression = New DateTime(2000, 1, 1) - New SQLFieldExpression("Field")

		Assert.AreEqual("('2000-1-1' - [Field])", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub MultiplyFieldWithOne()

        Dim expression = New SQLFieldExpression("Field") * 1

		Assert.AreEqual("([Field] * 1)", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub MultiplyOneWithField()

        Dim expression = 1 * New SQLFieldExpression("Field")

		Assert.AreEqual("(1 * [Field])", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub DivideFieldByOne()

        Dim expression = New SQLFieldExpression("Field") / 1

		Assert.AreEqual("([Field] / 1)", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub DivideOneByField()

        Dim expression = 1 / New SQLFieldExpression("Field")

		Assert.AreEqual("(1 / [Field])", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub ModulusFieldByOne()

        Dim expression = New SQLFieldExpression("Field") Mod 1

		Assert.AreEqual("([Field] % 1)", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub ModulusOneByField()

        Dim expression = 1 Mod New SQLFieldExpression("Field")

		Assert.AreEqual("(1 % [Field])", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub BitwiseFieldAndOne()

        Dim expression = New SQLFieldExpression("Field") And 1

		Assert.AreEqual("([Field] & 1)", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub BitwiseOneAndField()

        Dim expression = 1 And New SQLFieldExpression("Field")

		Assert.AreEqual("(1 & [Field])", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub BitwiseFieldOrOne()

        Dim expression = New SQLFieldExpression("Field") Or 1

		Assert.AreEqual("([Field] | 1)", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub BitwiseOneOrField()

        Dim expression = 1 Or New SQLFieldExpression("Field")

		Assert.AreEqual("(1 | [Field])", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub ConcatenateField1WithField2()

        'Will revert to using arithmetic + because neither of the expressions are of type string or SQLStringConcatExpression.
        Dim expression = New SQLFieldExpression("Field1") + New SQLFieldExpression("Field2")

		Assert.AreEqual("([Field1] + [Field2])", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub ConcatenateSQLStringContactExpressionWithField2()

        'Will not revert to arithmetic + because "Value" implies that the expression type is string 
        Dim expression = ("A'B" + New SQLFieldExpression("Field1")) + New SQLFieldExpression("Field2")

		Assert.AreEqual("'A''B' + [Field1] + [Field2]", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub ConcatenateField1WithSQLStringContactExpression()

        'Will not revert to arithmetic + because "Value" implies that the expression type is string 
        Dim expression = New SQLFieldExpression("Field1") + (New SQLFieldExpression("Field2") + "A'B")

		Assert.AreEqual("[Field1] + [Field2] + 'A''B'", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub ConcatenateFieldWithValue()

        Dim expression = New SQLFieldExpression("Field") + "Value"

		Assert.AreEqual("[Field] + 'Value'", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub ConcatenateValueWithField()

        Dim expression = "Value" + New SQLFieldExpression("Field")

		Assert.AreEqual("'Value' + [Field]", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub ConcatenateFieldAndStringUsingAmpersand()

        Dim expression = New SQLFieldExpression("Field") + "Value"

		Assert.AreEqual("[Field] + 'Value'", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub ConcatenateStringAndFieldUsingAmpersand()

        Dim expression = "Value" + New SQLFieldExpression("Field")

		Assert.AreEqual("'Value' + [Field]", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

    <TestMethod()>
    <TestCategory("SQL"), TestCategory("SQLExpression")>
    Public Sub MultipleArithmeticOperations()

        Dim expression = ((((New SQLFieldExpression("Field") + 2) - 3) * 4) / 5)

		Assert.AreEqual("(((([Field] + 2) - 3) * 4) / 5)", expression.SQL(Serializers.Items(Database.ConnectionType.SQLServer)))

    End Sub

End Class
