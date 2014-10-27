﻿namespace AsciiToSvg.Tests

module TxtFile =

  open AsciiToSvg
  open AsciiToSvg.Common
  open AsciiToSvg.Json
  open AsciiToSvg.TxtFile


  module TestLogo_txt =

    let splitTxtResultExpected =
      [|("Logo", "{\"fill\":\"#88d\",\"a2s:delref\":true}")|],
          [|" .-[Logo]------------------.   ";
            " |                         |   ";
            " | .---.-. .-----. .-----. |   ";
            " | | .-. | +-->  | |  <--+ |    ";
            " | | '-' | |  <--+ +-->  | |  ";
            " | '---'-' '-----' '-----' |  ";
            " |  ascii     2      svg   |  ";
            " |                         |  ";
            " '-------------------------'  ";
            "  https://9vx.org/~dho/a2s/   ";
            "   "|]
    let splitTxtResult =
      @"../../TestTxtFiles/TestLogo.txt"
      |> readFile
      |> framedSplitTxt

    let leftOffsetExpected = 1
    let leftOffsetResult = leftOffset (snd splitTxtResult)

    let makeFramedGridResultExpected =
      [|[|' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '|];
        [|' '; '.'; '-'; '['; 'L'; 'o'; 'g'; 'o'; ']'; '-'; '-'; '-'; '-'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '.';
          ' '; ' '; ' '; ' '; ' '|];
        [|' '; '|'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|';
          ' '; ' '; ' '; ' '; ' '|];
        [|' '; '|'; ' '; '.'; '-'; '-'; '-'; '.'; '-'; '.'; ' '; '.'; '-'; '-';
          '-'; '-'; '-'; '.'; ' '; '.'; '-'; '-'; '-'; '-'; '-'; '.'; ' '; '|';
          ' '; ' '; ' '; ' '; ' '|];
        [|' '; '|'; ' '; '|'; ' '; '.'; '-'; '.'; ' '; '|'; ' '; '+'; '-'; '-';
          '>'; ' '; ' '; '|'; ' '; '|'; ' '; ' '; '<'; '-'; '-'; '+'; ' '; '|';
          ' '; ' '; ' '; ' '; ' '|];
        [|' '; '|'; ' '; '|'; ' '; '\''; '-'; '\''; ' '; '|'; ' '; '|'; ' '; ' ';
          '<'; '-'; '-'; '+'; ' '; '+'; '-'; '-'; '>'; ' '; ' '; '|'; ' '; '|';
          ' '; ' '; ' '; ' '; ' '|];
        [|' '; '|'; ' '; '\''; '-'; '-'; '-'; '\''; '-'; '\''; ' '; '\''; '-'; '-';
          '-'; '-'; '-'; '\''; ' '; '\''; '-'; '-'; '-'; '-'; '-'; '\''; ' '; '|';
          ' '; ' '; ' '; ' '; ' '|];
        [|' '; '|'; ' '; ' '; 'a'; 's'; 'c'; 'i'; 'i'; ' '; ' '; ' '; ' '; ' ';
          '2'; ' '; ' '; ' '; ' '; ' '; ' '; 's'; 'v'; 'g'; ' '; ' '; ' '; '|';
          ' '; ' '; ' '; ' '; ' '|];
        [|' '; '|'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|';
          ' '; ' '; ' '; ' '; ' '|];
        [|' '; '\''; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '\'';
          ' '; ' '; ' '; ' '; ' '|];
        [|' '; ' '; 'h'; 't'; 't'; 'p'; 's'; ':'; '/'; '/'; '9'; 'v'; 'x'; '.';
          'o'; 'r'; 'g'; '/'; '~'; 'd'; 'h'; 'o'; '/'; 'a'; '2'; 's'; '/'; ' ';
          ' '; ' '; ' '; ' '; ' '|];
        [|' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '|];
        [|' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '|]|]
    let makeFramedGridResult = splitTxtResult |> fun (a, b) -> b |> makeFramedGrid


    let matchPositionsExpected = [3; 26]
    let replaceOptionInLineExpected =
      ([2; 25], "-----------------------------------------")
    let input = "--[Logo]-----------------[Logo]----------"
              //    01234567890123456789012345
    let matchPositionsResult = matchPositions "Logo" input
    let replaceOptionInLineResult = replaceOptionInLine '-' "Logo" input

    let input2 =
      [|"-----------------------------------------"
        "---[Logo] ---- [Logo]--------------------"
        "--[Logo]-----------------[Logo]----------"
        "-[Logo]---------------------------[Logo]-"
        "[Logo]-----------------------------[Logo]"|]
    let replaceOptionInAsciiExpected =
      ([|[||]; [||]; [|(2, 2); (2, 25)|]; [|(3, 1); (3, 34)|]; [||]|],
       [|"-----------------------------------------"
         "---[Logo] ---- [Logo]--------------------"
         "-----------------------------------------"
         "-----------------------------------------"
         "[Logo]-----------------------------[Logo]"|])
    let replaceOptionInAsciiResult = replaceOptionInAscii '-' input2 "Logo"
    let replaceOptionInAsciiOK = (replaceOptionInAsciiResult = replaceOptionInAsciiExpected)

    let personJString = "{ \"Name\": \"Phil\", \"Phone\": 12345-6789 }"
    let person = parse personJString
    let parseFailureResult =
      match person with
      | Success _ -> ""
      | _ as x -> sprintf "%A" x
    let parseFailureResultExpected =
      "Failure\n  [JsonParseError\n     (ErrorLabel \"Failed to parse JSON\", Stacktrace \"Unknown token: >-6789 }<\")]"

    let logoJString = "{ \"fill\":\"#88d\",\"a2s:delref\":true }"
    let logo = parse logoJString
    let parseSuccessResult =
      match logo with
      | Success x -> sprintf "%A" x
      | _ as x -> ""
    let parseSuccessResultExpected =
      "JObject [(\"fill\", JString \"#88d\"); (\"a2s:delref\", Boolean true)]"

  module ArrowGlyph_txt =

    let splitTxtResultExpected : (string * string)[] * string[] =
      [||],
      [|"";
        " ArrowUp  ArrowDown  ArrowLeftToRight  ArrowRightToLeft";
        "  ^   ^      |   +          ->                <-";
        "  |   +      v   v          +>                <+";
        " "|]
    let splitTxtResult =
      @"../../TestTxtFiles/ArrowGlyphs.txt"
      |> readFile
      |> splitTxt
    let splitTxtResultOk = (splitTxtResultExpected = splitTxtResult)


    let leftOffsetExpected = 1
    let leftOffsetResult = leftOffset (snd splitTxtResult)

    let trimWithOffsetExpected =
      [|[||];
        [|'A'; 'r'; 'r'; 'o'; 'w'; 'U'; 'p'; ' '; ' '; 'A'; 'r'; 'r'; 'o'; 'w'; 'D';
          'o'; 'w'; 'n'; ' '; ' '; 'A'; 'r'; 'r'; 'o'; 'w'; 'L'; 'e'; 'f'; 't'; 'T';
          'o'; 'R'; 'i'; 'g'; 'h'; 't'; ' '; ' '; 'A'; 'r'; 'r'; 'o'; 'w'; 'R'; 'i';
          'g'; 'h'; 't'; 'T'; 'o'; 'L'; 'e'; 'f'; 't'|];
        [|' '; '^'; ' '; ' '; ' '; '^'; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' '; ' ';
          ' '; '+'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '-'; '>'; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          '<'; '-'|];
        [|' '; '|'; ' '; ' '; ' '; '+'; ' '; ' '; ' '; ' '; ' '; ' '; 'v'; ' '; ' ';
          ' '; 'v'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '+'; '>'; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          '<'; '+'|];
        [||]|]
    let trimWithOffsetResult = trimWithOffset 1 (snd splitTxtResult)

    let makeGridResultExpected =
      [|[|'A'; 'r'; 'r'; 'o'; 'w'; 'U'; 'p'; ' '; ' '; 'A'; 'r'; 'r'; 'o'; 'w'; 'D';
          'o'; 'w'; 'n'; ' '; ' '; 'A'; 'r'; 'r'; 'o'; 'w'; 'L'; 'e'; 'f'; 't'; 'T';
          'o'; 'R'; 'i'; 'g'; 'h'; 't'; ' '; ' '; 'A'; 'r'; 'r'; 'o'; 'w'; 'R'; 'i';
          'g'; 'h'; 't'; 'T'; 'o'; 'L'; 'e'; 'f'; 't'|];
        [|' '; '^'; ' '; ' '; ' '; '^'; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' '; ' ';
          ' '; '+'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '-'; '>'; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          '<'; '-'; ' '; ' '; ' '; ' '; ' '; ' '; ' '|];
        [|' '; '|'; ' '; ' '; ' '; '+'; ' '; ' '; ' '; ' '; ' '; ' '; 'v'; ' '; ' ';
          ' '; 'v'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '+'; '>'; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          '<'; '+'; ' '; ' '; ' '; ' '; ' '; ' '; ' '|]|]
    let makeGridResult = splitTxtResult |> fun (_, b) -> b |> makeTrimmedGrid


  module ArrowGlyphWithFrame_txt =

    let splitTxtResultExpected : (string * string)[] * string[] =
      [||],
      [|"+---------+-----------+------------------+------------------+";
        "| ArrowUp | ArrowDown | ArrowLeftToRight | ArrowRightToLeft |";
        "+---------+-----------+------------------+------------------+";
        "|  ^   ^  |    |   +  |        ->        |        <-        |";
        "|  |   +  |    v   v  |        +>        |        <+        |";
        "+---------+-----------+------------------+------------------+ "|]
    let splitTxtResult =
      @"../../TestTxtFiles/ArrowGlyphsWithFrame.txt"
      |> readFile
      |> splitTxt
    let splitTxtResultOk = (splitTxtResultExpected = splitTxtResult)

    let leftOffsetExpected = 0
    let leftOffsetResult = leftOffset (snd splitTxtResult)

    let makeGridResultExpected =
      [|[|'+'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '+'; '-'; '-'; '-'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '-'; '+'; '-'; '-'; '-'; '-'; '-'; '-'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '+'; '-'; '-'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-';
          '+'|];
        [|'|'; ' '; 'A'; 'r'; 'r'; 'o'; 'w'; 'U'; 'p'; ' '; '|'; ' '; 'A'; 'r'; 'r';
          'o'; 'w'; 'D'; 'o'; 'w'; 'n'; ' '; '|'; ' '; 'A'; 'r'; 'r'; 'o'; 'w'; 'L';
          'e'; 'f'; 't'; 'T'; 'o'; 'R'; 'i'; 'g'; 'h'; 't'; ' '; '|'; ' '; 'A'; 'r';
          'r'; 'o'; 'w'; 'R'; 'i'; 'g'; 'h'; 't'; 'T'; 'o'; 'L'; 'e'; 'f'; 't'; ' ';
          '|'|];
        [|'+'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '+'; '-'; '-'; '-'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '-'; '+'; '-'; '-'; '-'; '-'; '-'; '-'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '+'; '-'; '-'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-';
          '+'|];
        [|'|'; ' '; ' '; '^'; ' '; ' '; ' '; '^'; ' '; ' '; '|'; ' '; ' '; ' '; ' ';
          '|'; ' '; ' '; ' '; '+'; ' '; ' '; '|'; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; '-'; '>'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; '<'; '-'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          '|'|];
        [|'|'; ' '; ' '; '|'; ' '; ' '; ' '; '+'; ' '; ' '; '|'; ' '; ' '; ' '; ' ';
          'v'; ' '; ' '; ' '; 'v'; ' '; ' '; '|'; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; '+'; '>'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; '<'; '+'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          '|'|];
        [|'+'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '+'; '-'; '-'; '-'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '-'; '+'; '-'; '-'; '-'; '-'; '-'; '-'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '+'; '-'; '-'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-';
          '+'|]|]
    let makeGridResult = splitTxtResult |> fun (_, b) -> b |> makeTrimmedGrid

  module TestPolygonBox_txt =

    let splitTxtResultExpected : (string * string)[] * string[] =
      ([||],
       [|"";
         "";
         "                |";
         "                v";
         "             -<-+------+";
         "                |      |";
         "   +            |      |";
         "   +------------+---++-+";
         "   |                ||";
         "   |                ++";
         "   |                ||";
         "   |                ||";
         "   +----------------++";
         "                    ++"|])

    let splitTxtResult =
      @"../../TestTxtFiles/TestPolygonBox.txt"
      |> readFile
      |> splitTxt
    let splitTxtResultOk = (splitTxtResultExpected = splitTxtResult)

    let makeGridResultExpected =
      [|[|' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' '; ' '; ' '; ' '; ' '; ' '; ' '|];
        [|' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; 'v'; ' '; ' '; ' '; ' '; ' '; ' '; ' '|];
        [|' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '-'; '<'; '-'; '+'; '-'; '-'; '-'; '-'; '-'; '-'; '+'|];
        [|' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' '; ' '; ' '; ' '; ' '; ' '; '|'|];
        [|'+'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' '; ' '; ' '; ' '; ' '; ' '; '|'|];
        [|'+'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '+'; '-'; '-'; '-'; '+'; '+'; '-'; '+'|];
        [|'|'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; '|'; ' '; ' '|];
        [|'|'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '+'; '+'; ' '; ' '|];
        [|'|'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; '|'; ' '; ' '|];
        [|'|'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; '|'; ' '; ' '|];
        [|'+'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '+'; '+'; ' '; ' '|];
        [|' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '+'; '+'; ' '; ' '|]|]
    let makeGridResult = splitTxtResult |> fun (_, b) -> b |> makeTrimmedGrid

  module TestMiniBox_txt =

    let splitTxtResultExpected : (string * string)[] * string[] =
      ([||],
       [|".-.";
         "| |";
         "'-'";
         ""|])

    let splitTxtResult =
      @"../../TestTxtFiles/TestMiniBox.txt"
      |> readFile
      |> splitTxt
    let splitTxtResultOk = (splitTxtResultExpected = splitTxtResult)

    let leftOffsetExpected = 0
    let leftOffsetResult = leftOffset (snd splitTxtResult)

    let makeGridResultExpected =
      [|[|'.'; '-'; '.'|];
        [|'|'; ' '; '|'|];
        [|'\''; '-'; '\''|]|]
    let makeGridResult = splitTxtResult |> fun (_, b) -> b |> makeTrimmedGrid

  module ZeroMQ_Fig1_txt =

    let splitTxtResultExpected : (string * string)[] * string[] =
      ([||],
       [|".------------.        .--------------.";
         "|            |        |              |";
         "| TCP socket +------->|              | ZAP!";
         "|            | BOOM!  |  ZeroMQ socket  |";
         "'------------'        |              |  POW!!";
         "  ^    ^    ^         |              |";
         "  |    |    |         '--------------'";
         "  |    |    |";
         "  |    |    |";
         "  |    |    '--------- Spandex";
         "  |    |";
         "  |    '-------------- Cosmic rays";
         "";
         " Illegal radioisotopes from";
         " secret Soviet atomic city";
         " ";
         " source: https://raw.githubusercontent.com/imatix/zguide/master/images/fig1.txt"|])

    let splitTxtResult =
      @"../../TestTxtFiles/ZeroMQ_Fig1.txt"
      |> readFile
      |> splitTxt
    let splitTxtResultOk = (splitTxtResultExpected = splitTxtResult)

    let leftOffsetExpected = 0
    let leftOffsetResult = leftOffset (snd splitTxtResult)



    let makeGridResultExpected =
      [|[|'.'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '.'; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; '.'; '-'; '-'; '-'; '-'; '-'; '-'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '-'; '.'; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|'|'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|'|'; ' '; 'T'; 'C'; 'P'; ' '; 's'; 'o'; 'c'; 'k'; 'e'; 't'; ' '; '+'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '>'; '|'; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' '; 'Z'; 'A'; 'P'; '!'; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|'|'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' ';
          'B'; 'O'; 'O'; 'M'; '!'; ' '; ' '; '|'; ' '; ' '; 'Z'; 'e'; 'r'; 'o'; 'M';
          'Q'; ' '; 's'; 'o'; 'c'; 'k'; 'e'; 't'; ' '; ' '; '|'; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|'\''; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '-'; '\''; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' '; ' '; 'P'; 'O'; 'W'; '!'; '!';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|' '; ' '; '^'; ' '; ' '; ' '; ' '; '^'; ' '; ' '; ' '; ' '; '^'; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; '|'; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|' '; ' '; '|'; ' '; ' '; ' '; ' '; '|'; ' '; ' '; ' '; ' '; '|'; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; '\''; '-'; '-'; '-'; '-'; '-'; '-'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '-'; '\''; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|' '; ' '; '|'; ' '; ' '; ' '; ' '; '|'; ' '; ' '; ' '; ' '; '|'; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|' '; ' '; '|'; ' '; ' '; ' '; ' '; '|'; ' '; ' '; ' '; ' '; '|'; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|' '; ' '; '|'; ' '; ' '; ' '; ' '; '|'; ' '; ' '; ' '; ' '; '\''; '-'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '-'; ' '; 'S'; 'p'; 'a'; 'n'; 'd'; 'e'; 'x';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|' '; ' '; '|'; ' '; ' '; ' '; ' '; '|'; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|' '; ' '; '|'; ' '; ' '; ' '; ' '; '\''; '-'; '-'; '-'; '-'; '-'; '-'; '-';
          '-'; '-'; '-'; '-'; '-'; '-'; '-'; ' '; 'C'; 'o'; 's'; 'm'; 'i'; 'c'; ' ';
          'r'; 'a'; 'y'; 's'; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|' '; 'I'; 'l'; 'l'; 'e'; 'g'; 'a'; 'l'; ' '; 'r'; 'a'; 'd'; 'i'; 'o'; 'i';
          's'; 'o'; 't'; 'o'; 'p'; 'e'; 's'; ' '; 'f'; 'r'; 'o'; 'm'; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|' '; 's'; 'e'; 'c'; 'r'; 'e'; 't'; ' '; 'S'; 'o'; 'v'; 'i'; 'e'; 't'; ' ';
          'a'; 't'; 'o'; 'm'; 'i'; 'c'; ' '; 'c'; 'i'; 't'; 'y'; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' '; ' ';
          ' '; ' '; ' '; ' '|];
        [|' '; 's'; 'o'; 'u'; 'r'; 'c'; 'e'; ':'; ' '; 'h'; 't'; 't'; 'p'; 's'; ':';
          '/'; '/'; 'r'; 'a'; 'w'; '.'; 'g'; 'i'; 't'; 'h'; 'u'; 'b'; 'u'; 's'; 'e';
          'r'; 'c'; 'o'; 'n'; 't'; 'e'; 'n'; 't'; '.'; 'c'; 'o'; 'm'; '/'; 'i'; 'm';
          'a'; 't'; 'i'; 'x'; '/'; 'z'; 'g'; 'u'; 'i'; 'd'; 'e'; '/'; 'm'; 'a'; 's';
          't'; 'e'; 'r'; '/'; 'i'; 'm'; 'a'; 'g'; 'e'; 's'; '/'; 'f'; 'i'; 'g'; '1';
          '.'; 't'; 'x'; 't'|]|]

    let makeGridResult = splitTxtResult |> fun (_, b) -> b |> makeTrimmedGrid
