Public Class UpdateIconsData
    Public Shared Sub UpdateNow()
        Dim iconsFile As String = My.Computer.FileSystem.ReadAllText(
            My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data")
        Dim newicofile As String = ""
        Dim isfirst = True
        For Each a As String In iconsFile.Split("|")
            If Not isfirst Then
                newicofile += "|"
            End If
            If a.Contains("*") Then
                Dim mt = a.Split("*")(0)
                If Not mt.Contains(":") Then
                    newicofile += "icon:" + a.Replace(":", "{BD-TD-}")
                ElseIf Not mt.Split(":")(0) = "icon" Then
                    newicofile += "icon:" + a.Replace(":", "{BD-TD-}")
                Else
                    newicofile += a
                End If
            Else
                newicofile += a
            End If
            isfirst = False
        Next
        My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data", newicofile, False)
    End Sub
End Class
