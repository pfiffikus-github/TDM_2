Imports System.Windows.Forms
Imports MDM.DataLayer

Public Class frmBackUpConfig

    Private myBackUp As BackUp

    Public Function ShowDiaAndEditObject(ByVal locObject As BackUp) As BackUp
        Using Me
            With Me
                .PropertyGrid.SelectedObject = locObject.Clone
                .myBackUp = .PropertyGrid.SelectedObject
                .ShowDialog()
                Return myBackUp
            End With
        End Using
    End Function

    Private Sub OK_Button_Click(sender As System.Object, e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As System.Object, e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Protected Overrides Sub OnShown(e As System.EventArgs)
        MyBase.OnShown(e)
        PropertyGrid_PropertyValueChanged(Nothing, Nothing)
    End Sub

    Protected Overrides Sub OnClosing(e As System.ComponentModel.CancelEventArgs)
        MyBase.OnClosing(e)
        If Me.DialogResult = Windows.Forms.DialogResult.OK Then
            myBackUp = PropertyGrid.SelectedObject
        Else
            myBackUp = Nothing
        End If
    End Sub

    Private Sub PropertyGrid_PropertyValueChanged(s As System.Object, e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles PropertyGrid.PropertyValueChanged
        Me.Text = myBackUp.ToString & " - BackUp-Konfuguration"
    End Sub

End Class


