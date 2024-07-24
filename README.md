<p align="center">
    <img src="static/images/RMCBanner.png" alt="RMC Banner" />
</p>

# RapidMessageCast

[![GitHub issues](https://img.shields.io/github/issues/lloyd99901/RapidMessageCast)](https://github.com/lloyd99901/RapidMessageCast/issues)
[![GitHub stars](https://img.shields.io/github/stars/lloyd99901/RapidMessageCast)](https://github.com/lloyd99901/RapidMessageCast/stargazers)
[![GitHub license](https://img.shields.io/github/license/lloyd99901/RapidMessageCast)](https://github.com/lloyd99901/RapidMessageCast/blob/master/LICENSE)
![GitHub CodeQL](https://github.com/lloyd99901/RapidMessageCast/actions/workflows/codeql.yml/badge.svg)
![GitHub DOTNET](https://github.com/lloyd99901/RapidMessageCast/actions/workflows/dotnet.yml/badge.svg)

> PC Messager, Email, Remote Program Execution (PSExec), and Wake-On-LAN Support

RapidMessageCast is a messaging tool designed to help with rapid communication to computers across a network. This program enables users to send messages to multiple computers simultaneously, ensuring swift dissemination of critical information, like in emergency scenarios.

## Features:
> - Message multiple PC's fast (msg.exe)
> - Send automatic emails or prewritten emails. (runs Outlook/email program of choice or uses SMTP) [Not complete]
> - PSExec Support (Remote Program Execution). [Not complete]
> - Quick save and loads of RMSG files that allows the user to quickly load and broadcast saved messages. (e.g. reminders, emergency messages, notices, etc).
> - Quick load message and PC list from a txt file
> - Load PC list from Active Directory
> - View broadcast history with a comprehensive broadcast log. [In Progress]
> - Schedule messages to be broadcasted. [Not complete]
> - Tailored for automation programs.

## Notices:

> [!IMPORTANT]
>
> This program is provided "as is" under the MIT License, without any guarantees or warranties, express or implied.
>
> By using this program, you acknowledge that the developers and distributors are not responsible for any failures, delays, errors, or other issues that may arise, including undelivered, delayed, or incorrect messages. Users should report any errors or problems on the GitHub Issues page of this project for support.

> [!CAUTION]
> For critical emergency communication, always have a backup method in place. Use additional resources as needed and conduct a thorough audit to ensure this tool meets your specific needs. This audit should include testing the program in various scenarios, verifying compatibility with existing systems, and assessing its reliability and performance under different conditions.
>
> Conducting an audit helps identify any potential limitations or issues affecting the program's effectiveness in emergencies. It is the user's responsibility to verify the program's suitability and implement necessary safeguards or alternative communication methods.


## Why use RMC?

### Speed.
RapidMessageCast efficiently sends messages to multiple computers using the native Windows utility msg.exe. This avoids the need for complex setups and ensures messages are delivered even if remote access software is unresponsive. Its simple interface allows quick message broadcasts, making it ideal for urgent communications. The focus is on functionality, with no startup animations.

### Automation.
RapidMessageCast can be used with automation tools to schedule message broadcasts at specific times, useful for routine notifications and reminders. It integrates with monitoring systems to automatically send alerts and notifications when issues are detected.

### Free forever and open-source
This program is free and open-source. Users can modify the code to fix issues or improve functionality. Contributions are welcome via pull requests or patches, ensuring continuous improvement and collaboration.

### Full Control
RapidMessageCast GUI and CLI are two ways of broadcasting your message.

The GUI gives them the full control over the features of RMC, the user can save their RMSG file and then can transfer it to the CLI.

![MainWindow](https://raw.githubusercontent.com/lloyd99901/RapidMessageCast/master/static/images/RMCManager.png)

The CLI will then automatically read the RMSG file and begin broadcasting, this is great for automation/security software that can run actions based on rules (e.g. If a certain door is forced open, run RapidMessageCast CLI and run the AlertSecurityStaff.RMSG).
  
![Dispatcher](https://raw.githubusercontent.com/lloyd99901/RapidMessageCast/master/static/images/ExampleRMCDispatcher.png)

### Platform Compatibility.
RapidMessageCast is designed specifically for Windows operating systems, ensuring seamless integration with existing infrastructure. It provides a reliable solution for organizations using Windows environments.

## Requirements
> [!NOTE]
>
>  - MSG.exe is not included in Windows Home editions. Professional/enterprise editions are required. Windows 7 and above
>
>  - Both the sender and the recipient computers must have a Windows operating system that supports the MSG program in order to utilize RapidMessageCastRun effectively.

> [Optional] - PsExec has to be downloaded by the user manually and placed in the same folder where RMC GUI is in order for the PsExec module to work.

## Developer Notes

- Source code is edited on Visual Studio 2022
- If you have any improvements, suggestions, or bug reports, please do report it.

## Program Development Status
- [x] PC Module
- [ ] Email Module
- [ ] PsExec Module
- [ ] Code Refactor
- [ ] Failsafe Checks (Ensure that the program can resume after errors to ensure the best possible chance of messaging)
- [x] Filter PC's via a Regex Form
- [ ] Redo the Broadcast History Form so it's more user friendly (add icons like x or tick)
- [ ] Test WOL class
- [ ] Create CLI program with the new classes
