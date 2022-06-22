Public Class Sepremover
    Public WithEvents sepgrid As Grid
    Property mainwin As MainWindow
    Property id As Integer
    Private Sub sepgrid_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles sepgrid.MouseDown
        If e.ChangedButton = MouseButton.Right Then
            If MsgBox("Do you want to remove this separator?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) Then
                mainwin.iconlist.RemoveAt(id)
                mainwin.savicon()
                mainwin.reicon()
            End If
        End If
    End Sub
End Class
