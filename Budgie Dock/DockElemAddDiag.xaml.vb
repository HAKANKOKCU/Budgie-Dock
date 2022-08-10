Public Class DockElemAddDiag
    Property result = Nothing
    Private Sub okbtn_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles okbtn.Click
        result = sCB.Items(sCB.SelectedIndex).Tag
        Close()
    End Sub

    Private Sub cancBTN_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles cancBTN.Click
        Close()
    End Sub
End Class
