Public Class ButtonStack
    Public WithEvents theStackPanel As StackPanel
    Property DefaultColor As Brush = Brushes.Transparent
    Property HoverColor As Brush = Brushes.White
    Property ClickingColor As Brush = Brushes.Gray

    Private Sub theStackPanel_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles theStackPanel.MouseDown
        theStackPanel.Background = ClickingColor
    End Sub
    
    Private Sub theStackPanel_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles theStackPanel.MouseEnter
        theStackPanel.Background = HoverColor
    End Sub

    Private Sub theStackPanel_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles theStackPanel.MouseLeave
        theStackPanel.Background = DefaultColor
    End Sub

    Private Sub theStackPanel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles theStackPanel.MouseUp
        theStackPanel.Background = HoverColor
    End Sub
End Class
