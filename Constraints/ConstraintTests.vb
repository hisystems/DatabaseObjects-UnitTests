Option Strict On
Option Explicit On

Imports System.Text

<TestClass()>
Public Class ConstraintTests

    Private Class TestTrueConstraint
        Implements Constraints.IConstraint(Of Boolean)

        Public Function ValueSatisfiesConstraint(value As Boolean) As Boolean Implements Constraints.IConstraint(Of Boolean).ValueSatisfiesConstraint

            Return value = True

        End Function

    End Class

    <TestMethod()>
    <TestCategory("Constraint")>
    Public Sub ConstraintValueSatisfiesConstraint()

        Dim constraint As New TestTrueConstraint

        Assert.IsTrue(constraint.ValueSatisfiesConstraint(True))

    End Sub

    <TestMethod()>
    <TestCategory("Constraint")>
    Public Sub ConstraintBindingEnsureConstraintSatisfied()

        Dim constraint As New TestTrueConstraint
        Dim constraintBinding As New Constraints.ConstraintBinding(Of Boolean)(Function() True, constraint)

        constraintBinding.EnsureConstraintSatisfied()

    End Sub

    <TestMethod()>
    <TestCategory("Constraint")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub ConstraintBindingEnsureConstraintSatisfiedFails()

        Dim constraint As New TestTrueConstraint
        Dim constraintBinding As New Constraints.ConstraintBinding(Of Boolean)(Function() False, constraint)

        constraintBinding.EnsureConstraintSatisfied()

    End Sub

    <TestMethod()>
    <TestCategory("Constraint")>
    Public Sub ConstraintBindingCloneAndEnsureConstraintSatisfied()

        Dim constraint As New TestTrueConstraint
        Dim constraintBinding As New Constraints.ConstraintBinding(Of Boolean)(Function() False, constraint)

        constraintBinding.Clone(True).EnsureConstraintSatisfied()

    End Sub

    <TestMethod()>
    <TestCategory("Constraint")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub ConstraintBindingCloneAndEnsureConstraintSatisfiedFails()

        Dim constraint As New TestTrueConstraint
        Dim constraintBinding As New Constraints.ConstraintBinding(Of Boolean)(Function() False, constraint)

        constraintBinding.Clone(False).EnsureConstraintSatisfied()

    End Sub

    <TestMethod()>
    <TestCategory("Constraint")>
    Public Sub ConstraintBindingDefaultErrorMessage()

        Dim constraint As New TestTrueConstraint
        Dim constraintBinding As New Constraints.ConstraintBinding(Of Boolean)(Function() False, constraint)

        Assert.AreNotEqual(String.Empty, constraintBinding.ErrorMessage())

    End Sub

    <TestMethod()>
    <TestCategory("Constraint")>
    Public Sub ConstraintBindingCustomErrorMessage()

        Dim constraint As New TestTrueConstraint
        Dim constraintBinding As New Constraints.ConstraintBinding(Of Boolean)(Function() False, constraint, "Value '{0}' is not true")

        Assert.AreEqual("Value 'False' is not true", constraintBinding.ErrorMessage())

    End Sub

    <TestMethod()>
    <TestCategory("Constraint")>
    Public Sub RegExConstraint()

        Dim constraint As New Constraints.RegExConstraint("[a-c]{3}")
        Assert.IsTrue(DirectCast(constraint, Constraints.IConstraint(Of String)).ValueSatisfiesConstraint("abc"))

    End Sub

    <TestMethod()>
    <TestCategory("Constraint")>
    Public Sub RegExConstraintDoesNotMatch()

        Dim constraint As New Constraints.RegExConstraint("[a-c]{3}")
        Assert.IsFalse(DirectCast(constraint, Constraints.IConstraint(Of String)).ValueSatisfiesConstraint("123"))

    End Sub

    <TestMethod()>
    <TestCategory("Constraint")>
    Public Sub Constraint()

        Dim constraint As New Constraints.Constraint(Of Integer)(Function(input) input > 0)

        Assert.IsTrue(DirectCast(constraint, Constraints.IConstraint(Of Integer)).ValueSatisfiesConstraint(1))

    End Sub

    <TestMethod()>
    <TestCategory("Constraint")>
    Public Sub ConstraintDoesNotMatch()

        Dim constraint As New Constraints.Constraint(Of Integer)(Function(input) input > 0)

        Assert.IsFalse(DirectCast(constraint, Constraints.IConstraint(Of Integer)).ValueSatisfiesConstraint(-1))

    End Sub

End Class
