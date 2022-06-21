Public Class BDOptions
    Dim acs As Boolean = False
    Private Sub restartapp_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles restartapp.MouseUp
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
        dopac.Text = My.Settings.dockopacity
        dcor.Text = My.Settings.dockcr
        docR.Text = My.Settings.dockRed
        docG.Text = My.Settings.dockGreen
        docB.Text = My.Settings.dockBlue
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

    Private Sub dopac_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles dopac.TextChanged
        Try
            If acs Then
                My.Settings.dockopacity = dopac.Text
                My.Settings.Save()
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub dcor_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles dcor.TextChanged
        Try
            If acs Then
                My.Settings.dockcr = dcor.Text
                My.Settings.Save()
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub docR_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles docR.TextChanged
        Try
            If acs Then
                My.Settings.dockRed = docR.Text
                My.Settings.Save()
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub docG_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles docG.TextChanged
        Try
            If acs Then
                My.Settings.dockGreen = docG.Text
                My.Settings.Save()
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub docB_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles docB.TextChanged
        Try
            If acs Then
                My.Settings.dockBlue = docB.Text
                My.Settings.Save()
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub
End Class
