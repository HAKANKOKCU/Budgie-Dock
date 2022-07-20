﻿Public Class ItemListing
    Dim itlist As New ArrayList
    Dim sepid As Integer = 0
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        For Each a As String In My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data").Split("|")
            itlist.Add(a)
            Try
                itemslist.Items.Add(a.Split("*")(2))
            Catch
                itemslist.Items.Add("-- Separator " + sepid.ToString + " --")
                sepid += 1
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
        itlist.Add("sep")
        itemslist.Items.Add("-- Separator " + sepid.ToString + " --")
        sepid += 1
    End Sub

    Private Sub Window_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
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
            My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data", filecontent, False)
            e.Cancel = False
        End If
    End Sub
End Class
