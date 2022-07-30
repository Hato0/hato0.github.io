---
title: "SQLi attack"
subtitle: ""
date: 2020-03-04T15:58:26+08:00
lastmod: 2020-03-04T15:58:26+08:00
draft: true
author: "Hato0"
description: "SQL injection cheatsheet"

tags: ["web", "penetest", "sqli"]
categories: ["Penetest - Web"]

hiddenFromHomePage: false
hiddenFromSearch: false
twemoji: false
lightgallery: false
ruby: false
fraction: true
fontawesome: true
linkToMarkdown: true
rssFullText: false
toc:
  enable: true
  auto: true
code:
  copy: true
  maxShownLines: 50
share:
  enable: true
comment:
  enable: true
---
# OS command injection

OS command injection is a web security vulnerability that allows an attacker to execute arbitrary operating system commands on the server that is running an application. This can lead to full server compromision.

This attack is really based on what you saw on the website and how does it handle everything.


## Some examples

- Blind OS command injection with time delays
- Blind OS command injection with output redirection
- Blind OS command injection with out-of-band interaction
- Blind OS command injection with out-of-band data exfiltration


## How to prevent them

Basicly never call an OS command on your web app code and also :

-   Validating against a whitelist of permitted values.
-   Validating that the input is a number.
-   Validating that the input contains only alphanumeric characters, no other syntax or whitespace.

But keep in mind that even if you do those 3 checks, you can still be in danger without changing your handling.


# Server-side template injection

Server-side template injection (SSTI) is when an attacker is able to use native template syntax to inject a malicious payload into a template, which is then executed server-side.

You can try to detect the presence of SSTI by using template related caracteres and look for errors. Those caracteres could be one or multiple of them `$<['"\`.

If you detect a potential SSTI you can explore the appropriate injection to perform by idenfying the template use. You can follow this diagram to find it :

![Template decision tree](https://portswigger.net/web-security/images/template-decision-tree.png)


## Basics

- Basic server-side template injection
- Basic server-side template injection (code context)



## Advance

- Server-side template injection in a sandboxed environment
- Server-side template injection with a custom exploit


## Other examples

- Server-side template injection using documentation
- Server-side template injection in an unknown language with a documented exploit
- Server-side template injection with information disclosure via user-supplied objects


## How to prevent them 

One of the simplest ways to avoid introducing server-side template injection vulnerabilities is to always use a "logic-less" template engine, such as Mustache, unless absolutely necessary. Separating the logic from presentation as much as possible can greatly reduce your exposure to the most dangerous template-based attacks.

Another measure is to only execute users' code in a sandboxed environment where potentially dangerous modules and functions have been removed altogether. Unfortunately, sandboxing untrusted code is inherently difficult and prone to bypasses.

Finally, another complementary approach is to accept that arbitrary code execution is all but inevitable and apply your own sandboxing by deploying your template environment in a locked-down Docker container, for example.


# Directory traversal

Directory traversal aims to access files and directories that are stored outside the web root folder. By manipulating variables that reference files with `../` sequences and its variations or by using absolute file paths, it may be possible to access arbitrary files and directories stored on file system including application source code or configuration and critical system files.


## Some examples

- File path traversal, simple case
- File path traversal, traversal sequences blocked with absolute path bypass
- File path traversal, traversal sequences stripped non-recursively
- File path traversal, traversal sequences stripped with superfluous URL-decode
- File path traversal, validation of start of path
- File path traversal, validation of file extension with null byte bypass


## How to prevent them

-   Prefer working without user input when using file system calls
-   Ensure the user cannot supply all parts of the path – surround it with your path code
-   Validate the user’s input by only accepting known good – do not sanitize the data
-   Use chrooted jails and code access policies to restrict where the files can be obtained or saved to
-   If forced to use user input for file operations, normalize the input before using in file io API’s, such as [normalize()](https://docs.oracle.com/javase/7/docs/api/java/net/URI.html#normalize()).


# Access control vulnerabilities

Access control is a family where the web app you are in doesn't implement a sufficiant control of rights / access and in that way you can perform actions that you shouldn't be able to perform. A good system should contain all of these points :

-   **Authentication** identifies the user and confirms that they are who they say they are.
-   **Session management** identifies which subsequent HTTP requests are being made by that same user.
-   **Access control** determines whether the user is allowed to carry out the action that they are attempting to perform.

This vulnerability can be split between 3 catégories as follow :
-   **Vertical access controls** : User can perform action that is not allowed for there roles or types of users
-   **Horizontal access controls** : User can access or modify data that can only be accessible to a specific user
-   **Context-dependent access controls** : A little mix between the previous ones

## Some examples

- Unprotected admin functionality
- Unprotected admin functionality with unpredictable URL
- User role controlled by request parameter
- User role can be modified in user profile
- URL-based access control can be circumvented
- Method-based access control can be circumvented
- User ID controlled by request parameter 
- User ID controlled by request parameter, with unpredictable user IDs
		
- User ID controlled by request parameter with data leakage in redirect 
- User ID controlled by request parameter with password disclosure
- Insecure direct object references
- Multi-step process with no access control on one step
		
- Referer-based access control


## How to prevent them

-   Never rely on obfuscation alone for access control.
-   Unless a resource is intended to be publicly accessible, deny access by default.
-   Wherever possible, use a single application-wide mechanism for enforcing access controls.
-   At the code level, make it mandatory for developers to declare the access that is allowed for each resource, and deny access by default.
-   Thoroughly audit and test access controls to ensure they are working as designed.


		
# Authentication

This one is a very large topic. Basicly this category is everything related to authentication on web apps. You can check for this vulnerability by checking potential:
-   weak authentication mechanisms because they fail to adequately protect against brute-force attacks.
-   Logic flaws or poor coding in the implementation allow the authentication mechanisms to be bypassed entirely by an attacker. 


## Some examples

- Username enumeration via different responses
- Username enumeration via subtly different responses
- Username enumeration via response timing
- Broken brute-force protection, IP block
- Username enumeration via account lock
- Broken brute-force protection, multiple credentials per request
- 2FA simple bypass
- 2FA broken logic
- 2FA bypass using a brute-force attack
- Brute-forcing a stay-logged-in cookie
- Offline password cracking
- Password reset broken logic
- Password reset poisoning via middleware
- Password brute-force via password change


## How to prevent them

- Implement reliable techniques
- Force HTTPS
- Prevent username fuzzing
- Multi-factor authentification
- ...


# WebSockets

WebSockets are a bi-directional, full duplex communications protocol initiated over HTTP. They are commonly used in modern web applications for streaming data and other asynchronous traffic.

As well as manipulating WebSocket messages, it is sometimes necessary to manipulate the WebSocket Handshake that establishes the connection.

There are various situations in which manipulating the WebSocket handshake might be necessary:

-   It can enable you to reach more attack surface.
-   Some attacks might cause your connection to drop so you need to establish a new one.
-   Tokens or other data in the original handshake request might be stale and need updating.

## Some examples

- Manipulating WebSocket messages to exploit vulnerabilities
- Manipulating the WebSocket handshake to exploit vulnerabilities
- Cross-site WebSocket hijacking


## How to prevent them

 To minimize the risk of security vulnerabilities arising with WebSockets, use the following guidelines:

   - Use the wss:// protocol (WebSockets over TLS).
   - Hard code the URL of the WebSockets endpoint, and certainly don't incorporate user-controllable data into this URL.
   - Protect the WebSocket handshake message against CSRF, to avoid cross-site WebSockets hijacking vulnerabilities.
   - Treat data received via the WebSocket as untrusted in both directions. Handle data safely on both the server and client ends, to prevent input-based vulnerabilities such as SQL injection and cross-site scripting.



# Web cache poisoning

Web cache poisoning involves two phases:
-	The attacker must work out how to elicit a response from the back-end server that inadvertently contains some kind of dangerous payload.
-	They need to make sure that their response is cached and subsequently served to the intended victims.

A poisoned web cache can potentially be a devastating means of distributing numerous different attacks, exploiting vulnerabilities such as XSS, JavaScript injection, open redirection, and so on.

Here is a schema of the attack.

![web cache poisoning](https://portswigger.net/web-security/images/cache-poisoning.svg)


## Some examples

- Web cache poisoning with an unkeyed header
- Web cache poisoning with an unkeyed cookie
- Web cache poisoning with multiple headers
- Targeted web cache poisoning using an unknown header
- Web cache poisoning to exploit a DOM vulnerability via a cache with strict cacheability criteria
- Combining web cache poisoning vulnerabilities
- Web cache poisoning via an unkeyed query string
- Web cache poisoning via an unkeyed query parameter
- Parameter cloaking
- Web cache poisoning via a fat GET request
- URL normalization
- Cache key injection
- Internal cache poisoning


## How to prevent them

-   If you are considering excluding something from the cache key for performance reasons, rewrite the request instead.
-   Don't accept fat `GET` requests. Be aware that some third-party technologies may permit this by default.
-   Patch client-side vulnerabilities even if they seem unexploitable. Some of these vulnerabilities might actually be exploitable due to unpredictable quirks in your cache's behavior. It could be a matter of time before someone finds a quirk, whether it be cache-based or otherwise, that makes this vulnerability exploitable.


# Insecure deserialization

Insecure deserialization is when user-controllable data is deserialized by a website. This potentially enables an attacker to manipulate serialized objects in order to pass harmful data into the application code.

It is even possible to replace a serialized object with an object of an entirely different class. Alarmingly, objects of any class that is available to the website will be deserialized and instantiated, regardless of which class was expected. For this reason, insecure deserialization is sometimes known as an "object injection" vulnerability.


## Some examples

- Modifying serialized objects
- Modifying serialized data types
- Using application functionality to exploit insecure deserialization
- Arbitrary object injection in PHP
- Exploiting Java deserialization with Apache Commons
- Exploiting PHP deserialization with a pre-built gadget chain
- Exploiting Ruby deserialization using a documented gadget chain
- Developing a custom gadget chain for Java deserialization
- Developing a custom gadget chain for PHP deserialization
- Using PHAR deserialization to deploy a custom gadget chain


# Information disclosure

Information disclosure, also known as information leakage, is when a website unintentionally reveals sensitive information to its users. Depending on the context, websites may leak all kinds of information to a potential attacker, including:

-   Data about other users, such as usernames or financial information
-   Sensitive commercial or business data
-   Technical details about the website and its infrastructure


## Some examples

- Information disclosure in error messages
- Information disclosure on debug page
- Source code disclosure via backup files
- Authentication bypass via information disclosure
- Information disclosure in version control history


# HTTP Host header attacks

Can lead to :

-   Web cache poisoning
-   Business [logic flaws](https://portswigger.net/web-security/logic-flaws) in specific functionality
-   Routing-based SSRF
-   Classic server-side vulnerabilities, such as SQL injection

- Basic password reset poisoning
- Password reset poisoning via dangling markup
- Web cache poisoning via ambiguous requests
- Host header authentication bypass
- Routing-based SSRF
- SSRF via flawed request parsing


# OAuth authentication

System of social login

- Authentication bypass via OAuth implicit flow
- Forced OAuth profile linking
- OAuth account hijacking via redirect_uri
- Stealing OAuth access tokens via an open redirect
- Stealing OAuth access tokens via a proxy page
- SSRF via OpenID dynamic client registration
