Public Class IconOptions
    Property iconid As Integer = 0
    Property mainwin As MainWindow
    Property iconn As iconobj
    Private Sub IconOptions_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        AppShell.Text = mainwin.iconlist(iconid)(1).Replace("{BD-TD-}", ":")
        IconPath.Text = mainwin.iconlist(iconid)(2).Replace("{BD-TD-}", ":")
        Appname.Text = mainwin.iconlist(iconid)(3).Replace("{BD-TD-}", ":")
        Try
            Dim bi As New BitmapImage
            bi.BeginInit()
            bi.UriSource = New Uri(mainwin.iconlist(iconid)(2).Replace("{BD-TD-}", ":"))
            bi.EndInit()
            Me.Icon = bi
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub ApplyBTN_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ApplyBTN.MouseUp
        mainwin.iconlist(iconid)(1) = AppShell.Text.Replace("*", "{BD-STAR-}").Replace("|", "{BD-FLINE-}").Replace(":", "{BD-TD-}")
        mainwin.iconlist(iconid)(2) = IconPath.Text.Replace(":", "{BD-TD-}")
        mainwin.iconlist(iconid)(3) = Appname.Text.Replace("*", "{BD-STAR-}").Replace("|", "{BD-FLINE-}").Replace(":", "{BD-TD-}")
        iconn.appname = Appname.Text
        iconn.iconpath = IconPath.Text
        If AppShell.Text.Split("^").Count > 1 Then
            iconn.apparams = AppShell.Text.Split("^")(1).Replace("{BD-UPL-}", "^")
        Else
            iconn.apparams = ""
        End If
        iconn.apppath = AppShell.Text.Split("^")(0)
        iconn.endinit()
    End Sub


    Private Sub EditBTN_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles EditBTN.MouseUp
        Process.Start(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data")
    End Sub

End Class
