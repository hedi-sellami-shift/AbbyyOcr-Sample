This project is a sample aiming to demonstrate an issue found regarding the AbbyyOcr API:

This page of the documentation shows different ways to load abbyy's engine:
https://help.abbyy.com/en-us/finereaderengine/12/user_guide/guidedtour.differentwaystoloadengine

We compare the performances of an engine loaded manually, and that of an engine loaded using InprocLoader
The document we use for benchmarking is a page taken from the samples included with the installation of Abbyy.

First we tried processing the document:
Both engines show identical performances

Then we tried parsing the processed document:
The engine loaded using InprocLoader show much slower performances
    
Results:
 - ManualLoader, Processing: 4.44s
 - InprocLoader, Processing: 4.44s

 - ManualLoader, Processing and Parsing: 4.45s
 - InprocLoader, Processing and Parsing: 7.57.s
