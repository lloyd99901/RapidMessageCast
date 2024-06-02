<p align="center">
    <img src="static/images/RMCBanner.png" alt="RMC Banner" />
</p>

# RapidMessageCast

[![GitHub issues](https://img.shields.io/github/issues/lloyd99901/RapidMessageCast)](https://github.com/lloyd99901/RapidMessageCast/issues)
[![GitHub stars](https://img.shields.io/github/stars/lloyd99901/RapidMessageCast)](https://github.com/lloyd99901/RapidMessageCast/stargazers)
[![GitHub license](https://img.shields.io/github/license/lloyd99901/RapidMessageCast)](https://github.com/lloyd99901/RapidMessageCast/blob/master/LICENSE)
[![CodeFactor](https://www.codefactor.io/repository/github/lloyd99901/rapidmessagecast/badge)](https://www.codefactor.io/repository/github/lloyd99901/rapidmessagecast)

 > PC Messager, Email, Remote Program Execution (PSExec), and Wake-On-LAN Support

RapidMessageCast is a messaging tool designed to help with rapid communication to computers across a network. This program enables users to send messages to multiple computers simultaneously, ensuring swift dissemination of critical information, like in emergency scenarios.

> [!NOTE]
> 
> This program is actively in development. Expect bugs, untidy code, and missing features since this is in indev phase.

![MainWindow](https://raw.githubusercontent.com/lloyd99901/RapidMessageCast/master/static/images/RMCManager.png)

## Features:
> - Message multiple PC's fast (msg.exe)
> - Send automatic emails or prewritten emails. (runs Outlook/email program of choice or uses SMTP)
> - PSExec Support (Remote Program Execution).
> - Quick save and loads of RMSG files that allows the user to quickly load and broadcast saved messages. (e.g. reminders, emergency messages, notices, etc).
> - Quick load message and PC list from a txt file
> - Load PC list from Active Directory
> - View broadcast history with a comprehensive broadcast log.
> - Schedule messages to be broadcasted.
> - Tailored for automation programs.

## Notices:

> [!IMPORTANT]
>
> This program is designed to facilitate messaging in both regular and emergency situations. However, it is provided "as is" without any guarantees or warranties of any kind, either express or implied. Under the MIT License.
>
> By using this program, you acknowledge that the developers and distributors are not responsible for any failures, delays, errors, or any other issues that may arise from the use of this program. This includes, but is not limited to, situations where messages are not delivered, are delayed, or are incorrect. If there are any errors or problems, the users are encouraged to report them to the GitHub Issues page of this project for support.


> [!CAUTION]
> For critical emergency communication, always have a backup method in place. Take all necessary precautions and use additional resources as needed. Before relying on this program for emergency communication, users are strongly encouraged to conduct a thorough audit to ensure that the tool meets their specific needs and requirements. This audit should include, but is not limited to, testing the program in various scenarios, verifying compatibility with existing systems, and assessing the program's reliability and performance under different conditions.
>
> By conducting an audit, users can identify any potential limitations or issues that may affect the program's effectiveness in emergency situations. It is the user's responsibility to verify that the program is appropriate for their intended use and to implement any necessary safeguards or alternative communication methods.


## Why use RMC?

### Speed.
RapidMessageCast is great at sending messages to multiple computers. By leveraging the native msg.exe utility in Windows, it bypasses the need for complex setups or installations of remote access software, allowing for immediate message broadcasting. This also means that computers that have non-responding remote access software still get the message.
RapidMessageCast offers an interface that allows the user to be quick with their broadcast. Users can quickly initiate message broadcasts with minimal effort, making it an ideal choice for urgent communication needs.
It also prioritizes functionality over frills, omitting flashy startup animations. When sending a message is your priority, it's primed for immediate action without delay.

### Automation.
Automation tools or scheduling frameworks can leverage RapidMessageCast to schedule message broadcasts at predefined intervals or times. This capability is particularly useful for sending routine notifications, reminders, or updates to users or system administrators without manual intervention.
RapidMessageCast can be integrated with monitoring and alerting systems to automate the messaging of alerts and notifications. When monitoring systems detect anomalies, performance issues, or security breaches, they can automatically trigger RapidMessageCast to notify relevant personnel or teams.

### Free forever and open-source
This program is, and will always remain, free and open source. If you encounter any issues and have the expertise to resolve them, you are encouraged to modify the code as needed. Contributions to the codebase are welcome, and you can share improvements with the community by submitting pull requests or patches. This ensures continuous enhancement and collaboration, benefiting all users.

### Two ways of using RMC: GUI and CLI
RapidMessageCast GUI and CLI are two ways of broadcasting your message.

The GUI gives them the full control over the features of RMC, the user can save their RMSG file and then can transfer it to the CLI.

![MainWindow](https://raw.githubusercontent.com/lloyd99901/RapidMessageCast/master/static/images/RMCManager.png)

Once RapidMessageCast CLI is run, it will then read the RMSG and:
- It will immediately begin sending messages to all of the saved computer names. 
- If no arguments are presented to point to a Message file, it will choose the file default.msg. This feature caters to automation programs that lack the functionality to add arguments, ensuring effortless integration of RapidMessageCast.
- Once done it will close out automatically.
  
![Dispatcher](https://raw.githubusercontent.com/lloyd99901/RapidMessageCast/master/static/images/ExampleRMCDispatcher.png)

### Platform Compatibility.
RapidMessageCast is designed specifically for Windows operating systems, ensuring seamless integration and compatibility with existing infrastructure. It provides a reliable solution for organizations that predominantly use Windows environments.

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
- [ ] Filter PC's via a Regex Form
- [ ] Redo the Broadcast History Form so it's more user friendly (add icons like x or tick)
- [ ] Test WOL class
- [ ] Create CLI program with the new classes
