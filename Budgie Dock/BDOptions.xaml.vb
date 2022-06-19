Public Class BDOptions
    Dim acs As Boolean = False
    Private Sub restartapp_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles restartapp.Click
        System.Windows.Forms.Application.Restart()
        End
    End Sub

    Private Sub as_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles asc.TextChanged
        Try
            If acs Then
                If Not asc.Text = 0 Then
                    My.Settings.animatescale = asc.Text
                    My.Settings.Save()
                Else
                    asc.Text = 1
                    My.Settings.animatescale = asc.Text
                    My.Settings.Save()
                End If
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        asc.Text = My.Settings.animatescale.ToString
        isz.Text = My.Settings.Size
        acs = True
    End Sub

    Private Sub isz_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles isz.TextChanged
        Try
            If acs Then
                My.Settings.Size = isz.Text
                My.Settings.Save()
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub
End Class
