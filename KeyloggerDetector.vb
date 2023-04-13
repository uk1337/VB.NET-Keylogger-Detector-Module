Imports System.Runtime.InteropServices

Module KeyloggerDetector

    'Declare the delegate (if using non-generic pattern).
    Public Delegate Function HookProc(ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer

    'Declare variables for hook handle and delegate.
    Private hookHandle As IntPtr = IntPtr.Zero
    Private hookDelegate As HookProc = Nothing

    'Declare constants for Windows API functions.
    Private Const WH_KEYBOARD_LL As Integer = 13
    Private Const WM_KEYDOWN As Integer = &H100
    Private Const WM_SYSKEYDOWN As Integer = &H104

    'Declare the keyboard hook structure.
    <StructLayout(LayoutKind.Sequential)> _
    Private Structure KBDLLHOOKSTRUCT
        Public vkCode As Integer
        Public scanCode As Integer
        Public flags As Integer
        Public time As Integer
        Public dwExtraInfo As IntPtr
    End Structure

    'Declare the Windows API functions.
    Private Declare Function SetWindowsHookEx Lib "user32.dll" Alias "SetWindowsHookExA" _
        (ByVal idHook As Integer, ByVal lpfn As HookProc, ByVal hMod As IntPtr, ByVal dwThreadId As UInteger) As IntPtr

    Private Declare Function UnhookWindowsHookEx Lib "user32.dll" _
        (ByVal hhk As IntPtr) As Boolean

    Private Declare Function CallNextHookEx Lib "user32.dll" _
        (ByVal hhk As IntPtr, ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer

    Private Declare Function GetModuleHandle Lib "kernel32.dll" Alias "GetModuleHandleA" _
        (ByVal lpModuleName As String) As IntPtr

    'Define the hook procedure.
    Private Function HookCallback(ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
        If nCode >= 0 AndAlso (wParam = WM_KEYDOWN OrElse wParam = WM_SYSKEYDOWN) Then
            'A key was pressed - take appropriate action to prevent keylogging.
            'For example, you could send a notification or sound an alarm.
        End If

        'Pass control to the next hook in the chain.
        Return CallNextHookEx(hookHandle, nCode, wParam, lParam)
    End Function

    'Public function to start the keyboard hook.
    Public Sub StartKeyloggerDetection()
        hookDelegate = New HookProc(AddressOf HookCallback)

        'Set the keyboard hook.
        hookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, hookDelegate, GetModuleHandle(Nothing), 0)
    End Sub

    'Public function to stop the keyboard hook.
    Public Sub StopKeyloggerDetection()
        UnhookWindowsHookEx(hookHandle)
    End Sub

End Module
