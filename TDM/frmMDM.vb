Imports MDM.LogicLayer
Imports MDM.DataLayer




Public Class frmMDM

    Private Sub frmTDM_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim x As New MDM.DataLayer.LocalSettings

        PropertyGrid1.SelectedObject = x

        Try

            With Me
                .Text = Application.ProductName & " V" & Application.ProductVersion.IndexOf(".")
                .TreeView.Nodes.Item(0).Text = .Text
                .dateTimeTimer.Start() : dateTimeTimer_Tick(Nothing, Nothing)
            End With

        Catch ex As Exception
            Stop
        End Try

    End Sub

    Private Sub dateTimeTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dateTimeTimer.Tick

        With Me
            .ToolStripStatusDateTime.Text = Date.Now.ToString("dd.MM.yy HH:mm")
            .ToolStripStatusNetworkState.Text = "Netzwerk " & IIf(My.Computer.Network.IsAvailable, "", "nicht ") & "verbunden |"
        End With

    End Sub


    Private Sub PropertyGrid2_Click(sender As System.Object, e As System.EventArgs) Handles PropertyGrid2.Click

    End Sub
End Class


