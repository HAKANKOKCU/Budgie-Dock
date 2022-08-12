Public Class WebSearch
    Dim sedata As New Dictionary(Of String, Array)
    Property se As String
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        sedata.Add("google", {"Google", "https://www.google.com/search?q=%input%"})
        sedata.Add("yandex", {"Yandex", "https://www.yandex.com/search?text=%input%"})
        sedata.Add("duckduckgo", {"DuckDuckGo", "https://www.duckduckgo.com?q=%input%"})
        sedata.Add("startpage", {"Startpage", "https://www.startpage.com/sp/search?q=%input%"})
        infs.Content = infs.Content.ToString.Replace("{SEARCHENGINE}", sedata(se)(0))
        searchTB.Focus()
        Dim nwLS = My.Settings.LastSearches
        Try
            For i As Integer = nwLS.Count - 1 To nwLS.Count - 8 Step -1
                Dim bdr As New Border
                bdr.CornerRadius = New CornerRadius(15)
                bdr.Padding = New Thickness(3)
                bdr.Background = New SolidColorBrush(Color.FromArgb(100, 255, 255, 255))
                Dim lbl As New Label
                lbl.Content = nwLS(i)
                bdr.Child = lbl
                lsPanel.Children.Add(bdr)
                AddHandler bdr.MouseEnter, Sub(senderer, evt)
                                               senderer.Background = New SolidColorBrush(Color.FromArgb(150, 255, 255, 255))
                                           End Sub
                AddHandler bdr.MouseLeave, Sub(senderer, evt)
                                               senderer.Background = New SolidColorBrush(Color.FromArgb(100, 255, 255, 255))
                                           End Sub
                AddHandler bdr.PreviewMouseDown, Sub(senderer, evt)
                                                     senderer.Background = New SolidColorBrush(Color.FromArgb(50, 255, 255, 255))
                                                 End Sub
                AddHandler bdr.PreviewMouseUp, Sub(senderer, evt)
                                                   senderer.Background = New SolidColorBrush(Color.FromArgb(100, 255, 255, 255))
                                                   Process.Start(sedata(se)(1).ToString.Replace("%input%", senderer.child.content))
                                                   csw()
                                               End Sub
            Next
        Catch
        End Try
    End Sub

    Private Sub csw()
        Close()
    End Sub

    Private Sub searchTB_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles searchTB.KeyDown
        If e.Key = Key.Enter Then
            Process.Start(sedata(se)(1).ToString.Replace("%input%", searchTB.Text))
            My.Settings.LastSearches.Add(searchTB.Text)
            My.Settings.Save()
            csw()
        ElseIf e.Key = Key.Escape Then
            csw()
        End If
    End Sub
End Class
