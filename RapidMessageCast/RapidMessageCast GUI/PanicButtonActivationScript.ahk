; RapidMessageCast - Panic Button Activation Script
; DMY - 06-06-2024
; When pressing a certain key combo, activate program with PANIC argument.
; The key combinations can be changed to something else. But the PANIC argument is required.

; CONFIRMATION REQUIRED SCRIPT VERSION:

^+Pause::
    ; Show confirmation dialog
    MsgBox, 4,, Are you sure you want to activate the panic button?
    IfMsgBox Yes
    {
        ; Log timestamped entry
        FormatTime, CurrentDateTime,, yyyy-MM-dd HH:mm:ss
        FileAppend, [PANIC] %CurrentDateTime% - Activated by user`n, C:\Path\To\Log.txt

        ; Launch the program with "PANIC" argument
        Run, "C:\Path\To\Your\Program.exe" PANIC
    }
return

; INSTANT ACTIVATION SCRIPT VERSION:

; For instant activation without confirmation, uncomment the below code and comment out the top code.

;^+Pause::
;    Run, "C:\Path\To\Your\Program.exe" PANIC
;return