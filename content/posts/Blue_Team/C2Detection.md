---
title: "C2 detection using fingerprinting"
subtitle: ""
draft: false
author: "Hato0"
description: "Behavioral and fingerprint analysis to hunt for uncovered C2 over HTTP"
Last Update: <time datetime="{{ .Page.Lastmod.Format "Mon Jan 10 17:13:38 2020 -0700" }}" class="text-muted">  {{ $.Page.Lastmod.Format "January 02, 2006" }} </time>
Date: 2022-07-31

tags: ["web", "blueteam", "detection", "development"]
categories: ["BlueTeam - Detection"]

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
  enable: false
---
## Summary

Adversaries leverage their access to assets using initial access tool (custom or not). These tools are mainly used to get a remote session of the asset.

This remote access is known as Command and Control (C2) and can be implemented through many protocols and many technics. This channel will allow adversaries to perform lateral movement, internal discovery, exfiltration and much more. 

Willing to hunt for C2 in companies and homemade networks, I've focused my effort into C2 over HTTP.

## C2 infrastructure

C2 needs a dedicated infrastructure to work. This infrastructure must be **scalable** as adversaries will not only compromise about ten assets but thousands worldwide. 

Three models are widely implemented :

#### Centralized

Centralized model work like traditional client-server Web Apps. 

The client located on the compromised asset will contact the C2 server and check if any instructions as been set for him.
This model involved the creation of a company like network with load balancer, defense measures (not to detect adversaries but threat researchers). They often hide themselves behind CDNs or legitimate website (compromised upstream) to mask their activities.

As C2 are discovered due to incident response process, adversaries must plan for a backup solution and find innovative way to escape the full discovery. 

These plan might include :
- Use of legitimate companies solution
- Use social networks to gather C2 information
- Use steganography to obfuscate the data
- ...

#### Peer-to-Peer (P2P)

Peer-to-Peer model is used to deliver instruction as a botnet would do it. Every compromise host knows his neighbors and relay messages to each other. 

This model is harder to take down, as taking down the responsible adversary will not stop the infection of these thousands of devices linked in P2P. 
This is mainly used as fallback channel in case of main C2 server takedown.

#### Out of Band and Random

A lot of unusual techniques have been observed to send instructions to compromised assets.

Communication website like Slack, Teams, Messenger and others have been used to send instructions undetected as communications are as clean as legit ones.

A lot of other technics are not known and still to be uncovered by researchers.

![C2 Infra schema](https://cyberhoot.com/wp-content/uploads/2020/02/security-botnet_architecture-1024x540.jpg)

## Possible detections

Many detections possibilities are used but a lot of them are not as efficient as it could be. 

Many analyst and companies rely on two factors:
- IOCs
- Providers

These two options present a lot of inconvenient:
- IOCs are extracted from payloads or response teams investigations. They are very often outdated as adversaries infrastructures rotate very often, and they are also not exhaustive as unused channels are not investigated by most response teams. 
Keep in mind that if we can share IOC between SOC/CSIRT (or whatever name you give your analyst team), adversaries also know which endpoint / sample has leak and case easily replace/change it.
- Security providers don't priories your security, they have to sell products to exist. They take choices based on marketing plan and not based on security impact. Of course, this is not a generality, but take this into account and double check features with your own tests to make sure they really perform what they are meant to. 

Additionally, you can study and implement detection on loader and on stage2. This is time-consuming but can be useful. Keep in mind that you need to stay updated on loader if you choose this detection path.

## Implemented solution

As IOCs and providers product are not my go to (even if I use them). I've decided to build my own detection based on multiple factor to detect communication with adversaries infrastructure over HTTP. 

{{< admonition note "Note" >}} {{< version 0.2.10 >}}
This solution can either be implemented by yourself or you can use the developed tool located [here](https://github.com/Hato0/C2Hunter)
{{< /admonition >}}

This detection use three aspects :
- Proxy logs with enrich data
- Fingerprinting
- Open source data

We will combine these three aspects to create a detection that proxy provider should be implemented. 

#### Overview

We know that adversaries must have a scalable architecture, which mean that automatic deployment must be used.
With that type of deployment, parts of the deployed infra will not be changed (deployment methods, installed libraries, server and version used, ...).

These unchanged elements, can be tracked by their fingerprint. 

This solution is based on JARM and JA3/JA3S fingerprinting algorithms (TLS fingerprint). 
Quick details on these fingerprinting algorithms:
- **JARM**: The JARM fingerprint hash is a hybrid fuzzy hash, it uses the combination of a reversible and non-reversible hash algorithm to produce a 62 character fingerprint. The first 30 characters are made up of the cipher and TLS version chosen by the server for each of the 10 client hello's sent. A "000" denotes that the server refused to negotiate with that client hello. The remaining 32 characters are a truncated SHA256 hash of the cumulative extensions sent by the server, ignoring x509 certificate data. This method is an active one. More details [here](https://github.com/salesforce/jarm).
- **JA3**: JA3 gathers the decimal values of the bytes for the following fields in the Client Hello packet; SSL Version, Accepted Ciphers, List of Extensions, Elliptic Curves, and Elliptic Curve Formats. It then concatenates those values together in order, using a "," to delimit each field and a "-" to delimit each value in each field. This method is a passive one. More details [here](https://github.com/salesforce/ja3).
- **JA3S**: JA3S is JA3 for the Server side of the SSL/TLS communication and fingerprints how servers respond to particular clients. This method is a passive one. More details [here](https://github.com/salesforce/ja3).

#### Detailed explanation

With this information in our hand, we can now describe the solution step by step:
- As this detection is based on fingerprint. We need to build a database of malicious signatures. I know, I've said before that I don't like IOC but that's not a simple domain or IP, we are talking about a signature which is associated to a specific configuration and which is not related to domain nor IP.
To create this database, I'm using `abuse.ch` updated database. 
The script fetch JA3/S signatures known as malicious and active domain extracted by security researchers with identified C2 tooling.
- The next step is to ingest these data in our database. JA3 and JA3S signatures are added to the database without any modification. For the JARM signatures, we need to scan each domain or IP and gather his signature. If you remember well the description above, adversaries also monitor their infrastructure to avoid these types of signatures to be done. To avoid any false data to be ingested, for each domain or IP we will perform the JARM scan three time and compare these three signatures. If they are identical, we ingest it to the database, if not we discard the domain/IP.
- Now that the database is built, we can focus on the detection system. That's where the proxy come in. Two actions will be done from these logs:
    - **Active monitoring**: JA3 and JA3S are passive methods. These signatures are generated from PCAP data. Nothing more simple to do when we've got a proxy on our end. Each time a request is done through the proxy, these signatures are generated and added to the DB (linked with the IP/Domain). A comparison is done with the malicious data we ingested earlier. If the JA3 **AND** the JA3S match, the connection is closed and the website is categorized as malicious (no more connection will be created for this destination). 
    - **Passive monitoring**: As web navigation data in companies are really noisy, we first need to filter on a specific comportment. The typical C2 comportment is to fetch every X mins a static page to fetch for command to execute. 
    To target this comportment, here is an SQL example:
    {{< highlight SQL "lineNos=false" >}}
    SELECT Id, URL FROM proxyLogs group by username having foHoursSeen>={criteria};
    -- foHoursSeen: Number of consecutive hours where the domain/IP has been contacted
    -- criteria: should be a quite big amount to avoid overload (default=10)
    {{< /highlight >}}
    Then we will generate the JARM signature for each domain/IP identified. During the scan process (JARM generated 3 times to avoid false data), if signatures are rotating, the website will be marked as malicious. At the end of the scan every thing will be compared to the database created before and in case of a match, the domain/IP will be marked as malicious.

{{< admonition note "Note" >}} {{< version 0.2.10 >}}
- The database update is performed every day to stay as much as possible up to date.
- In my implementation, each criteria can be customized to fit your needs.
- During the database build and the passive monitoring phase, keep in mind that you will contact the adversaries infrastructures.
{{< /admonition >}}

#### Solution architecture

Below the architecture of the solution. Find also a working example.

![C2Hunter Architecture](https://www.hato0.xyz/C2Hunter.png)

#### Interfacing it with SOC tooling

If you have a SOC, I assume that:
- You have a proxy
- Your proxy logs are centralized in a SIEM
- You have a tooling dedicated instance

So basically nothing really crazy to adapt. The SQL statement to gather information from my proxy should be your SIEM request (should be performed on a 24h flowing time slot). 
Then JARM signatures should be generated following the below model. Comparison will remain with the same mechanism.

For the JA3 part, it can be already done by your proxy/firewall provider (might want to check before implement it). If you are using Qradar and have your flow integrated, using QFlow is a valuable option. 

To build your DB, I recommend using an instance totally decorrelated from your company to avoid any targeting or detection bypass in case of targeting attack. You can also use your private threat intel in addition to `abuse.ch`.

For the reporting, investigate the source of these connections in your SIEM/EDR and perform required remediation.

## Conclusion

As adversaries find new ways to implement their Command and Control, we need to find new ways to implement a durable solution. To do so, the implementation of solution needs to go higher and higher in the pyramid of pain model. 

This solution is almost on the top of this pyramid as it cover at least everything under "Network and hosts artifacts" and quite a bit of the "Tools" section.

Keep in mind that you need to understand the overall solution to build a solution yourself or to use the one I've created.


If you have ideas to improve this detection, don't hesitate to contact me.