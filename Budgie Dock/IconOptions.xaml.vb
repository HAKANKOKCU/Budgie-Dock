Public Class IconOptions
    Property iconid As Integer = 0
    Property mainwin As MainWindow
    Property iconn As iconobj
    Private Sub IconOptions_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        AppShell.Text = mainwin.iconlist(iconid)(0)
        IconPath.Text = mainwin.iconlist(iconid)(1)
        Appname.Text = mainwin.iconlist(iconid)(2)
    End Sub

    Private Sub ApplyBTN_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ApplyBTN.MouseUp
        mainwin.iconlist(iconid)(0) = AppShell.Text
        mainwin.iconlist(iconid)(1) = IconPath.Text
        mainwin.iconlist(iconid)(2) = Appname.Text
        iconn.appname = Appname.Text
        iconn.iconpath = IconPath.Text
        iconn.apppath = AppShell.Text
        iconn.endinit()
    End Sub


    Private Sub EditBTN_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles EditBTN.MouseUp
        Process.Start(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data")
    End Sub
End Class
