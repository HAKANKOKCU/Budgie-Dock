Imports System.Runtime.InteropServices

Module BMain
    Public Declare Auto Function IsIconic Lib "user32.dll" (ByVal hwnd As IntPtr) As Boolean
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As UInteger, ByVal lParam As IntPtr) As IntPtr
    End Function
    Public Const WM_SYSCOMMAND As UInt32 = &H112
    Public Const SC_RESTORE As UInt32 = &HF120
    Public Const SC_MAXIMIZE As UInt32 = &HF030
    Public Const SC_MINIMIZE As UInt32 = &HF020
    Public Sub ActivateApp(ByVal handle As IntPtr)
        'Minimize Window
        SendMessage(handle,
          WM_SYSCOMMAND, SC_MINIMIZE, CType(0, IntPtr))

        'Restore Window
        SendMessage(handle,
          WM_SYSCOMMAND, SC_RESTORE, CType(0, IntPtr))
    End Sub
    Public Sub showSpecialDiag(ByVal str As String)
        If str = "!AppsDrawer" Then
            Dim ad As New appdrawer
            'containerwin.Visibility = Visibility.Hidden
            ad.ShowDialog()
            'containerwin.Visibility = Visibility.Visible
        ElseIf str = "!Shutdown" Then
            Dim ad As New shutdownDialog
            ad.ShowDialog()
        End If
    End Sub
End Module
