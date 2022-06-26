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
                    My.Settings.animatescale = 1
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
        scolorR.Text = My.Settings.separatorRed
        scolorG.Text = My.Settings.SeparatorGreen
        scolorB.Text = My.Settings.SeparatorBlue
        ahd.IsChecked = My.Settings.autoHide
        tpm.IsChecked = My.Settings.topMost
        dmtop.Text = My.Settings.paddingTop
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

    Private Sub scolorR_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles scolorR.TextChanged
        Try
            If acs Then
                My.Settings.separatorRed = scolorR.Text
                My.Settings.Save()
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub scolorG_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles scolorG.TextChanged
        Try
            If acs Then
                My.Settings.SeparatorGreen = scolorG.Text
                My.Settings.Save()
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub scolorB_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles scolorB.TextChanged
        Try
            If acs Then
                My.Settings.SeparatorBlue = scolorB.Text
                My.Settings.Save()
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub ahd_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ahd.Click
        If acs Then
            My.Settings.autoHide = ahd.IsChecked
            My.Settings.Save()
        End If
    End Sub

    Private Sub tpm_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles tpm.Click
        If acs Then
            My.Settings.topMost = tpm.IsChecked
            My.Settings.Save()
        End If
    End Sub

    Private Sub dmtop_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles dmtop.TextChanged
        Try
            If acs Then
                My.Settings.paddingTop = dmtop.Text
                My.Settings.Save()
            End If
        Catch ex As Exception
            If Not dmtop.Text = "-" Then MsgBox("Please Enter A Valid Number")
        End Try
    End Sub
End Class
