; RapidMessageCast - Panic Button Activation Script
; DMY - 06-06-2024
; When pressing a certain key combo, activate program with PANIC argument.
; The key combinations can be changed to something else. But the PANIC argument is required.

; INSTANT ACTIVATION SCRIPT VERSION:

^+Pause::
    Run, "C:\Path\To\Your\Program.exe" PANIC
return