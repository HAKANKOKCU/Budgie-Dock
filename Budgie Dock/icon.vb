Public Class icon
    Dim alreadyadded As Boolean = False
    Public WithEvents imageiconobj As New Image
    Property iconpath As String = ""
    Property apppath As String = ""
    Property stackpanel As StackPanel
    Property containerwin As MainWindow
    Property idd As Integer = Nothing
    Property isremoved As Boolean = False
    Property appname As String = ""
    Sub endinit()
        Dim img As New BitmapImage(New Uri(iconpath))
        imageiconobj.Source = img
        imageiconobj.Width = My.Settings.Size
        imageiconobj.Height = My.Settings.Size
        If Not alreadyadded Then
            stackpanel.Children.Add(imageiconobj)
            alreadyadded = True
        End If
    End Sub

    Private Sub imageiconobj_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles imageiconobj.MouseDown
        imageiconobj.Opacity = 0.5
        containerwin.menustack.Visibility = Visibility.Hidden
    End Sub

    Private Sub imageiconobj_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles imageiconobj.MouseEnter
        imageiconobj.Opacity = 0.8
        containerwin.appname.Content = appname
        containerwin.appname.UpdateLayout()
        Canvas.SetLeft(containerwin.appname, System.Windows.Forms.Control.MousePosition.X - containerwin.Left - (containerwin.appname.ActualWidth / 2))
        containerwin.appname.Visibility = Visibility.Visible
    End Sub

    Private Sub imageiconobj_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles imageiconobj.MouseLeave
        imageiconobj.Opacity = 1
        containerwin.appname.Visibility = Visibility.Hidden
    End Sub

    Private Sub imageiconobj_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles imageiconobj.MouseMove
        Canvas.SetLeft(containerwin.appname, System.Windows.Forms.Control.MousePosition.X - containerwin.Left - (containerwin.appname.ActualWidth / 2))
    End Sub

    Private Sub imageiconobj_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles imageiconobj.MouseUp
        imageiconobj.Opacity = 1
        If e.ChangedButton = 0 Then
            Process.Start(apppath)
        ElseIf e.ChangedButton = 2 Then
            containerwin.menustack.Visibility = Visibility.Visible
            Canvas.SetLeft(containerwin.menustack, System.Windows.Forms.Control.MousePosition.X - containerwin.Left - 100)
            containerwin.OptionsIcon = Me
        End If
    End Sub

    Private Sub imageiconobj_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Input.MouseWheelEventArgs) Handles imageiconobj.MouseWheel
        If e.Delta > 0 Then
            containerwin.menustack.Visibility = Visibility.Visible
            containerwin.OptionsIcon = Me
        Else
            containerwin.menustack.Visibility = Visibility.Hidden
        End If
    End Sub

    Sub remove()
        iconpath = ""
        apppath = ""
        Dim img As New BitmapImage()
        imageiconobj.Source = img
        imageiconobj.Visibility = False
        imageiconobj.Width = 1
        imageiconobj = Nothing
        containerwin.OptionsIcon = Nothing
        containerwin.menustack.Visibility = Visibility.Hidden
        isremoved = True
    End Sub
End Class
