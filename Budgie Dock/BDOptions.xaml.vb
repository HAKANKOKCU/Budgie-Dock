Imports Microsoft.Win32
Public Class BDOptions
    Dim acs As Boolean = False
    Public filepick As New OpenFileDialog
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
        ithmPath.Content = My.Settings.CurrentIconThemePath
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
        iuR.Text = My.Settings.iuRed
        iuG.Text = My.Settings.iuGreen
        iuB.Text = My.Settings.iuBlue
        dciara.IsChecked = My.Settings.ApplyDockColorAtIsAppRuning
        useastb.IsChecked = My.Settings.UseDockAsTaskbar
        dps.SelectedIndex = IIf(My.Settings.pos = "Bottom", 0, 1)
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

    Private Sub pickfile_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles pickfile.MouseUp
        filepick.Filter = "INI|*.ini"
        If filepick.ShowDialog = True Then
            My.Settings.CurrentIconThemePath = filepick.FileName
            ithmPath.Content = My.Settings.CurrentIconThemePath
        End If
    End Sub

    Private Sub ithmPath_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ithmPath.MouseUp
        If MsgBox("Remove Icon Pack?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.Yes Then
            My.Settings.CurrentIconThemePath = ""
            ithmPath.Content = My.Settings.CurrentIconThemePath
        End If
    End Sub

    Private Sub iuR_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles iuR.TextChanged
        Try
            If acs Then
                My.Settings.iuRed = iuR.Text
                My.Settings.Save()
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub iuG_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles iuG.TextChanged
        Try
            If acs Then
                My.Settings.iuGreen = iuG.Text
                My.Settings.Save()
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub iuB_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles iuB.TextChanged
        Try
            If acs Then
                My.Settings.iuBlue = iuB.Text
                My.Settings.Save()
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub dciara_Clicked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles dciara.Click
        If acs Then
            My.Settings.ApplyDockColorAtIsAppRuning = dciara.IsChecked
            My.Settings.Save()
        End If
    End Sub

    Private Sub useastb_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles useastb.Click
        If acs Then
            My.Settings.UseDockAsTaskbar = useastb.IsChecked
            My.Settings.Save()
        End If
    End Sub

    Private Sub dps_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles dps.SelectionChanged
        If acs Then
            My.Settings.pos = dps.SelectedItem.Content
            My.Settings.Save()
        End If
    End Sub
End Class
