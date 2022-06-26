Imports System.Windows.Threading
Imports System.Runtime.InteropServices

Public Class iconobj
    <DllImport("user32.dll", EntryPoint:="SetForegroundWindow")> _
    Private Shared Function SetForegroundWindow(ByVal hWnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    Private Sub ActivateApp(ByVal aid As Integer)

        'Minimize Window
        SendMessage(runproc.MainWindowHandle,
          WM_SYSCOMMAND, SC_MINIMIZE, CType(0, IntPtr))

        'Restore Window
        SendMessage(runproc.MainWindowHandle,
          WM_SYSCOMMAND, SC_RESTORE, CType(0, IntPtr))
    End Sub

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As UInteger, ByVal lParam As IntPtr) As IntPtr
    End Function

    Const WM_SYSCOMMAND As UInt32 = &H112
    Const SC_RESTORE As UInt32 = &HF120
    Const SC_MAXIMIZE As UInt32 = &HF030
    Const SC_MINIMIZE As UInt32 = &HF020

    Dim alreadyadded As Boolean = False
    Public WithEvents imageiconobj As New Image
    Public WithEvents isapopen As New Grid
    Public WithEvents tick As New DispatcherTimer
    Property iconpath As String = ""
    Property apppath As String = ""
    Property stackpanel As StackPanel
    Property containerwin As MainWindow
    Property idd As Integer = Nothing
    Property isremoved As Boolean = False
    Property appname As String = ""
    Property apparams As String = ""
    Public WithEvents runproc As Process
    Dim hr = False
    Dim isopen = False
    Dim isprocfound = False
    Private fi As IO.FileInfo
    Property runid As Integer = 0
    Sub endinit()
        Dim img As New BitmapImage(New Uri(iconpath))
        imageiconobj.Source = img
        imageiconobj.Width = My.Settings.Size
        Try
            imageiconobj.Height = My.Settings.Size - 5
        Catch
        End Try
        imageiconobj.ClipToBounds = True
        isapopen.Width = My.Settings.Size / 3
        isapopen.Margin = New Thickness(My.Settings.Size / 3, 0, My.Settings.Size / 3, 0)
        isapopen.Height = 5
        isapopen.Background = Brushes.Transparent
        isapopen.ClipToBounds = True
        tick.Interval = TimeSpan.FromMilliseconds(1000)
        fi = New IO.FileInfo(apppath)
        tick.Start()
        If Not alreadyadded Then
            stackpanel.Children.Add(imageiconobj)
            containerwin.isappopen.Children.Add(isapopen)
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
            If Not isopen Then
                runproc = Process.Start(apppath, apparams)
                hr = True
                isprocfound = True
            Else
                Try
                    AppActivate(runproc.ProcessName)
                Catch ex As Exception
                    'MsgBox(ex.Message)
                    Try
                        AppActivate(My.Computer.FileSystem.GetName(apppath))
                    Catch
                        'ActivateAppl(runproc.Id)
                    End Try
                End Try
                ActivateApp(runproc.Id)
            End If
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

    Private Sub tick_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tick.Tick
        isopen = False
        isapopen.Background = Brushes.Transparent
        If Not fi.Extension = "" Then
            For Each prc As Process In Process.GetProcesses
                If prc.ProcessName.ToLower = My.Computer.FileSystem.GetName(apppath).Replace(fi.Extension, "").ToLower Then
                    isapopen.Background = Brushes.White
                    isopen = True
                    If hr Then If runproc.HasExited Then isprocfound = False
                    If Not isprocfound Then If Not prc.MainWindowTitle = "" Then runproc = prc
                End If
            Next
        End If
        If Not containerwin.rid = runid Then
            tick.Stop()
            Try
                remove()
            Catch
            End Try
        End If
    End Sub
    Private Sub ActivateAppl(ByVal pID As Integer)
        Dim p As Process = Process.GetProcessById(pID)
        If p IsNot Nothing Then
            SetForegroundWindow(p.MainWindowHandle)
        End If
    End Sub

    Private Sub runproc_Exited(ByVal sender As Object, ByVal e As System.EventArgs) Handles runproc.Exited
        isapopen.Background = Brushes.Transparent
        isprocfound = False
    End Sub

    Private Sub isapopen_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles isapopen.MouseUp
        runproc = Process.Start(apppath, apparams)
        hr = True
        isprocfound = True
    End Sub
End Class
