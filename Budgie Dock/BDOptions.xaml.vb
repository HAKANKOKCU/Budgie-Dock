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
                    SetSetting("animateScale", asc.Text, True)
                Else
                    SetSetting("animateScale", 1, True)
                End If
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        ithmPath.Content = GetSetting("currentIconThemePath")
        asc.Text = GetSetting("animateScale")
        isz.Text = GetSetting("size")
        dopac.Text = GetSetting("dockOpacity")
        dcor.Text = GetSetting("dockCornerRadius")
        docR.Text = GetSetting("dockRed")
        docG.Text = GetSetting("dockGreen")
        docB.Text = GetSetting("dockBlue")
        scolorR.Text = GetSetting("separatorRed")
        scolorG.Text = GetSetting("separatorGreen")
        scolorB.Text = GetSetting("separatorBlue")
        ahd.IsChecked = GetSetting("autoHide") = 1
        tpm.IsChecked = GetSetting("topMost") = 1
        dmtop.Text = GetSetting("paddingTop")
        iuR.Text = GetSetting("isAppRuningRed")
        iuG.Text = GetSetting("isAppRuningGreen")
        iuB.Text = GetSetting("isAppRuningBlue")
        dciara.IsChecked = GetSetting("applyDockColorAtIsAppRuning") = 1
        useastb.IsChecked = GetSetting("useDockAsTaskbar") = 1
        dps.SelectedIndex = IIf(GetSetting("pos") = "Bottom", 0, IIf(GetSetting("pos") = "Top", 1, IIf(GetSetting("pos") = "Right", 2, 3)))
        lmappsdw.IsChecked = GetSetting("lightThemeInAppsDrawer") = 1
        For i As Integer = 0 To Forms.Screen.AllScreens.Count - 1
            stp.Items.Add(i + 1)
        Next
        stp.SelectedIndex = GetSetting("placedScreenID")
        acs = True
    End Sub

    Private Sub isz_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles isz.TextChanged
        Try
            If acs Then
                SetSetting("size", isz.Text, True)
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub dopac_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles dopac.TextChanged
        Try
            If acs Then
                SetSetting("dockOpacity", dopac.Text, True)
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub dcor_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles dcor.TextChanged
        Try
            If acs Then
                SetSetting("dockCornerRadius", dcor.Text, False)
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub docR_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles docR.TextChanged
        Try
            If acs Then
                SetSetting("dockRed", docR.Text, True)
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub docG_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles docG.TextChanged
        Try
            If acs Then
                SetSetting("dockGreen", docG.Text, True)
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub docB_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles docB.TextChanged
        Try
            If acs Then
                SetSetting("dockBlue", docB.Text, True)
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub scolorR_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles scolorR.TextChanged
        Try
            If acs Then
                SetSetting("separatorRed", scolorR.Text, True)
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub scolorG_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles scolorG.TextChanged
        Try
            If acs Then
                SetSetting("separatorGreen", scolorG.Text, True)
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub scolorB_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles scolorB.TextChanged
        Try
            If acs Then
                SetSetting("separatorBlue", scolorB.Text, True)
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub ahd_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ahd.Click
        If acs Then
            SetSetting("autoHide", IIf(ahd.IsChecked, 1, 0), True)
        End If
    End Sub

    Private Sub tpm_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles tpm.Click
        If acs Then
            SetSetting("topMost", IIf(tpm.IsChecked, 1, 0), True)
        End If
    End Sub

    Private Sub dmtop_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles dmtop.TextChanged
        Try
            If acs Then
                SetSetting("paddingTop", dmtop.Text, True)
            End If
        Catch ex As Exception
            If Not dmtop.Text = "-" Then MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub pickfile_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles pickfile.MouseUp
        filepick.Filter = "INI|*.ini"
        If filepick.ShowDialog = True Then
            SetSetting("currentIconThemePath", filepick.FileName, False)
            ithmPath.Content = GetSetting("currentIconThemePath")
        End If
    End Sub

    Private Sub ithmPath_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ithmPath.MouseUp
        If MsgBox("Remove Icon Pack?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.Yes Then
            SetSetting("currentIconThemePath", filepick.FileName, False)
            ithmPath.Content = GetSetting("currentIconThemePath")
        End If
    End Sub

    Private Sub iuR_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles iuR.TextChanged
        Try
            If acs Then
                SetSetting("isAppRuningRed", iuR.Text, True)
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub iuG_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles iuG.TextChanged
        Try
            If acs Then
                SetSetting("isAppRuningGreen", iuG.Text, True)
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub iuB_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles iuB.TextChanged
        Try
            If acs Then
                SetSetting("isAppRuningBlue", iuB.Text, True)
            End If
        Catch ex As Exception
            MsgBox("Please Enter A Valid Number")
        End Try
    End Sub

    Private Sub dciara_Clicked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles dciara.Click
        If acs Then
            SetSetting("applyDockColorAtIsAppRuning", IIf(dciara.IsChecked, 1, 0), True)
        End If
    End Sub

    Private Sub useastb_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles useastb.Click
        If acs Then
            SetSetting("useDockAsTaskbar", IIf(useastb.IsChecked, 1, 0), True)
        End If
    End Sub

    Private Sub dps_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles dps.SelectionChanged
        If acs Then
            SetSetting("pos", dps.SelectedItem.Content, False)
        End If
    End Sub

    Private Sub exit_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles [Exit].MouseUp
        End
    End Sub

    Private Sub lmappsdw_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles lmappsdw.Click
        If acs Then
            SetSetting("lightThemeInAppsDrawer", IIf(lmappsdw.IsChecked, 1, 0), False)
        End If
    End Sub

    Private Sub stp_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles stp.SelectionChanged
        If acs Then
            SetSetting("placedScreenID", stp.SelectedIndex, True)
        End If
    End Sub
End Class
