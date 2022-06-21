Public Class ItemListing
    Dim itlist As New ArrayList
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        For Each a As String In My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data").Split("|")
            itlist.Add(a)
            itemslist.Items.Add(a.Split("*")(2))
        Next
    End Sub

    Private Sub closebtn_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles closebtn.MouseUp
        Dim filecontent As String = ""
        Dim firstone As Boolean = True
        For Each a As String In itlist
            If Not firstone Then
                filecontent += "|"
            End If
            filecontent += a
            firstone = False
        Next
        My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data", filecontent, False)
        Me.Close()
    End Sub

    Private Sub moveup_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles moveup.MouseUp
        Try
            Dim selid = itemslist.SelectedIndex
            Dim cntunselect As String = itemslist.Items(selid - 1)
            Dim usl As String = itlist(selid - 1)
            itemslist.Items(selid - 1) = itemslist.SelectedItem
            itlist(selid - 1) = itlist(itemslist.SelectedIndex)
            itemslist.Items(selid) = cntunselect
            itlist(selid) = usl
            itemslist.SelectedItem = itemslist.Items(selid - 1)
        Catch
        End Try
    End Sub

    Private Sub movedown_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles movedown.MouseUp
        Try
            Dim selid = itemslist.SelectedIndex
            Dim cntunselect As String = itemslist.Items(selid + 1)
            Dim usl As String = itlist(selid + 1)
            itemslist.Items(selid + 1) = itemslist.SelectedItem
            itlist(selid + 1) = itlist(selid)
            itemslist.Items(selid) = cntunselect
            itlist(selid) = usl
            itemslist.SelectedItem = itemslist.Items(selid + 1)
        Catch
        End Try
    End Sub
End Class
