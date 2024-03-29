﻿Public Class shutdownDialog

    Private Sub shutdown_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles shutdown.Click
        Process.Start("shutdown", "/s /f /t 0")
        Close()
        End
    End Sub

    Private Sub restart_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles restart.Click
        Process.Start("shutdown", "/r /f /t 0")
        Close()
        End
    End Sub

    Private Sub logoff_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles logoff.Click
        Process.Start("shutdown", "/l")
        Close()
        End
    End Sub

    Private Sub hibernate_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles hibernate.Click
        Process.Start("shutdown", "/h")
        Close()
    End Sub

    Private Sub cancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles cancel.Click
        Close()
    End Sub

    Private Sub shutdownDialog_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Me.KeyDown
        If e.Key = Key.Escape Then
            Close()
        End If
    End Sub

    Private Sub shutdownDialog_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        shutdown.Focus()
    End Sub
End Class
