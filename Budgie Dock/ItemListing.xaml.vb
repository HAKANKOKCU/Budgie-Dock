Public Class ItemListing
    Dim itlist As New ArrayList
    Dim sepid As Integer = 0
    Dim dontallowclosing As Boolean = True
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        For Each a As String In My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons" + SettingsLoadID + ".data").Split("|")
            itlist.Add(a)
            Try
                itemslist.Items.Add(a.Split("*")(2))
            Catch
                If a = "sep" Then
                    itemslist.Items.Add("-- Separator " + sepid.ToString + " --")
                    sepid += 1
                Else
                    itemslist.Items.Add("-- Right Dock Contents Start --")
                End If
            End Try
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
        My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons" + SettingsLoadID + ".data", filecontent, False)
        dontallowclosing = False
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
            itemslist.ScrollIntoView(itemslist.SelectedItem)
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
            itemslist.ScrollIntoView(itemslist.SelectedItem)
        Catch
        End Try
    End Sub

    Private Sub del_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles del.MouseUp
        Try
            Dim selid = itemslist.SelectedIndex
            itemslist.Items.RemoveAt(selid)
            itlist.RemoveAt(selid)
        Catch
        End Try
    End Sub

    Private Sub adsep_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles adsep.MouseUp
        Dim dig As New DockElemAddDiag
        dig.ShowDialog()
        If dig.result = "sep" Then
            itlist.Add("sep")
            itemslist.Items.Add("-- Separator " + sepid.ToString + " --")
            sepid += 1
        ElseIf dig.result = "rightstart" Then
            itlist.Add("rightstart")
            itemslist.Items.Add("-- Right Dock Contents Start --")
        End If
    End Sub

    Private Sub Window_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If dontallowclosing Then
            e.Cancel = True
            Dim qs As MsgBoxResult = MsgBox("Do You Want To Discard The Changes? (Click ""No"" To Save)", MsgBoxStyle.Question + MsgBoxStyle.YesNoCancel, "Unsaved Changes")
            If qs = MsgBoxResult.Yes Then
                e.Cancel = False
            ElseIf qs = MsgBoxResult.No Then
                Dim filecontent As String = ""
                Dim firstone As Boolean = True
                For Each a As String In itlist
                    If Not firstone Then
                        filecontent += "|"
                    End If
                    filecontent += a
                    firstone = False
                Next
                My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons" + SettingsLoadID + ".data", filecontent, False)
                e.Cancel = False
            End If
        End If
    End Sub
End Class
