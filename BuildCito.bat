del /q /s CitoOutput

mkdir CitoOutput
mkdir CitoOutput\C
mkdir CitoOutput\Java
mkdir CitoOutput\Cs
mkdir CitoOutput\JsTa

copy CitoPlatform\Cs\* CitoOutput\Cs\*
copy CitoPlatform\Java\* CitoOutput\Java\*
copy CitoPlatform\JsTa\* CitoOutput\JsTa\*
copy CitoPlatform\C\* CitoOutput\C\*

setlocal enabledelayedexpansion enableextensions
set LIST=
for %%x in (ManicDiggerLib\*.ci.cs) do set LIST=!LIST! %%x
set LIST=%LIST:~1%
echo %LIST%

cito -D CITO -D C -l c -o CitoOutput\C\ManicDigger.c %LIST%
cito -D CITO -D JAVA -l java -o CitoOutput\Java\ManicDigger.java -n ManicDigger.lib  %LIST%
cito -D CITO -D CS -l cs -o CitoOutput\Cs\ManicDigger.cs %LIST%
cito -D CITO -D JS -D JSTA -l js-ta -o CitoOutput\JsTa\ManicDigger.js %LIST%
